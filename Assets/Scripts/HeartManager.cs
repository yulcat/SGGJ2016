using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeartManager : MonoBehaviour
{
	static HeartManager _instance;
	const int MaxHeart = 10;
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
	public static void RefreshHeart()
	{
		if(instance.refreshProcessing) return;
		instance.StartCoroutine("RefreshHeartCount");
	}
	public void Initialize()
	{
		if(initialized) return;
		if(!SaveDataManager.data.timeInitialized)
		{
			StartCoroutine(InitializeFromServerTime());
		}
		StartCoroutine(RefreshHeartCount());
		initialized = true;
	}
	IEnumerator InitializeFromServerTime()
	{
		Debug.Log("trying to get time from server...");
		var www = new WWW("http://52.78.26.149/api/timer");
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.LogWarning(www.error);
		}
		SaveDataManager.data.lastHeartServerTime = DateTime.FromBinary(long.Parse(www.text));
		SaveDataManager.data.lastHeartLocalTime = DateTime.Now;
		SaveDataManager.data.timeInitialized = true;
		SaveDataManager.data.heartLeft = 0;
		Debug.Log("initialized time from server");
	}
	bool refreshProcessing = false;
	IEnumerator RefreshHeartCount()
	{
		refreshProcessing = true;
		var www = new WWW("http://52.78.26.149/api/timer");
		Debug.Log("trying to refresh time from server...");
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.LogWarning(www.error);
		}
		var serverTime = DateTime.FromBinary(long.Parse(www.text));
		Debug.Log("ServerTime : " + serverTime.ToLongTimeString());
		var savedServerTime = SaveDataManager.data.lastHeartServerTime;
		var targetServerTime = savedServerTime.AddSeconds(9);
		while(serverTime.CompareTo(targetServerTime)>0)
		{
			savedServerTime = SaveDataManager.data.lastHeartServerTime.AddSeconds(10);
			SaveDataManager.data.lastHeartServerTime = savedServerTime;
			targetServerTime = savedServerTime.AddSeconds(9);
			if(SaveDataManager.data.heartLeft >= MaxHeart)
			{
				break;
			}
			else
			{
				SaveDataManager.data.heartLeft++;
			}
		}
		var timeNow = DateTime.Now;
		var timeDiff = serverTime.Subtract(savedServerTime);
		SaveDataManager.data.lastHeartLocalTime = timeNow.Subtract(timeDiff);
		yield return null;
		refreshProcessing = false;
	}
}