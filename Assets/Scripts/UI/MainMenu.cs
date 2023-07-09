using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GithubButton()
    {
        Application.OpenURL("https://github.com/TangVdv/Ultimate-Boat-Racing-Arcade");
    }
    
    public void ItchioButton()
    {
        Application.OpenURL("https://kabajsah.itch.io/ultimate-boat-racing-arcade");
    }
}
