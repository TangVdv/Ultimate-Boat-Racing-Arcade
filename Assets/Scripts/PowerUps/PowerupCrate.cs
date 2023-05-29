using System;
using Boat.New;
using UnityEngine;
using Boat.New.Canon;

namespace PowerUps
{
    public class PowerupCrate : MonoBehaviour
    {
        public PowerupArea powerupArea;
        public bool debug;
        
    
        public void SetArea(PowerupArea area)
        {
            powerupArea = area;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            //Look for NewBulletManagerScript on other
            var bulletManagerScript = other.GetComponent<NewBulletManager>();
            if(!bulletManagerScript) return;
            
            if(debug)Debug.Log(bulletManagerScript.manager);
            
            bulletManagerScript.manager.AddRandomMunition();
            //Destroy this object
            powerupArea.PowerUpCollected(gameObject);
            Destroy(gameObject);
        }
    }
}
