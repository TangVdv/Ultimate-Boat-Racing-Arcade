﻿using System;
using UnityEngine;

namespace Boat.New.Canon
{
    public class NewBulletManager: MonoBehaviour
    {
        private BulletType _bulletType = BulletType.Basic;
        public Rigidbody rigidBody;
        public NewAimingManager manager;

        public GameObject smokeScreenPrefab;
        public GameObject explosionPrefab;
        
        public GameObject originBoat;
        
        public void SetBulletType(BulletType type)
        {
            _bulletType = type;
        }

        public void Update()
        {
            if (transform.position.y < 0) Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            
            //if other is origin boat, do nothing
            if (other.gameObject == originBoat)
            {
                Destroy(gameObject);
                return;
            }
            
            switch (_bulletType)
            {
                case BulletType.Explosive:
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    break;
                case BulletType.SmokeScreen:
                    Instantiate(smokeScreenPrefab, transform.position, Quaternion.identity);
                    break;
                case BulletType.Basic: break;
            }
            Destroy(gameObject);
        }

        public void SetManager(NewAimingManager aimingManager)
        {
            manager = aimingManager;
        }
        
        public void SetParent(GameObject parent)
        {
            originBoat = parent;
        }
    }
}