using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class HeartAdvertise {
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
                HeartManager.AddHeart(10);
                var finishedText = MessageData.dictionary["heart_get"];
                finishedText = string.Format(finishedText,10);
                WindowPop.Open(finishedText);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                HeartManager.AddHeart(3);
                var skippedText = MessageData.dictionary["ad_skipped"];
                finishedText = string.Format(skippedText,3);
                WindowPop.Open(finishedText);
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                WindowPop.Open(MessageData.dictionary["ad_failed"]);
                break;
        }
    }
}
