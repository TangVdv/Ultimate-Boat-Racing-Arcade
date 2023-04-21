using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    public GameObject boatAI;
    [SerializeField] private ConfigScript config;

    private GameObject [] _boats;
    private float _startX = -10f;
    private float _endX = 10f;
    private int _spawnAmount;

    public void BoatsSetup()
    {
        _spawnAmount = config.PlayerAmount + config.AIAmount;
        _boats = new GameObject[_spawnAmount];

        if (_spawnAmount == 1)
        {
            _boats[0] = Instantiate(config.Boat, transform.position, Quaternion.identity);
        }
        else
        {
            int spawnPlayerIndex = Random.Range(1, _spawnAmount);
        
            for (int i = 0; i < _spawnAmount; i++)
            {
                if (i == spawnPlayerIndex)
                {
                    _boats[i] = Instantiate(config.Boat, transform.position, Quaternion.identity);
                }
                else
                {
                    _boats[i] = Instantiate(boatAI, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void Spawn()
    {
        Debug.Log("spawnAmount : "+_spawnAmount);
        float distanceBetweenSpawn = (_endX - _startX) / (_spawnAmount - 1);

        for (int i = 0; i < _spawnAmount; i++)
        {
            float x = _startX + i * distanceBetweenSpawn;
            if (_spawnAmount == 1)
            {
                x = transform.position.x;
            }
            Vector3 spawnPosition = new Vector3(x, transform.position.y, transform.position.z); 
            _boats[i].transform.position = spawnPosition;
        }
    }
}
