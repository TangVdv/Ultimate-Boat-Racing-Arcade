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

    private int _aiAmount = 0;
    private GameObject _playerBoat;
    private SpawnScript _spawnScript;

    private void Start()
    {
        levelManagers.GetChild(config.Level).gameObject.SetActive(true);
        _playerBoat = config.Boat;
        
        if (config.ShowFPS)
        {
            fpsUI.SetActive(true);
        }

        foreach (var spawner in spawners)
        {
            if (spawner != null && spawner.activeInHierarchy)
            {
                _spawnScript = spawner.GetComponent<SpawnScript>();
            }   
        }
        Setup();
    }

    public void Setup()
    {
        Debug.Log("SETUP");
        _spawnScript.BoatsSetup();
        switch (config.GameMode)
        {
            case 1:
                Debug.Log("ChronoMode");
                chronoUI.SetActive(true);
                break;
            case 0:
                Debug.Log("RaceMode");
                raceUI.SetActive(true);
                _aiAmount = config.AIAmount;
                break;
        }
        ResetGame();
    }

    public void ResetGame()
    {
        if (chrono != null && chrono.isActiveAndEnabled)
        {
            chrono.Reset();
        }
        else if (race != null && race.isActiveAndEnabled)
        {
            race.MaxPosText.text = "/"+(config.AIAmount + config.PlayerAmount);
            race.ResetRanking();
        }
        else
        {
            Debug.Log("No GameMode found, couldn't reset");
        }
        _spawnScript.Spawn();
        startUI.StartGame();
    }
}
