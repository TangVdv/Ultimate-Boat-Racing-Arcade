using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionUI : MonoBehaviour
{
    [SerializeField] private Button nextButton;

    private void Start()
    {
        DisableButton();
    }

    private void DisableButton()
    {
        nextButton.enabled = false;
    }

    public void OnPlayerJoined()
    {
        
    }

    public void OnPlayerLeft()
    {
        
    }
    
}
