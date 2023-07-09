using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boat.New;
using Checkpoints;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;

    public float size = 50f;
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

    public void SpawnSetup(List<GameObject> boats)
    {
        _boats = boats;
        var position = transform.position;
        var right = transform.right;
        
        _start = position - right * size;
        _end = position + right * size;
    }
    
    public void Spawn()
    {
        if (_boats.Count > 0)
        {
            float distanceBetweenSpawn = Vector3.Distance(_start,_end) / (_boats.Count - 1);
            int i = 0;
            foreach (GameObject boat in _boats)
            {
                var boatManager = boat.GetComponent<NewInputManagerInterface>();
                boatManager.lastCheckpoint = transform;
                boatManager.rigidbody.velocity = Vector3.zero;
                Vector3 pos = _start + i * distanceBetweenSpawn * transform.right;
                if (_boats.Count == 1)
                {
                    pos = transform.position;
                }
                Vector3 spawnPosition = pos;
                boat.transform.position = spawnPosition;
            
                Quaternion spawnerRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
                Quaternion boatRotation = Quaternion.Euler(0f, spawnerRotation.eulerAngles.y, 0f);
                boat.transform.rotation = boatRotation;
                
                PlayerInput playerInput = boat.GetComponent<PlayerInput>();
                if (playerInput) playerInput.enabled = true;
                
                i++;
            }   
        }
    }
}
