using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class WindowHeartInsufficient : Window
{
    public Button showAdButton;
    public Text showAdText;
    static WindowHeartInsufficient instanciated;
    readonly HeartAdvertise advertise = new HeartAdvertise();

    public static void Open()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var heartWindow = Instantiate(Resources.Load<GameObject>("UI/Window_notice_Heart"));
        heartWindow.transform.SetParent(canvas.transform, false);
        instanciated = heartWindow.GetComponent<WindowHeartInsufficient>();
        instanciated.OpenWindow();
    }

    [UsedImplicitly]
    public void ShowRewardedAd()
    {
        advertise.ShowRewardedAd();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        var timeLeft = advertise.TimeLeftToShowAd();
        if (!timeLeft.HasValue)
        {
            showAdButton.interactable = true;
            showAdText.text = string.Empty;
            return;
        }
        var timevalue = timeLeft.Value;
        showAdButton.interactable = false;
        showAdText.text = timevalue.CompareTo(TimeSpan.Zero) <= 0
            ? "0:00"
            : $"{timevalue.Minutes}:{timevalue.Seconds:00}";
    }
}