using System;
using System.Collections;
using UnityEngine;

namespace Boat.New
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

        public void Start()
        {
            if (manager.State.Munitions > 0)
            {
                StartCoroutine(Reload());
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
            
            if(manager.wantsToFire)Fire();
        }

        public void LateUpdate()
        {
 
            Quaternion qt = Quaternion.Euler(0, _localRotation, 0);
            
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, qt, Time.deltaTime * orbitDampening);
            
        }

        public void Fire( /*Ajouter munitiontype en argument ?*/)
        {
            if (_isLoaded)
            {
                foreach (var canon in canons)
                {
                    canon.Fire();
                }

                _isLoaded = false;

            }else if (manager.State.Munitions > 0 && _isReloading == false)
            {
                StartCoroutine(Reload());
            }

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