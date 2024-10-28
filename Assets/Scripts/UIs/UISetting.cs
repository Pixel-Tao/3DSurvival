using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public GameObject settingWindow;
    public Slider lookSensitivity;

    private PlayerController controller;

    private void Start()
    {
        lookSensitivity.value = CharacterManager.Instance.Player.controller.lookSensitivity;
        controller = CharacterManager.Instance.Player.controller;
        controller.setting += Toggle;
        settingWindow.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            settingWindow.SetActive(false);
        }
        else
        {
            settingWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return settingWindow.activeInHierarchy;
    }

    public void OnLookSensitivityChanged(float value)
    {
        CharacterManager.Instance.Player.controller.SetLookSensitivity(value);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnCloseClick()
    {
        controller.ToggleSetting();
    }
}
