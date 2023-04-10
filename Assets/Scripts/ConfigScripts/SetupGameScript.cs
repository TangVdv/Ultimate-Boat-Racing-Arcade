using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject fpsUI;
    [SerializeField] private GameObject startUI;

    private int _aiAmount = 0;
    private GameObject _playerBoat;

    private void Awake()
    {
        _playerBoat = config.Boat;
        
        if (config.ShowFPS)
        {
            fpsUI.SetActive(true);
        }

        switch (config.GameMode)
        {
            case 0:
                ChronoMode();
                break;
            case 1:
                RaceMode();
                break;
            default:
                RaceMode();
                break;
        }
    }

    private void ChronoMode()
    {
        Debug.Log("ChronoMode");
        StartUI();
    }

    private void RaceMode()
    {
        Debug.Log("RaceMode");
        _aiAmount = config.AIAmount;
        //SPAWN AI
        StartUI();
    }

    private void StartUI()
    {
        startUI.SetActive(true);
    }
}
