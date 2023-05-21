using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ChronoScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject chronoModeUI;
    [SerializeField] private Text timerDifferenceText;
    [SerializeField] private Image timerDifferencePanel;
    
    private int _levelIndex;

    public void ResetChrono()
    {
        if (!chronoModeUI.activeInHierarchy)
        {
            chronoModeUI.SetActive(true);
        }
        _levelIndex = config.Level;
        timerDifferenceText.text = "";
    }

    public void ShowCheckpointTimeDifference(float timerDiff, string timerText )
    {
        if (config.CheckpointTimes.Length > 0)
        {
            if (config.CheckpointTimes[_levelIndex] != null)
            {
                if (timerDiff > 0)
                {
                    timerDifferenceText.text = "+ "+ timerText;
                    timerDifferencePanel.color = new Color(1, 0, 0, .4f);
                }
                else if(timerDiff < 0) 
                { 
                    timerDifferenceText.text = "- " + timerText;
                    timerDifferencePanel.color = new Color(0, 0, 1, .4f);
                }
                else 
                { 
                    timerDifferenceText.text = timerText;
                    timerDifferencePanel.color = new Color(.7f, .7f, .7f, .4f);
                }
            }
            else
            {
                timerDifferenceText.text = timerText;
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

    public void SaveCheckpointsTime(List<float> checkpointTime)
    {
        //PrintCheckpointsTime(checkpointTime);
        config.CheckpointTimes[_levelIndex] = checkpointTime;
    }
}
