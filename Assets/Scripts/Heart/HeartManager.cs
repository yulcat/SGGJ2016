using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeartManager : MonoBehaviour
{
	static HeartManager _instance;
	const int maxHeart = 24;
	public const double heartRefillMinutes = 5;
	const int heartRefillCount = 4;
	bool initialized = false;
	public static HeartManager instance
	{
		get
		{
			if(_instance == null)
			{
				var go = new GameObject("HeartManager");
				DontDestroyOnLoad(go);
				_instance = go.AddComponent<HeartManager>();
			}
			return _instance;
		}
	}
	public static int heartLeft
	{
		get
		{
			return SaveDataManager.data.heartLeft;
		}
	}
	public static bool heartIsMax
	{
		get
		{
			return heartLeft >= maxHeart;
		}
	}
	public static void SpendHeart()
	{
		if(heartLeft <= 0)
		{
			throw new System.Exception("Spending Heart when Heart is zero!");
		}
		if(heartIsMax)
		{
			instance.StartCoroutine("SpendHeartFromMax");
			return;
		}
		SaveDataManager.data.heartLeft--;
		RefreshHeart();
	}
	public static void AddHeart()
	{
		SaveDataManager.data.heartLeft++;
		RefreshHeart();
	}
	public static void AddHeart(int count)
	{
		SaveDataManager.data.heartLeft+=count;
		RefreshHeart();
	}

	public static Coroutine RefreshHeart()
	{
		if(instance.refreshProcessing) return null;
		return instance.StartCoroutine("RefreshHeartCount");
	}
	public void Initialize()
	{
		if(initialized) return;
		if(!SaveDataManager.data.timeInitialized)
		{
			StartCoroutine(InitializeFromServerTime());
			SaveDataManager.data.heartLeft = maxHeart;
		}
		StartCoroutine(RefreshHeartCount());
		initialized = true;
	}
	IEnumerator SpendHeartFromMax()
	{
		yield return StartCoroutine(InitializeFromServerTime());
		SaveDataManager.data.heartLeft--;
	}
	IEnumerator InitializeFromServerTime()
	{
		var www = new WWW("http://google.com/robots.txt");
		Debug.Log("trying to refresh time from server...");
		yield return www;
		var time = www.responseHeaders["DATE"];
		var strings = time.Split(',');
		SaveDataManager.data.lastHeartServerTime = System.Convert.ToDateTime(strings[strings.Length - 1]);
		SaveDataManager.data.lastHeartLocalTime = DateTime.Now;
		SaveDataManager.data.timeInitialized = true;
		Debug.Log("initialized time from server");
	}
	bool refreshProcessing = false;
	IEnumerator RefreshHeartCount()
	{
		refreshProcessing = true;
		var www = new WWW("http://google.com/robots.txt");
		Debug.Log("trying to refresh time from server...");
		yield return www;
		var time = www.responseHeaders["DATE"];
		var strings = time.Split(',');
		var serverTime = System.Convert.ToDateTime(strings[strings.Length - 1]);
		Debug.Log("ServerTime : " + serverTime.ToLongTimeString());
		var savedServerTime = SaveDataManager.data.lastHeartServerTime;
		var targetServerTime = savedServerTime.AddMinutes(heartRefillMinutes * 0.99);
		while(serverTime.CompareTo(targetServerTime)>0)
		{
			savedServerTime = SaveDataManager.data.lastHeartServerTime.AddMinutes(heartRefillMinutes);
			SaveDataManager.data.lastHeartServerTime = savedServerTime;
			targetServerTime = savedServerTime.AddMinutes(heartRefillMinutes * 0.99);
			if(heartIsMax)
			{
				break;
			}
			else
			{
				SaveDataManager.data.heartLeft += heartRefillCount;
			}
		}
		var timeNow = DateTime.Now;
		var timeDiff = serverTime.Subtract(savedServerTime);
		SaveDataManager.data.lastHeartLocalTime = timeNow.Subtract(timeDiff);
		yield return null;
		refreshProcessing = false;
	}
	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		SaveDataManager.Save();
	}
}