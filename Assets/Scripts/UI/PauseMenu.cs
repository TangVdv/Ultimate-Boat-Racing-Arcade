using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InputAction pauseAction;
    [SerializeField] private TimerScript timer;
    [SerializeField] private SetupLevelScript setupLevelScript;

    private bool _isGamePaused;

    private void Awake()
    {
        pauseAction.Enable();
        pauseAction.performed += OnPause;
    }
    
    private void OnDisable()
    {
        pauseAction.Disable();
        pauseAction.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        timer.ResumeTimer();
        _isGamePaused = false;
    }

    private void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        timer.PauseTimer();
        Time.timeScale = 0f;
        _isGamePaused = true;
    }

    public void BackToMenu()
    {
        Resume();
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        Resume();
        setupLevelScript.ResetCurrentMap();
    }
}
