using System;
using UnityEngine;

namespace Boat.New
{
    public class NewBoatMovementManager : MonoBehaviour
    {
        public NewInputManagerInterface manager;
        public Rigidbody rigidBody;
        
        public float forwardAcceleration = 10f;
        public float backwardAcceleration = 5f;
        public float slowModifier = 0.6f;
        public float fastModifier = 1.5f;

        public float rotationSpeed = 5f;
        
        //TODO
        public float maxSpeed = 20f;
        
        public void FixedUpdate()
        {
            float speedModifier = 1f;
            if (manager.State.IsFastened) speedModifier *= fastModifier;
            if (manager.State.IsSlowed) speedModifier *= slowModifier;

            if (manager.movementZ < 0)
            {
                speedModifier *= backwardAcceleration;
            }
            else
            {
                speedModifier *= forwardAcceleration;
            }

            speedModifier *= manager.movementZ;
            
            float forwardSpeed = Vector3.Dot(rigidBody.velocity, transform.forward);
            if (forwardSpeed < maxSpeed) rigidBody.AddForce(transform.forward * speedModifier);
            
            rigidBody.AddTorque(transform.up * (manager.movementX * rotationSpeed));
            
        }
    }
}