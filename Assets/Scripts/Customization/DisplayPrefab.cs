using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayPrefab : MonoBehaviour
{
    [SerializeField] private TemplatesDictionary templatesDictionaryConfig;
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject boatPanel;
    [SerializeField] private GameObject cannonPanel;
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private BoatSelection boatSelection;
    [SerializeField] private GameObject buttonColorTemplate;
    [SerializeField] private GameObject colorPanel;
    public float colorButtonScale = 70f;

    private string _layerName;
    private List<GameObject> _boatPreview;
    private List<GameObject> _boatTemplate;
    private List<GameObject> _cannonPreview;
    private List<GameObject> _cannonTemplate;
    
    private void Start()
    {
        _boatPreview = templatesDictionaryConfig.BoatPreview;
        _boatTemplate = templatesDictionaryConfig.BoatTemplate;
        _cannonPreview = templatesDictionaryConfig.CannonPreview;
        _cannonTemplate = templatesDictionaryConfig.CannonTemplate;

        if (boatSelection)
        {
            _layerName = boatSelection.LayerName;
        }
        if (_boatPreview.Count > 0)
        {
            SetupBoatMenu();
            if (config.BoatTemplate.Count == 0)
            {
                config.BoatTemplate = _boatTemplate;
            }
        }

        if (_cannonPreview.Count > 0)
        {
            SetupCannonMenu();
            if (config.CannonTemplate.Count == 0)
            {
                config.CannonTemplate = _cannonTemplate;
            }
        }

        if (config.Colors.Count > 0)
        {
            SetupColorPanel();
        }
    }

    private void SetupBoatMenu()
    {
        foreach (GameObject boat in _boatPreview)
        {
            var button = Instantiate(buttonTemplate, boatPanel.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = boat.name;
            var prefab = Instantiate(boat, button.transform.GetChild(1).transform);
            if(_layerName != "") prefab.layer = LayerMask.NameToLayer(_layerName);
            prefab.transform.position = button.transform.GetChild(1).transform.position;
            int index = _boatPreview.FindIndex(b => b.Equals(boat));
            if (boatSelection)
            {
                UnityAction buttonClickHandler = () => { boatSelection.SetPrefab(_boatTemplate[index], boat, 0); };

                button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
            }
        }
    }
    
    private void SetupCannonMenu()
    {
        foreach (GameObject cannon in _cannonPreview)
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
            int index = _cannonPreview.FindIndex(b => b.Equals(cannon));
            if (boatSelection)
            {
                UnityAction buttonClickHandler = () => { boatSelection.SetPrefab(_cannonTemplate[index], cannon, 1); };

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
