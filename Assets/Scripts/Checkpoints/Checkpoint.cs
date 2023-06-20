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

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(_checkpointManager.boats);
            if (_checkpointManager.boats.Contains(other.gameObject))
            {
                _checkpointManager.CheckPointPassed(ID, other.transform.parent.gameObject);
            }
        }
    
        public void SetCheckpointManager(CheckpointManager checkpointManager)
        {
            Debug.Log("Set checkpoint manager");
            _checkpointManager = checkpointManager;
        }
        
        public Dictionary<string, float> PlayerTimer
        {
            get => _playerTimer;
            set => _playerTimer = value;
        }
    }
}
