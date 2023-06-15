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
    [SerializeField] private GameObject globalObjectParentPreview;
    [SerializeField] private GameObject boatObjectParentPreview;
    [SerializeField] private GameObject cannonObjectParentPreview;

    private int _playerIndex;
    private bool _isCannonSet;
    private bool _isBoatSet;

    private void Awake()
    {
        SetPlayerIndex(GetComponentInParent<PlayerInput>().playerIndex);
        readyButton.enabled = false;
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
        readyButton.gameObject.SetActive(false);
    }

    public void PlayerIsConnected()
    {
    }
    

    public void SetPreview(GameObject prefab)
    {
        foreach (Transform child in globalObjectParentPreview.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in boatObjectParentPreview.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in cannonObjectParentPreview.transform)
        {
            Destroy(child.gameObject);
        }
        
        GameObject boat = PlayerConfigurationManager.Instance.GetPlayerBoat(_playerIndex);
        GameObject cannon = PlayerConfigurationManager.Instance.GetPlayerCannon(_playerIndex);
        if (boat)
        {
            boat = Instantiate(boat);
            boat.transform.position = globalObjectParentPreview.transform.position;
            boat.transform.parent = globalObjectParentPreview.transform;
            
            boat = Instantiate(boat);
            boat.transform.position = boatObjectParentPreview.transform.position;
            boat.transform.parent = boatObjectParentPreview.transform; 
        }

        if (cannon)
        {
            cannon = Instantiate(cannon);
            cannon.transform.position = cannonObjectParentPreview.transform.position;
            cannon.transform.parent = cannonObjectParentPreview.transform; 
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
