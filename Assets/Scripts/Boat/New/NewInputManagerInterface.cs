using System.Collections.Generic;
using Boat.New.Canon;
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
        public Transform lastCheckpoint;

        public struct StateStruct
        { 

            public bool IsBlinded;
            public bool IsSlowed;
            public bool IsFastened;

        }

        public StateStruct State;

        public Dictionary<BulletType, int> BulletInventory;
        public BulletType currentBulletType;

        public void Start()
        {
            lastCheckpoint = GameObject.Find("Spawn").transform;
            State.IsBlinded = false;
            State.IsSlowed = false;
            State.IsFastened = false;
            
            
            movementX = 0;
            movementZ = 0;
            movementCam = 0;
            movementBarrels = 0;
            wantsToFire = false;
            BulletInventory = new Dictionary<BulletType, int>()
            {
                {BulletType.Basic, 2000000},
                {BulletType.Explosive, 0},
                {BulletType.SmokeScreen, 0}
            };
            currentBulletType = BulletType.Basic;

            switchingMunition = 0;
            var playerManager = GameObject.Find("PlayerContainer");
            transform.SetParent(playerManager.transform);
            playerManager.GetComponent<PlayerManager>().AddPlayer(gameObject);
        }
    }
}