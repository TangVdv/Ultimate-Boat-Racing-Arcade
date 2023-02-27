using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControls : MonoBehaviour
{
    KeyCode forward = KeyCode.Z;
    KeyCode backward = KeyCode.S;
    KeyCode left = KeyCode.Q;
    KeyCode right = KeyCode.D;

    public float forwardAcceleration = 10f;
    public float backwardAcceleration = 5f;
    
    //Also balance using rigidbody's angular drag
    public float rotationAcceleration = 0.5f;

    public float maxSpeed = 100f;

    // Must be below 1.0f
    public float decceleration = 0.01f;

    public float currentSpeed = 0.0f;

	public bool isBot = false;

	private GameObject body;


    // Start is called before the first frame update
    void Start()
    {
		//Find children element with Player tag
		foreach (Transform child in transform)
	        if(child.gameObject.tag == "Player") body = child.gameObject;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
     	if(isBot) BotBehavior();
		else ManualBehavior();   
    }

	void ManualBehavior(){
		// If forward or backwards key is pressed, accelerate in the corresponding direction
        if (Input.GetKey(forward)) currentSpeed += forwardAcceleration;
        else if (Input.GetKey(backward)) currentSpeed -= backwardAcceleration;
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
        if (Input.GetKey(left)) rotationDirection = -1;
        else if (Input.GetKey(right))rotationDirection = 1;

		GetComponent<Rigidbody>().AddTorque(transform.up * rotationDirection * rotationAcceleration);
        GetComponent<Rigidbody>().AddForce(transform.forward * currentSpeed);
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
		
		var manager = GameObject.FindWithTag("CheckpointController").GetComponent<CheckpointManager>();

		int passedCheckpoint = manager.getPlayerProgress(body).Item2;

		var direction = manager.getNextCheckpointCoordinates(body) - transform.position;

		if(nextCheckpoint == currentCheckpoint || passedCheckpoint == nextCheckpoint){
			currentCheckpoint = nextCheckpoint;
			nextCheckpoint = (nextCheckpoint + 1) % manager.getCheckpointCount();

			initialAngularDifference = Vector3.Angle(transform.forward, direction);
        }
		
		double angularDifference = Vector3.Angle(transform.forward, direction);
		angularDifference = Mathf.Abs((float) angularDifference);

		var angularVelocity = GetComponent<Rigidbody>().angularVelocity;

		int rotationDirection = 0;
		if(initialAngularDifference > 10f){
			if(angularDifference > initialAngularDifference / 2) {
				Debug.Log("Rotating");
				rotationDirection = Vector3.Cross(transform.forward, direction).y > 0 ? 1 : -1;
				currentSpeed *=  (1 - decceleration);
				if(Mathf.Abs(currentSpeed) < 0.5f) currentSpeed = 0;
			}
        	else if(angularDifference < initialAngularDifference / 2 && angularVelocity.magnitude > 0.5f) {
				Debug.Log("Undoing rotation");
				rotationDirection = Vector3.Cross(transform.forward, direction).y > 0 ? -1 : 1;
				currentSpeed += forwardAcceleration;
			} else {
				Debug.Log("Accelerating");
            	currentSpeed += forwardAcceleration;
        	}
		}else{
            Debug.Log("Accelerating");
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

	    GetComponent<Rigidbody>().AddTorque(transform.up * rotationDirection * rotationAcceleration);
        GetComponent<Rigidbody>().AddForce(transform.forward * currentSpeed);
    }
}
    
