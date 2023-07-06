using System.Collections.Generic;
using UnityEngine;

namespace Boat.New.Canon
{
    public class NewFiringManager: MonoBehaviour
    {
        public GameObject body;
        public GameObject barrels;
        public Transform[] barrelOutputs;
        public GameObject cannonBall;
        public GameObject boat;
        public Rigidbody boatRigidbody;

        public float initialVelocity = 20;
        public NewAimingManager aimingManager;

        private GameObject _bullet;

        public void Fire( BulletType bulletType)
        {
            foreach (var barrelOutput in barrelOutputs)
            {
                _bullet = Instantiate(cannonBall, barrelOutput.position, barrelOutput.rotation);  
                NewBulletManager bulletManager = _bullet.GetComponent<NewBulletManager>();
                bulletManager.SetManager(aimingManager);
                bulletManager.SetBulletType(bulletType);
                bulletManager.SetParent(boat);
                _bullet.GetComponent<Rigidbody>().velocity = barrelOutput.forward * initialVelocity + boatRigidbody.velocity;
            }
        }
    }
}