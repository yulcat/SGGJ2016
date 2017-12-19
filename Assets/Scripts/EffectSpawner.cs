using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EffectSpawner : MonoBehaviour
{
    static EffectSpawner _instance;

    public static EffectSpawner instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("Effect Spawner");
                _instance = obj.AddComponent<EffectSpawner>();
            }
            return _instance;
        }
    }

    Dictionary<string, List<GameObject>> dic = new Dictionary<string, List<GameObject>>();

    public static GameObject GetEffect(string name)
    {
        return instance.GetEffectInstance(name);
    }

    GameObject GetEffectInstance(string name)
    {
        if (!dic.ContainsKey(name))
        {
            dic.Add(name, new List<GameObject>());
        }
        var idle = dic[name].FirstOrDefault(o => !o.activeSelf);
        if (idle == null)
        {
            var original = Resources.Load<GameObject>(name);
            var obj = Instantiate<GameObject>(original);
            dic[name].Add(obj);
            obj.transform.SetParent(transform);
            return obj;
        }
        else return idle;
    }
}