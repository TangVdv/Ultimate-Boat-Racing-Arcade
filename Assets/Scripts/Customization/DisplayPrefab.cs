using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayPrefab : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private ConfigAPI configAPI;
    [SerializeField] private GameObject[] boats;
    [SerializeField] private GameObject[] boatsTemplate;
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private GameObject[] cannonsTemplate;
    [SerializeField] private GameObject boatPanel;
    [SerializeField] private GameObject cannonPanel;
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private BoatSelection boatSelection;
    [SerializeField] private GameObject buttonColorTemplate;
    [SerializeField] private GameObject colorPanel;
    public float colorButtonScale = 70f;

    private string _layerName;
    private async void Start()
    {
        StartCoroutine(configAPI.GetDataTest(jsonData =>
        {
            Debug.Log(jsonData);
        }));
        
        StartCoroutine(configAPI.GetSkins(jsonData =>
        {
            Debug.Log(jsonData);
        }, "lul"));
        
        if (boatSelection)
        {
            _layerName = boatSelection.LayerName;
            boatSelection.SetTemplates(boats, boatsTemplate, cannons, cannonsTemplate);
        }
        if (boats.Length > 0)
        {
            SetupBoatMenu();
            if (config.BoatTemplates.Length == 0)
            {
                config.BoatTemplates = boatsTemplate;
            }
        }

        if (cannons.Length > 0)
        {
            SetupCannonMenu();
            if (config.CannonTemplates.Length == 0)
            {
                config.CannonTemplates = cannonsTemplate;
            }
        }

        if (config.Colors.Count > 0)
        {
            SetupColorPanel();
        }
    }

    private void SetupBoatMenu()
    {
        foreach (GameObject boat in boats)
        {
            var button = Instantiate(buttonTemplate, boatPanel.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = boat.name;
            var prefab = Instantiate(boat, button.transform.GetChild(1).transform);
            if(_layerName != "") prefab.layer = LayerMask.NameToLayer(_layerName);
            prefab.transform.position = button.transform.GetChild(1).transform.position;
            int index = System.Array.IndexOf(boats, boat);
            if (boatSelection)
            {
                UnityAction buttonClickHandler = () => { boatSelection.SetPrefab(boatsTemplate[index], boat, 0); };

                button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
            }
        }
    }
    
    private void SetupCannonMenu()
    {
        foreach (GameObject cannon in cannons)
        {
            var button = Instantiate(buttonTemplate, cannonPanel.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = cannon.name;
            var prefab = Instantiate(cannon, button.transform.GetChild(1).transform);
            if (_layerName != "")
            {
                prefab.layer = LayerMask.NameToLayer(_layerName);
                if (boatSelection) boatSelection.ApplyLayerRecursively(prefab.transform);
            }

            prefab.transform.position = button.transform.GetChild(1).transform.position;
            int index = System.Array.IndexOf(cannons, cannon);
            if (boatSelection)
            {
                UnityAction buttonClickHandler = () => { boatSelection.SetPrefab(cannonsTemplate[index], cannon, 1); };

                button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
            }
        }
    }

    private void SetupColorPanel()
    {
        foreach (var color in config.Colors)
        {
            var button = Instantiate(buttonColorTemplate, colorPanel.transform);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(colorButtonScale, colorButtonScale);
            button.GetComponent<Image>().color = color;
            if (boatSelection)
            {
                UnityAction buttonClickHandler = () => { boatSelection.SetColor(color); };

                button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
            }
        }
    }
}
