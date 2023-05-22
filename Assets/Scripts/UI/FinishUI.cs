using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishUI : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private SetupGameScript setupGameScript;

    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private GameObject finishMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (config.GameMode == 1)
        {
            // IF CHRONO MODE
            continueButton.SetActive(false);
        }
        else
        {
            //IF RACE MODE
            restartButton.SetActive(false);
        }   
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Continue()
    {
        Resume();
        setupGameScript.CurrentLevelIndex++;
        setupGameScript.SetupLevel();
        setupGameScript.SetupGame();
    }

    public void Restart()
    {
        Resume();
        setupGameScript.SetupGame();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        finishMenu.SetActive(false);
    }
}
