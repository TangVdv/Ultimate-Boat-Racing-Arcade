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
    [SerializeField] private Logger logger;

    private float _startX = -10f;
    private float _endX = 10f;

    private void Start()
    {
        logger.Log("player amount : " +config.PlayerAmount, "", LogType.Log);
        logger.Log("ai amount : "+config.AIAmount, "", LogType.Log);
        int spawnAmount = config.PlayerAmount + config.AIAmount;
        logger.Log("spawnAmount : "+spawnAmount, "", LogType.Log);
        int spawnPlayerIndex = Random.Range(1, spawnAmount);
        logger.Log("playerIndex : "+spawnPlayerIndex, "", LogType.Log);
        float distanceBetweenSpawn = (_endX - _startX) / (spawnAmount - 1);
        logger.Log("boat : "+config.Boat, "", LogType.Log);

        for (int i = 1; i <= spawnAmount; i++)
        {
            float x = _startX + i * distanceBetweenSpawn;   
            if (spawnAmount == 1)
            {
                x = transform.position.x;
            }
            Vector3 spawnPosition = new Vector3(x, transform.position.y, transform.position.z);
            
            if (i == spawnPlayerIndex)
            {
                Instantiate(config.Boat, spawnPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(boatAI, spawnPosition, Quaternion.identity);
            }
        }
    }
}
