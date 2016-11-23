using UnityEngine;
using System.Collections;
using LitJson;

public class PyramidBuilder : MonoBehaviour {
	public string stageToLoad;

	// Use this for initialization
	void Start () {
		var stage = Resources.Load<TextAsset>(stageToLoad);
		if(!stage) return;
		// JsonMapper.ToObject(stage);
	}
}
