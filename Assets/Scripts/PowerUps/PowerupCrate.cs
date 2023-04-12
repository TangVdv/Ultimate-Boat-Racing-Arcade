using UnityEngine;

namespace PowerUps
{
    public class PowerupCrate : MonoBehaviour
    {
        public PowerupArea powerupArea;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }
    
        public void SetArea(PowerupArea area)
        {
            powerupArea = area;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
