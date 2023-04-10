using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private Text currentFPSText;

    private int[] _fpsArray = {30, 60, 120};
    private float timer, timelapse, avgFramerate;

    private void Start()
    {
        Application.targetFrameRate = _fpsArray[config.FPSIndex];
    }

    void Update()
    {
        if (config.ShowFPS)
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
