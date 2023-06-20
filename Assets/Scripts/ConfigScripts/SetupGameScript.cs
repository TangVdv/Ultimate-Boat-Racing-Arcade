using System;
using System.Collections;
using System.Collections.Generic;
using Boat.New;
using Checkpoints;
using Terrain;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private SpawnScript spawner;
    [SerializeField] private CheckpointManager checkpointManager;
    [SerializeField] private GameObject water;
    
    public bool debug;

    private SpawnScript _spawnScript;
    private GameObject _currentLevel;
    
    public void SetupGame(List<GameObject> boats)
    {
        if(debug)Debug.Log("SetupGame");
        spawner.SpawnSetup(boats);
        WaveManager waveManager = water.GetComponent<WaveManager>();
        if (!waveManager)
        {
            if(debug)Debug.Log("Add Wave Manager");
            waveManager = water.AddComponent<WaveManager>();
            if(debug)Debug.Log(waveManager);
        }
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
