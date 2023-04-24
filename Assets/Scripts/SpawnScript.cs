using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private Transform playerContainer;
    
    public float startX = -10f;
    public float endX = 10f;
    public bool debug = false;

    private List<GameObject> _boats;
    private int _spawnAmount;

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(startX, transform.position.y, transform.position.z), 
                new Vector3(endX, transform.position.y, transform.position.z));
        }
    }

    public void BoatsSetup()
    {
        _spawnAmount = config.PlayerAmount + config.AIAmount;
        Debug.Log("SpawnAmount : "+_spawnAmount);
        _boats = new List<GameObject>(playerContainer.childCount);
        foreach (Transform child in playerContainer)
        {
            _boats.Add(child.gameObject);
        }
        
        for (int i = _boats.Count; i > _spawnAmount; i--)
        {
            Destroy(_boats[i-1]);
            _boats.RemoveAt(i-1);
        }
    }
    
    public void Spawn()
    {
        float distanceBetweenSpawn = (endX - startX) / (_boats.Count - 1);
        int i = 0;
        foreach (GameObject boat in _boats)
        {
            float x = startX + i * distanceBetweenSpawn;
            if (_boats.Count == 1)
            {
                x = transform.position.x;
            }
            Vector3 spawnPosition = new Vector3(x, transform.position.y, transform.position.z); 
            boat.transform.position = spawnPosition;
            i++;
        }
    }
}
