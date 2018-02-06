using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            var obj = new GameObject();
            instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
            return instance;
        }
    }
}