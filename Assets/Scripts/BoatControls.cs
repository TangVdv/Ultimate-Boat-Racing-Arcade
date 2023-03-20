using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoatControls : MonoBehaviour
{
    KeyCode _forward = KeyCode.Z;
    KeyCode _backward = KeyCode.S;
    KeyCode _left = KeyCode.Q;
    KeyCode _right = KeyCode.D;

    public CheckpointManager manager;
    
    public float forwardAcceleration = 10f;
    public float backwardAcceleration = 5f;
    
    //Also balance using rigidbody's angular drag
    public float rotationAcceleration = 0.5f;

    public float maxSpeed = 100f;

    // Must be below 1.0f
    public float decceleration = 0.01f;

    public float currentSpeed;

	public bool isBot;

	public GameObject body;

	public List<(string, int)> effects = new List<(string, int)>(); // (effectName, effectTime)

	private Quaternion initialRotation;
	
	public Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
		initialRotation = transform.rotation;
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
    void FixedUpdate()
    {
		short directionZ = 0;
		short directionX = 0;

		if(isBot){
			Tuple<short, short> botDirection = BotBehavior();

	        directionZ = botDirection.Item1;
			directionX = botDirection.Item2;
		}
		else{
			directionZ += (short) (Input.GetKey(_forward) ? 1 : 0);
			directionZ -= (short) (Input.GetKey(_backward) ? 1 : 0);
			directionX -= (short) (Input.GetKey(_left) ? 1 : 0);
			directionX += (short) (Input.GetKey(_right) ? 1 : 0);
		}

	    PhysicalBehavior(directionZ, directionX);
    }

	public void TriggerEffect(string effectName, int effectTime){
		effects.Add((effectName, effectTime));
	}

	public bool hasEffect(string effectName){
        foreach((string, int) effect in effects) if(effect.Item1 == effectName) return true;
        return false;
    }

	void PhysicalBehavior(short directionZ, short directionX){
		// If forward or backwards key is pressed, accelerate in the corresponding direction
        if (directionZ < 0) currentSpeed -= backwardAcceleration;
        else if (directionZ > 0) currentSpeed += forwardAcceleration;
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

	private Vector3 botTargetPosition = Vector3.zero;
	private int nextCheckpoint = 0;
	Tuple<short, short> BotBehavior(){
		short directionZ = 0;
		short directionX = 0;

		if(botTargetPosition == Vector3.zero){
            botTargetPosition = manager.GetNextCheckpointCoordinates(body);
        }

		int passedCheckpoint = manager.GetPlayerProgress(body).Item2;
		if(passedCheckpoint == nextCheckpoint){
            nextCheckpoint = (nextCheckpoint + 1) % manager.GetCheckpointCount();
            botTargetPosition = manager.GetNextCheckpointCoordinates(body);
        }

		Vector2 direction = new Vector2(botTargetPosition.x - transform.position.x, botTargetPosition.z - transform.position.z);
		direction.Normalize();
		float angularDifference = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), direction);
		angularDifference /= 45.0f;

		if(angularDifference >= 2.0f) directionZ = -1;

	    //float angularDifference = Vector2.Angle(up, targetDirection);

		float angularSpeed = rigidBody.angularVelocity.y;
		float steer = Mathf.Clamp(angularDifference - angularSpeed, -1.0f, 1.0f);

		if (steer > 0.3f) directionX = 1;
        else if (steer < -0.3f) directionX = -1;
	
	    //If has effect Blind, change directionX randomly
		if(hasEffect("Blind")){
		    directionX = (short) UnityEngine.Random.Range(-1, 1);
        }

		if(Mathf.Abs(angularDifference) < 0.3f) directionZ = 1;

		Debug.Log(
		"Angular speed: " + angularSpeed + "\n" +
		"Angular difference: " + angularDifference + "\n" +
		"Direction X: " + directionX + "\n" +
		"Direction Z: " + directionZ + "\n" 
		);

		return new Tuple<short, short>(directionZ, directionX);
	}

/*
	private int currentCheckpoint = 0;
	private Vector3 direction;
	private double initialAngularDifference = 0.0;
	private int nextCheckpoint = 0;
	void BotBehavior(){
		int passedCheckpoint = manager.GetPlayerProgress(body).Item2;

		var direction = manager.GetNextCheckpointCoordinates(body) - transform.position;

		if(nextCheckpoint == currentCheckpoint || passedCheckpoint == nextCheckpoint){
			currentCheckpoint = nextCheckpoint;
			nextCheckpoint = (nextCheckpoint + 1) % manager.GetCheckpointCount();

			initialAngularDifference = Vector3.Angle(transform.forward, direction);
        }
		
		double angularDifference = Vector3.Angle(transform.forward, direction);
		angularDifference = Mathf.Abs((float) angularDifference);

		//If has effect Blind, change angularDifference randomly
		if(hasEffect("Blind")){
			angularDifference += Random.Range(-10, 10);
		}

		var angularVelocity = rigidBody.angularVelocity;

		int rotationDirection = 0;
		if(initialAngularDifference > 10f){
			if(angularDifference > initialAngularDifference / 2) {
				//Debug.Log("Rotating");
				rotationDirection = Vector3.Cross(transform.forward, direction).y > 0 ? 1 : -1;
				currentSpeed *=  (1 - decceleration);
				if(Mathf.Abs(currentSpeed) < 0.5f) currentSpeed = 0;
			}
        	else if(angularDifference < initialAngularDifference / 2 && angularVelocity.magnitude > 0.5f) {
				//Debug.Log("Undoing rotation");
				rotationDirection = Vector3.Cross(transform.forward, direction).y > 0 ? -1 : 1;
				currentSpeed += forwardAcceleration;
			} else {
				//Debug.Log("Accelerating");
            	currentSpeed += forwardAcceleration;
        	}
		}else{
            //Debug.Log("Accelerating");
            currentSpeed += forwardAcceleration;
        }
		    
        if (Mathf.Abs(currentSpeed) > maxSpeed) currentSpeed = Mathf.Sign(currentSpeed) * maxSpeed;

		float speedModifier = 1;
		
		if(hasEffect("Slow")) speedModifier = 0.5f;
		else if(hasEffect("Fast")) speedModifier = 2.0f;

		rigidBody.AddTorque(transform.up * (rotationDirection * rotationAcceleration));
		rigidBody.AddForce(transform.forward * (currentSpeed * speedModifier));
    }
*/
}
    
