using System;
using Boat.New;
using UnityEngine;
using Boat.New.Canon;

namespace PowerUps
{
    public class PowerupCrate : MonoBehaviour
    {
        public PowerupArea powerupArea;
        
    
        public void SetArea(PowerupArea area)
        {
            powerupArea = area;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            //Look for NewBulletManagerScript on other
            var bulletManagerScript = other.GetComponent<NewBulletManager>();
            if(!bulletManagerScript) return;
            
            Debug.Log(bulletManagerScript.manager);
            
            bulletManagerScript.manager.AddRandomMunition();
        }
    }
}
