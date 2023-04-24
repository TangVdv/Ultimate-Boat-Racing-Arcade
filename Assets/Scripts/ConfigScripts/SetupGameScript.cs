using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject fpsUI;
    [SerializeField] private StartUI startUI;
    [SerializeField] private GameObject chronoUI;
    [SerializeField] private ChronoScript chrono;
    [SerializeField] private SpawnScript spawner;

    private int _aiAmount = 0;
    private GameObject _playerBoat;

    private void Start()
    {
        _playerBoat = config.Boat;
        
        if (config.ShowFPS)
        {
            fpsUI.SetActive(true);
        }

        spawner.BoatsSetup();
        Setup();
    }

    public void Setup()
    {
        Debug.Log("SETUP");

        spawner.Spawn();
        switch (config.GameMode)
        {
            case 1:
                ChronoMode();
                break;
            case 0:
                RaceMode();
                break;
        }
    }

    private void ChronoMode()
    {
        Debug.Log("ChronoMode");
        chronoUI.SetActive(true);
        chrono.Reset();
        startUI.StartGame();
    }

    private void RaceMode()
    {
        Debug.Log("RaceMode");
        _aiAmount = config.AIAmount;
        startUI.StartGame();
    }
}
