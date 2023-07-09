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
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, affectedLayer);
            foreach (var hit in hits)
            {
                var rb = hit.GetComponentInParent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce((hit.transform.position - transform.position) * force, ForceMode.Impulse);
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
