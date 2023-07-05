using System;
using System.Collections.Generic;
using System.Transactions;
using Boat.New.Canon;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Boat.New
{
    public class NewPlayerInputManager : NewInputManagerInterface
    {
        [SerializeField] private ConfigScript config;
        [SerializeField] private RaceModeScript raceModeScript;
        [SerializeField] private ChronoScript chronoScript;
        [SerializeField] private PlayerUI playerUI;
        private PlayerInput _playerInput;
        [SerializeField] private List<Camera> cameras;
        [SerializeField] private int currentCamera;

        public PlayerUI PlayerUI
        {
            get => playerUI;
        }

        public bool debug;

        private Logger _logger;
        
        public void InitializePlayer(PlayerConfiguration playerConfiguration)
        {
            if (debug) Debug.Log("Initialize");
            _playerInput = this.gameObject.GetComponent<PlayerInput>();
            currentCamera = 0;
            cameras = new List<Camera>();
            cameras.Add(this.gameObject.GetComponentInChildren<Camera>());
            buildBoat.Initiate(
                playerConfiguration.PlayerBoat, 
                playerConfiguration.PlayerCannon,
                playerConfiguration.PlayerBoatMaterial,
                playerConfiguration.PlayerCannonMaterial,
                cameras);
            globalPlayerUI = playerUI;
            
            playerType = PlayerType.Player;
            playerName = playerConfiguration.Name;
            if(BulletInventory != null) playerUI.HotbarManager(BulletInventory);
            _playerInput.camera = cameras[currentCamera];
            
            _logger = FindObjectOfType<Logger>();
            ResetPlayerProgress();
        }

        public void ResetPlayerProgress()
        {
            if (config.GameMode == 0)
            {
                if(debug)Debug.Log("RaceMode");
                raceModeScript.ResetRace();   
            }
            else if (config.GameMode == 1)
            {
                if(debug)Debug.Log("ChronoMode");
                chronoScript.ResetChrono();   
            }
            else
                if(debug)Debug.Log("No GameMode found, couldn't reset");
        }
        
        // INPUTS
        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            movementX = value.x;
            movementZ = value.y;
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            wantsToFire = context.ReadValue<float>() > 0 ? true : false;
            playerUI.UpdateBulletAmount(BulletInventory[currentBulletType]);
        }
        public void OnCamera(InputAction.CallbackContext context)
        {
            movementCam = context.ReadValue<float>();
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            movementBarrels = context.ReadValue<float>();
        }
        public void OnSwitchAmmo(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                int currentBulletTypeInt = (int) currentBulletType;
                currentBulletTypeInt += (int)context.ReadValue<float>();
                currentBulletTypeInt %= BulletInventory.Count;
                if (currentBulletTypeInt < 0) currentBulletTypeInt = BulletInventory.Count - 1;

                currentBulletType = (BulletType) currentBulletTypeInt;
                playerUI.BulletSelection(currentBulletTypeInt);
            }
        }
        public void Respawn(InputAction.CallbackContext context)
        {
            if (context.performed) Respawn();
        }
        public void Logger(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _logger.ToggleConsole();
            }
        }
        public void OnChangeCamera(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                cameras[currentCamera].enabled = false;
                
                currentCamera += (int)context.ReadValue<float>();

                if (currentCamera < 0) currentCamera += cameras.Count;
                if (currentCamera >= cameras.Count) currentCamera %= cameras.Count;

                _playerInput.camera = cameras[currentCamera];
                cameras[currentCamera].enabled = true;
            }
        }
    }
}