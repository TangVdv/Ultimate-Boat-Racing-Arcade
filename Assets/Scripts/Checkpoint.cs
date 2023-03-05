using System;
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
        if (Array.Exists(checkpointManager.boats, boat => boat == other.gameObject)) checkpointManager.CheckPointPassed(ID, other.gameObject);
    }
    
    public void SetCheckpointManager(CheckpointManager checkpointManager)
    {
        this.checkpointManager = checkpointManager;
    }
}
