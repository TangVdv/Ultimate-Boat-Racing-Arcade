using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private Text languageText;
    [SerializeField] private Text resolutionText;
    [SerializeField] private Text fpsText;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectVolumeSlider;
    [SerializeField] private LocalizeStringEvent localStringWindowMode;

    private int _languageIndex = 1;
    private string[] _languageArray = {"English", "Français", "Español", "Deutsch", "Polski", "Português", "Русский", "日本", "中国人", "Türkçe"};
    
    private bool _windowModeBool = true; // FULLSCREEN

    private string[] _windowModeArray = new[]
    {
        "label-windowmode-fullscreen",
        "label-windowmode-window"
    };
    
    private int _resolutionIndex = 28; // 1920x1080
    private int[] _resolutionWidthArray = {800, 1020, 1280, 1600, 1920, 2560};
    private int[] _resolutionHeightArray = {600, 720, 800, 900, 1080, 1440};
    private Vector2Int[] _resolutions;

    private int _fpsIndex = 1;
    private int[] _fpsArray = {30, 60, 120};

    private bool _showFPS = false;

    private int _masterVolume = 50;
    private int _musicVolume = 50;
    private int _effectVolume = 50;

    private float timer, timelapse, avgFramerate;

    private void Start()
    {
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
        config.FPSIndex = _fpsIndex;
        masterVolumeSlider.value = _masterVolume;
        musicVolumeSlider.value = _musicVolume;
        effectVolumeSlider.value = _effectVolume;
    }

    private void SetText()
    {
        languageText.text = _languageArray[_languageIndex].ToUpper();
        resolutionText.text = _resolutions[_resolutionIndex].x + "x" + _resolutions[_resolutionIndex].y;
        fpsText.text = _fpsArray[_fpsIndex].ToString();
        if(_windowModeBool)
            localStringWindowMode.StringReference = new LocalizedString("UBRA Translation Table", _windowModeArray[0]);
        else
            localStringWindowMode.StringReference = new LocalizedString("UBRA Translation Table", _windowModeArray[1]);

        fpsToggle.isOn = _showFPS;
    }

    /** LANGUAGE **/
    public void LeftLanguageCarousel()
    {
        _languageIndex = _languageIndex == 0 
            ? _languageArray.Length-1 
            : Mathf.Clamp(_languageIndex-1, 0, _languageArray.Length-1);
        StartCoroutine(SetLocale());
        SetText();
    }
    
    public void RightLanguageCarousel()
    {
        _languageIndex = _languageIndex == _languageArray.Length-1 
            ? 0
            : Mathf.Clamp(_languageIndex+1, 0, _languageArray.Length-1);
        StartCoroutine(SetLocale());
        SetText();
    }

    private IEnumerator SetLocale()
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_languageIndex];
    }

    /** RESOLUTION **/
    public void LeftResolutionCarousel()
    {
        _resolutionIndex = _resolutionIndex == 0
            ? _resolutions.Length - 1
            : Mathf.Clamp(_resolutionIndex - 1, 0, _resolutions.Length - 1);
        SetText();
    }

    public void RightResolutionCarousel()
    {
        _resolutionIndex = _resolutionIndex == _resolutions.Length - 1 
            ? 0 
            : Mathf.Clamp(_resolutionIndex + 1, 0, _resolutions.Length - 1);
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
    
    public void WindowModeCarousel()
    {
        _windowModeBool = !_windowModeBool;
        SetText();
    }

    /** REFRESH RATE **/
    
    public void LeftFPSCarousel()
    {
        _fpsIndex = _fpsIndex == 0 
            ? _fpsArray.Length-1 
            : Mathf.Clamp(_fpsIndex - 1, 0, _fpsArray.Length - 1);
        SetText();
    }

    public void RightFPSCarousel()
    {
        _fpsIndex = _fpsIndex == _fpsArray.Length - 1
            ? 0
            : Mathf.Clamp(_fpsIndex + 1, 0, _fpsArray.Length - 1);
        SetText();
    }

    /** HUD **/

    public void FPSToggle(bool value)
    {
        _showFPS = value;
        config.ShowFPS = value;
    }
}
