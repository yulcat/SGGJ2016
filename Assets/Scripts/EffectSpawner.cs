using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EffectSpawner : MonoBehaviour
{
    static EffectSpawner instance;

    public static EffectSpawner Instance
    {
        get
        {
            if (instance != null) return instance;
            var obj = new GameObject("Effect Spawner");
            instance = obj.AddComponent<EffectSpawner>();
            return instance;
        }
    }

    readonly Dictionary<string, List<GameObject>> dic = new Dictionary<string, List<GameObject>>();

    public static GameObject GetEffect(string name)
    {
        return Instance.GetEffectInstance(name);
    }

    GameObject GetEffectInstance(string effectName)
    {
        if (!dic.ContainsKey(effectName))
        {
            dic.Add(effectName, new List<GameObject>());
        }
        var idle = dic[effectName].FirstOrDefault(o => !o.activeSelf);
        if (idle == null)
        {
            var original = Resources.Load<GameObject>(effectName);
            var obj = Instantiate(original);
            dic[effectName].Add(obj);
            obj.transform.SetParent(transform);
            return obj;
        }
        else return idle;
    }
}