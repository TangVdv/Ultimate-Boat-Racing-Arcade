using System.Collections;
using System.Collections.Generic;
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
    
    public GameObject FinishUIPanel
    {
        get => finishUIPanel;
    }
    
    private void Start()
    {
        if (config.GameMode == 1)
        {
            // IF CHRONO MODE
            chronoInfoPanel.SetActive(true);
            
        }
        else
        {
            //IF RACE MODE
            scoreBoardPanel.SetActive(true);
        }   
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
    }

    public void NextMap()
    {
        config.Level++;
        setupLevelScript.SetupLevel();
        finishUIPanel.SetActive(false);
    }
}
