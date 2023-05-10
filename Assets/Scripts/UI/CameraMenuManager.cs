using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuManager : MonoBehaviour
{
    private CameraMenuMotion _menuCameraMotion;
    private void Awake()
    {
        _menuCameraMotion = Camera.main.GetComponent<CameraMenuMotion>();
    }

    public void SettingsButton()
    {
        _menuCameraMotion.SwitchPositionCamera(2);
    }

    public void BackButton()
    {
        _menuCameraMotion.SwitchPositionCamera(0);
    }

    public void PlayButton()
    {
        _menuCameraMotion.SwitchPositionCamera(3);
    }

    public void CustomButton()
    {
        _menuCameraMotion.SwitchPositionCamera(1);
    }

    public void CreditButton()
    {
        _menuCameraMotion.SwitchPositionCamera(2);
    }
}
