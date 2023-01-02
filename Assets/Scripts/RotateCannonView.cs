using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCannonView : MonoBehaviour
{
    public GameObject bodyCannon;
    public Camera cameraCannon;

    public float rotationSideSpeed = .01f;
    public float rotationUpDownSpeed = .01f;

    public float topClamp = 25.0f;
    public float bottomClamp = .0f;
    
    // Update is called once per frame
    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Update()
    {
        BodyCannonRotation();
    }

    private void CameraRotation()
    {
        
    }

    private void BodyCannonRotation()
    {
        Debug.Log(bodyCannon.transform.localRotation.y);
        Debug.Log(cameraCannon.transform.localRotation.y);
    }
}
