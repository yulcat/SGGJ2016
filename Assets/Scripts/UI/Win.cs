using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {
	bool active;
	void Start()
	{
		Invoke("Activate",1f);
	}
	void Activate()
	{
		active = true;
	}
	// Update is called once per frame
	void Update () {
		if(!active) return;
		if(Input.GetMouseButtonDown(0))
		{
			var current = SceneManager.GetActiveScene().buildIndex;
			if(current == SceneManager.sceneCountInBuildSettings -1)
				SceneLoader.LoadScene(0);
			else
				SceneLoader.LoadScene(current+1);
		}
	}
}
