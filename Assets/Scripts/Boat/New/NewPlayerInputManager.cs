using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Boat.New
{
    public class NewPlayerInputManager : NewInputManagerInterface
    {
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
    }
}