using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject gameSettings;
    [SerializeField] private Text currentFPSText;

    private GameSettings _gameSettings;

    private bool _showFPS = false;
    
    private float timer, timelapse, avgFramerate;
    void Start()
    {
        _gameSettings = gameSettings.GetComponent<GameSettings>();
    }
    
    void Update()
    {
        if (_showFPS)
        {
            // calcul current framerate
            timelapse = Time.smoothDeltaTime;
            timer = timer <= 0 ? 0 : timer -= timelapse;
            if (timer <= 0)
                avgFramerate = (int)(1f / timelapse);

            currentFPSText.text = avgFramerate.ToString();
        }
        else
            currentFPSText.text = "";
    }
}
