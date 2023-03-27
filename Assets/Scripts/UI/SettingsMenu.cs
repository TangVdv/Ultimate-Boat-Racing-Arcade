using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private Text languageText;
    [SerializeField] private Text resolutionText;
    [SerializeField] private Text windowModeText;
    [SerializeField] private Text fpsText;
    [SerializeField] private Text currentFPSText;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private Toggle hudToggle;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectVolumeSlider;
    [SerializeField] private Slider lookSensitivitySlider;

    private GameSettings _gameSettings;
    
    private int _languageIndex = 0;
    private string[] _languageArray = {"FRANÇAIS", "ENGLISH"};
    
    private bool _windowModeBool = true; // FULLSCREEN
    
    private int _resolutionIndex = 28; // 1920x1080
    private int[] _resolutionWidthArray = {800, 1020, 1280, 1600, 1920, 2560};
    private int[] _resolutionHeightArray = {600, 720, 800, 900, 1080, 1440};
    private Vector2Int[] _resolutions;

    private int _fpsIndex = 1;
    private int[] _fpsArray = {30, 60, 120};

    private bool _showHUD = true;
    private bool _showFPS = false;

    private int _masterVolume = 50;
    private int _musicVolume = 50;
    private int _effectVolume = 50;

    private float _lookSensitivity = 5;

    private float timer, timelapse, avgFramerate;

    private void Start()
    {
        _gameSettings = eventSystem.GetComponent<GameSettings>();
        
        GenerateResolutions();
        Apply();
        SetText();
        
        /** AUDIO **/
        
        masterVolumeSlider.onValueChanged.AddListener(v =>
        {
            _masterVolume = (int)v;
        });
        
        musicVolumeSlider.onValueChanged.AddListener(v =>
        {
            _musicVolume = (int)v;
        });
        
        effectVolumeSlider.onValueChanged.AddListener(v =>
        {
            _effectVolume = (int)v;
        });
        
        /** CONTROL **/
        
        lookSensitivitySlider.onValueChanged.AddListener(v =>
        {
            _lookSensitivity = (int)v;
        });
    }

    private void Update()
    {
        if (_showFPS)
        {
            // calcul current framerate
            timelapse = Time.smoothDeltaTime;
            timer = timer <= 0 ? 0 : timer -= timelapse;
            if (timer <= 0)
                avgFramerate = (int)(1f / timelapse);

            currentFPSText.text = avgFramerate.ToString();
        }
        else
            currentFPSText.text = "";
    }

    public void Apply()
    {
        // set resolution
        Screen.SetResolution(
            _resolutions[_resolutionIndex].x,
            _resolutions[_resolutionIndex].y, 
            _windowModeBool);
        
        // set framerate
        Application.targetFrameRate = _fpsArray[_fpsIndex];
        masterVolumeSlider.value = _masterVolume;
        musicVolumeSlider.value = _musicVolume;
        effectVolumeSlider.value = _effectVolume;
    }

    private void SetText()
    {
        languageText.text = _languageArray[_languageIndex];
        resolutionText.text = _resolutions[_resolutionIndex].x + "x" + _resolutions[_resolutionIndex].y;
        fpsText.text = _fpsArray[_fpsIndex].ToString();
        if(_windowModeBool)
            windowModeText.text = "FULLSCREEN";
        else
            windowModeText.text = "WINDOW";
        
        fpsToggle.isOn = _showFPS;
        hudToggle.isOn = _showHUD;
    }

    /** LANGUAGE **/
    public void LeftLanguageCarousel()
    {
        _languageIndex = Mathf.Clamp(_languageIndex-1, 0, _languageArray.Length-1);
        SetText();
    }
    
    public void RightLanguageCarousel()
    {
        _languageIndex = Mathf.Clamp(_languageIndex+1, 0, _languageArray.Length-1);
        SetText();
    }

    /** RESOLUTION **/
    public void LeftResolutionCarousel()
    {
        _resolutionIndex = Mathf.Clamp(_resolutionIndex - 1, 0, _resolutions.Length - 1);
        SetText();
    }

    public void RightResolutionCarousel()
    {
        _resolutionIndex = Mathf.Clamp(_resolutionIndex + 1, 0, _resolutions.Length - 1);
        SetText();
    }

    private void GenerateResolutions()
    {
        int index = 0;
        _resolutions = new Vector2Int[_resolutionWidthArray.Length * _resolutionHeightArray.Length];
        for (int i = 0; i < _resolutionWidthArray.Length; i++)
        {
            for (int j = 0; j < _resolutionHeightArray.Length; j++)
            {
                _resolutions[index] = new Vector2Int(_resolutionWidthArray[i], _resolutionHeightArray[j]);
                index++;
            }
        }
    }
    
    /** WINDOW MODE **/
    
    public void LeftWindowModeCarousel()
    {
        _windowModeBool = true;
        SetText();
    }

    public void RightWindowModeCarousel()
    {
        _windowModeBool = false;
        SetText();
    }

    /** REFRESH RATE **/
    
    public void LeftFPSCarousel()
    {
        _fpsIndex = Mathf.Clamp(_fpsIndex - 1, 0, _fpsArray.Length - 1);
        SetText();
    }

    public void RightFPSCarousel()
    {
        _fpsIndex = Mathf.Clamp(_fpsIndex + 1, 0, _fpsArray.Length - 1);
        SetText();
    }

    /** HUD **/

    public void FPSToggle(bool value)
    {
        _showFPS = value;
        _gameSettings.ShowFPS = value;
    }

    public void HUDToggle(bool value)
    {
        _showHUD = value;
        _gameSettings.ShowHUD = value;
    }
}
