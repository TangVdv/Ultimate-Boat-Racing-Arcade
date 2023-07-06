using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostScript : MonoBehaviour
{
    public int boostForce;
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.AddForce(rigidbody.transform.forward * boostForce, ForceMode.Impulse);
            Debug.Log("Add force");
        }
    }
}
