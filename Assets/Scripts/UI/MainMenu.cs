using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GithubButton()
    {
        Application.OpenURL("https://github.com/TangVdv/Ultimate-Boat-Racing-Arcade");
    }
}
