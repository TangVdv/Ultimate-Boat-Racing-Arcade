using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ModeSelection : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private Text playerAmountText;
    [SerializeField] private Text aiAmountText;
    [SerializeField] private Text difficultyText;

    private int _modeSelect;
    private int _circuitIndex;
    private int _playerAmount = 1;
    private int _aiAmount = 2;
    private int _difficultyIndex = 0;
    private string[] _difficulty = {"EASY", "MEDIUM", "HARD"};

    private GameSettings _gameSettings;

    private void Start()
    {
        DisableButton();
        _gameSettings = eventSystem.GetComponent<GameSettings>();
        SetText();
    }

    private void SetText()
    {
        playerAmountText.text = _playerAmount.ToString();
        aiAmountText.text = _aiAmount.ToString();
        difficultyText.text = _difficulty[_difficultyIndex];
    }

    public void PlayGame()
    {
        _gameSettings.Difficulty = _difficultyIndex;
        _gameSettings.AIAmount = _aiAmount;
        _gameSettings.PlayerAmount = _playerAmount;
        _gameSettings.GameMode = _modeSelect;
        SceneManager.LoadScene(_gameSettings.Circuit[_circuitIndex]);
    }

    public void DisableButton()
    {
        playButton.enabled = false;
    }

    public void CircuitSelection(int value)
    {
        _circuitIndex = value;
        playButton.enabled = true;
        detailsPanel.SetActive(true);
        detailsPanel.transform.GetChild(0).GetComponent<Text>().text = "Circuit : " + value;
        detailsPanel.transform.GetChild(1).GetComponent<Text>().text = "Best time : 00:00:00";
        detailsPanel.transform.GetChild(2).GetComponent<Text>().text = "Boat : None";
        detailsPanel.transform.GetChild(3).GetComponent<Text>().text = "Current boat : " + _gameSettings.Boat.name;
    }

    public void SetMode(int value)
    {
        _modeSelect = value;
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
        _aiAmount = Mathf.Clamp(_aiAmount - 1, 2, 5);
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
