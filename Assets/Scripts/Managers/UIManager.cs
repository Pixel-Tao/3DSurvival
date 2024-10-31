using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : ManagerSingleton<UIManager>
{
    Dictionary<string, UIBase> uiDict = new Dictionary<string, UIBase>();

    public bool CanLook { get; private set; } = true;

    private T Generate<T>() where T : UIBase
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/UI/{typeof(T).Name}.prefab");
        GameObject go = Instantiate(prefab);
        go.name = typeof(T).Name;
        UIBase uiBase = go.GetComponent<UIBase>();
        uiDict.Add(go.name, uiBase);

        return uiBase as T;
    }

    // UIBaseType == T == Clase Name

    public T Open<T>() where T : UIBase
    {
        Type type = typeof(T);
        if (!uiDict.TryGetValue(type.Name, out UIBase ui))
        {
            ui = Generate<T>();
        }

        ui.gameObject.SetActive(true);

        return ui as T;
    }

    public void Close<T>() where T : UIBase
    {
        if (uiDict.TryGetValue(typeof(T).Name, out UIBase ui))
        {
            ui.gameObject.SetActive(false);
        }
    }

    public T Get<T>() where T : UIBase
    {
        if (uiDict.TryGetValue(typeof(T).Name, out UIBase ui))
        {
            return ui as T;
        }

        return null;
    }

    public bool IsOpen<T>() where T : UIBase
    {
        if (uiDict.TryGetValue(typeof(T).Name, out UIBase ui))
        {
            return ui.gameObject.activeInHierarchy;
        }

        return false;
    }

    public void Toggle<T>(bool cursorLock = false) where T : UIBase
    {
        if (IsOpen<T>())
        {
            Close<T>();
            if (cursorLock) LockCursor(false);
        }
        else
        {
            Open<T>();
            if (cursorLock) LockCursor(true);
        }
    }

    public void LockCursor(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.None : CursorLockMode.Locked;
        CanLook = Cursor.lockState == CursorLockMode.Locked;
    }
}
