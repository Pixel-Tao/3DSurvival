using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            InitManager();
            return _instance;
        }
    }

    private static void InitManager()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
