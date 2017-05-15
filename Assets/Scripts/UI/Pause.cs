using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Pause : Window {
    public static bool paused
    {
        get
        {
            return gamePaused;
        }
    }
    static Pause instanciated;
    static bool gamePaused;
    public static void Open()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var heartWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_menu"));
        heartWindow.transform.SetParent(canvas.transform, false);
        instanciated = heartWindow.GetComponent<Pause>();
        instanciated.OpenWindow();
    }
    public override void BackToPrevWindow()
    {
        transform.DOScale(Vector2.zero, 0.3f)
			.SetEase(Ease.InBack)
			.OnComplete(() => base.BackToPrevWindow());
        Time.timeScale = 1f;
        gamePaused = false;
    }
    public void ReloadCurrentStage()
	{
        BackToPrevWindow();
		if(HeartManager.heartLeft <= 0)
		{
			WindowHeartInsufficient.Open();
			return;
		}
		HeartManager.SpendHeart();
		StageManager.ReloadCurrentStage();
	}
    public void ToStageSelect()
	{
        BackToPrevWindow();
		StageManager.LoadStageSelectScene();
	}
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    override protected void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
        gamePaused = true;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            BackToPrevWindow();
        }
    }
}
