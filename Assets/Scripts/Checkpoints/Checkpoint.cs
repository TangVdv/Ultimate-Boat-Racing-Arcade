using UnityEngine;

namespace Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {

        private CheckpointManager _checkpointManager;
    
        public int ID;

        private void OnTriggerEnter(Collider other)
        {
            if (_checkpointManager.boats.Contains(other.gameObject)) _checkpointManager.CheckPointPassed(ID, other.gameObject);
        }
    
        public void SetCheckpointManager(CheckpointManager checkpointManager)
        {
            _checkpointManager = checkpointManager;
        }
    }
}
