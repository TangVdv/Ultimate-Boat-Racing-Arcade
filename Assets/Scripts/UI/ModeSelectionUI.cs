using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class ModeSelectionUI : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private TerrainDictionary terrainDictionary;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private Text playerAmountText;
    [SerializeField] private Text aiAmountText;
    [SerializeField] private GameObject mapContainer;
    [SerializeField] private LocalizeStringEvent difficultyStringEvent;
    [SerializeField] private Text circuitNameText;
    [SerializeField] private Text circuitBestTimeText;
    [SerializeField] private Text playerNameText;

    private int _modeSelect;
    private int _levelIndex = 0;
    private int _playerAmount = 1;
    private int _maxPlayerAmount = 2;
    private int _maxAIAmount = 5;
    private int _aiAmount = 2;
    private int _difficultyIndex = 1;
    private string[] _difficulty = new []
    {
        "label-difficulty-easy",
        "label-difficulty-medium",
        "label-difficulty-hard"
    };

    private void Start()
    {
        InstantiateMap();
        DisableButton();
        SetText();
    }

    private void InstantiateMap()
    {
        if (terrainDictionary.TerrainPreview.Count > _levelIndex)
        {
            if (mapContainer.transform.childCount > 0)
            {
                foreach (Transform child in mapContainer.transform)
                {
                    Destroy(child.gameObject);
                } 
            }
            Instantiate(terrainDictionary.TerrainPreview[_levelIndex], mapContainer.transform);
        }

    }

    private void SetText()
    {
        difficultyStringEvent.StringReference = new LocalizedString("UBRA Translation Table", _difficulty[_difficultyIndex]);
        playerAmountText.text = _playerAmount.ToString();
        aiAmountText.text = _aiAmount.ToString();
    }

    public void ModeSelection()
    {
        config.Difficulty = _difficultyIndex+1;
        if (_modeSelect == 1)
        {
            _aiAmount = 0;
            _playerAmount = 1;
        }

        config.LastLevelIndex = terrainDictionary.TerrainPreview.Count - 1;
        config.AIAmount = _aiAmount;
        config.PlayerAmount = _playerAmount;
        config.GameMode = _modeSelect;
        config.Level = _levelIndex;
        SceneManager.LoadScene("SetupPlayersMenu");
    }

    public void CircuitSelection(int value)
    {
        _modeSelect = 1;
        _levelIndex = value;
        selectButton.enabled = true;
        SetChronoLevelInfo(value);
        InstantiateMap();
    }

    private void SetChronoLevelInfo(int value)
    {
        detailsPanel.SetActive(true);
        circuitNameText.text = (value + 1).ToString();
        string text = "";
        if (config.CheckpointTimes.ElementAtOrDefault(value) != null)
        {
            int lastIndex = config.CheckpointTimes[value].Count - 1;
            float timer = config.CheckpointTimes[value][lastIndex];
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt((timer * 1000) % 1000);
            text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        }
        
        circuitBestTimeText.text = text;
        text = "";
        if (config.BestTimePlayerName.ElementAtOrDefault(value) != null)
            text = config.BestTimePlayerName[value];
        playerNameText.text = text;
    }

    public void DisableButton()
    {
        selectButton.enabled = false;
    }
    

    public void RaceMode()
    {
        selectButton.enabled = true;
        _modeSelect = 0;
        _levelIndex = 0;
        InstantiateMap();
    }

    public void ChronoMode()
    {
        DisableButton();
        _modeSelect = 1;
    }

    /** PLAYER AMOUNT **/

    public void LeftPlayerAmountCarousel()
    {
        _playerAmount = _playerAmount == 1 
            ? _maxPlayerAmount 
            : Mathf.Clamp(_playerAmount - 1, 1, _maxPlayerAmount);
        SetText();
    }
    
    public void RightPlayerAmountCarousel()
    {
        _playerAmount = _playerAmount == _maxPlayerAmount 
            ? 1 
            : Mathf.Clamp(_playerAmount + 1, 1, _maxPlayerAmount);
        SetText();
    }
    
    /** AI AMOUNT **/
    
    public void LeftAIAmountCarousel()
    {
        _aiAmount = _aiAmount == 0 
            ? _maxAIAmount 
            : Mathf.Clamp(_aiAmount - 1, 0, _maxAIAmount);
        SetText();
    }
    
    public void RightAIAmountCarousel()
    {
        _aiAmount = _aiAmount == _maxAIAmount
            ? 0 
            : Mathf.Clamp(_aiAmount + 1, 0, _maxAIAmount);
        SetText();
    }
    
    /** DIFFICULTY **/
    
    public void LeftDifficultyCarousel()
    {
        _difficultyIndex = _difficultyIndex == 0 
            ? _difficulty.Length-1 
            : Mathf.Clamp(_difficultyIndex - 1, 0, _difficulty.Length-1);
        SetText();
    }
    
    public void RightDifficultyCarousel()
    {
        _difficultyIndex = _difficultyIndex == _difficulty.Length-1 
            ? 0 
            : Mathf.Clamp(_difficultyIndex + 1, 0, _difficulty.Length-1);
        SetText();
    }
}
