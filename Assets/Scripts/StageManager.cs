using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {
	static StageManager _instance;
	public static StageManager instance
	{
		get
		{
			if(_instance != null) return _instance;
			var obj = new GameObject();
			_instance = obj.AddComponent<StageManager>();
			return _instance;
		}
	}
	void Awake()
	{
		if(_instance != null)
		{
			Destroy(this);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(gameObject);
	}
	public static void LoadStage(int stage)
	{
		instance.stageToLoad = stage;
		instance.StartCoroutine(instance.LoadStageCoroutine());
	}
	public static void ReloadCurrentStage()
	{
		instance.StartCoroutine(instance.LoadStageCoroutine());
	}
	public static void LoadNextStage()
	{
		instance.stageToLoad ++;
		instance.StartCoroutine(instance.LoadStageCoroutine());
	}
	int stageToLoad = -1;
	IEnumerator LoadStageCoroutine()
	{
		yield return SceneLoader.LoadScene(2);
		Debug.Log("try loading stage");
		var builder = FindObjectOfType<PyramidBuilder>();
		if(stageToLoad != -1)
			builder.stageToLoad = stageToLoad;
		builder.LoadStage();
	}

}
