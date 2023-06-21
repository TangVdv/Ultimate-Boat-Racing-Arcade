using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public int speed = 2;
    public Vector3 rotation;
    private void Update()
    {
        transform.Rotate(rotation, speed * Time.deltaTime);
    }
}
