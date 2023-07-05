using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Checkpoints;
using Unity.AI.Navigation.Editor;
using UnityEditor;
using UnityEngine;

public class GenerateMapPrefab : EditorWindow
{
    private GameObject _object;
    private string _mapName;
    private int _graceAmount;

    [MenuItem("Tools/Generate Map Prefab")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GenerateMapPrefab));
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Object", EditorStyles.boldLabel);
        _object = EditorGUILayout.ObjectField("", _object, typeof(GameObject), false) as GameObject;
        
        GUILayout.Space(20);
        GUILayout.Label("Components", EditorStyles.boldLabel);
       
        GUILayout.Space(20);
        GUILayout.Label("Parameters", EditorStyles.boldLabel);
        _mapName = EditorGUILayout.TextField("Map Name", _mapName);
        _graceAmount = EditorGUILayout.IntField("Grace Amount", _graceAmount);
        
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate Template"))
        {
            GenerateMap();
        }
        GUILayout.EndVertical();
    }

    private void GenerateMap()
    {
        if (_object == null)
        {
            Debug.LogError("Error : Please assign an object");
        }

        if (string.IsNullOrEmpty(_mapName))
            _mapName = _object.name;
        
        GameObject start = null;
        GameObject goal = null;
        GameObject obstacle = null;
        List<GameObject> checkpointsArray = new List<GameObject>();
        List<GameObject> powerupZonesArray = new List<GameObject>();
        //For every mesh in imported map, check its name and save it
        foreach (Transform child in _object.transform)
        {
            if (child.name == "Goal") goal = child.gameObject;
            if (child.name == "Start") start = child.gameObject;
            if (child.name == "Checkpoint_1") checkpointsArray.Add(child.gameObject);
            if (child.name == "PowerupZone_1") powerupZonesArray.Add(child.gameObject);
            if (child.name == "Obstacle") obstacle = child.gameObject;
        }

        //Load ConfigScript
        ConfigScript configScript =
            AssetDatabase.LoadAssetAtPath<ConfigScript>("Assets/Scripts/ConfigScripts/Config.asset");

        //Create prefab
        GameObject root = new GameObject("root");
        GameObject prefabInstance = PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/Terrain/"+ _mapName +"/" + _mapName + "Prefab.prefab");
        DestroyImmediate(root);
        prefabInstance = PrefabUtility.InstantiatePrefab(prefabInstance) as GameObject;

        // Add SetupGameScript to the root prefab 
        SetupGameScript setupGameScript = prefabInstance.AddComponent<SetupGameScript>();

        // Create the level container
        GameObject levelContainer = new GameObject("LevelContainer");
        levelContainer.transform.SetParent(prefabInstance.transform);

        // Create the terrain container and add the map object to it
        GameObject terrain = new GameObject("Terrain");
        terrain.transform.SetParent(levelContainer.transform);
        GameObject map = PrefabUtility.InstantiatePrefab(_object) as GameObject;
        map.transform.SetParent(terrain.transform);
        MeshCollider meshCollider = map.AddComponent<MeshCollider>();
        if(obstacle) meshCollider.sharedMesh = obstacle.GetComponent<MeshFilter>().sharedMesh;
        else Debug.LogWarning("Couldn't added sharedMesh in MeshCollider component");
        
        // Create the checkpoints container and add the CheckpointManager to it
        GameObject checkpoints = new GameObject("Checkpoints");
        checkpoints.transform.SetParent(levelContainer.transform);
        CheckpointManager checkpointManager = checkpoints.AddComponent<CheckpointManager>();
        setupGameScript.SetCheckpointManager(checkpointManager);
        if (_graceAmount < 1) _graceAmount = 1;
        checkpointManager.grace = _graceAmount;
        checkpointManager.SetConfigScript(configScript);

        // Load checkpoint prefab
        GameObject checkpoint = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RaceSystem/Checkpoint.prefab");
        if (checkpoint)
        {
            // Instantiate goal prefab to the goal mesh position
            if (goal)
            {
                GameObject checkpointInstance = Instantiate(checkpoint, checkpoints.transform, true);
                Checkpoint checkpointScript = checkpointInstance.AddComponent<Checkpoint>();
                checkpointScript.ID = 0;
                checkpointInstance.name = "Goal"; 
                checkpointInstance.transform.position = goal.transform.position;
                checkpointInstance.transform.rotation = goal.transform.rotation;
            }
            else
            {
                Debug.LogWarning("Goal couldn't be added");
            }
            int i = 1;
            // For each checkpoint mesh found, Instantiate the checkpoint prefab to the checkpoint mesh position
            foreach (var cp in checkpointsArray)
            {
                GameObject checkpointInstance = Instantiate(checkpoint, checkpoints.transform, true);
                Checkpoint checkpointScript = checkpointInstance.AddComponent<Checkpoint>();
                checkpointScript.ID = i;
                checkpointInstance.transform.position = cp.transform.position;
                checkpointInstance.transform.rotation = cp.transform.rotation;
                i++;
            }
        }
        else
        {
            Debug.LogWarning("Checkpoint prefab not found, couldn't add it to the map prefab");
        }

        // Create Powerup Zone container
        GameObject powerupZones = new GameObject("Powerup Zones");
        powerupZones.transform.SetParent(levelContainer.transform);
        
        // Load Powerup Zone prefab
        GameObject powerupZone = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RaceSystem/Powerup Zone.prefab");
        if (powerupZone)
        {
            // Instantiate Powerup Zone prefab to the Powerup Zone mesh position
            foreach (var zone in powerupZonesArray)
            {
                GameObject powerupZoneInstance = Instantiate(powerupZone, powerupZones.transform, true);
                powerupZoneInstance.transform.position = zone.transform.position;
            }

        }
        else
        {
            Debug.LogWarning("Powerup Zone prefab not found, couldn't add it to the map prefab");
        }

        // Load Spawn prefab
        GameObject spawn = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RaceSystem/Spawn.prefab");
        if (spawn)
        {
            // Instantiate spawn prefab to the start mesh position
            if (start)
            {
                GameObject spawnInstance = Instantiate(spawn, levelContainer.transform, true);
                spawnInstance.transform.position = start.transform.position;
                spawnInstance.transform.rotation = start.transform.rotation;
                setupGameScript.SetSpawner(spawnInstance.GetComponent<SpawnScript>());
            }
            else
            {
                Debug.LogWarning("Spawn couldn't be added");
            }
        }
        else
        {
            Debug.LogWarning("Spawn prefab not found, couldn't add it to the map prefab");
        }

        // Apply prefab instance to the asset database and destroy it from scene 
        PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.UserAction);
        DestroyImmediate(prefabInstance);
        
        AssetDatabase.Refresh();
    }
}
