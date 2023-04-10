using UnityEngine;

namespace Boat.New
{
    public class NewBulletManager: MonoBehaviour
    {
        public enum BulletType
        {
            Basic,
            Explosive,
            SmokeScreen
        }

        public BulletType bulletType = BulletType.Basic;
        public Rigidbody rigidBody;
    }
}