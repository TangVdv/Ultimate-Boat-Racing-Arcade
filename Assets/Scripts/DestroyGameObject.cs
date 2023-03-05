using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float floorLevel = -20.0f;
    public float delayTimeDestroy = 1f;

    private void Start()
    {
        StartCoroutine(DelayBeforeDestroy());
    }

    void Update()
    {
        if (transform.position.y < floorLevel)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(delayTimeDestroy);
        
        Destroy(gameObject);
    }
    
    
}
