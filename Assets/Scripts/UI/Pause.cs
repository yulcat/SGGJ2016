using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : Window
{
    public static bool Paused { get; set; }

    public Slider bgmSlider;
    public Slider sfxSlider;
    static Pause instanciated;

    public static void Open()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var newWindow = Instantiate(SceneManager.GetActiveScene().name == "StageSelect"
            ? Resources.Load<GameObject>("UI/Window_setting")
            : Resources.Load<GameObject>("UI/Window_menu"));
        newWindow.transform.SetParent(canvas.transform, false);
        instanciated = newWindow.GetComponent<Pause>();
        instanciated.OpenWindow();
    }

    [UsedImplicitly]
    public void ReloadCurrentStage()
    {
        BackToPrevWindow();
        if (HeartManager.HeartLeft < GameState.GetHeartCost())
        {
            WindowHeartInsufficient.Open();
            return;
        }
        HeartManager.SpendHeart();
        StageManager.ReloadCurrentStage();
    }

    [UsedImplicitly]
    public void ToStageSelect()
    {
        BackToPrevWindow();
        StageManager.LoadStageSelectScene();
    }

    [UsedImplicitly]
    public void SetBGVolume(Slider slider)
    {
        VolumeControl.SetBGVolume(slider.value);
    }

    [UsedImplicitly]
    public void SetSFXVolume(Slider slider)
    {
        VolumeControl.SetSEVolume(slider.value);
    }

    [UsedImplicitly]
    public void ShowCredit()
    {
        WindowCredit.OpenCredit();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
        Paused = true;
        if (bgmSlider != null) bgmSlider.value = VolumeControl.bgVol;
        if (sfxSlider != null) sfxSlider.value = VolumeControl.seVol;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Time.timeScale = 1f;
        Paused = false;
    }
}