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
    
    public float size = 10f;
    public bool debug = false;

    private List<GameObject> _boats;
    private int _spawnAmount;
    private float _startX;
    private float _endX;

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
        _spawnAmount = config.PlayerAmount + config.AIAmount;
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

        _startX = transform.localPosition.x - size;
        _endX = transform.localPosition.x + size;
    }
    
    public void Spawn()
    {
        float distanceBetweenSpawn = (_endX - _startX) / (_boats.Count - 1);
        int i = 0;
        foreach (GameObject boat in _boats)
        {
            float x = _startX + i * distanceBetweenSpawn;
            if (_boats.Count == 1)
            {
                x = transform.localPosition.x;
            }
            
            Vector3 spawnPosition = new Vector3(x, transform.position.y, transform.position.z);
            boat.transform.position = spawnPosition;
            
            Quaternion spawnerRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            Quaternion boatRotation = Quaternion.Euler(0f, spawnerRotation.eulerAngles.y, 0f);
            boat.transform.rotation = boatRotation;
            

            i++;
        }
    }
}
