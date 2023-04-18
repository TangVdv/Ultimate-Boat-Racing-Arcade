using UnityEngine;

namespace Boat.New
{
    public abstract class NewInputManagerInterface : MonoBehaviour
    {
        public float movementX;
        public float movementZ;
        public float movementCam;
        public float movementBarrels;
        public bool wantsToFire;
        public int switchingMunition;

        public struct StateStruct
        { 

            public bool IsBlinded;
            public bool IsSlowed;
            public bool IsFastened;
            public int Munitions;

        }

        public StateStruct State;

        public void Start()
        {
            State.IsBlinded = false;
            State.IsSlowed = false;
            State.IsFastened = false;
            State.Munitions = 10;
            
            
            movementX = 0;
            movementZ = 0;
            movementCam = 0;
            movementBarrels = 0;
            wantsToFire = false;

            switchingMunition = 0;
        }
    
    }
}