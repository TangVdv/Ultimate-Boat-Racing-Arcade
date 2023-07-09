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
using Cursor = UnityEngine.Cursor;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private SpawnScript spawner;
    [SerializeField] private CheckpointManager checkpointManager;

    public bool debug;

    private SpawnScript _spawnScript;
    private GameObject _currentLevel;

    public void SetSpawner(SpawnScript spawnScript)
    {
        spawner = spawnScript;
    }

    public void SetCheckpointManager(CheckpointManager checkpointManager)
    {
        this.checkpointManager = checkpointManager;
    }
    
    public void SetupGame(List<GameObject> boats)
    {
        if(debug)Debug.Log("SetupGame");
        spawner.SpawnSetup(boats);
        foreach (var boat in boats)
        {
            if(debug)Debug.Log(boat);
            checkpointManager.AddPlayer(boat);
            boat.GetComponent<NewInputManagerInterface>().Respawn();
        }
        spawner.Spawn();
        DisableMouse();
    }

    public void Reset()
    {
        if(spawner) spawner.Spawn();
        if(checkpointManager) checkpointManager.Setup();
        DisableMouse();
    }

    private void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
