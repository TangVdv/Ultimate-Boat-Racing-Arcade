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
    [SerializeField] private List<GameObject> boats;
    
    public float startX = -10f;
    public float endX = 10f;
    public bool debug = false;
    
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
        Debug.Log("spawnAmount : "+_spawnAmount);
        for (int i = boats.Count; i > _spawnAmount; i--)
        {
            Destroy(boats[i-1]);
            boats.RemoveAt(i-1);
        }
    }
    
    public void Spawn()
    {
        float distanceBetweenSpawn = (endX - startX) / (boats.Count - 1);
        int i = 0;
        foreach (GameObject boat in boats)
        {
            float x = startX + i * distanceBetweenSpawn;
            if (boats.Count == 1)
            {
                x = transform.position.x;
            }
            Vector3 spawnPosition = new Vector3(x, transform.position.y, transform.position.z); 
            boat.transform.position = spawnPosition;
            i++;
        }
    }
}
