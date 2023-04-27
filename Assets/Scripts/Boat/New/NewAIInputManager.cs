using System;
using System.Collections.Generic;
using System.Numerics;
using Boat.New.Canon;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Boat.New
{
    public class NewAIInputManager : NewInputManagerInterface
    {
	    
	    private Vector3 _botTargetPosition;
        private int _nextCheckpoint;
        public GameObject boat;
        public Rigidbody rigidBody;
        
        public CheckpointManager manager;
        public NewAimingManager aimingManager;
        private float initialVelocity;

        private float canonDirection = 0;
        private float canonAngle = 0;
        
        public float firingAngleMercy = 0.5f;
        
        public LayerMask targetingMask;
        
        private NewBoatMovementManager _newBoatMovementManager;
        
        //TODO: mettre dans un fichier séparé
        public class AIDecision
        {
	        public float angularThreshold;
	        public float maxSpeed;
	        public float minSpeed;

	        public AIDecision(float angularThreshold, float maxSpeed, float minSpeed)
	        {
		        this.angularThreshold = angularThreshold;
		        this.maxSpeed = maxSpeed;
		        this.minSpeed = minSpeed;
	        }
	        
	        public bool IsApplicable(float angularDifference)
	        {
		        return angularDifference >= angularThreshold;
	        }

	        public int DecisionTree(float speed)
	        {
		        if(speed >= maxSpeed) return -1;
		        return speed <= minSpeed ? 1 : 0;
	        }
        }
        
        public List<AIDecision> decisionTree = new List<AIDecision>();

        private new void Start()
        {
	        _newBoatMovementManager = boat.GetComponent<NewBoatMovementManager>();
	        base.Start();
	        _botTargetPosition = Vector3.zero;
	        _nextCheckpoint = 0;
	        
	        initialVelocity = aimingManager.canons[0].initialVelocity;
	        
	        float maxSpeed = _newBoatMovementManager.maxSpeed;

	        decisionTree.Add(new AIDecision(2.0f, maxSpeed * 0.05f, maxSpeed * 0.00f));
	        decisionTree.Add(new AIDecision(1.5f, maxSpeed * 0.10f, maxSpeed * 0.05f));
	        decisionTree.Add(new AIDecision(1.0f, maxSpeed * 0.25f, maxSpeed * 0.10f));
	        decisionTree.Add(new AIDecision(0.5f, maxSpeed * 0.50f, maxSpeed * 0.25f));
	        decisionTree.Add(new AIDecision(0.25f, maxSpeed * 0.70f, maxSpeed * 0.50f));
	        decisionTree.Add(new AIDecision(0.0f, maxSpeed * 1.00f, maxSpeed * 0.90f));
        }

        //TODO: Register target
        private void TakeAimingDecision()
        {
	        Debug.Log("Targeting");
	        if (State.IsBlinded) return;

	        //Raycast in a sphere, in Targeting Physics layer
	        //If hit, check if it's an instance of a prefab in potentialTargetPrefabs
	        RaycastHit[] hits =
		        Physics.SphereCastAll(transform.position, 100.0f, transform.forward, 100.0f, targetingMask);

	        if (hits.Length <= 0) return;

	        //if self in hits, remove self
	        List<RaycastHit> filteredHits = new List<RaycastHit>();
	        foreach (var hit in hits)
	        {
		        if (hit.transform.gameObject == boat) continue;
		        filteredHits.Add(hit);
	        }

	        // Get closest
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

	        double maxDistance = (2 * Math.Pow(initialVelocity, 2)
	                                * Math.Sin(Mathf.Deg2Rad * canonAngle)
	                                * Math.Cos(Mathf.Deg2Rad * canonAngle))
	                             / 9.81;

	        double maxHeight = Math.Pow(initialVelocity * Math.Sin(Mathf.Deg2Rad * canonAngle), 2)
		        / (2 * 9.81) + transform.position.y;
	        maxHeight -= (1 - firingAngleMercy);

	        if (maxDistance < direction.magnitude) movementBarrels = 1;
	        if(maxHeight > targetPosition.y) movementBarrels = -1;
	        else wantsToFire = true;
        }

        private void UpdateCanonAngle()
        {
	        if (movementCam != 0)
	        {
		        if (movementCam == 1) canonDirection += 3.0f;
		        else canonDirection -= 3.0f;
	        
		        if(canonDirection >= 180) canonDirection = -180+(canonDirection-180);
		        else if (canonDirection <= -180) canonDirection = 180-(canonDirection+180);   
	        }

	        if (movementBarrels > 0) canonAngle += 3;
	        else if (movementBarrels < 0) canonAngle -= 3;
	        
	        if(canonAngle >= 45) canonAngle = 45;
	        else if (canonAngle <= 0) canonAngle = 0;
        }

        private void TakeMovementDecision()
        {
	        movementZ = 0;
	        movementX = 0;

	        if (_botTargetPosition == Vector3.zero)
	        {
		        _botTargetPosition = manager.GetNextCheckpointCoordinates(boat);
	        }

	        int passedCheckpoint = manager.GetPlayerProgress(boat).Item2;
	        if (passedCheckpoint == _nextCheckpoint)
	        {
		        _nextCheckpoint = (_nextCheckpoint + 1) % manager.GetCheckpointCount();
		        _botTargetPosition = manager.GetNextCheckpointCoordinates(boat);
	        }

	        RaycastHit hit;
	        if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
	        {
		        //If object hit has same coordinates as next checkpoint, change target to straight ahead
		        if (hit.transform.position == _botTargetPosition)
		        {
			        //Get exact coordinates of raycast hit
			        _botTargetPosition = hit.point;
		        }
	        }

	        var position = transform.position;
	        Vector2 movement = new Vector2(_botTargetPosition.x - position.x, _botTargetPosition.z - position.z);
	        movement.Normalize();
	        var forward = transform.forward;
	        float angularDifference = Vector2.Angle(new Vector2(forward.x, forward.z), movement);
	        angularDifference /= 45.0f;

	        float angularSpeed = rigidBody.angularVelocity.y;
	        float steer = Mathf.Clamp(angularDifference - angularSpeed, -1.0f, 1.0f);

	        if (steer > 0.3f) movementX = 1;
	        else if (steer < -0.3f) movementX = -1;

	        //If has effect Blind, change movementX randomly
	        if (State.IsBlinded)
	        {
		        movementX = (short)UnityEngine.Random.Range(-1, 1);
	        }
	        
	        movementZ = movement.y;

	        float forwardSpeed = Vector3.Dot(rigidBody.velocity, transform.forward);

	        foreach (var decision in decisionTree)
	        {
		        if (decision.IsApplicable(angularDifference))
		        {
			        movementZ = decision.DecisionTree(forwardSpeed);
			        break;
		        }
	        }
        }

        private void Update()
        {
	        TakeMovementDecision();
	        TakeAimingDecision();

	        UpdateCanonAngle();
        }

    }
}