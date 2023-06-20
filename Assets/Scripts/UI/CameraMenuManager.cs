using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuManager : MonoBehaviour
{
    [SerializeField] private CameraMenuMotion menuCameraMotion;
    [SerializeField] private Camera camera;

    public void SettingsButton()
    {
        menuCameraMotion.SwitchPositionCamera(2);
    }

    public void BackButton()
    {
        menuCameraMotion.SwitchPositionCamera(0);
        camera.orthographic = false;
    }

    public void PlayButton()
    {
        menuCameraMotion.SwitchPositionCamera(3);
    }

    public void CustomButton()
    {
        menuCameraMotion.SwitchPositionCamera(1);
        camera.orthographic = true;
    }

    public void CreditButton()
    {
        menuCameraMotion.SwitchPositionCamera(2);
    }
}
