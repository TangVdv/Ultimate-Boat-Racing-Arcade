using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostScript : MonoBehaviour
{
    public int boostForce;
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidbody = other.GetComponentInParent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.AddForce(rigidbody.transform.forward * boostForce, ForceMode.VelocityChange);
        }
    }
}
