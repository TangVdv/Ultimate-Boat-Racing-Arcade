using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boat.New;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class SetupLevelScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private TimerScript timerScript;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;

    [SerializeField] private AIConfigurationManager aiConfigurationManager;
    
    public List<GameObject> mapTemplates;
    
    public List<NavMeshData> navMeshes;
    private NavMeshDataInstance _currentNavMesh;

    private SetupGameScript _setupGameScript;
    private GameObject _currentMap;
    
    private List<GameObject> _boats = new List<GameObject>();
    void Awake()
    {
        PlayerSetup();
        if (config.AIAmount > 0)
        {
            AISetup();   
        }
        SetupLevel();
    }

    private void ClearLevel()
    {
        if (_currentMap != null)
        {
            Destroy(_currentMap);
            NavMesh.RemoveNavMeshData(_currentNavMesh);
        }
    }

    public void SetupLevel()
    {
        ClearLevel();
        _currentMap = Instantiate(mapTemplates[config.Level]);
        _setupGameScript = _currentMap.GetComponent<SetupGameScript>();
        _currentNavMesh = NavMesh.AddNavMeshData(navMeshes[config.Level]);
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

    private void PlayerSetup()
    {
        // Instantiate players
        var playerConfigs = config.PlayerConfigurations;
        foreach (var player in playerConfigs)
        {
            var playerInstantiate = Instantiate(playerPrefab);
            playerInstantiate.GetComponent<NewPlayerInputManager>().InitializePlayer(player);
            _boats.Add(playerInstantiate);
        }
    }

    private void AISetup()
    {
        var boats = config.BoatTemplates;
        var cannons = config.CannonTemplates;
        var colors = config.DefaultColors;
        // Create AI
        for (int i = 0; i < config.AIAmount; i++)
        {
            Material boatMat = new Material(Shader.Find("Standard"));
            Material cannonMat = new Material(Shader.Find("Standard"));
            int randomIndex = Random.Range(0, colors.Count);   
            boatMat.color = colors[randomIndex];
            randomIndex = Random.Range(0, colors.Count);   
            cannonMat.color = colors[randomIndex];
            
            aiConfigurationManager.HandleAIJoin(i);
            aiConfigurationManager.SetupAI(
                i,
                boatMat,
                cannonMat,
                boats[Random.Range(0, boats.Count)],
                cannons[Random.Range(0, cannons.Count)]
            );
        }
        
        // Instantiate AI
        var AIConfigs = aiConfigurationManager.AIConfigs;
        foreach (var AI in AIConfigs)
        {
            Debug.Log(AI);
            var AIInstantiate = Instantiate(botPrefab);
            AIInstantiate.GetComponent<NewAIInputManager>().InitializeAI(AI);
            _boats.Add(AIInstantiate);   
        }
    }
}
