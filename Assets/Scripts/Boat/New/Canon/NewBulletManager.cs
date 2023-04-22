using UnityEngine;

namespace Boat.New.Canon
{
    public class NewBulletManager: MonoBehaviour
    {
        private BulletType bulletType = BulletType.Basic;
        public Rigidbody rigidBody;
        public NewAimingManager manager;

        public GameObject smokeScreenPrefab;
        public GameObject explosionPrefab;
        
        public void SetBulletType(BulletType type)
        {
            bulletType = type;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            switch (bulletType)
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
    }
}