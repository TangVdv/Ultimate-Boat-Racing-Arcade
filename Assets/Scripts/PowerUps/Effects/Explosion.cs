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

        public LayerMask affectedLayer;
        
        private void Start()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.forward, 100.0f, affectedLayer);
            foreach (var hit in hits)
            {
                var rb = hit.collider.GetComponent<Rigidbody>();
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
