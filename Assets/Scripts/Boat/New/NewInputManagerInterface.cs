using System.Collections.Generic;
using Boat.New.Canon;
using Checkpoints;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public string playerName;
        public int score = 0;
        
        public NewBoatMovementManager newBoatMovementManager;
        public Rigidbody rigidbody;
        public BuildBoat buildBoat;

        public enum PlayerType
        {
            AI,
            Player
        }
        public void Respawn()
        {
            if (lastCheckpoint)
            {
                transform.position = lastCheckpoint.position;
                transform.rotation = lastCheckpoint.rotation;
                rigidbody.velocity = Vector3.zero;
            }
        }
        
        public struct StateStruct
        { 

            public bool IsBlinded;
            public bool IsSlowed;
            public bool IsFastened;

        }

        public StateStruct State;
        public PlayerType playerType;

        public Dictionary<BulletType, int> BulletInventory;
        public BulletType currentBulletType;
        public int currentBulletTypeInt;

        public PlayerUI globalPlayerUI;
        
        public CheckpointManager checkpointManager;

        public void Awake()
        {
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
                {BulletType.Explosive, 100},
                {BulletType.SmokeScreen, 0}
            };
            currentBulletType = BulletType.Basic;

            switchingMunition = 0;
            var playerManager = GameObject.Find("PlayerContainer");
            transform.SetParent(playerManager.transform);
        }
    }
}