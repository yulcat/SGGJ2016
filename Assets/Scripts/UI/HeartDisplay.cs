using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeartDisplay : MonoBehaviour
{
    public GameObject plusButton;
    public Text heartCount;
    public Text timeLeft;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        HeartManager.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SaveDataManager.data.timeInitialized) return;
        if (HeartManager.HeartIsMax)
        {
            heartCount.text = SaveDataManager.data.heartLeft.ToString();
            timeLeft.text = "MAX";
            return;
        }
        var timeNow = DateTime.Now;
        var timeToNewHeart = SaveDataManager.data.lastHeartLocalTime.AddMinutes(HeartManager.HeartRefillMinutes)
            .Subtract(timeNow);
        if (timeToNewHeart.CompareTo(TimeSpan.Zero) <= 0)
        {
            timeLeft.text = "0:00";
            HeartManager.RefreshHeart();
        }
        else
        {
            timeLeft.text = string.Format("{0}:{1}", timeToNewHeart.Minutes, timeToNewHeart.Seconds.ToString("00"));
        }
        heartCount.text = SaveDataManager.data.heartLeft.ToString();
    }
}