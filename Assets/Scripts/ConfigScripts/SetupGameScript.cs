using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameScript : MonoBehaviour
{
    [SerializeField] private Logger logger;
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject fpsUI;
    [SerializeField] private GameObject startUI;
    [SerializeField] private PlayerUI playerUI;

    private int _aiAmount = 0;
    private GameObject _playerBoat;

    private void Awake()
    {
        logger.Log("SETUP", "", LogType.Log);

        _playerBoat = config.Boat;
        
        if (config.ShowFPS)
        {
            fpsUI.SetActive(true);
        }

        switch (config.GameMode)
        {
            case 0:
                ChronoMode();
                logger.Log("ChronoMode", "", LogType.Log);
                break;
            case 1:
                RaceMode();
                logger.Log("RaceMode", "", LogType.Log);
                break;
            default:
                RaceMode();
                logger.Log("RaceMode", "", LogType.Log);
                break;
        }
    }

    private void ChronoMode()
    {
        Debug.Log("ChronoMode");
        playerUI.ChronoModeUI.SetActive(true);
        StartUI();
    }

    private void RaceMode()
    {
        Debug.Log("RaceMode");
        _aiAmount = config.AIAmount;
        StartUI();
    }

    private void StartUI()
    {
        StartCoroutine(playerUI.UpdateTimer());
        //startUI.SetActive(true);
    }
}
