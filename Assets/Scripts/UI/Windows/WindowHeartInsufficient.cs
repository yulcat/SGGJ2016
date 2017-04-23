using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class WindowHeartInsufficient : Window
{
    public static void Open()
    {
        var canvas = FindObjectOfType<Canvas>();
        var instanciated = canvas.GetComponentInChildren<WindowHeartInsufficient>(true);
        if (instanciated != null)
        {
            instanciated.OpenWindow();
            return;
        }
        var heartWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_notice_Heart"));
        heartWindow.transform.SetParent(canvas.transform, false);
        heartWindow.GetComponent<WindowHeartInsufficient>().OpenWindow();
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}

