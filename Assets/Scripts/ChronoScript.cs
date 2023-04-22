using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ChronoScript : MonoBehaviour
{
    [SerializeField] private Text timerText;

    private Dictionary<string, Dictionary<int, string>> _checkpointTimes =
        new Dictionary<string, Dictionary<int, string>>();

    private string _timerString;
    private float _timerChrono;
    private bool _isTimerOn;

    private IEnumerator _timerCoroutine;

    public void Reset()
    {
        timerText.text = "00:00:000";
        _timerChrono = 0f;
        PauseTimer();
    }
    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            _timerChrono += 0.01f;
            int minutes = Mathf.FloorToInt(_timerChrono / 60);
            int seconds = Mathf.FloorToInt(_timerChrono % 60);
            int milliseconds = Mathf.FloorToInt((_timerChrono * 1000) % 1000);
            _timerString = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
            timerText.text = _timerString;
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

    public void SaveCheckpointTime(string mapName, int checkpointID)
    {
        if (!_checkpointTimes.ContainsKey(mapName))
        {
            _checkpointTimes[mapName] = new Dictionary<int, string>();
        }

        _checkpointTimes[mapName][checkpointID] = _timerString;
    }

    public string GetCheckpointTime(string mapName, int checkpointID)
    {
        if (_checkpointTimes.ContainsKey(mapName))
        {
            if (_checkpointTimes[mapName].ContainsKey(checkpointID))
            {
                return _checkpointTimes[mapName][checkpointID];
            }
        }

        return "";
    }

    public void PrintCheckpointsTime(string mapName)
    {
        foreach (KeyValuePair<int, string> entry in _checkpointTimes[mapName])
        {
            Debug.Log("Checkpoint" + entry.Key + " : " + entry.Value);
        }
    }
}
