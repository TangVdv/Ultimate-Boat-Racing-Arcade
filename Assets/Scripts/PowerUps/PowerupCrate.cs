using System;
using Boat.New;
using UnityEngine;

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
            var newBulletManagerScript = other.GetComponent<NewBulletManager>();
            if(!newBulletManagerScript) return;
            
            Debug.Log("Touché");
        }
    }
}
