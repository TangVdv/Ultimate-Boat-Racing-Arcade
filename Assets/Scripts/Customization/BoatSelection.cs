using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class BoatSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] boats;
    [SerializeField] private GameObject[] boatsTemplate;
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private GameObject[] cannonsTemplate;
    [SerializeField] private GameObject boatPanel;
    [SerializeField] private GameObject cannonPanel;
    [SerializeField] private GameObject playerSetupPanel;
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private PlayerSetupMenuController playerSetupMenuController;
    [SerializeField] private Button selectButton;

    private GameObject _currentPrefab;
    private int _currentIndex;
    private UnityEngine.Color color;

    private void Start()
    {
        selectButton.enabled = false;
        if(boats.Length > 0)
            SetupBoatMenu();
        
        if(cannons.Length > 0)
            SetupCannonMenu();
    }

    private void SetupBoatMenu()
    {
        foreach (GameObject boat in boats)
        {
            var button = Instantiate(buttonTemplate, boatPanel.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = boat.name;
            var prefab = Instantiate(boat, button.transform.GetChild(1).transform);
            prefab.transform.position = button.transform.GetChild(1).transform.position;
            int index = System.Array.IndexOf(boats, boat);
            UnityAction buttonClickHandler = () =>
            {
                SetPrefab(boatsTemplate[index], 0);
            };

            button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
        }
    }
    
    private void SetupCannonMenu()
    {
        foreach (GameObject cannon in cannons)
        {
            var button = Instantiate(buttonTemplate, cannonPanel.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = cannon.name;
            var prefab = Instantiate(cannon, button.transform.GetChild(1).transform);
            prefab.transform.position = button.transform.GetChild(1).transform.position;
            int index = System.Array.IndexOf(cannons, cannon);
            UnityAction buttonClickHandler = () =>
            {
                SetPrefab(cannonsTemplate[index], 1);
            };

            button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
        }
        
    }

    private void SetPrefab(GameObject prefab, int index)
    {
        _currentPrefab = prefab;
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
        boatPanel.SetActive(false);
        cannonPanel.SetActive(false);
        gameObject.SetActive(false);
        playerSetupPanel.SetActive(true);
        selectButton.enabled = false;
    }

}
