using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {
	enum Theme { Sand = 3, Ice = 4, Grass = 5 };
	static StageManager _instance;
	public static StageManager instance
	{
		get
		{
			if(_instance != null) return _instance;
			var obj = new GameObject("StageManager");
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
		var builder = FindObjectOfType<PyramidBuilder>();
		instance.stageToLoad = builder.stageToLoad;
		instance.StartCoroutine(instance.LoadStageCoroutine());
	}
	public static void LoadNextStage()
	{
		instance.stageToLoad ++;
		instance.StartCoroutine(instance.LoadStageCoroutine());
	}
	public static void LoadStageSelectScene()
	{
		instance.openStartWindow = false;
		instance.StartCoroutine(instance.LoadStageSelectCoroutine());
	}
	public static void LoadNextStageSelectScene()
	{
		instance.openStartWindow = true;
		instance.stageToLoad ++;
		instance.StartCoroutine(instance.LoadStageSelectCoroutine());
	}
	int stageToLoad = -1;
	IEnumerator LoadStageCoroutine()
	{
		var theme = StageDataLoader.GetStageData(stageToLoad).theme;
		int sceneNumber = (int)System.Enum.Parse(typeof(Theme),theme);
		yield return SceneLoader.LoadScene(sceneNumber);
		Debug.Log("try loading stage");
		var builder = FindObjectOfType<PyramidBuilder>();
		if(stageToLoad != -1)
			builder.stageToLoad = stageToLoad;
		builder.LoadStage();
	}
	bool openStartWindow;
	IEnumerator LoadStageSelectCoroutine()
	{
		yield return SceneLoader.LoadScene(1);
		if(stageToLoad != -1)
		{
			var scroll = FindObjectOfType<InfiniteScroll>();
			scroll.JumpToStage(stageToLoad, openStartWindow);
		}
	}
}
