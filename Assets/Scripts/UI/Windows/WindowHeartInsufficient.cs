using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class WindowHeartInsufficient : Window
{
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
}

