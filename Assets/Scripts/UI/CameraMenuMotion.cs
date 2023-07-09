using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuMotion : MonoBehaviour
{
    [SerializeField] private GameObject[] menuPoints;

    private GameObject _positions;

    private void Awake()
    {
        _positions = menuPoints[0];
    }

    private void Update()
    {
        MoveToPosition();
    }

    private void MoveToPosition()
    {
        if (_positions)
        {
            if (transform.position != _positions.transform.position ||
                transform.rotation != _positions.transform.rotation)
            {
                transform.position = Vector3.Lerp(transform.position, _positions.transform.position, 1f * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, _positions.transform.rotation, 1f * Time.deltaTime);
            }
            else
            {
                _positions = null;
            }
        }
    }

    public void SwitchPositionCamera(int index)
    {
        _positions = menuPoints[index];
    }
}
