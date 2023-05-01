using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ChronoScript : TimerScript
{
    [SerializeField] private Text timerDifferenceText;
    [SerializeField] private Image timerDifferencePanel;
    
    private int _levelIndex;
    
    public void ResetChrono()
    {
        _levelIndex = config.Level;
        timerDifferenceText.text = "";
        ResetTimer();
    }

    public void ShowCheckpointTimeDifference(int index)
    {
        if (config.CheckpointTimes.Length > 0)
        {
            if (config.CheckpointTimes[_levelIndex] != null)
            {
                float checkPointTimer = config.CheckpointTimes[_levelIndex][index];
                float timerDiff = TimerChrono - checkPointTimer;
                if (timerDiff > 0)
                {
                    timerDifferenceText.text = "+ "+ ConvertTimerToString(timerDiff);
                    timerDifferencePanel.color = new Color(1, 0, 0, .4f);
                }
                else if(timerDiff < 0) 
                { 
                    timerDifferenceText.text = "- " + ConvertTimerToString(-timerDiff);
                    timerDifferencePanel.color = new Color(0, 0, 1, .4f);
                }
                else 
                { 
                    timerDifferenceText.text = ConvertTimerToString(timerDiff);
                    timerDifferencePanel.color = new Color(.7f, .7f, .7f, .4f);
                }
            }
            else
            {
                timerDifferenceText.text = ConvertTimerToString(TimerChrono);
                timerDifferencePanel.color = new Color(.7f, .7f, .7f, .4f);
            }
            StartCoroutine(TimerDifferenceFadeAway());
        }
    }

    private IEnumerator TimerDifferenceFadeAway()
    {
        yield return new WaitForSeconds(2.0f);
        timerDifferencePanel.color = Color.clear;
        timerDifferenceText.text = "";
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
        //PrintCheckpointsTime(checkpointTime);
        config.CheckpointTimes[_levelIndex] = checkpointTime;
    }
}
