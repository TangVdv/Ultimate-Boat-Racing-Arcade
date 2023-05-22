using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuMotion : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPoint;
    [SerializeField] private GameObject boatMenuPoint;
    [SerializeField] private GameObject settingsMenuPoint;
    [SerializeField] private GameObject CircuitMenuPoint;

    private GameObject positions;
    
    private void Awake()
    {
        positions = mainMenuPoint;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPosition();
    }

    private void MoveToPosition()
    {
        if (positions)
        {
            if (transform.position != positions.transform.position ||
                transform.rotation != positions.transform.rotation)
            {
                transform.position = Vector3.Lerp(transform.position, positions.transform.position, 1f * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, positions.transform.rotation, 1f * Time.deltaTime);
            }
        }
    }

    public void SwitchPositionCamera(int index)
    {
        switch (index)
        {
            case 0:
                positions = mainMenuPoint;
                break;
            case 1:
                positions = boatMenuPoint;
                break;
            case 2:
                positions = settingsMenuPoint;
                break;
            case 3:
                positions = CircuitMenuPoint;
                break;
        }
    }
}
