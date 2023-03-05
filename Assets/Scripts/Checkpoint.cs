using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{

    private CheckpointManager _checkpointManager;
    
    public int ID; 
    
    void OnTriggerEnter(Collider other)
    {
        if (Array.Exists(_checkpointManager.boats, boat => boat == other.gameObject)) _checkpointManager.CheckPointPassed(ID, other.gameObject);
    }
    
    public void SetCheckpointManager(CheckpointManager checkpointManager)
    {
       _checkpointManager = checkpointManager;
    }
}
