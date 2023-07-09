using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public ConfigScript config;
    [SerializeField] private Text timerText;
    
    private float _timerChrono;

    public float TimerChrono
    {
        get => _timerChrono;
        set => _timerChrono = value;
    }

    private Text _timerText;
    private bool _isTimerOn;
    private IEnumerator _timerCoroutine;
    private IEnumerator _timerPausedCoroutine;
    private float _startTime;
    private float _startPausedTime;
    private float _currentPausedTime;
    private float _totalPausedTime;
    
    public void ResetTimer()
    {
        timerText.text = "00:00:000";
        _timerChrono = 0f;
        _totalPausedTime = 0f;
        PauseTimer();
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return null;
            _timerChrono = Time.realtimeSinceStartup - _startTime - _totalPausedTime;
            timerText.text = ConvertTimerToString(_timerChrono);
        }
    }

    private IEnumerator UpdatePausedTimer()
    {
        while (true)
        {
            yield return null;
            _currentPausedTime = Time.realtimeSinceStartup - _startPausedTime;
            timerText.text = ConvertTimerToString(_timerChrono);
        }
    }

    public void StartTimer()
    {
        if (!_isTimerOn)
        {
            _startTime = Time.realtimeSinceStartup;
            _timerCoroutine = UpdateTimer();
            _timerPausedCoroutine = UpdatePausedTimer();
            ResumeTimer();
        }
    }

    public void ResumeTimer()
    {
        if (!_isTimerOn)
        {
            if (_timerPausedCoroutine != null)
            {
                StopCoroutine(_timerPausedCoroutine);
                _totalPausedTime += _currentPausedTime;
            }
            StartCoroutine(_timerCoroutine);
            _isTimerOn = true;
        }
    }

    public void PauseTimer()
    {
        if (_timerCoroutine != null)
        {
            _startPausedTime = Time.realtimeSinceStartup;
            StartCoroutine(_timerPausedCoroutine);
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
