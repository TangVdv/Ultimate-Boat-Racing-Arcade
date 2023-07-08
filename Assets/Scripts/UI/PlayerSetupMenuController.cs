using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSetupMenuController : MonoBehaviour
{
    [SerializeField] private InputField playerName;
    [SerializeField] private Button readyButton;
    [SerializeField] private GameObject globalObjectParentPreview;
    [SerializeField] private GameObject boatObjectParentPreview;
    [SerializeField] private GameObject cannonObjectParentPreview;
    [SerializeField] private Camera playerCamera;
    public bool debug;
    
    private int _playerIndex;
    private static string _layerName;
    public string LayerName => _layerName;
    private bool _isCannonSet;
    private bool _isBoatSet;
    private GameObject _boat;
    private GameObject _cannon;
    private Material _cannonMat;

    private void Awake()
    {
        SetPlayerIndex(GetComponentInParent<PlayerInput>().playerIndex);
        readyButton.enabled = false;
        CreateNewLayer();
        playerCamera.cullingMask |= (1 << LayerMask.NameToLayer(_layerName));
    }
    
    private static void CreateNewLayer()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProperty = tagManager.FindProperty("layers");
        int emptyLayerIndex = -1;
        for (int i = 8; i < layersProperty.arraySize; i++)
        {
            SerializedProperty layerProperty = layersProperty.GetArrayElementAtIndex(i);
            if (string.IsNullOrEmpty(layerProperty.stringValue))
            {
                emptyLayerIndex = i;
                break;
            }
        }
            
        if (emptyLayerIndex != -1)
        {
            SerializedProperty newLayer = layersProperty.GetArrayElementAtIndex(emptyLayerIndex);
            newLayer.stringValue = _layerName;
            tagManager.ApplyModifiedProperties();
            Debug.Log("New layer created: "+_layerName);
        }
        else
        {
            Debug.LogError("No empty layer slot available. Please free up a layer and try again.");
        }
    }
    private void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
        _layerName = "Player " + (pi + 1);
        playerName.text = _layerName;
    }

    public void ReadyPlayer()
    {
        PlayerConfigurationManager.Instance.SetPlayerName(_playerIndex, playerName.text);
        PlayerConfigurationManager.Instance.ReadyPlayer(_playerIndex);
        readyButton.gameObject.SetActive(false);
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
                _boat = Instantiate(prefab, boatObjectParentPreview.transform);
                _boat.layer = LayerMask.NameToLayer(_layerName);
            
                _boat = Instantiate(prefab, globalObjectParentPreview.transform);
                _boat.layer = LayerMask.NameToLayer(_layerName);
                _boat.GetComponent<BuildBoatPreview>().ApplyColor(mat, _boat.GetComponent<MeshRenderer>());
                if (_cannon)
                {
                    _boat.GetComponent<BuildBoatPreview>().CreateCannon(_cannon, _cannonMat, _layerName);
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

            var cannon = Instantiate(prefab, cannonObjectParentPreview.transform);
            cannon.layer = LayerMask.NameToLayer(_layerName);
            ApplyLayerRecursively(cannon.transform);
            if (_boat) _boat.GetComponent<BuildBoatPreview>().CreateCannon(prefab, mat, _layerName);
        }
    }
    
    private void ApplyLayerRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.layer = LayerMask.NameToLayer(_layerName);
            if (child.childCount > 0)
            {
                ApplyLayerRecursively(child);
            }
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

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
