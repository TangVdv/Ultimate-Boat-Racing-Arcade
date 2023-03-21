using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Text languageText;
    [SerializeField] private Text resolutionText;

    private int _languageIndex = 0;
    private string[] _languageArray = {"FRANÇAIS", "ENGLISH"};
    
    private int _windowModeIndex = 0;
    private string[] _windowModeArray = {"FULLSCREEN", "WINDOW"};
    
    private int _resolutionIndex = 0;
    private int[] _resolutionWidthArray = {800, 1020, 1280, 1600, 1920, 2560};
    private int[] _resolutionHeightArray = {600, 720, 800, 900, 1080, 1440};
    private Vector2Int[] _resolutions;

    private int _fpsIndex = 0;
    private string[] _fpsArray = {"30", "60", "120"};

    private void Start()
    {
        languageText.text = _languageArray[_languageIndex];
        
        GenerateResolutions();
        SetResolution();
    }

    public void LeftLanguageCarousel()
    {
        _languageIndex = Mathf.Clamp(_languageIndex-1, 0, _languageArray.Length-1);
        languageText.text = _languageArray[_languageIndex];
    }
    
    public void RightLanguageCarousel()
    {
        _languageIndex = Mathf.Clamp(_languageIndex+1, 0, _languageArray.Length-1);
        languageText.text = _languageArray[_languageIndex];
    }

    public void LeftResolutionCarousel()
    {
        _resolutionIndex = Mathf.Clamp(_resolutionIndex - 1, 0, _resolutionWidthArray.Length - 1);
        SetResolution();
    }

    public void RightResolutionCarousel()
    {
        _resolutionIndex = Mathf.Clamp(_resolutionIndex + 1, 0, _resolutionWidthArray.Length - 1);
        SetResolution();
    }

    private void SetResolution()
    {
        int width = _resolutions[_resolutionIndex].x;
        int height = _resolutions[_resolutionIndex].y;
        Screen.SetResolution(width, height, Screen.fullScreen);
        resolutionText.text = width + "x" + height;
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
}
