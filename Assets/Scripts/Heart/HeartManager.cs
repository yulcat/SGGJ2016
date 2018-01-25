using System;
using System.Collections;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    static HeartManager instance;
    const int MaxHeart = 24;
    public const double HeartRefillMinutes = 5;
    const int HeartRefillCount = 4;
    public const double AdRefillMinutes = 5;
    bool initialized;
    public static bool adAvailable;

    public static HeartManager Instance
    {
        get
        {
            if (instance != null) return instance;
            var go = new GameObject("HeartManager");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<HeartManager>();
            return instance;
        }
    }

    public static int HeartLeft => SaveDataManager.data.heartLeft;

    public static bool HeartIsMax => HeartLeft >= MaxHeart;

    public static void SpendHeart()
    {
        var heartCost = DB.characterDB[(int) GameState.selectedCharacter].heartCost;
        SpendHeart(heartCost);
    }

    public static void SpendHeart(int count)
    {
        if (HeartLeft < count)
        {
            throw new Exception("Spending Heart when Heart is zero!");
        }
        if (HeartIsMax)
        {
            Instance.StartCoroutine(Instance.SpendHeartFromMax(count));
            return;
        }
        SaveDataManager.data.heartLeft -= count;
        RefreshHeart();
    }

    public static void AddHeart(int count = 1)
    {
        SaveDataManager.data.heartLeft += count;
        RefreshHeart();
    }

    public static Coroutine AdShowed()
    {
        return Instance.StartCoroutine(nameof(AfterAdShowed));
    }

    public static Coroutine CheckAd()
    {
        return Instance.StartCoroutine(nameof(CheckAdAvailable));
    }

    public static Coroutine RefreshHeart()
    {
        return Instance.refreshProcessing ? null : Instance.StartCoroutine(nameof(RefreshHeartCount));
    }

    public void Initialize()
    {
        if (initialized) return;
        if (!SaveDataManager.data.timeInitialized)
        {
            StartCoroutine(InitializeFromServerTime(true));
            SaveDataManager.data.heartLeft = MaxHeart;
        }
        StartCoroutine(RefreshHeartCount());
        initialized = true;
    }

    IEnumerator SpendHeartFromMax(int count)
    {
        yield return StartCoroutine(InitializeFromServerTime(false));
        SaveDataManager.data.heartLeft -= count;
    }

    IEnumerator InitializeFromServerTime(bool resetAdRefill)
    {
        var www = new WWW("http://google.com/robots.txt");
        Debug.Log("trying to refresh time from server...");
        yield return www;
        var time = www.responseHeaders["DATE"];
        var strings = time.Split(',');
        SaveDataManager.data.lastHeartServerTime = Convert.ToDateTime(strings[strings.Length - 1]);
        SaveDataManager.data.lastHeartLocalTime = DateTime.Now;
        if (resetAdRefill)
        {
            SaveDataManager.data.lastRefillServerTime =
                SaveDataManager.data.lastHeartServerTime.AddMinutes(-AdRefillMinutes);
            SaveDataManager.data.lastRefillLocalTime =
                SaveDataManager.data.lastHeartLocalTime.AddMinutes(-AdRefillMinutes);
        }
        SaveDataManager.data.timeInitialized = true;
        Debug.Log("initialized time from server");
    }

    bool refreshProcessing;

    IEnumerator RefreshHeartCount()
    {
        refreshProcessing = true;
        var www = new WWW("http://google.com/robots.txt");
        Debug.Log("trying to refresh time from server...");
        yield return www;
        var serverTime = ParseResponse(www);
        Debug.Log("ServerTime : " + serverTime.ToLongTimeString());
        var savedServerTime = SaveDataManager.data.lastHeartServerTime;
        var targetServerTime = savedServerTime.AddMinutes(HeartRefillMinutes * 0.99);
        while (serverTime.CompareTo(targetServerTime) > 0)
        {
            savedServerTime = SaveDataManager.data.lastHeartServerTime.AddMinutes(HeartRefillMinutes);
            SaveDataManager.data.lastHeartServerTime = savedServerTime;
            targetServerTime = savedServerTime.AddMinutes(HeartRefillMinutes * 0.99);
            if (HeartIsMax)
            {
                break;
            }
            SaveDataManager.data.heartLeft += HeartRefillCount;
        }
        var timeNow = DateTime.Now;
        var timeDiff = serverTime.Subtract(savedServerTime);
        SaveDataManager.data.lastHeartLocalTime = timeNow.Subtract(timeDiff);
        yield return null;
        refreshProcessing = false;
    }

    bool checkingAd;

    IEnumerator CheckAdAvailable()
    {
        if (checkingAd) yield break;
        checkingAd = true;
        var www = new WWW("http://google.com/robots.txt");
        Debug.Log("trying to refresh time from server...");
        yield return www;
        var serverTime = ParseResponse(www);
        var targetTime = SaveDataManager.data.lastRefillServerTime.AddMinutes(AdRefillMinutes);
        adAvailable = serverTime > targetTime;
        if (!adAvailable)
        {
            var timeDiff = targetTime.Subtract(serverTime);
            SaveDataManager.data.lastRefillLocalTime = DateTime.Now.AddMinutes(-AdRefillMinutes).Add(timeDiff);
        }
        checkingAd = false;
    }

    IEnumerator AfterAdShowed()
    {
        adAvailable = false;
        SaveDataManager.data.lastRefillLocalTime = DateTime.Now;
        var www = new WWW("http://google.com/robots.txt");
        Debug.Log("trying to refresh time from server...");
        yield return www;
        var serverTime = ParseResponse(www);
        SaveDataManager.data.lastRefillServerTime = serverTime;
    }

    DateTime ParseResponse(WWW response)
    {
        var time = response.responseHeaders["DATE"];
        var strings = time.Split(',');
        return Convert.ToDateTime(strings[strings.Length - 1]);
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        SaveDataManager.Save();
    }

    /// <summary>
    /// Callback sent to all game objects when the player gets or loses focus.
    /// </summary>
    /// <param name="focusStatus">The focus state of the application.</param>
    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus) return;
        SaveDataManager.Save();
    }

    /// <summary>
    /// Callback sent to all game objects when the player pauses.
    /// </summary>
    /// <param name="pauseStatus">The pause state of the application.</param>
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) return;
        SaveDataManager.Save();
    }
}