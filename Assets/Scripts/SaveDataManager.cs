using System.Collections.Generic;
using UnityEngine;
using AvoEx;
using System;

public static class SaveDataManager {
	public class ClearData
	{
		public string scoreGuid;
		public int score;
		public int stars;
	}
	public class SaveData
	{
		public Dictionary<string,ClearData> clearRecord = new Dictionary<string,ClearData>();
		public DateTime lastHeartLocalTime;
		public DateTime lastHeartServerTime;
		public DateTime lastRefillLocalTime;
		public DateTime lastRefillServerTime;
		public bool timeInitialized = false;
		public int heartLeft;
	}
	private static SaveData _data;
	public static SaveData data
	{
		get
		{
			if(_data != null)
				return _data;
			if(PlayerPrefs.HasKey("data"))
			{
				_data = LoadData();
			}
			else
				_data = new SaveData();
			return _data;
		}
	}
	public static Dictionary<string,ClearData> clearRecord
	{
		get
		{
			return data.clearRecord;
		}
	}
    private static SaveData LoadData()
    {
		try
		{
        	var loadedString = PlayerPrefs.GetString("data");
			var decrypted = AesEncryptor.DecryptString(loadedString);
			return LitJson.JsonMapper.ToObject<SaveData>(decrypted);
		}
		catch
		{
			return new SaveData();
		}
    }
	public static void Save()
	{
		PlayerPrefs.SetString("data",AesEncryptor.Encrypt(LitJson.JsonMapper.ToJson(_data)));
	}
}
