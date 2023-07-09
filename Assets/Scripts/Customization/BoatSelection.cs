using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Color = System.Drawing.Color;
using Random = UnityEngine.Random;

public class BoatSelection : MonoBehaviour
{
    [SerializeField] private TemplatesDictionary templatesDictionaryConfig;
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject playerSetupPanel;
    [SerializeField] private PlayerSetupMenuController playerSetupMenuController;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject boatSetupMenu;

    private GameObject _currentPrefab;
    private GameObject _currentPrefabPreview;
    private int _currentIndex;
    private UnityEngine.Color color;
    private string _layerName;
    public string LayerName => _layerName;
    
    private List<GameObject> _boatPreview;
    private List<GameObject> _boatTemplate;
    private List<GameObject> _cannonPreview;
    private List<GameObject> _cannonTemplate;


    private void Awake()
    {
        _boatPreview = templatesDictionaryConfig.BoatPreview;
        _boatTemplate = templatesDictionaryConfig.BoatTemplate;
        _cannonPreview = templatesDictionaryConfig.CannonPreview;
        _cannonTemplate = templatesDictionaryConfig.CannonTemplate;
        
        _layerName = playerSetupMenuController.LayerName;
        selectButton.enabled = false;
    }

    public void RandomPrefab()
    {   
        //Random boat
        int randomIndex = Random.Range(0, _boatPreview.Count);
        SetPrefab(_boatTemplate[randomIndex], _boatPreview[randomIndex], 0);
        randomIndex = Random.Range(0, config.DefaultColors.Count);
        color = config.DefaultColors[randomIndex];
        Select();
        
        //Random cannon
        randomIndex = Random.Range(0, _cannonPreview.Count());
        SetPrefab(_cannonTemplate[randomIndex], _cannonPreview[randomIndex], 1);
        randomIndex = Random.Range(0, config.DefaultColors.Count);
        color = config.DefaultColors[randomIndex];
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
    
    public void SetColor(UnityEngine.Color buttonColor)
    {
        color = buttonColor;
    }

    public void Select()
    {
        Material mat = CreateMaterial();
        playerSetupMenuController.SetPlayerPrefab(_currentIndex, _currentPrefab, mat);
        playerSetupMenuController.SetPreview(_currentIndex, _currentPrefabPreview, mat);
    }

    private Material CreateMaterial()
    {
        Material newMaterial = new Material(Shader.Find("Standard"))
        {
            color = color
        };
        color = UnityEngine.Color.clear;
        return newMaterial;
    }

    public void Back()
    {
        boatSetupMenu.SetActive(false);
        playerSetupPanel.SetActive(true);
        selectButton.enabled = false;
    }

}
