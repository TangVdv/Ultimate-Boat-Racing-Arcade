using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ChronoScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private Text timerText;
    [SerializeField] private Text timerDifferenceText;
    
    private float _timerChrono;

    public float TimerChrono
    {
        get => _timerChrono;
        set => _timerChrono = value;
    }

    private bool _isTimerOn;
    private int _circuitIndex;
    private IEnumerator _timerCoroutine;
    
    public void Reset()
    {
        _circuitIndex = config.Circuit;
        timerText.text = "00:00:000";
        timerDifferenceText.text = "";
        _timerChrono = 0f;
        PauseTimer();
    }
    
    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            _timerChrono += 0.01f;
            timerText.text = ConvertTimerToString(_timerChrono);
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
/*
    public void SaveCheckpointTime(int checkpointID)
    {
        if (_checkpointTimes[_circuitIndex].ContainsKey(checkpointID))
        {
            if (_checkpointTimes[_circuitIndex][checkpointID] > _timerChrono)
            {
                _checkpointTimes[_circuitIndex][checkpointID] = _timerChrono;   
            }   
        }
        else
        {
            _checkpointTimes[_circuitIndex][checkpointID] = _timerChrono;
        }
    }
*/
    public void ShowCheckpointTimeDifference(int index)
    {
        if (config.CheckpointTimes.Length > 0)
        {
            if (config.CheckpointTimes[_circuitIndex] != null)
            {
                float checkPointTimer = config.CheckpointTimes[_circuitIndex][index];
                float timerDiff = _timerChrono - checkPointTimer;
                Debug.Log(ConvertTimerToString(checkPointTimer));
                if (timerDiff > 0)
                {
                    timerDifferenceText.text = "+ "+ ConvertTimerToString(timerDiff); 
                    timerDifferenceText.color = Color.red;
                }
                else if(timerDiff < 0) 
                { 
                    timerDifferenceText.text = "- " + ConvertTimerToString(timerDiff);
                    timerDifferenceText.color = Color.blue;
                }
                else 
                { 
                    timerDifferenceText.text = ConvertTimerToString(timerDiff); 
                    timerDifferenceText.color = Color.grey;
                }       
            }
        }
    }

    private string ConvertTimerToString(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 1000) % 1000);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
    
    public void PrintCheckpointsTime(List<float> checkpointTime)
    {
        int i = 0;
        foreach (var time in checkpointTime)
        {
            Debug.Log("Checkpoint " + i + " : " + ConvertTimerToString(time));
            i++;
        }
    }
    
    public void SaveCheckpointsTime(List<float> checkpointTime)
    {
        PrintCheckpointsTime(checkpointTime);
        config.CheckpointTimes[_circuitIndex] = checkpointTime;
    }
}
