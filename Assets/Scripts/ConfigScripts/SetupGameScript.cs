using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    public bool debug;
    
    private SpawnScript _spawnScript;
    private int _currentLevelIndex;
    private GameObject _currentLevel;

    public int CurrentLevelIndex
    {
        get => _currentLevelIndex;
        set => _currentLevelIndex = value;
    }

    private void Start()
    {
        _currentLevelIndex = config.Level;
        SetupGame();
    }

    public void SetupLevel()
    {
        // TODO: Level setup
        
        // Spawn setup
        var spawner = GameObject.Find("Spawn");
        _spawnScript = spawner.GetComponent<SpawnScript>();
        _spawnScript.BoatsSetup();
    }

    public void SetupGame()
    {
        if(debug)Debug.Log("SetupGame");
        SetupLevel();
        _spawnScript.Spawn();
    }
}
