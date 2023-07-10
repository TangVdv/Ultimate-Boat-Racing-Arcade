using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [SerializeField] private CheatUIScript cheatUI;

        public PlayerUI PlayerUI
        {
            get => playerUI;
        }

        public CheatUIScript CheatUI
        {
            get => cheatUI;
        }

        public bool debug;

        private Logger _logger;
        private bool _frozen = false;

        public bool Frozen
        {
            set => _frozen = value;
        }

        public void InitializePlayer(PlayerConfiguration playerConfiguration)
        {
            if (debug) Debug.Log("Initialize");
            buildBoat.Initiate(
                playerConfiguration.PlayerBoat, 
                playerConfiguration.PlayerCannon,
                playerConfiguration.PlayerBoatMaterial,
                playerConfiguration.PlayerCannonMaterial);
            globalPlayerUI = playerUI;
            playerType = PlayerType.Player;
            playerName = playerConfiguration.Name;
            if(BulletInventory != null) playerUI.HotbarManager(BulletInventory);
            cheatUI.SetPlayer(this);
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
            if (!_frozen)
            {
                Vector2 value = context.ReadValue<Vector2>();
                movementX = value.x;
                movementZ = value.y;   
            }
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            if (!_frozen)
            {
                wantsToFire = context.ReadValue<float>() > 0 ? true : false;
                playerUI.DecreaseBulletAmount(BulletInventory[currentBulletType]);   
            }
        }
        public void OnCamera(InputAction.CallbackContext context)
        {
            if (!_frozen)
            {
                movementCam = context.ReadValue<Vector2>().x;   
            }
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            if (!_frozen)
            {
                movementBarrels = context.ReadValue<float>();   
            }
        }
        public void OnSwitchAmmo(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (!_frozen)
                {
                    int currentBulletTypeInt = (int) currentBulletType;
                    currentBulletTypeInt += (int)context.ReadValue<float>();
                    currentBulletTypeInt %= BulletInventory.Count;
                    if (currentBulletTypeInt < 0) currentBulletTypeInt = BulletInventory.Count - 1;

                    currentBulletType = (BulletType) currentBulletTypeInt;
                    playerUI.BulletSelection(currentBulletTypeInt);   
                }
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

        public void CheatMode(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (cheatUI.isActiveAndEnabled)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                    _frozen = false;
                    cheatUI.gameObject.SetActive(false);
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    _frozen = true;
                    cheatUI.gameObject.SetActive(true);
                }
            }
        }
    }
}