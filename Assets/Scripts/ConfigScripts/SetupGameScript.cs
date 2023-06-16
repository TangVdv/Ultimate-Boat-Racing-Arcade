using System;
using System.Collections;
using System.Collections.Generic;
using Boat.New;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private SpawnScript spawner;
    [SerializeField] private CheckpointManager checkpointManager;
    
    public bool debug;

    private SpawnScript _spawnScript;
    private GameObject _currentLevel;
    
    public void SetupGame(List<GameObject> boats)
    {
        if(debug)Debug.Log("SetupGame");
        spawner.SpawnSetup(boats);
        foreach (var boat in boats)
        {
            if(debug)Debug.Log(boat);
            checkpointManager.AddPlayer(boat);   
        }
        spawner.Spawn();
    }

    public void Reset()
    {
        spawner.Spawn();
        checkpointManager.Setup();
    }
}
