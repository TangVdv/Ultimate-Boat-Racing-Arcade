using System.Collections.Generic;
using UnityEngine;

namespace Boat.New.Canon
{
    public class NewFiringManager: MonoBehaviour
    {
        public GameObject body;
        public GameObject barrels;
        public Transform[] barrelOutputs;
        [SerializeField] private GameObject cannonBall;

        public GameObject boat;
        
        public float initialVelocity = 20;
        private GameObject _bullet;

        public NewAimingManager aimingManager;

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
                _bullet.GetComponent<Rigidbody>().velocity = barrelOutput.forward * initialVelocity + boat.GetComponent<Rigidbody>().velocity;
            }
        }
    }
}