using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {
	static BGM instance;

	// Use this for initialization
	void Start () {
		if(instance != null)
			Destroy(gameObject);
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		var c = Camera.main;
		if(c != null) transform.position = c.transform.position;
	}
}
