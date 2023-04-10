using System;
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

        private new void Start()
        {
	        base.Start();
	        _botTargetPosition = Vector3.zero;
	        _nextCheckpoint = 0;
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

	        if (angularDifference >= 2.0f) movementZ = -1;

	        //float angularDifference = Vector2.Angle(up, targetmovement);

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

	        /*
	        float percentOfMaxSpeed = boat.currentSpeed / boat.maxSpeed;
	        float absolutePercentOfMaxSpeed = Mathf.Abs(percentOfMaxSpeed);
	        if (angularDifference >= 3.0f){
		        if(absolutePercentOfMaxSpeed >= 0.20f) movementZ = -1;
	        }
	        else if(angularDifference >= 1.5f){
		        if(percentOfMaxSpeed >= 0.10f) movementZ = -1;
		        else if (absolutePercentOfMaxSpeed >= 0.50f) movementZ = 1;
		        else movementZ = 0;
	        }else if(angularDifference >= 1.0f){
                if(percentOfMaxSpeed >= 0.25f) movementZ = -1;
		        else if (absolutePercentOfMaxSpeed >= 0.25f)movementZ = 1;
                else movementZ = 0;
	        }else if(angularDifference >= 0.5f){
		        if(percentOfMaxSpeed >= 0.50f)movementZ = -1;
		        else if (absolutePercentOfMaxSpeed >= 0.10f) movementZ = 1;
                else movementZ = 0;
	        }else if(angularDifference >= 0.25f){
                if(percentOfMaxSpeed >= 0.75f) movementZ = -1;
                else movementZ = 1;
	        }else{
	            if(percentOfMaxSpeed >= 0.90f) movementZ = 0;
                else movementZ = 1;
            }*/
        }
    }
}