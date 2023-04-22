using System;
using System.Collections.Generic;
using UnityEngine;

namespace PowerUps.Effects
{
    public class Explosion : MonoBehaviour
    {
        public float radius = 5f;
        public float force = 10f;

        public float lifeCycle = 1f;
        
        private void Start()
        {
            Collider[] colliders = new Collider[100];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders);
            for (int i = 0; i < size; i++)
            {
                var rb = colliders[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                }
            }
        }
        
        private void Update()
        {
            lifeCycle -= Time.deltaTime;
            if (lifeCycle <= 0) Destroy(gameObject);
        }
    }
}
