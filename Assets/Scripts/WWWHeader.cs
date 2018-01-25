using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWHeader : MonoBehaviour
{
    // Use this for initialization
    IEnumerator Start()
    {
        var www = new WWW("http://google.com/robots.txt");
        yield return www;
        var time = www.responseHeaders["DATE"];
        var strings = time.Split(',');
        Debug.Log(strings[strings.Length - 1]);
        var dt = System.Convert.ToDateTime(strings[strings.Length - 1]);
        Debug.Log(dt);
    }
}