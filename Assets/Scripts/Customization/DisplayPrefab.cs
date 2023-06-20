using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayPrefab : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject[] boats;
    [SerializeField] private GameObject[] boatsTemplate;
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private GameObject[] cannonsTemplate;
    [SerializeField] private GameObject boatPanel;
    [SerializeField] private GameObject cannonPanel;
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private BoatSelection boatSelection;

    private string _layerName;
    private void Start()
    {
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
}
