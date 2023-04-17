using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private Text currentFPSText;
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject chronoModeUI;

    public GameObject ChronoModeUI
    {
        get => chronoModeUI;
        set => chronoModeUI = value;
    }

    private int[] _fpsArray = {30, 60, 120};
    private float _timer, _timelapse, _avgFramerate;

    private float _timerChrono;

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

    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f); //
            _timerChrono += 0.01f;
            int minutes = Mathf.FloorToInt(_timerChrono / 60);
            int seconds = Mathf.FloorToInt(_timerChrono % 60);
            int milliseconds = Mathf.FloorToInt((_timerChrono * 1000) % 1000);
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}
