using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerSetupMenuController : MonoBehaviour
{
    [SerializeField] private InputField playerName;
    [SerializeField] private Button readyButton;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private GameObject boatTest;
    
    public int _playerIndex;
    private bool _isCannonSet;
    private bool _isBoatSet;

    private void Awake()
    {
        SetPlayerIndex(GetComponentInParent<PlayerInput>().playerIndex);
        readyButton.enabled = false;
        PlayerIsConnected();
        PlayerConfigurationManager.Instance.SetPlayerBoat(_playerIndex, boatTest);
        SetPreview();
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
        readyButton.gameObject.SetActive(false);
    }

    public void PlayerIsConnected()
    {
    }
    

    private void SetPreview()
    {
        GameObject boat = PlayerConfigurationManager.Instance.GetPlayerBoat(_playerIndex); 
        if (boat)
        {
            boat = Instantiate(boat);
            boat.transform.position = objectParent.transform.position;
            boat.transform.parent = objectParent.transform;   
        }
    }

    public void SetPlayerPrefab(int index, GameObject prefab, Material color)
    {
        if (index == 0)
        {
            Debug.Log("set boat");
            PlayerConfigurationManager.Instance.SetPlayerBoat(_playerIndex, prefab);
            PlayerConfigurationManager.Instance.SetPlayerBoatColor(_playerIndex, color);
            _isBoatSet = true;
        }

        if (index == 1)
        {
            Debug.Log("set cannon");
            PlayerConfigurationManager.Instance.SetPlayerCannon(_playerIndex, prefab);
            PlayerConfigurationManager.Instance.SetPlayerCannonColor(_playerIndex, color);
            _isCannonSet = true;
        }

        if (_isBoatSet && _isCannonSet)
        {
            readyButton.enabled = true;
        }
    }
}
