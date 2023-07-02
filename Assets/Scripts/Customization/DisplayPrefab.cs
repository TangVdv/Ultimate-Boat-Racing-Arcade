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
    [SerializeField] private ConfigAPI configAPI;
    [SerializeField] private GameObject boatPanel;
    [SerializeField] private GameObject cannonPanel;
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private BoatSelection boatSelection;
    [SerializeField] private GameObject buttonColorLockedTemplate;
    [SerializeField] private GameObject buttonColorUnlockedTemplate;
    [SerializeField] private GameObject colorPanel;
    public float colorButtonScale = 70f;

    private string _layerName;
    private List<GameObject> _boatPreview;
    private List<GameObject> _boatTemplate;
    private List<GameObject> _cannonPreview;
    private List<GameObject> _cannonTemplate;
    
    private string _identifier;
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
            if (config.BoatTemplates.Count == 0)
            {
                config.BoatTemplates = _boatTemplate;
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
                UnityAction setPrefabHandler = () => { boatSelection.SetPrefab(_boatTemplate[index], boat, 0); };
                button.GetComponent<Button>().onClick.AddListener(setPrefabHandler);
            }
            
            BoatConfigurationParameters boatConfigurationParameters = _boatTemplate[index].GetComponent<BoatConfigurationParameters>();
            UnityAction setColorHandler = () => { SetupColorPanel(boatConfigurationParameters.Identifier); };
            button.GetComponent<Button>().onClick.AddListener(setColorHandler);
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
            
            CannonConfigurationParameters cannonConfigurationParameters = _cannonTemplate[index].GetComponent<CannonConfigurationParameters>();
            UnityAction setColorHandler = () => { SetupColorPanel(cannonConfigurationParameters.Identifier); };
            button.GetComponent<Button>().onClick.AddListener(setColorHandler);
        }
    }

    private void ClearColorPanel()
    {
        foreach (Transform child in colorPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetupColorPanel(string identifier)
    {
        ClearColorPanel();
        if (string.IsNullOrEmpty(identifier)) return;
        _identifier = identifier;
        
        // Create default color buttons
        foreach (Color color in config.DefaultColors)
        {
            var template =  Instantiate(buttonColorUnlockedTemplate, colorPanel.transform);
            var button = template.transform.GetChild(0);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(colorButtonScale, colorButtonScale);
            button.GetComponent<Image>().color = color;
            if (button.GetComponent<Button>() != null)
            {
                if(boatSelection)
                {
                    UnityAction buttonClickHandler = () => { boatSelection.SetColor(color); };
                    button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
                }
            }
        }

        if (configAPI.IsConnected == false) return;
        
        // Create other color buttons
        foreach (KeyValuePair<string, Color> color in config.ColorsByIdentifier)
        {
            GameObject templateColor;
            UnityAction buttonClickHandler = null;

            if (config.ColorIdentifierByBoat.ContainsKey(color.Key))
            {
                if (config.ColorIdentifierByBoat[color.Key] == identifier)
                {
                    //Unlocked button
                    templateColor = buttonColorUnlockedTemplate; 
                    if(boatSelection) buttonClickHandler = () => { boatSelection.SetColor(color.Value); };
                }
                else
                {
                    //Locked button
                    templateColor = buttonColorLockedTemplate;
                }
            }
            else
            {
                //Locked button
                templateColor = buttonColorLockedTemplate;
            }
            var template =  Instantiate(templateColor, colorPanel.transform);
            var button = template.transform.GetChild(0);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(colorButtonScale, colorButtonScale);
            button.GetComponent<Image>().color = color.Value;
            if (button.GetComponent<Button>() != null)
            {
                if (buttonClickHandler != null)
                {
                    button.GetComponent<Button>().onClick.AddListener(buttonClickHandler);
                }
            }
        }
    }

    public void ShopRedirection()
    {
        Application.OpenURL(configAPI.GetApiUrl+"/shop");
    }

    public void BackButton()
    {
        ClearColorPanel();
    }
}
