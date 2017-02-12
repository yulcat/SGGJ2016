using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AvoEx;

public static class SaveDataManager {
	public class ClearData
	{
		public string scoreGuid;
		public int score;
		public int stars;
	}
	public class SaveData
	{
		public Dictionary<int,ClearData> clearRecord = new Dictionary<int,ClearData>();
	}
	private static SaveData _data;
	public static SaveData data
	{
		get
		{
			if(_data != null)
				return _data;
			if(PlayerPrefs.HasKey("data"))
				_data = LoadData();
			else
				_data = new SaveData();
			return _data;
		}
	}
	public static Dictionary<int,ClearData> clearRecord
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
		PlayerPrefs.SetString("data",AesEncryptor.Encrypt(LitJson.JsonMapper.ToJson(data)));
	}
}
