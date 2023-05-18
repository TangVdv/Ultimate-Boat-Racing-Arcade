using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ModeSelectionUI : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private Text playerAmountText;
    [SerializeField] private Text aiAmountText;
    [SerializeField] private Text difficultyText;
    [SerializeField] private GameObject[] maps;
    [SerializeField] private GameObject mapContainer;

    private int _modeSelect;
    private int _levelIndex = 0;
    private int _playerAmount = 1;
    private int _aiAmount = 2;
    private int _difficultyIndex = 0;
    private string[] _difficulty = {"EASY", "MEDIUM", "HARD"};

    private void Start()
    {
        InstantiateMap();
        DisableButton();
        SetText();
    }

    private void InstantiateMap()
    {
        if (maps.Length > _levelIndex)
        {
            if (mapContainer.transform.childCount > 0)
            {
                foreach (Transform child in mapContainer.transform)
                {
                    Destroy(child.gameObject);
                } 
            }
            Instantiate(maps[_levelIndex], mapContainer.transform);
        }

    }

    private void SetText()
    {
        playerAmountText.text = _playerAmount.ToString();
        aiAmountText.text = _aiAmount.ToString();
        difficultyText.text = _difficulty[_difficultyIndex];
    }

    public void ModeSelection()
    {
        config.Difficulty = _difficultyIndex;
        if (_modeSelect == 1)
        {
            _aiAmount = 0;
            _playerAmount = 1;
        }
        config.AIAmount = _aiAmount;
        config.PlayerAmount = _playerAmount;
        config.GameMode = _modeSelect;
        config.Level = _levelIndex;
        SceneManager.LoadScene("SetupPlayersMenu");
    }

    public void CircuitSelection(int value)
    {
        _levelIndex = value;
        selectButton.enabled = true;
        detailsPanel.SetActive(true);
        detailsPanel.transform.GetChild(0).GetComponent<Text>().text = "Circuit : " + value;
        string text = "None";
        if (config.CheckpointTimes[value] != null)
        {
            int lastIndex = config.CheckpointTimes[value].Count - 1;
            text = config.CheckpointTimes[value][lastIndex].ToString();   
        }
        detailsPanel.transform.GetChild(1).GetComponent<Text>().text = "Best time : " + text;
        detailsPanel.transform.GetChild(2).GetComponent<Text>().text = "Boat : None";
        detailsPanel.transform.GetChild(3).GetComponent<Text>().text = "Current boat : ";
        InstantiateMap();
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
        _playerAmount = Mathf.Clamp(_playerAmount - 1, 1, 2);
        SetText();
    }
    
    public void RightPlayerAmountCarousel()
    {
        _playerAmount = Mathf.Clamp(_playerAmount + 1, 1, 2);
        SetText();
    }
    
    /** AI AMOUNT **/
    
    public void LeftAIAmountCarousel()
    {
        _aiAmount = Mathf.Clamp(_aiAmount - 1, 0, 5);
        SetText();
    }
    
    public void RightAIAmountCarousel()
    {
        _aiAmount = Mathf.Clamp(_aiAmount + 1, 2, 5);
        SetText();
    }
    
    /** DIFFICULTY **/
    
    public void LeftDifficultyCarousel()
    {
        _difficultyIndex = Mathf.Clamp(_difficultyIndex - 1, 0, _difficulty.Length-1);
        SetText();
    }
    
    public void RightDifficultyCarousel()
    {
        _difficultyIndex = Mathf.Clamp(_difficultyIndex + 1, 0, _difficulty.Length-1);
        SetText();
    }
}
