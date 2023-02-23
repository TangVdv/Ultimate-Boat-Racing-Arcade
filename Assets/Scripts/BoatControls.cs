using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControls : MonoBehaviour
{


    public bool DEBUG_IS_CURRENT = false;
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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!DEBUG_IS_CURRENT) return;
        
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
        if (Input.GetKey(left))
        {
            rotationDirection = -1;
        }
        else if (Input.GetKey(right))
        {
            rotationDirection = 1;
        }

        GetComponent<Rigidbody>().AddTorque(transform.up * rotationDirection * rotationAcceleration);

        // Apply the speed to the boat
        GetComponent<Rigidbody>().AddForce(transform.forward * currentSpeed);
    }
}
    
