using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public ConfigScript config;
    
    private float _timerChrono;

    public float TimerChrono
    {
        get => _timerChrono;
        set => _timerChrono = value;
    }

    private Text _timerText;
    private bool _isTimerOn;
    private IEnumerator _timerCoroutine;

    public void ResetTimer(Text timer)
    {
        _timerText = timer;
        _timerText.text = "00:00:000";
        _timerChrono = 0f;
        PauseTimer();
    }
    
    private IEnumerator UpdateTimer()
    {
        float startTime = Time.realtimeSinceStartup;
        while (true)
        {
            yield return null;
            _timerChrono = Time.realtimeSinceStartup - startTime;
            _timerText.text = ConvertTimerToString(_timerChrono);
        }
    }
    
    public void StartTimer()
    {
        if (!_isTimerOn)
        {
            _timerCoroutine = UpdateTimer();
            StartCoroutine(_timerCoroutine);
            _isTimerOn = true;
        }
    }

    public void PauseTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _isTimerOn = false;
        }
    }
    
    public string ConvertTimerToString(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 1000) % 1000);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
