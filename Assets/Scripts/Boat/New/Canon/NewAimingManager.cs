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

        public bool debug = false;
        public float reloadTime = 5f;
        
        private bool _isReloading;
        private bool _isLoaded;

        private Dictionary<BulletType, int> _bulletInventory;



        public void Start()
        {
            
            _bulletInventory = manager.BulletInventory;
            
            if (_bulletInventory[manager.currentBulletType] > 0)
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

            if (_isLoaded)
            {
                if (manager.wantsToFire)
                {
                    Fire();
                }
            }
            else if (_bulletInventory[manager.currentBulletType] > 0 && _isReloading == false)
            {
                    StartCoroutine(Reload());
            }
            

            if(manager.switchingMunition != 0)SwitchMunition();
        }

        public void AddRandomMunition()
        {
            _bulletInventory[(BulletType) Random.Range(1, _bulletInventory.Count)] += 1;
            foreach (var bulletType in _bulletInventory)
            {
                Debug.Log(bulletType.Key + " : " + bulletType.Value);
            }
        }

        public void SwitchMunition()
        {
            int currentBulletTypeInt = (int) manager.currentBulletType;
            currentBulletTypeInt += manager.switchingMunition;
            currentBulletTypeInt %= _bulletInventory.Count;
            if (currentBulletTypeInt < 0) currentBulletTypeInt = _bulletInventory.Count - 1; 
            
            manager.currentBulletType = (BulletType) currentBulletTypeInt;
            if(debug) Debug.Log("Current munition : " + manager.currentBulletType);
        }

        public void LateUpdate()
        {
 
            Quaternion qt = Quaternion.Euler(0, _localRotation, 0);
            
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, qt, Time.deltaTime * orbitDampening);
            
        }

        public void Fire()
        {

                if(!_bulletInventory.ContainsKey(manager.currentBulletType)) return;
                if(_bulletInventory[manager.currentBulletType] <= 0) return;
                
                foreach (var canon in canons)
                {
                    canon.Fire(manager.currentBulletType);
                }

                _isLoaded = false;
                _bulletInventory[manager.currentBulletType]--;


        }
        
        IEnumerator Reload()
        {
            if(debug) print("isReloading");
            _isReloading = true;
            yield return new WaitForSeconds(reloadTime);
            _isLoaded = true;
            if(debug) print("isLoaded");
            _isReloading = false;
        }
    }
}