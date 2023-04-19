using UnityEngine;

namespace Boat.New.Canon
{
    public class NewBulletManager: MonoBehaviour
    {
        private BulletType bulletType = BulletType.Basic;
        public Rigidbody rigidBody;
        public NewAimingManager manager;

        public void SetBulletType(BulletType type)
        {
            bulletType = type;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            switch (bulletType)
            {
                case BulletType.Explosive:
                    //TODO
                    break;
                case BulletType.SmokeScreen:
                    //TODO
                    break;
                case BulletType.Basic: break;
            }
        }

        public void SetManager(NewAimingManager aimingManager)
        {
            manager = aimingManager;
        }
    }
}