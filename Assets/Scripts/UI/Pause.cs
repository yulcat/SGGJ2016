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
        GameObject newWindow;
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StageSelect")
            newWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_setting"));
        else
            newWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_menu"));
        newWindow.transform.SetParent(canvas.transform, false);
        instanciated = newWindow.GetComponent<Pause>();
        instanciated.OpenWindow();
    }
    public override void BackToPrevWindow()
    {
        transform.DOScale(Vector2.zero, 0.3f)
			.SetEase(Ease.InBack)
            .SetUpdate(true)
			.OnComplete(() => base.BackToPrevWindow());
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
    public void SetBGVolume(UnityEngine.UI.Slider slider)
    {
        Debug.Log(slider.value);
    }
    public void SetSFXVolume(UnityEngine.UI.Slider slider)
    {
        
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
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Time.timeScale = 1f;
        gamePaused = false;
    }
}
