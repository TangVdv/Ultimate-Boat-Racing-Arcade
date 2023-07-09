using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Boat.New.Canon;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Boat.New
{
    public class NewAIInputManager : NewInputManagerInterface
    {
	    public ConfigScript config;
        public Rigidbody rigidBody;
        
        public NewAimingManager aimingManager;

        private float canonDirection = 0;
        private float canonAngle = 0;
        
        private static float firingAngleMercy = 45.0f;
        
        
        public LayerMask targetingMask;

        public bool debug = false;

        public enum Difficulty
        {
	        Easy = 1,
	        Normal = 2,
	        Hard = 3
        }

        public Difficulty difficulty = Difficulty.Normal;
        
        private NavMeshPath path = null;
        private bool pathPending = false;
        private Vector3 _botTargetPosition;
        private int _nextCheckpoint;
        private int pathCornerIndex = 0;
        public float pathReSamplingInterval = 0.2f;
        private float reSamplingTimer;
        public float pathCornerDistanceThreshold = 15.0f;
        private Collider botTargetCollider; 
        
        public LayerMask checkPointMask;
        
        private new void Awake()
        {
	        base.Awake();
	        _botTargetPosition = Vector3.zero;
	        _nextCheckpoint = 0;
			//TODO: find an other way to set initialVelocity
	        //initialVelocity = aimingManager.canons[0].initialVelocity;
        }

        public void InitializeAI(AIConfiguration botConfiguration)
        {
	        buildBoat.Initiate(
		        botConfiguration.AIBoat, 
		        botConfiguration.AICannon, 
		        botConfiguration.AIBoatMaterial, 
		        botConfiguration.AICannonMaterial);
	        
	        playerName = botConfiguration.Name;
	        playerType = PlayerType.AI;
	        difficulty = (Difficulty) config.Difficulty;

	        switch (difficulty)
	        {
		        case Difficulty.Easy:
			        firingAngleMercy = 90.0f;
			        break;
		        case Difficulty.Normal:
			        firingAngleMercy = 30.0f;
			        break;
		        case Difficulty.Hard:
			        firingAngleMercy = 10.0f;
			        break;
	        }
        }
        
        private void TakeAimingDecision()
        {
	        if (State.IsBlinded) return;

	        if (difficulty == Difficulty.Easy)
	        {
		        wantsToFire = true;
		        movementBarrels = UnityEngine.Random.Range(-1, 2);
		        movementCam = UnityEngine.Random.Range(-1, 2);
		        return;
	        }
	        
	        //Raycast in a sphere, in Targeting Physics layer
	        //If hit, check if it's an instance of a prefab in potentialTargetPrefabs
	        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 100.0f, transform.forward, 100.0f, targetingMask);

	        if (hits.Length <= 0) return;

	        //if self in hits, remove self
	        List<RaycastHit> filteredHits = new List<RaycastHit>();
	        foreach (var hit in hits)
	        {
		        if (hit.transform.gameObject == gameObject) continue;
		        filteredHits.Add(hit);
	        }
	        if(filteredHits.Count <= 0) return;
	        
	        
	        RaycastHit closestHit = filteredHits[0];
	        foreach (var hit in filteredHits)
	        {
		        if (hit.distance < closestHit.distance)
			        closestHit = hit;
	        }
	        
	        Vector3 targetPosition = closestHit.transform.position;
	        Vector3 direction = targetPosition - transform.position;

	        //get angle in degrees with 0 being forward
	        float angularDifference = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
	        
	        float diffToCanon = angularDifference - canonDirection;
	        
	        wantsToFire = false;
	        movementBarrels = 0;
	        movementCam = 0;

	        if (Mathf.Abs(diffToCanon) < 20.0f) movementCam = 0;
	        else if (diffToCanon > 0) movementCam = 1;
	        else movementCam = -1;
	        
	        float initialVelocity = aimingManager.canons[0].initialVelocity;

	        double maxDistance = (2 * Math.Pow(initialVelocity, 2)
	                                * Math.Sin(Mathf.Deg2Rad * canonAngle)
	                                * Math.Cos(Mathf.Deg2Rad * canonAngle))
	                             / 9.81;

	        double maxHeight = Math.Pow(initialVelocity * Math.Sin(Mathf.Deg2Rad * canonAngle), 2)
		        / (2 * 9.81) + transform.position.y;
	        maxHeight -= (1 - firingAngleMercy);

	        if (maxDistance < direction.magnitude) movementBarrels = 1;
	        if(maxHeight > targetPosition.y) movementBarrels = -1;
	        wantsToFire = true;
        }

        private void UpdateCanonAngle()
        {
	        if (movementCam != 0)
	        {
		        if (movementCam == 1) canonDirection += 3.0f;
		        else canonDirection -= 3.0f;

		        if (canonDirection >= 180) canonDirection = -180 + (canonDirection - 180);
		        else if (canonDirection <= -180) canonDirection = 180 - (canonDirection + 180);
	        }

	        if (movementBarrels > 0) canonAngle += 3;
	        else if (movementBarrels < 0) canonAngle -= 3;

	        if (canonAngle >= 45)
	        {
		        canonAngle = 44;
		        movementBarrels = 0;
	        }
	        else if (canonAngle <= 0)
	        {
				canonAngle = 1;
				movementBarrels = 0;
	        }

        }

        public void ResetPathing()
        {
	        reSamplingTimer = pathReSamplingInterval;
	        _botTargetPosition = Vector3.zero;
	        botTargetCollider = null;
	        pathPending = true;
	        path = null;
	        _nextCheckpoint = 0;
        }
        
        private void TakeMovementDecision()
        {
	        movementZ = 0;
	        movementX = 0;

	        Vector3 position = transform.position;

	        int passedCheckpoint = checkpointManager.GetPlayerProgress(gameObject).Item2;
	        
	        reSamplingTimer -= Time.deltaTime;
	        if (reSamplingTimer <= 0) pathPending = true;


	        if (passedCheckpoint >= _nextCheckpoint || _botTargetPosition == Vector3.zero)
	        {
		        _nextCheckpoint = (_nextCheckpoint + 1) % checkpointManager.GetCheckpointCount();

		        botTargetCollider = checkpointManager.GetNextCheckpointCollider(gameObject, (int) difficulty);
		        
		        pathPending = true;
	        }

	        if (pathPending)
	        {
		        reSamplingTimer = pathReSamplingInterval;
		        
		        pathPending = false;
		        
		        //Raycast from the front in the CheckPointMask layer and if not found, use ClosestPoint instead
		        Physics.Raycast(position, transform.forward, out var hitForward, 100000.0f, checkPointMask);
		        if (hitForward.collider == botTargetCollider)
		        {
			        if (debug) Debug.DrawLine(position, hitForward.point, Color.blue, pathReSamplingInterval);
			        _botTargetPosition = hitForward.point;
		        }
		        else _botTargetPosition = botTargetCollider.ClosestPoint(position);
		        
		        path = new NavMeshPath();
		        NavMesh.CalculatePath(position, _botTargetPosition, NavMesh.AllAreas, path);

		        pathCornerIndex = 0;
		        if (debug)
		        {
			        NavMesh.SamplePosition(position, out var hit, 10, NavMesh.AllAreas);
			        NavMesh.SamplePosition(_botTargetPosition, out var hitNext, 10, NavMesh.AllAreas);
			        Debug.DrawLine(position, hitNext.position, Color.green, pathReSamplingInterval);
			        for (var i = 1; i < path.corners.Length; i++)
			        {
				        Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.red, pathReSamplingInterval);
				        Debug.DrawLine(path.corners.Length == 0 ? position : path.corners[1], hitNext.position, Color.red, pathReSamplingInterval);
			        }
		        }
	        }

	        Vector3 targetCorner = pathCornerIndex >= path.corners.Length ? _botTargetPosition : path.corners[pathCornerIndex];
	        
	        Vector2 movement = new Vector2(targetCorner.x - position.x, targetCorner.z - position.z);

	        if (movement.magnitude <= pathCornerDistanceThreshold)
	        {
		        pathCornerIndex++;
		        movement = new Vector2(targetCorner.x - position.x, targetCorner.z - position.z);
	        }

	        movement.Normalize();
	        var forward = transform.forward;
	        
	        float angularDifference = Vector2.SignedAngle(new Vector2(forward.x, forward.z), movement);
	        angularDifference /= 45.0f;

	        float angularSpeed = 0.5f * rigidBody.angularVelocity.y;
	        float steer = Mathf.Clamp(angularDifference - angularSpeed, -1.0f, 1.0f);
	        
	        if (steer >= 0.50f) movementX = -1;
			else if (steer <= -0.50f) movementX = 1;
	        
	        //If has effect Blind, change movementX randomly
	        if (State.IsBlinded) movementX = (short)UnityEngine.Random.Range(-1, 1);
	        
			if (Mathf.Abs(angularDifference) >= 1.5f) movementZ = -1;
			else if (Mathf.Abs(angularDifference) <= 1f) movementZ = 1;
			else movementZ = 0;
        }

        private bool TakeRespawnDecision()
        {
	        //Check if the boat is upside down
	        if (transform.up.y < 0.0f)
	        {
		        Respawn();
		        return true;
	        }

	        return false;
        }

        private void TakeAmmoDecision()
        {
	        if(difficulty == Difficulty.Easy) return;

	        if (BulletInventory[currentBulletType] <= 0)
	        {
		        switchingMunition = 1;
		        return;
	        }

	        foreach (var pair in BulletInventory)
	        {
		        if (pair.Key != BulletType.Basic && pair.Value > 0 && currentBulletType != pair.Key)
		        { 
			        switchingMunition = 1; 
			        return;
		        }
	        }
	        switchingMunition = 0;
        }

        private void Update()
        {
	        //If world position is too low, respawn
	        if (transform.position.y < -5.0f)
	        {
		        Respawn();
		        return;
	        }

	        if(TakeRespawnDecision()) return;
	        
	        TakeAmmoDecision();
	        
	        if(checkpointManager) TakeMovementDecision();
	        TakeAimingDecision();

	        UpdateCanonAngle();
        }

    }
}