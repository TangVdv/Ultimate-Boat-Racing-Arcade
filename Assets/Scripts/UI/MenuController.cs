using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private MenuCameraMotion _menuCameraMotion;
    private void Awake()
    {
        _menuCameraMotion = Camera.main.GetComponent<MenuCameraMotion>();
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
        _menuCameraMotion.SwitchPositionCamera(1);
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
