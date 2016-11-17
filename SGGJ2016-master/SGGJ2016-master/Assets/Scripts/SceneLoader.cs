using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	static SceneLoader _instance;
	static SceneLoader instance
	{
		get
		{
			if(_instance == null)
			{
				var original = Resources.Load("SceneLoadCurtain") as GameObject;
				_instance = Instantiate<GameObject>(original).GetComponent<SceneLoader>();
			}
			return _instance;
		}
	}
	public static void LoadScene(int sceneNumber)
	{
		if(instance.loadingScene) return;
		instance.gameObject.SetActive(true);
		instance.targetScene = sceneNumber;
		instance.StartCoroutine(instance.LoadSceneInstance());
	}
	[UnityEngine.HideInInspector]
	public bool loadingScene = false;
	public float fadeTime = 1.5f;
	int targetScene;
	Image img;
	IEnumerator LoadSceneInstance()
	{
		loadingScene = true;
		img.color = Color.clear;
		complete = false;
		img.DOColor(Color.black,fadeTime).OnComplete(() => complete = true);
		yield return StartCoroutine(WaitForFade());
		operation = SceneManager.LoadSceneAsync(targetScene);
		yield return StartCoroutine(WaitForLoad());
		complete = false;
		img.DOColor(Color.clear,fadeTime).OnComplete(() => complete = true);
		yield return StartCoroutine(WaitForFade());
		loadingScene = false;
		gameObject.SetActive(false);
	}
	bool complete;
	IEnumerator WaitForFade()
	{
		while(true)
		{
			yield return null;
			if(complete) yield break;
		}
	}
	AsyncOperation operation;
	IEnumerator WaitForLoad()
	{
		while(true)
		{
			yield return null;
			if(operation.isDone) yield break;
		}
	}

	// Use this for initialization
	void Awake () {
		img = GetComponentInChildren<Image>();
		DontDestroyOnLoad(gameObject);
	}
}
