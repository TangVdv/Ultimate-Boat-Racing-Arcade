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
    private float _timer, _timelapse, _avgFramerate;
    
    private void Start()
    {
        Application.targetFrameRate = _fpsArray[config.FPSIndex];
    }

    void Update()
    {
        if (config.ShowFPS)
        {
            // calcul current framerate
            _timelapse = Time.smoothDeltaTime;
            _timer = _timer <= 0 ? 0 : _timer -= _timelapse;
            if (_timer <= 0)
                _avgFramerate = (int)(1f / _timelapse);

            currentFPSText.text = _avgFramerate.ToString();
        }
        else
            currentFPSText.text = "";
    }
}
