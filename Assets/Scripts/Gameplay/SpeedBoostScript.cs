using System.Collections;
using System.Collections.Generic;
using Boat.New;
using UnityEngine;

public class SpeedBoostScript : MonoBehaviour
{
    public int boostForce;
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidbody = other.GetComponentInParent<Rigidbody>();
        NewBoatMovementManager boatMovementManager = other.GetComponentInParent<NewBoatMovementManager>();
        if (rigidbody && boatMovementManager)
        {
            rigidbody.AddForce(transform.forward * boostForce * boatMovementManager.boostMultiplier, ForceMode.VelocityChange);
        }
    }
}
