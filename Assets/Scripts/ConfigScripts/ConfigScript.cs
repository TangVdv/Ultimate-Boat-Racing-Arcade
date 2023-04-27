using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ConfigScript", order = 0)]
public class ConfigScript : ScriptableObject
{
    private GameObject _boat;
    private int _circuit;
    private int _gameMode;
    private int _aiAmount;
    private int _playerAmount;
    private int _difficulty;

    private bool _showHUD;
    private bool _showFPS;

    private int _fpsIndex;
    private List<float>[] _checkpointTimes = new List<float>[5];
    public List<float>[] CheckpointTimes
    {
        get => _checkpointTimes;
        set => _checkpointTimes = value;
    }

    public GameObject Boat
    {
        get => _boat;
        set => _boat = value;
    }

    public int Circuit
    {
        get => _circuit;
        set => _circuit = value;
    }

    public int GameMode
    {
        get => _gameMode;
        set => _gameMode = value;
    }

    public int AIAmount
    {
        get => _aiAmount;
        set => _aiAmount = value;
    }

    public int PlayerAmount
    {
        get => _playerAmount;
        set => _playerAmount = value;
    }

    public int Difficulty
    {
        get => _difficulty;
        set => _difficulty = value;
    }

    public bool ShowHUD
    {
        get => _showHUD;
        set => _showHUD = value;
    }

    public bool ShowFPS
    {
        get => _showFPS;
        set => _showFPS = value;
    }
    
    public int FPSIndex
    {
        get => _fpsIndex;
        set => _fpsIndex = value;
    }
}
