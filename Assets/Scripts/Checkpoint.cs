using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{

    private CheckpointManager checkpointManager;
    
    public int ID; 
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") checkpointManager.CheckPointPassed(ID, other.gameObject);
    }
    
    public void SetCheckpointManager(CheckpointManager checkpointManager)
    {
        this.checkpointManager = checkpointManager;
    }
}
