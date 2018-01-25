using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public enum Theme
    {
        Sand = 0,
        Ice = 1,
        Grass = 2,
        Rock = 3,
        Common = -1
    };

    string[] sceneNames = {"Scene_Desert01", "Scene_Ice01", "Stage_Grass01", "Scene_Rocks01"};
    static StageManager _instance;

    public static StageManager instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = new GameObject("StageManager");
            _instance = obj.AddComponent<StageManager>();
            return _instance;
        }
    }

    public static bool IsInitialized()
    {
        return _instance != null;
    }

    void Awake()
    {
        if (_instance != null)
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
        instance.stageToLoad++;
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
        instance.stageToLoad++;
        instance.StartCoroutine(instance.LoadStageSelectCoroutine());
    }

    public int stageToLoad = -1;

    IEnumerator LoadStageCoroutine()
    {
        var theme = StageDataLoader.GetStageData(stageToLoad).theme;
        var sceneName = sceneNames[(int) theme];
        Debug.Log("try loading stage : " + sceneName);
        yield return SceneLoader.LoadSceneByName(sceneName);
        var builder = FindObjectOfType<PyramidBuilder>();
        if (stageToLoad != -1)
            builder.stageToLoad = stageToLoad;
        builder.LoadStage();
    }

    bool openStartWindow;

    IEnumerator LoadStageSelectCoroutine()
    {
        yield return SceneLoader.LoadScene(0);
        if (stageToLoad != -1)
        {
            var scroll = FindObjectOfType<InfiniteScroll>();
            scroll.JumpToStage(stageToLoad, openStartWindow);
        }
    }
}