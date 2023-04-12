using UnityEngine;

namespace Boat.Legacy
{
	public class AInputManager : InputManagerInterface
	{
		private Vector3 botTargetPosition = Vector3.zero;
		private int nextCheckpoint = 0;
    
		protected override void setMovement()
		{
			directionZ = 0;
			directionX = 0;

			if(botTargetPosition == Vector3.zero){
				botTargetPosition = boat.manager.GetNextCheckpointCoordinates(boat.body);
			}

			int passedCheckpoint = boat.manager.GetPlayerProgress(boat.body).Item2;
			if(passedCheckpoint == nextCheckpoint){
				nextCheckpoint = (nextCheckpoint + 1) % boat.manager.GetCheckpointCount();
				botTargetPosition = boat.manager.GetNextCheckpointCoordinates(boat.body);
			}

			RaycastHit hit;
			if(Physics.Raycast(boat.transform.position, boat.transform.forward, out hit, 100.0f)){
				//If object hit has same coordinates as next checkpoint, change target to straight ahead
				if(hit.transform.position == botTargetPosition){
					//Get exact coordinates of raycast hit
					botTargetPosition = hit.point;
				}
			}

			Vector2 direction = new Vector2(botTargetPosition.x - transform.position.x, botTargetPosition.z - transform.position.z);
			direction.Normalize();
			float angularDifference = Vector2.Angle(new Vector2(boat.transform.forward.x, boat.transform.forward.z), direction);
			angularDifference /= 45.0f;

			if(angularDifference >= 2.0f) directionZ = -1;

			//float angularDifference = Vector2.Angle(up, targetDirection);

			float angularSpeed = boat.rigidBody.angularVelocity.y;
			float steer = Mathf.Clamp(angularDifference - angularSpeed, -1.0f, 1.0f);

			if (steer > 0.3f) directionX = 1;
			else if (steer < -0.3f) directionX = -1;
	
			//If has effect Blind, change directionX randomly
			if(boat.hasEffect("Blind")){
				directionX = (short) UnityEngine.Random.Range(-1, 1);
			}

			float percentOfMaxSpeed = boat.currentSpeed / boat.maxSpeed;
			float absolutePercentOfMaxSpeed = Mathf.Abs(percentOfMaxSpeed);
			if (angularDifference >= 3.0f){
				if(absolutePercentOfMaxSpeed >= 0.20f) directionZ = -1;
			}
			else if(angularDifference >= 1.5f){
				if(percentOfMaxSpeed >= 0.10f) directionZ = -1;
				else if (absolutePercentOfMaxSpeed >= 0.50f) directionZ = 1;
				else directionZ = 0;
			}else if(angularDifference >= 1.0f){
				if(percentOfMaxSpeed >= 0.25f) directionZ = -1;
				else if (absolutePercentOfMaxSpeed >= 0.25f)directionZ = 1;
				else directionZ = 0;
			}else if(angularDifference >= 0.5f){
				if(percentOfMaxSpeed >= 0.50f)directionZ = -1;
				else if (absolutePercentOfMaxSpeed >= 0.10f) directionZ = 1;
				else directionZ = 0;
			}else if(angularDifference >= 0.25f){
				if(percentOfMaxSpeed >= 0.75f) directionZ = -1;
				else directionZ = 1;
			}else{
				if(percentOfMaxSpeed >= 0.90f) directionZ = 0;
				else directionZ = 1;
			}
		}
	}
}
