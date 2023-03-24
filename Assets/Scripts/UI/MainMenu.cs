using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ProtoScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GithubButton()
    {
        Application.OpenURL("https://github.com/TangVdv/Ultimate-Boat-Racing-Arcade");
    }
}