using UnityEngine;
using System.Collections;
using LitJson;

public class PyramidMaker : MonoBehaviour {
	public string stageName;

	// Use this for initialization
	void Start () {
		var stage = Resources.Load<TextAsset>(stageName);
		if(!stage) return;
		// JsonMapper.ToObject(stage);
	}
}
