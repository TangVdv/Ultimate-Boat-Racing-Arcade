using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RaceModeScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject raceModeUI;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject rankingTemplate;
    [SerializeField] private Text currentPosText;
    [SerializeField] private Text maxPosText;
    [SerializeField] private Text currentLapText;
    [SerializeField] private Text maxLapText;
    
    private int _index = 0;
    
    public void ResetRace()
    {
        if (!raceModeUI.activeInHierarchy)
        {
            raceModeUI.SetActive(true);
        }
        maxPosText.text = "/"+(config.AIAmount + config.PlayerAmount);
        currentLapText.text = "1";
        currentPosText.text = "1";
        ResetRanking();
    }

    public void ResetRanking()
    {
        foreach (Transform child in rankingPanel.transform)
        {
            Destroy(child.gameObject);
        }
        _index = 0;
    }

    public void InstantiateRanking(string playerName, string playerTimer, bool isPlayer)
    {
        GameObject currentTemplate = Instantiate(rankingTemplate, rankingPanel.transform);
        currentTemplate.transform.GetChild(0).GetComponent<Text>().text = playerName;
        Text timer = currentTemplate.transform.GetChild(1).GetComponent<Text>();
        if (isPlayer)
        {
            timer.color = new Color(0.34f,1f,0.43f);
        }
        else
        {
            timer.color = new Color(1f,0.46f,0.34f);
        }
        
        if (_index > 0) playerTimer = "+" + playerTimer;
        timer.text = playerTimer;
        _index++;
    }

    public void SetCurrentPosText(int value)
    {
        currentPosText.text = value.ToString();
    }

    public void SetCurrentLapText(int value)
    {
        currentLapText.text = value.ToString();
    }
    
    public void SetMaxLapText(int value)
    {
        maxLapText.text = "/"+value;
    }
}
