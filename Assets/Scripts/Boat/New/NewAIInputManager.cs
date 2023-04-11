using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boat.New
{
    public class NewAIInputManager : NewInputManagerInterface
    {
	    
	    private Vector3 _botTargetPosition;
        private int _nextCheckpoint;
        public GameObject boat;
        public Rigidbody rigidBody;
        
        public CheckpointManager manager;
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
	        
	        float maxSpeed = _newBoatMovementManager.maxSpeed;
	        
	        decisionTree.Add(new AIDecision(2.0f, maxSpeed * 0.05f, maxSpeed * 0.00f));
	        decisionTree.Add(new AIDecision(1.5f, maxSpeed * 0.10f, maxSpeed * 0.05f));
	        decisionTree.Add(new AIDecision(1.0f, maxSpeed * 0.25f, maxSpeed * 0.10f));
	        decisionTree.Add(new AIDecision(0.5f, maxSpeed * 0.50f, maxSpeed * 0.25f));
	        decisionTree.Add(new AIDecision(0.25f, maxSpeed * 0.70f, maxSpeed * 0.50f));
	        decisionTree.Add(new AIDecision(0.0f, maxSpeed * 1.00f, maxSpeed * 0.90f));
        }

        private void Update()
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
			        Debug.Log("Decision applicable: "+decision.angularThreshold + "Speed: " + forwardSpeed);
			        movementZ = decision.DecisionTree(forwardSpeed);
			        break;
		        }
	        }
        }

    }
}