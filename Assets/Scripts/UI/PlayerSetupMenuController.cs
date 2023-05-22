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
    
    private int _playerIndex;
    private float _ignoreInputTime = 1.5f;
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
        _ignoreInputTime = Time.time + _ignoreInputTime;
    }

    private void Update()
    {
        if (Time.time > _ignoreInputTime)
        {
            _inputEnabled = true;
        }
    }

    public void ReadyPlayer()
    {
        if (!_inputEnabled) return;
        PlayerConfigurationManager.Instance.SetPlayerName(_playerIndex, playerName.text);
        PlayerConfigurationManager.Instance.ReadyPlayer(_playerIndex);
        nextButton.enabled = false;
    }

    public void PlayerIsConnected()
    {
        if (PlayerConfigurationManager.Instance.PlayerIsConnected(_playerIndex))
        {
            verifiedImage.color = Color.green;
            nextButton.enabled = true;
        }
        else
        {
            verifiedImage.color = Color.red;
            nextButton.enabled = false;
        }
    }
}
