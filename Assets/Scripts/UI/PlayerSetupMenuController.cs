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
    public bool debug;
    
    private int _playerIndex;
    private bool _isCannonSet;
    private bool _isBoatSet;
    private GameObject _boat;
    private GameObject _cannon;
    private Material _cannonMat;

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
    

    public void SetPreview(int index, GameObject prefab, Material mat)
    {
        if (index == 0)
        {
            foreach (Transform child in globalObjectParentPreview.transform)
            {
                Destroy(child.gameObject);
            }
        
            foreach (Transform child in boatObjectParentPreview.transform)
            {
                Destroy(child.gameObject);
            }  
            
            if (prefab)
            {
                Instantiate(prefab, boatObjectParentPreview.transform);
            
                _boat = Instantiate(prefab, globalObjectParentPreview.transform);
                _boat.GetComponent<BuildBoatPreview>().ApplyColor(mat, _boat.GetComponent<MeshRenderer>());
                if (_cannon)
                {
                    _boat.GetComponent<BuildBoatPreview>().CreateCannon(_cannon, _cannonMat);
                }
            }
        }

        if (index == 1)
        {
            foreach (Transform child in cannonObjectParentPreview.transform)
            {
                Destroy(child.gameObject);
            }

            _cannon = prefab;
            _cannonMat = mat;

            Instantiate(prefab, cannonObjectParentPreview.transform);
            if (_boat) _boat.GetComponent<BuildBoatPreview>().CreateCannon(prefab, mat);
        }
    }

    public void SetPlayerPrefab(int index, GameObject prefab, Material color)
    {
        if(debug)Debug.Log("Prefab : "+prefab+" ; index : "+index+" ; color : "+color.color);
        if (index == 0)
        {
            PlayerConfigurationManager.Instance.SetPlayerBoat(_playerIndex, prefab);
            PlayerConfigurationManager.Instance.SetPlayerBoatColor(_playerIndex, color);
            _isBoatSet = true;
        }

        if (index == 1)
        {
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
