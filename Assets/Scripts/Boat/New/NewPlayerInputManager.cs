using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Boat.New
{
    public class NewPlayerInputManager : NewInputManagerInterface
    {
        public enum ControlScheme{
            BasicZqsd,
        }

        public ControlScheme controlScheme;

        private KeyCode _forward;
        private KeyCode _backward;
        private KeyCode _left;
        private KeyCode _right;
        private int _fireClic;
        private KeyCode _changeWeaponLeft;
        private KeyCode _changeWeaponRight;
        private string _cameraAxis;
        private string _aimAxis;

        public new void Start()
        {
            base.Start();
            switch (controlScheme)
            {
                case(ControlScheme.BasicZqsd):
                    _forward = KeyCode.Z;
                    _backward = KeyCode.S;
                    _left = KeyCode.Q;
                    _right = KeyCode.D;
                    _fireClic = 0;
                    _changeWeaponLeft = KeyCode.A;
                    _changeWeaponRight = KeyCode.E;
                    _cameraAxis = "Mouse X";
                    _aimAxis = "Mouse ScrollWheel";
                    break;
                default:
                    break;
            }
        }

        public void Update()
        {
            movementZ = 0;
            movementX = 0;
        
            movementZ += Input.GetKey(_forward) ? 1 : 0;
            movementZ -= Input.GetKey(_backward) ? 1 : 0;
            movementX -= Input.GetKey(_left) ? 1 : 0;
            movementX += Input.GetKey(_right) ? 1 : 0;

            movementCam = Input.GetAxis(_cameraAxis);
            movementBarrels = Input.GetAxis(_aimAxis);

            wantsToFire = Input.GetMouseButtonDown(_fireClic);
            switchingMunition = 0;
            switchingMunition += Input.GetKeyDown(_changeWeaponLeft) ? -1 : 0;
            switchingMunition += Input.GetKeyDown(_changeWeaponRight) ? 1 : 0;
            
            
        }
    }
}