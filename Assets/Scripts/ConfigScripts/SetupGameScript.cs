using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public void SetupGame()
    {
        if(debug)Debug.Log("SetupGame");
        spawner.BoatsSetup();
        Reset();
    }

    public void Reset()
    {
        spawner.Spawn();
        checkpointManager.Setup();
    }
}
