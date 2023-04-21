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

    // Update is called once per frame
    void FixedUpdate()
    {

	    if(isBot) BotBehavior();
		else ManualBehavior();


		List<int> newTimes = new List<int>();
		foreach((string, int) effect in effects) newTimes.Add(effect.Item2 - 1);
		foreach(int time in newTimes) {
			if(time == 0) effects.RemoveAt(newTimes.IndexOf(time));
			else effects[newTimes.IndexOf(time)] = (effects[newTimes.IndexOf(time)].Item1, time);
		}

		var progress = manager.GetPlayerProgress(body);
		//Debug.Log(progress);
    }

	public void TriggerEffect(string effectName, int effectTime){
		effects.Add((effectName, effectTime));
	}

	public bool hasEffect(string effectName){
        foreach((string, int) effect in effects) if(effect.Item1 == effectName) return true;
        return false;
    }

	void ManualBehavior(){
		// If forward or backwards key is pressed, accelerate in the corresponding direction
        if (Input.GetKey(_forward)) currentSpeed += forwardAcceleration;
        else if (Input.GetKey(_backward)) currentSpeed -= backwardAcceleration;
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
        if (Input.GetKey(_left)) rotationDirection = -1;
        else if (Input.GetKey(_right))rotationDirection = 1;

		float speedModifier = 1;
		
		if(hasEffect("Slow")) speedModifier = 0.5f;
		else if(hasEffect("Fast")) speedModifier = 2.0f;

		rigidBody.AddTorque(transform.up * (rotationDirection * rotationAcceleration));
        rigidBody.AddForce(transform.forward * (currentSpeed * speedModifier));
	}

	private int currentCheckpoint = 0;
	private Vector3 direction;
	private double initialAngularDifference = 0.0;
	private int nextCheckpoint = 0;
	void BotBehavior(){
		//Locate next checkpoint
		//Calculate direction to next checkpoint
	    //Calculate angle to next checkpoint
		//If angle is too big, rotate towards it
		//If angle is small enough, accelerate
		
		//var manager = GameObject.FindWithTag("CheckpointController").GetComponent<CheckpointManager>();

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
		
		
		/*
		//If not facing the right direction, rotate towards it
		int rotationDirection = 0;
		if(Vector3.Angle(transform.forward, direction) > 10){
			rotationDirection = Vector3.Cross(transform.forward, direction).y > 0 ? 1 : -1;
		}

		//If facing the right direction, accelerate
		if(rotationDirection == 0) currentSpeed += forwardAcceleration;
        else{
	        //apply inertia 
			currentSpeed *=  (1 - decceleration);
			if (Mathf.Abs(currentSpeed) < 0.5f) currentSpeed = 0;
		}
		*/
		    
        if (Mathf.Abs(currentSpeed) > maxSpeed) currentSpeed = Mathf.Sign(currentSpeed) * maxSpeed;

		float speedModifier = 1;
		
		if(hasEffect("Slow")) speedModifier = 0.5f;
		else if(hasEffect("Fast")) speedModifier = 2.0f;

		rigidBody.AddTorque(transform.up * (rotationDirection * rotationAcceleration));
		rigidBody.AddForce(transform.forward * (currentSpeed * speedModifier));
    }
}
    
