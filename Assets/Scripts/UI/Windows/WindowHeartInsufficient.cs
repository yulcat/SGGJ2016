using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowHeartInsufficient : Window
{
    public Button showAdButton;
    public Text showAdText;
    static WindowHeartInsufficient instanciated;
    HeartAdvertise advertise = new HeartAdvertise();

    public static void Open()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var heartWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_notice_Heart"));
        heartWindow.transform.SetParent(canvas.transform, false);
        instanciated = heartWindow.GetComponent<WindowHeartInsufficient>();
        instanciated.OpenWindow();
    }

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
        if (timevalue.CompareTo(TimeSpan.Zero) <= 0)
        {
            showAdText.text = "0:00";
        }
        else
        {
            showAdText.text = string.Format("{0}:{1}", timevalue.Minutes, timevalue.Seconds.ToString("00"));
        }
    }
}