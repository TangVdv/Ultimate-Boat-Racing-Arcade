using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {

        private CheckpointManager _checkpointManager;
        private Dictionary<string, float> _playerTimer = new Dictionary<string, float>();
        public GameObject core;

        public int ID;
        public bool isAlternative = false;
        public bool debug = false;

        private void OnTriggerEnter(Collider other)
        {
            if(debug) Debug.Log(_checkpointManager.boats);
            if (_checkpointManager.boats.Contains(other.gameObject))
            {
                if(debug) Debug.Log("Checkpoint passed");
                _checkpointManager.CheckPointPassed(ID, other.transform.parent.gameObject);
            }
        }
    
        public void SetCheckpointManager(CheckpointManager checkpointManager)
        {
            if(debug) Debug.Log("Set checkpoint manager");
            _checkpointManager = checkpointManager;
        }
        
        public Dictionary<string, float> PlayerTimer
        {
            get => _playerTimer;
            set => _playerTimer = value;
        }
    }
}
