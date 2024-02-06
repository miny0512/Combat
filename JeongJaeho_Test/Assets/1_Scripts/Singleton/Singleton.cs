using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject($"@{typeof(T).Name}");
                    _instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    protected void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
            return;
        }

        _instance = GetComponent<T>();
        DontDestroyOnLoad(_instance);
    }
}
