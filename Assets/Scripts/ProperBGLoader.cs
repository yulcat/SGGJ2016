using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProperBGLoader : MonoBehaviour {
    internal void LoadBG(string path)
    {
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Screenshots/"+path);
    }
}
