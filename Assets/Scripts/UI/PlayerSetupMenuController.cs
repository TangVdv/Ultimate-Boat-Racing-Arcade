using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerSetupMenuController : MonoBehaviour
{
    [SerializeField] private InputField playerName;
    [SerializeField] private Button nextButton;
    [SerializeField] private Image verifiedImage;
    [SerializeField] private Material defaultMat;
    
    public int _playerIndex;
    private bool _inputEnabled;

    private void Awake()
    {
        SetPlayerIndex(GetComponentInParent<PlayerInput>().playerIndex);
        nextButton.enabled = false;
        PlayerIsConnected();
    }

    private void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
        playerName.text = "Player "+(pi+1);
    }

    public void ReadyPlayer()
    {
        PlayerConfigurationManager.Instance.SetPlayerName(_playerIndex, playerName.text);
        PlayerConfigurationManager.Instance.ReadyPlayer(_playerIndex);
        nextButton.gameObject.SetActive(false);
    }

    public void PlayerIsConnected()
    {
        if (PlayerConfigurationManager.Instance.PlayerIsConnected(_playerIndex))
        {
            verifiedImage.color = Color.green;
        }
        else
        {
            verifiedImage.color = Color.red;
            nextButton.enabled = false;
        }
    }

    public void SetColor(Image buttonImage)
    {
        defaultMat.color = buttonImage.color;
        PlayerConfigurationManager.Instance.SetPlayerColor(_playerIndex, defaultMat);
        nextButton.enabled = true;
    }
}
