using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {
	static BGM instance;
	AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		Debug.Log(source.clip.name);
		if(instance != null)
		{
			if(instance.source.clip == source.clip)
			{
				Destroy(gameObject);
				return;
			}
			Destroy(instance.gameObject);
		}
		instance = this;
		source.volume = VolumeControl.bgVol;
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		var c = Camera.main;
		if(c != null) transform.position = c.transform.position;
	}
	public static void SetVolume(float vol)
	{
		if(instance != null)
			instance.source.volume = vol;
	}
}
