using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControls : MonoBehaviour
{


    KeyCode forward = KeyCode.Z;
    KeyCode backward = KeyCode.S; 
    KeyCode left = KeyCode.Q;
    KeyCode right = KeyCode.D;
    
    public float forwardAcceleration = 0.0001f;
    public float backwardAcceleration = 0.00005f;
    public float rotationAcceleration = 0.0001f;
    
    public float maxSpeed = 1f;
    public float maxRotationSpeed = 0.1f;
    
    // Must be bellow 1.0f
    public float inertia = 0.001f;
    public float rotationInertia = 0.001f;
    
    float currentSpeed = 0.0f;
    float currentRotation = 0.0f;
    
    Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = velocity*currentSpeed*(1-inertia);
        
        changeAcceleration();
        
        changeRotation();
        
        // Rotate the boat
        transform.Rotate(0, currentRotation, 0);
        
        // Move the boat by finding the middle vector between the velocity and the forward vector
        transform.position += (velocity + transform.forward*currentSpeed)/2;
    }
    
    void changeRotation()
    {
        if (Input.GetKey(left)) currentRotation -= rotationAcceleration;
        else if (Input.GetKey(right)) currentRotation += rotationAcceleration;
        else
        {
            currentRotation *= (1-rotationInertia);

            if (Mathf.Abs(currentRotation) < 0.0001f) currentRotation = 0.0f;
        }
        
        if (Mathf.Abs(currentRotation) > maxRotationSpeed) currentRotation = Mathf.Sign(currentRotation) * maxRotationSpeed;
    }

    void changeAcceleration()
    {
        // If forward or backwards key is pressed, accelerate in the corresponding direction
        if (Input.GetKey(forward)) currentSpeed += forwardAcceleration;
        else if (Input.GetKey(backward)) currentSpeed -= backwardAcceleration;
        else
        {
            // If no key is pressed, slow down using inertia
            currentSpeed *= (1-inertia);

            //Snap to 0 if the speed is too low
            if (Mathf.Abs(currentSpeed) < 0.00001f) currentSpeed = 0;
        }

        if (Mathf.Abs(currentSpeed) > maxSpeed) currentSpeed = Mathf.Sign(currentSpeed) * maxSpeed;
    }
}
