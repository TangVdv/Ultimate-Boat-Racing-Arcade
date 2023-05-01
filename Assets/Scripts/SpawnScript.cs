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

        var transform1 = transform;
        var localPosition = transform1.localPosition;
        var right = transform1.right;
        
        _start = localPosition - right * size;
        _end = localPosition + right * size;
    }
    
    public void Spawn()
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
