using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    public bool debug;

    private List<PlayerConfiguration> _playerConfigs;
    private int _maxPlayers;
    
    public static PlayerConfigurationManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            if(debug)Debug.Log("SINGLETON - Trying to create another instance of singleton !");
        }
        else
        {
            Instance = this;
            _playerConfigs = new List<PlayerConfiguration>();
            _maxPlayers = config.PlayerAmount;
            if (_maxPlayers <= 0) _maxPlayers = 1;
            GetComponent<PlayerInputManager>().maxPlayerCount = _maxPlayers;
        }
    }

    public bool PlayerIsConnected(int index)
    {
        if (_playerConfigs.Find(p => p.PlayerIndex == index) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;
        if(debug)Debug.Log("Player "+ _playerConfigs[index].Name+" is ready !");
        if (_playerConfigs.Count == _maxPlayers && _playerConfigs.All(p => p.IsReady == true))
        {
            config.PlayerConfigurations = _playerConfigs;
            if(debug)Debug.Log("All ready");
            SceneManager.LoadScene("AlphaSceneNew");
        }
    }

    public void SetPlayerBoatColor(int index, Material color)
    {
        _playerConfigs[index].PlayerBoatMaterial = color;
    }
    
    public void SetPlayerCannonColor(int index, Material color)
    {
        _playerConfigs[index].PlayerCannonMaterial = color;
    }
    
    public void SetPlayerBoat(int index, GameObject boat)
    {
        _playerConfigs[index].PlayerBoat = boat;
    }

    public void SetPlayerCannon(int index, GameObject cannon)
    {
        _playerConfigs[index].PlayerCannon = cannon;
    }

    public void SetPlayerName(int index, string playerName)
    {
        if (playerName == "")
        {
            playerName = "Player " + index;
        }
        _playerConfigs[index].Name = playerName;
    }
    
    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        if (_playerConfigs.Count < _maxPlayers)
        {
            if(debug)Debug.Log("Player Joined "+playerInput.playerIndex);
        
            var defaultCamera = GameObject.Find("DefaultCamera");
            if(defaultCamera)
            {
                Destroy(defaultCamera);
            }

            if (_playerConfigs.All(p => p.PlayerIndex != playerInput.playerIndex))
            {
                _playerConfigs.Add(new PlayerConfiguration(playerInput));
            }   
        }
        else
        {
            if(debug)Debug.Log("Player capacity exceeded");
        }
    }
    
    /*
        public void OnDeviceLost(int index)
        {
            var deviceLostEvent = new PlayerInput.DeviceLostEvent();
            deviceLostEvent.AddListener(_playerConfigs[index].Input.onDeviceLost);
        }
    */
    
    public void HandlePlayerLeft()
    {
        if(debug)Debug.Log("Player Left");
    }
}
    
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
    }
    public PlayerInput Input { get; set; }
    
    public int PlayerIndex { get; set; }
    
    public string Name { get; set; }
    
    public bool IsReady { get; set; } 
    
    public Material PlayerBoatMaterial { get; set; }
    
    public Material PlayerCannonMaterial { get; set; }
    
    public GameObject PlayerBoat { get; set; }
    
    public GameObject PlayerCannon { get; set; }
}
