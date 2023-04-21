using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject playerUI;

    private bool _isGamePaused = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }    
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        playerUI.SetActive(true);
        
        Time.timeScale = 1f;
        _isGamePaused = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        _isGamePaused = true;
    }

    public void BackToMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
