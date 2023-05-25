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

    public (string, Color) GetTimerDiffValues(float timerDiff, string timerDiffText)
    {
        Color timerDiffColor;
        if (timerDiff > 0)
        {
            timerDiffText = "+ "+ timerDiffText;
            timerDiffColor = new Color(1, 0, 0, .4f);
        }
        else if(timerDiff < 0) 
        { 
            timerDiffText = "- " + timerDiffText;
            timerDiffColor = new Color(0, 0, 1, .4f);
        }
        else 
        {
            timerDiffColor = new Color(.7f, .7f, .7f, .4f);
        }

        return (timerDiffText, timerDiffColor);
    }

    public void ShowCheckpointTimeDifference(float timerDiff, string timerText )
    {
        if (config.CheckpointTimes.Length > 0)
        {
            if (config.CheckpointTimes[_levelIndex] != null)
            {
                timerDifferenceText.text = GetTimerDiffValues(timerDiff, timerText).Item1;
                timerDifferencePanel.color = GetTimerDiffValues(timerDiff, timerText).Item2;
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
}
