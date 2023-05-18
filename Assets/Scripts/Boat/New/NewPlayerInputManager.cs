using System;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Boat.New
{
    public class NewPlayerInputManager : NewInputManagerInterface
    {
        [SerializeField] private ConfigScript config;
        [SerializeField] private Transform playerMesh;
        [SerializeField] private RaceModeScript raceModeScript;
        [SerializeField] private ChronoScript chronoScript;
        [SerializeField] private PlayerUI playerUI;

        public bool debug;
        
        private string _playerName;
        private Logger _logger;
        private SetupGameScript _setupGameScript;

        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
            _setupGameScript = FindObjectOfType<SetupGameScript>();
            StartGame();
        }
        
        public void InitializePlayer(PlayerConfiguration playerConfiguration, Transform spawner)
        {
            _playerName = playerConfiguration.Name;
            lastCheckpoint = spawner;
            foreach (Transform childMesh in playerMesh)
            {
                childMesh.GetComponent<MeshRenderer>().material = playerConfiguration.PlayerMaterial;
            }
        }

        public void ResetPlayerProgress()
        {
            if (config.GameMode == 1)
            {
                if(debug)Debug.Log("RaceMode");
                raceModeScript.ResetRace();   
            }
            else if (config.GameMode == 0)
            {
                if(debug)Debug.Log("ChronoMode");
                chronoScript.ResetChrono();   
            }
            else
                if(debug)Debug.Log("No GameMode found, couldn't reset");
        }

        private void StartGame()
        {
            lastCheckpoint = GameObject.Find("Spawn").transform;
            Respawn();
            ResetPlayerProgress();
            _setupGameScript.SetupGame();
        }

        private void Respawn()
        {
            if (lastCheckpoint != null)
            {
                transform.position = lastCheckpoint.position;
                transform.rotation = lastCheckpoint.rotation;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
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
        }
        public void OnCamera(InputAction.CallbackContext context)
        {
            Debug.Log(context.ReadValue<float>());
            movementCam = context.ReadValue<float>();
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            movementBarrels = context.ReadValue<float>();
        }
        public void OnSwitchAmmo(InputAction.CallbackContext context)
        {
            switchingMunition = (int)context.ReadValue<float>();
        }
        public void Respawn(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Respawn();
            }
        }
        public void Logger(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _logger.ToggleConsole();
            }
        }
        public void StartGame(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartGame();
            }
        }
    }
}