using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageData : MonoBehaviour
{
    static Dictionary<string, string> _dictionary;

    public static Dictionary<string, string> dictionary
    {
        get
        {
            if (_dictionary == null)
            {
                var text = Resources.Load<TextAsset>("Data/messages");
                _dictionary = CSVLoader.ToDictionary<string>(text.text);
            }
            return _dictionary;
        }
    }
}