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
        [SerializeField] private Transform playerMesh;
        
        private string _playerName;
        private PlayerConfiguration _playerConfiguration;
        private Logger _logger;
        
        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
        }
        
        public void InitializePlayer(PlayerConfiguration playerConfiguration, Transform spawner)
        {
            _playerConfiguration = playerConfiguration;
            _playerName = playerConfiguration.Name;
            lastCheckpoint = spawner;
            foreach (Transform childMesh in playerMesh)
            {
                childMesh.GetComponent<MeshRenderer>().material = playerConfiguration.PlayerMaterial;
            }
        }
        
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
                if (lastCheckpoint != null)
                {
                    transform.position = lastCheckpoint.position;
                    transform.rotation = lastCheckpoint.rotation;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
        public void Logger(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _logger.ToggleConsole();
            }
        }
    }
}