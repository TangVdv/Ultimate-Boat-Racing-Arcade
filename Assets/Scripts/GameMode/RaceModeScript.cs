using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RaceModeScript : TimerScript
{
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject rankingTemplate;
    [SerializeField] private Text currentPosText;
    [SerializeField] private Text maxPosText;
    [SerializeField] private Text currentLapText;
    [SerializeField] private Text maxLapText;

    private float _startPosY = 45f;
    private float _spacingY = 17.5f;
    private int _index = 0;

    public void ResetRanking()
    {
        currentLapText.text = "0";
        currentPosText.text = "1";
        if (_index > 0)
        {
            foreach (Transform child in rankingPanel.transform)
            {
                Destroy(child.gameObject);
            }

            _index = 0;
        }
    }

    public void InstantiateRanking(string playerName, float playerTimer, bool isPlayer)
    {
        Vector3 position = new Vector3(0f, _startPosY - _index * _spacingY, 0f);
        GameObject currentTemplate = Instantiate(rankingTemplate, rankingPanel.transform);
        currentTemplate.transform.GetChild(0).GetComponent<Text>().text = playerName;
        Text timer = currentTemplate.transform.GetChild(1).GetComponent<Text>();
        if (isPlayer)
        {
            timer.color = new Color(0.34f,0.84f,1f);
        }
        else
        {
            timer.color = new Color(0.34f,1f,0.43f);
        }

        string timerStr = ConvertTimerToString(playerTimer);
        if (_index > 0) timerStr = "+" + timerStr;
        timer.text = timerStr;
        currentTemplate.transform.localPosition = position;
        _index++;
    }
    
    public Text CurrentPosText
    {
        get => currentPosText;
        set => currentPosText = value;
    }

    public Text MaxPosText
    {
        get => maxPosText;
        set => maxPosText = value;
    }

    public Text CurrentLapText
    {
        get => currentLapText;
        set => currentLapText = value;
    }

    public Text MaxLapText
    {
        get => maxLapText;
        set => maxLapText = value;
    }
}
