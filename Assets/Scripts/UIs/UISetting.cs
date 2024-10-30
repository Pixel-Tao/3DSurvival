using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    public GameObject settingWindow;
    public Slider lookSensitivity;

    private PlayerController controller;

    private void Start()
    {
        lookSensitivity.value = CharacterManager.Instance.Player.controller.lookSensitivity;
        controller = CharacterManager.Instance.Player.controller;
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
        UIManager.Instance.Close<UISetting>();
        UIManager.Instance.LockCursor(false);
    }
}
