using System.Collections.Generic;
using Checkpoints;
using UnityEngine;

namespace Boat.Legacy
{
	public class BoatControls : MonoBehaviour
	{
		public CheckpointManager manager;

		public InputManagerInterface inputManager;

		public float forwardAcceleration = 10f;
		public float backwardAcceleration = 5f;
    
		//Also balance using rigidbody's angular drag
		public float rotationAcceleration = 0.5f;

		public float maxSpeed = 100f;

		// Must be below 1.0f
		public float decceleration = 0.01f;

		public float currentSpeed;

		public GameObject body;

		public List<(string, int)> effects = new List<(string, int)>(); // (effectName, effectTime)

		private Quaternion initialRotation;
	
		public Rigidbody rigidBody;

		// Start is called before the first frame update
		void Start()
		{
			initialRotation = transform.rotation;
			inputManager.setController(this);
			manager.AddPlayer(body);
		}


		void update(){
			List<int> newTimes = new List<int>();
			foreach((string, int) effect in effects) newTimes.Add(effect.Item2 - 1);
			foreach(int time in newTimes) {
				if(time == 0) effects.RemoveAt(newTimes.IndexOf(time));
				else effects[newTimes.IndexOf(time)] = (effects[newTimes.IndexOf(time)].Item1, time);
			}

			var progress = manager.GetPlayerProgress(body);
		}

		// Update is called once per frame
		public void TriggerEffect(string effectName, int effectTime){
			effects.Add((effectName, effectTime));
		}

		public bool hasEffect(string effectName){
			foreach((string, int) effect in effects) if(effect.Item1 == effectName) return true;
			return false;
		}

		public void Movement(short directionZ, short directionX){
			// If forward or backwards key is pressed, accelerate in the corresponding direction
			if (directionZ < 0) currentSpeed -= backwardAcceleration;

			if (directionZ > 0) currentSpeed += forwardAcceleration;
			else
			{
				//Apply inertia if not pressing keys
				currentSpeed *=  (1 - decceleration);

				// If the speed is too low, set it to 0
				if (Mathf.Abs(currentSpeed) < 0.5f) currentSpeed = 0;
			}

			if (Mathf.Abs(currentSpeed) > maxSpeed) currentSpeed = Mathf.Sign(currentSpeed) * maxSpeed;
        
			// Rotate the boat

			int rotationDirection = 0;
			if (directionX < 0) rotationDirection = -1;
			else if (directionX > 0) rotationDirection = 1;

			float speedModifier = 1;
		
			if(hasEffect("Slow")) speedModifier = 0.5f;
			else if(hasEffect("Fast")) speedModifier = 2.0f;

			rigidBody.AddTorque(transform.up * (rotationDirection * rotationAcceleration));
			rigidBody.AddForce(transform.forward * (currentSpeed * speedModifier));
		}
	}
}
    
