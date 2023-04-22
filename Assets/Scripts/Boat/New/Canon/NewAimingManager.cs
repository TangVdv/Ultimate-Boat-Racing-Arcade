using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boat.New.Canon
{
    public class NewAimingManager: MonoBehaviour
    {

        public NewInputManagerInterface manager;
        public NewFiringManager[] canons;
        public GameObject pivot;
        
        public float mouseSensitivity = 3.0f;
        public float scrollSensitivity = 40;
        public float orbitDampening = 10f;
        public float topClamp = 25.0f;

        private float _localRotation;
        private float _scrollAmount;
        
        
        public float reloadTime = 5f;
        
        private bool _isReloading;
        private bool _isLoaded;
        
        private Dictionary<BulletType, int> bulletInventory = new Dictionary<BulletType, int>()
        {
            {BulletType.Basic, 2000000},
            {BulletType.Explosive, 0},
            {BulletType.SmokeScreen, 0}
        };
        private BulletType currentBulletType = BulletType.Basic;

        public void Start()
        {
            if (manager.State.Munitions > 0)
            {
                StartCoroutine(Reload());
            }

            foreach (var canon in canons)
            {
                canon.aimingManager = this;
            }
        }

        public void Update()
        {
            if (manager.movementCam != 0)
            {
                _localRotation += manager.movementCam * mouseSensitivity;
            }

            foreach (var canon in canons)
            {
                Quaternion qt = Quaternion.Euler(0, _localRotation, 0);
                canon.body.transform.localRotation =  Quaternion.Lerp(canon.body.transform.localRotation, qt, Time.deltaTime * orbitDampening);

                if (manager.movementBarrels != 0)
                {
                    _scrollAmount += manager.movementBarrels * scrollSensitivity;
                    _scrollAmount = Mathf.Clamp(_scrollAmount, 0, topClamp);
                    
                    qt = Quaternion.Euler(_scrollAmount, 0, 0);
                    canon.barrels.transform.localRotation = Quaternion.Lerp(canon.barrels.transform.localRotation, qt, Time.deltaTime * orbitDampening);
                }
            }

            if (manager.wantsToFire)
            {
                if (_isLoaded)
                {
                    Fire();
                }else if (manager.State.Munitions > 0 && _isReloading == false)
                {
                    StartCoroutine(Reload());
                }
            }

            if(manager.switchingMunition != 0)SwitchMunition();
        }

        public void AddRandomMunition()
        {
            bulletInventory[(BulletType) Random.Range(1, bulletInventory.Count-1)] += 1;
            foreach (var bulletType in bulletInventory)
            {
                Debug.Log(bulletType.Key + " : " + bulletType.Value);
            }
        }

        public void SwitchMunition()
        {
            //TODO: Sauter les munitions vides
            int currentBulletTypeInt = (int) currentBulletType;
            currentBulletTypeInt += manager.switchingMunition;
            if (currentBulletTypeInt > bulletInventory.Count - 1) currentBulletTypeInt = 1;
            else if (currentBulletTypeInt < 0) currentBulletTypeInt = bulletInventory.Count - 1; 
                
            currentBulletType = (BulletType) currentBulletTypeInt;
            Debug.Log("Current munition : " + currentBulletType);
        }

        public void LateUpdate()
        {
 
            Quaternion qt = Quaternion.Euler(0, _localRotation, 0);
            
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, qt, Time.deltaTime * orbitDampening);
            
        }

        public void Fire()
        {

                if(!bulletInventory.ContainsKey(currentBulletType)) return;
                if(bulletInventory[currentBulletType] <= 0) return;
                
                foreach (var canon in canons)
                {
                    canon.Fire(currentBulletType);
                }

                _isLoaded = false;
                bulletInventory[currentBulletType]--;


        }
        
        IEnumerator Reload()
        {
            print("isReloading");
            _isReloading = true;
            yield return new WaitForSeconds(reloadTime);
            manager.State.Munitions--;
            _isLoaded = true;
            print("isLoaded");
            print("Ammo left : " + manager.State.Munitions);
            _isReloading = false;
        }
    }
}