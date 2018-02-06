using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class HeartAdvertise
{
    public TimeSpan? TimeLeftToShowAd()
    {
        if (HeartManager.adAvailable) return null;
        var now = DateTime.Now;
        var last = SaveDataManager.data.lastRefillLocalTime;
        var target = last.AddMinutes(HeartManager.AdRefillMinutes);
        if (target < now)
        {
            HeartManager.CheckAd();
        }
        return target.Subtract(now);
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            HeartManager.AdShowed();
            var options = new ShowOptions {resultCallback = HandleShowResult};
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                HeartManager.AddHeart(10);
                var finishedText = DB.MessageDB["heart_get"];
                finishedText = string.Format(finishedText, 10);
                WindowPop.Open(finishedText);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                HeartManager.AddHeart(3);
                var skippedText = DB.MessageDB["ad_skipped"];
                finishedText = string.Format(skippedText, 3);
                WindowPop.Open(finishedText);
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                WindowPop.Open(DB.MessageDB["ad_failed"]);
                break;
        }
    }
}