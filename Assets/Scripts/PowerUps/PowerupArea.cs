using System.Collections.Generic;
using UnityEngine;

namespace PowerUps
{
    public class PowerupArea : MonoBehaviour
    {
        public GameObject powerupPrefab;

        public int maxPowerups = 3;

        public float cooldown = 5;
        
        public float spaceBuffer = 5.0f;
    
        private Bounds bounds;
        
        private List<GameObject> powerups = new List<GameObject>();
        
        void Start()
        {
            bounds = transform.GetComponent<Collider>().bounds;
        
            for (int i = 0; i < maxPowerups; i++) RandomInstantiatePowerUp();
        
        }

        void Update()
        {
            if(cooldown > 0) cooldown -= Time.deltaTime;
            else if(powerups.Count < maxPowerups)
            {
                cooldown = 5;
            
                RandomInstantiatePowerUp();
            }
        }

        public bool CoordinatesAreValid(Vector3 coordinates)
        {
            foreach (var powerup in powerups)
                if (Vector3.Distance(powerup.transform.position, coordinates) < spaceBuffer) return false;

            return true;
        }

        public void RandomInstantiatePowerUp()
        {
            Vector3 position;
            do
            {
                float instanceX = Random.Range(
                    bounds.min.x,
                    bounds.max.x
                );
                float instanceY = Random.Range(
                    bounds.min.y,
                    bounds.max.y
                );
                float instanceZ = Random.Range(
                    bounds.min.z,
                    bounds.max.z
                );
                position = new Vector3(instanceX, instanceY, instanceZ);
            } while (!CoordinatesAreValid(position));

            InstantiatePowerUp(position);
        }
    
        public void InstantiatePowerUp(Vector3 position)
        {
            GameObject powerup = Instantiate(
                powerupPrefab, 
                position,
                Quaternion.identity
            );
            powerup.GetComponent<PowerupCrate>().SetArea(this);
            powerups.Add(powerup);
        }
    
        public void PowerUpCollected(GameObject powerup)
        {
            powerups.Remove(powerup);
        }
    }
}
