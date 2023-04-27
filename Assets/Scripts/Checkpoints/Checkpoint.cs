using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {

        private CheckpointManager _checkpointManager;
        private Dictionary<string, float> _playerTimer = new Dictionary<string, float>();

        public int ID;

        private void OnTriggerEnter(Collider other)
        {
            if (_checkpointManager.boats.Contains(other.gameObject)) _checkpointManager.CheckPointPassed(ID, other.gameObject);
        }
    
        public void SetCheckpointManager(CheckpointManager checkpointManager)
        {
            _checkpointManager = checkpointManager;
        }
        
        public Dictionary<string, float> PlayerTimer
        {
            get => _playerTimer;
            set => _playerTimer = value;
        }
    }
}
