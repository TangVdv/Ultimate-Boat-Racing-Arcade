using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Color = System.Drawing.Color;
using Random = UnityEngine.Random;

public class BoatSelection : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject playerSetupPanel;
    [SerializeField] private PlayerSetupMenuController playerSetupMenuController;
    [SerializeField] private Button selectButton;

    private GameObject _currentPrefab;
    private GameObject _currentPrefabPreview;
    private int _currentIndex;
    private UnityEngine.Color color;
    private string _layerName;
    public string LayerName => _layerName;
    
    private GameObject[] _boats;
    private GameObject[] _boatsTemplate;
    private GameObject[] _cannons;
    private GameObject[] _cannonsTemplate;


    private void Awake()
    {
        _layerName = playerSetupMenuController.LayerName;
        selectButton.enabled = false;
    }

    public void SetTemplates(GameObject[] boats, GameObject[] boatsTemplate, GameObject[] cannons, GameObject[] cannonsTemplate)
    {
        _boats = boats;
        _boatsTemplate = boatsTemplate;
        _cannons = cannons;
        _cannonsTemplate = cannonsTemplate;
    }

    public void RandomPrefab()
    {   
        //Random boat
        int randomIndex = Random.Range(0, _boats.Length);
        SetPrefab(_boatsTemplate[randomIndex], _boats[randomIndex], 0);
        randomIndex = Random.Range(0, config.Colors.Count);
        color = config.Colors[randomIndex];
        Select();
        
        //Random cannon
        randomIndex = Random.Range(0, _cannons.Length);
        SetPrefab(_cannonsTemplate[randomIndex], _cannons[randomIndex], 1);
        randomIndex = Random.Range(0, config.Colors.Count);
        color = config.Colors[randomIndex];
        Select();
    }

    public void ApplyLayerRecursively(Transform parent)
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

    public void SetPrefab(GameObject prefab, GameObject prefabPreview, int index)
    {
        _currentPrefab = prefab;
        _currentPrefabPreview = prefabPreview;
        _currentIndex = index;
        selectButton.enabled = true;
    }
    
    public void SetColor(Image buttonImage)
    {
        color = buttonImage.color;
    }

    public void Select()
    {
        Material mat = CreateMaterial();
        playerSetupMenuController.SetPlayerPrefab(_currentIndex, _currentPrefab, mat);
        playerSetupMenuController.SetPreview(_currentIndex, _currentPrefabPreview, mat);
        Back();
    }

    private Material CreateMaterial()
    {
        Material newMaterial = new Material(Shader.Find("Standard"))
        {
            color = color
        };
        return newMaterial;
    }

    public void Back()
    {
        gameObject.SetActive(false);
        playerSetupPanel.SetActive(true);
        selectButton.enabled = false;
    }

}
