using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private ChronoScript chronoScript;
    [SerializeField] private Text countdownText;
    [SerializeField] private Button startButton;
    
    public int countdown = 3;
    
    public void StartGame()
    {
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        int count = countdown;
        
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1.0f);

            count--;
        }

        countdownText.text = "";
        startButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
        chronoScript.StartTimer();
    }
}
