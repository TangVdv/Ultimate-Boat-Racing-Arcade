using UnityEngine;

namespace Boat.Legacy
{
    public class PlayerInputManager : InputManagerInterface
    {
        KeyCode _forward = KeyCode.Z;
        KeyCode _backward = KeyCode.S;
        KeyCode _left = KeyCode.Q;
        KeyCode _right = KeyCode.D;
    
        //TODO: Fournir les paramètres gauche, droite,... sur start
        void Start()
        {
        
        }

        // Update is called once per frame
        protected override void setMovement()
        {
            directionZ = 0;
            directionX = 0;
        
            directionZ += (short) (Input.GetKey(_forward) ? 1 : 0);
            directionZ -= (short) (Input.GetKey(_backward) ? 1 : 0);
            directionX -= (short) (Input.GetKey(_left) ? 1 : 0);
            directionX += (short) (Input.GetKey(_right) ? 1 : 0);
        }
    }
}
