using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject fpsUI;
    [SerializeField] private StartUI startUI;
    [SerializeField] private GameObject chronoUI;
    [SerializeField] private GameObject raceUI;
    [SerializeField] private ChronoScript chrono;
    [SerializeField] private RaceModeScript race;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private Transform levelManagers;
    public bool debug = false;

    private int _aiAmount = 0;
    private GameObject _playerBoat;
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
        
        _playerBoat = config.Boat;
        
        if (config.ShowFPS)
        {
            fpsUI.SetActive(true);
        }
        SetupGameMode();
    }

    public void SetupLevel()
    {
        _currentLevel = levelManagers.GetChild(_currentLevelIndex).gameObject;
        ActivateLevel(true);
        foreach (var spawner in spawners)
        {
            if (spawner != null && spawner.activeInHierarchy)
            {
                _spawnScript = spawner.GetComponent<SpawnScript>();
            }   
        }
        _spawnScript.BoatsSetup();
    }

    public void ActivateLevel(bool active)
    {
        _currentLevel.SetActive(active);
    }

    public void SetupGameMode()
    {
        if(debug)Debug.Log("SETUP");
        switch (config.GameMode)
        {
            case 1:
                if(debug)Debug.Log("ChronoMode");
                chronoUI.SetActive(true);
                break;
            case 0:
                if(debug)Debug.Log("RaceMode");
                raceUI.SetActive(true);
                _aiAmount = config.AIAmount;
                break;
        }
        ResetGame();
    }

    public void ResetGame()
    {
        SetupLevel();
        _spawnScript.Spawn();
        if (chrono != null && chrono.isActiveAndEnabled)
        {
            chrono.ResetChrono();
            chrono.StartTimer();
        }
        else if (race != null && race.isActiveAndEnabled)
        {
            race.MaxPosText.text = "/"+(config.AIAmount + config.PlayerAmount);
            race.ResetTimer();
            race.ResetRanking();
            race.StartTimer();
        }
        else
        {
            if(debug)Debug.Log("No GameMode found, couldn't reset");
        }
    }
}
