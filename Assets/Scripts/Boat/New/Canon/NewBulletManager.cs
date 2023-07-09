using System;
using UnityEngine;

namespace Boat.New.Canon
{
    public class NewBulletManager: MonoBehaviour
    {
        private BulletType _bulletType = BulletType.Basic;
        public NewAimingManager manager;

        public GameObject smokeScreenPrefab;
        public GameObject explosionPrefab;
        
        public GameObject originBoat;
        public LayerMask affectedLayer;
        
        public void SetBulletType(BulletType type)
        {
            _bulletType = type;
        }

        public void Update()
        {
            if (transform.position.y < 0) Apply();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != affectedLayer) return;
            
            //if other is origin boat, do nothing
            if (other.gameObject == originBoat) return;
            
            Apply();
        }

        public void Apply()
        {
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