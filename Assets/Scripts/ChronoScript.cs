using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ChronoScript : MonoBehaviour
{
    [SerializeField] private Text timerText;

    private Dictionary<int, Dictionary<int, string>> _checkpointTimes =
        new Dictionary<int, Dictionary<int, string>>();

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

    public void SaveCheckpointTime(int mapID, int checkpointID)
    {
        if (!_checkpointTimes.ContainsKey(mapID))
        {
            _checkpointTimes[mapID] = new Dictionary<int, string>();
        }

        _checkpointTimes[mapID][checkpointID] = _timerString;
    }

    public string GetCheckpointTime(int mapID, int checkpointID)
    {
        if (_checkpointTimes.ContainsKey(mapID))
        {
            if (_checkpointTimes[mapID].ContainsKey(checkpointID))
            {
                return _checkpointTimes[mapID][checkpointID];
            }
        }

        return "";
    }

    public void PrintCheckpointsTime(int mapID)
    {
        foreach (KeyValuePair<int, string> entry in _checkpointTimes[mapID])
        {
            Debug.Log("Checkpoint" + entry.Key + " : " + entry.Value);
        }
    }
}
