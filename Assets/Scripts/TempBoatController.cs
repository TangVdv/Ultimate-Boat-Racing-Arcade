using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoatController : MonoBehaviour
{
    //public Transform[] propellers;
    public float propulsionForce;
    public float rotationForce;
    private bool _forward, _backward, _left, _right;
    public Rigidbody rigidBody;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) _forward = true;
        if (Input.GetKeyDown(KeyCode.Q)) _left = true;
        if (Input.GetKeyDown(KeyCode.S)) _backward = true;
        if (Input.GetKeyDown(KeyCode.D)) _right = true;
        
        if (Input.GetKeyUp(KeyCode.Z)) _forward = false;
        if (Input.GetKeyUp(KeyCode.Q)) _left = false;
        if (Input.GetKeyUp(KeyCode.S)) _backward = false;
        if (Input.GetKeyUp(KeyCode.D)) _right = false;
    }

    private void FixedUpdate()
    {
        Vector3 acceleration = Vector3.zero;
        if(_forward)acceleration += transform.up;
        if (_backward) acceleration -= transform.up;
        rigidBody.AddForce(acceleration * (propulsionForce * Time.deltaTime), ForceMode.Acceleration);
        
        Vector3 rotation = Vector3.zero;
        if(_left)rotation -= transform.worldToLocalMatrix.MultiplyVector(transform.up);
        if(_right)rotation += transform.worldToLocalMatrix.MultiplyVector(transform.up);
        rigidBody.AddTorque(rotation * (rotationForce * Time.deltaTime),ForceMode.Acceleration);


    }
}
