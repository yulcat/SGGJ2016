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
    public UnityEngine.UI.Slider BGMSlider;
    public UnityEngine.UI.Slider SFXSlider;
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
        VolumeControl.SetBGVolume(slider.value);
    }
    public void SetSFXVolume(UnityEngine.UI.Slider slider)
    {
        VolumeControl.SetSEVolume(slider.value);
    }
    public void ShowCredit()
    {
        WindowCredit.OpenCredit();
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    override protected void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
        gamePaused = true;
        if(BGMSlider != null) BGMSlider.value = VolumeControl.bgVol;
        if(SFXSlider != null) SFXSlider.value = VolumeControl.seVol;
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
