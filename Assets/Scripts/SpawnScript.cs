using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boat.New;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;
    
    public float size = 10f;
    public bool debug;

    private List<GameObject> _boats = new List<GameObject>();

    private Vector3 _start;
    private Vector3 _end;

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;

            Vector3 leftPoint = transform.TransformPoint(new Vector3(-size, 0f, 0f));
            Vector3 rightPoint = transform.TransformPoint(new Vector3(size, 0f, 0f));

            Gizmos.DrawLine(leftPoint, rightPoint);
        }
    }

    public void BoatsSetup()
    {
        // Instantiate players
        var playerConfigs = config.PlayerConfigurations;
        for (int i = 0; i < config.PlayerAmount; i++)
        {
            var player = Instantiate(playerPrefab);
            if (playerConfigs[i] != null)
            {
                player.GetComponent<NewPlayerInputManager>().InitializePlayer(playerConfigs[i], transform);
            }
            _boats.Add(player);   
        }
        
        // Instantiate bots
        for (int i = 0; i < config.AIAmount; i++)
        {
            _boats.Add(Instantiate(botPrefab));
        }

        var transform1 = transform;
        var localPosition = transform1.localPosition;
        var right = transform1.right;
        
        _start = localPosition - right * size;
        _end = localPosition + right * size;
    }
    
    public void Spawn()
    {
        if (_boats.Count > 0)
        {
            float distanceBetweenSpawn = Vector3.Distance(_start,_end) / (_boats.Count - 1);
            int i = 0;
            foreach (GameObject boat in _boats)
            {
                boat.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Vector3 pos = _start + i * distanceBetweenSpawn * transform.right;
                if (_boats.Count == 1)
                {
                    pos = transform.localPosition;
                }
            
                Vector3 spawnPosition = pos;
                boat.transform.position = spawnPosition;
            
                Quaternion spawnerRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
                Quaternion boatRotation = Quaternion.Euler(0f, spawnerRotation.eulerAngles.y, 0f);
                boat.transform.rotation = boatRotation;
            

                i++;
            }   
        }
    }
}
