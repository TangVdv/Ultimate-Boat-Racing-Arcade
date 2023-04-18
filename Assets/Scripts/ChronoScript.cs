using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChronoScript : MonoBehaviour
{
    [SerializeField] private Text timerText;
    private float _timerString;
    private string _timerChrono;
    private bool _isTimerOn;
    
    public string TimerChrono
    {
        get => _timerChrono;
        set => _timerChrono = value;
    }

    private IEnumerator _timerCoroutine;
    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            _timerString += 0.01f;
            int minutes = Mathf.FloorToInt(_timerString / 60);
            int seconds = Mathf.FloorToInt(_timerString % 60);
            int milliseconds = Mathf.FloorToInt((_timerString * 1000) % 1000);
            _timerChrono = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            timerText.text = _timerChrono;
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

    public void StartTimer()
    {
        if (!_isTimerOn)
        {
            _timerCoroutine = UpdateTimer();
            StartCoroutine(_timerCoroutine);
            _isTimerOn = true;
        }
    }
}
