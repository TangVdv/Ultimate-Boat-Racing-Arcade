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
                var lossyScale =  barrelOutput.parent.lossyScale;
                var localScale = new Vector3(
                    lossyScale.x, 
                    lossyScale.x, 
                    lossyScale.z
                );
                cannonBall.transform.localScale = localScale;
                _bullet = Instantiate(cannonBall, barrelOutput.position, barrelOutput.rotation);  
                NewBulletManager bulletManager = _bullet.GetComponent<NewBulletManager>();
                bulletManager.SetManager(aimingManager);
                bulletManager.SetBulletType(bulletType);
                bulletManager.SetParent(boat);
                //TODO change velocity location to bulletManager script
                _bullet.GetComponent<Rigidbody>().velocity = barrelOutput.forward * initialVelocity + boatRigidbody.velocity;
            }
        }
    }
}