using System;
using UnityEngine;

namespace Boat.New
{
    public class NewBoatMovementManager : MonoBehaviour
    {
        public NewInputManagerInterface manager;
        public Rigidbody rigidBody;
        
        public bool frozen = false;
        
        public float forwardAcceleration = 10f;
        public float backwardAcceleration = 5f;
        public float slowModifier = 0.6f;
        public float fastModifier = 1.5f;

        public float rotationSpeed = 5f;
        
        private float _speedModifier;

        public void FixedUpdate()
        {
            if (frozen) return;

            _speedModifier = 1f;
            if (manager.State.IsFastened) _speedModifier *= fastModifier;
            if (manager.State.IsSlowed) _speedModifier *= slowModifier;

            if (manager.movementZ < 0)
            {
                _speedModifier *= backwardAcceleration;
            }
            else
            {
                _speedModifier *= forwardAcceleration;
            }

            _speedModifier *= manager.movementZ;
            
            float forwardSpeed = Vector3.Dot(rigidBody.velocity, transform.forward);
            //if (forwardSpeed < maxSpeed) rigidBody.AddForce(transform.forward * _speedModifier);
            // NB : Moved to Floaters
            
            rigidBody.AddTorque(transform.up * (manager.movementX * rotationSpeed));
            
        }

        public float GetSpeedModifier()
        {
            return _speedModifier;
        }
    }
}