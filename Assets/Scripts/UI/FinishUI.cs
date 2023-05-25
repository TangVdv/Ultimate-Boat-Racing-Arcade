using System.Collections;
using System.Collections.Generic;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishUI : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private SetupLevelScript setupLevelScript;
    
    [SerializeField] private GameObject playerScoreTemplate;
    [SerializeField] private GameObject checkpointInfoTemplate;
    [SerializeField] private GameObject finishUIPanel;

    [SerializeField] private GameObject scoreBoardPanel;
    [SerializeField] private GameObject scorePanel;

    [SerializeField] private GameObject chronoInfoPanel;
    [SerializeField] private Text currentTimerText;
    [SerializeField] private Image timerDiffPanel;
    [SerializeField] private Text timerDiffText;
    [SerializeField] private GameObject checkpointPanel;

    [SerializeField] private GameObject finalScoreboardPanel;
    [SerializeField] private GameObject playerFinalScoreTemplate;
    [SerializeField] private GameObject playerFinalScorePanel;
    
    [SerializeField] private Text firstPlayerNameText;
    [SerializeField] private Text firstPlayerScoreText;
    
    [SerializeField] private Text secondPlayerNameText;
    [SerializeField] private Text secondPlayerScoreText;
    
    [SerializeField] private Text thirdPlayerNameText;
    [SerializeField] private Text thirdPlayerScoreText;
    
    public GameObject ScoreBoardPanel
    {
        get => scoreBoardPanel;
    }

    public GameObject ChronoInfoPanel
    {
        get => chronoInfoPanel;
    }
    
    public GameObject FinalScoreboardPanel
    {
        get => finalScoreboardPanel;
    }
    
    public GameObject FinishUIPanel
    {
        get => finishUIPanel;
    }

    public void ClearPlayerScoreboard()
    {
        foreach (Transform child in scorePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void ClearCheckpointInfo()
    {
        foreach (Transform child in checkpointPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void InstantiatePlayerScore(int pos, string name, string timer, int score, bool isPlayer)
    {
        GameObject currentTemplate = Instantiate(playerScoreTemplate, scorePanel.transform);
        currentTemplate.transform.GetChild(0).GetComponent<Text>().text = pos.ToString();
        currentTemplate.transform.GetChild(1).GetComponent<Text>().text = name;
        currentTemplate.transform.GetChild(2).GetComponent<Text>().text = timer;
        currentTemplate.transform.GetChild(3).GetComponent<Text>().text = score.ToString();
        var image = currentTemplate.GetComponent<Image>();
        if (isPlayer)
        {
            image.color = new Color(0.34f,1f,0.43f);
        }
        else
        {
            image.color = new Color(1f,0.46f,0.34f);
        }
    }

    public void InstantiateCheckpointInfo(int id, string time, string timerDiff, Color timerDiffColor)
    {
        GameObject currentTemplate = Instantiate(checkpointInfoTemplate, checkpointPanel.transform);
        currentTemplate.transform.GetChild(0).GetComponent<Text>().text = id.ToString();
        currentTemplate.transform.GetChild(1).GetComponent<Text>().text = time;
        var diffText = currentTemplate.transform.GetChild(2).GetComponent<Text>();
        if (config.CheckpointTimes[config.Level] == null)
        {
            timerDiff = "00:00:000";
            timerDiffColor = new Color(.7f, .7f, .7f, .4f);
        }
        diffText.text = timerDiff;
        diffText.color = timerDiffColor;
    }

    public void SetChronoInfo(string currentTimer, string timerDiff, Color timerDiffColor)
    {
        currentTimerText.text = currentTimer;
        if (config.CheckpointTimes.Length > 0)
        {
            if (config.CheckpointTimes[config.Level] != null)
            {
                timerDiffText.text = timerDiff;
                timerDiffPanel.color = timerDiffColor;
            }
            else
            {
                this.timerDiffText.text = "";
                timerDiffPanel.color = new Color(0f, 0f, 0f, 0f);
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        setupLevelScript.ResetCurrentMap();
        finishUIPanel.SetActive(false);
        chronoInfoPanel.SetActive(false);
    }

    public void NextMap()
    {
        config.Level++;
        setupLevelScript.SetupLevel();
        finishUIPanel.SetActive(false);
        scoreBoardPanel.SetActive(false);
    }

    public void SetTop3Scoreboard(int pos, string name, int score)
    {
        switch (pos)
        {
            case 0:
                firstPlayerNameText.text = name;
                firstPlayerScoreText.text = score.ToString();
                break;
            case 1:
                secondPlayerNameText.text = name;
                secondPlayerScoreText.text = score.ToString();
                break;
            case 2:
                thirdPlayerNameText.text = name;
                thirdPlayerScoreText.text = score.ToString();
                break;
        }
    }

    public void ClearFinalScoreboard()
    {
        foreach (Transform child in playerFinalScorePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void InstantiateFinalScoreboard(int pos, string name, int score)
    {
        GameObject currentTemplate = Instantiate(playerFinalScoreTemplate, playerFinalScorePanel.transform);
        currentTemplate.transform.GetChild(0).GetComponent<Text>().text = pos.ToString();
        currentTemplate.transform.GetChild(1).GetComponent<Text>().text = name;
        currentTemplate.transform.GetChild(3).GetComponent<Text>().text = score.ToString();
    }

}
