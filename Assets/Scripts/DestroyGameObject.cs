using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float floorLevel = -20.0f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < floorLevel)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
    
}
