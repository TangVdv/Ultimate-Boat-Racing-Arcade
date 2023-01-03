using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCannonView : MonoBehaviour
{
    [SerializeField] private Transform bodyCannon;
    [SerializeField] private Transform cameraCannon;

    private Vector3 _localRotation;
    private float _cameraDistance = 6f;

    public float mouseSensitivity = 10.0f;
    public float rotationSideSpeed = 100f;
    public float rotationUpDownSpeed = 50f;
    public float orbitDampening = 10f;

    public float topClamp = 25.0f;

    private void Start()
    {
        cameraCannon.position = bodyCannon.position;
        cameraCannon.Translate(new Vector3(0, _cameraDistance, -_cameraDistance));
        cameraCannon.Rotate(new Vector3(25, 0, 0));
    }

    // Update is called once per frame
    private void Update()
    {
        BodyCannonRotation();
    }

    // LateUpdate is called once per frame, after Update() on every game object in the scene
    private void LateUpdate()
    {
        CameraRotation();
    }

    // Rotation of the camera on the X axis based on Mouse Coordinates
    private void CameraRotation()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            _localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        }
        
        Quaternion QT = Quaternion.Euler(0, _localRotation.x, 0);
        cameraCannon.parent.rotation = Quaternion.Lerp(cameraCannon.parent.rotation, QT, Time.deltaTime * orbitDampening);
    }

    private void BodyCannonRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(0, _localRotation.x, 0);
        Quaternion actualRotation = Quaternion.RotateTowards(bodyCannon.rotation, targetRotation, rotationSideSpeed * Time.deltaTime);

        if (targetRotation != actualRotation)
        {
            Debug.Log("call");
            bodyCannon.rotation = actualRotation;
        }
    }
}
