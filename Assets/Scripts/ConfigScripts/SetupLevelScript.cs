using System.Collections;
using System.Collections.Generic;
using Boat.New;
using UnityEngine;

public class SetupLevelScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private TimerScript timerScript;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;
    
    public List<GameObject> mapTemplates;

    private SetupGameScript _setupGameScript;
    private GameObject _currentMap;
    
    private List<GameObject> _boats = new List<GameObject>();
    void Start()
    {
        BoatsSetup();
        SetupLevel();
    }

    private void ClearLevel()
    {
        if (_currentMap != null)
        {
            Destroy(_currentMap);
        }
    }

    public void SetupLevel()
    {
        ClearLevel();
        _currentMap = Instantiate(mapTemplates[config.Level]);
        _setupGameScript = _currentMap.GetComponent<SetupGameScript>();
        _setupGameScript.SetupGame(_boats);
        timerScript.ResetTimer();
        timerScript.StartTimer();
    }

    public void ResetCurrentMap()
    {
        _setupGameScript.Reset();
        timerScript.ResetTimer();
        timerScript.StartTimer();
    }

    private void BoatsSetup()
    {
        // Instantiate players
        var playerConfigs = config.PlayerConfigurations;
        for (int i = 0; i < config.PlayerAmount; i++)
        {
            var player = Instantiate(playerPrefab);
            if (playerConfigs[i] != null)
            {
                player.GetComponent<NewPlayerInputManager>().InitializePlayer(playerConfigs[i]);
            }
            _boats.Add(player);   
        }
        
        // Instantiate bots
        for (int i = 0; i < config.AIAmount; i++)
        {
            var bot = Instantiate(botPrefab);
            bot.GetComponent<NewAIInputManager>().InitializeAI("Bot " + (i+1));
            _boats.Add(bot);
        }
    }
}
