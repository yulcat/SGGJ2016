using System.Collections.Generic;
using UnityEngine;
using AvoEx;
using System;
using System.Linq;

public static class SaveDataManager
{
    public class ClearData
    {
        public string scoreGuid;
        public int score;
        public int stars;
    }
    public class SaveData
    {
        public Dictionary<string, ClearData> clearRecord = new Dictionary<string, ClearData>();
        public DateTime lastHeartLocalTime;
        public DateTime lastHeartServerTime;
        public DateTime lastRefillLocalTime;
        public DateTime lastRefillServerTime;
        public bool timeInitialized = false;
        public int heartLeft;
        public float bgmVolume = 1f;
        public float seVolume = 1f;
        public int unlockedCharacters = 0;
        public int spentStars = 0;
    }
    private static SaveData _data;
    public static SaveData data
    {
        get
        {
            if (_data != null)
                return _data;
            if (PlayerPrefs.HasKey("data"))
            {
                _data = LoadData();
            }
            else
                _data = new SaveData();
            return _data;
        }
    }
    public static Dictionary<string, ClearData> clearRecord
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
            var data = LitJson.JsonMapper.ToObject<SaveData>(decrypted);
            VolumeControl.SetBGVolumeFromLoad(data.bgmVolume);
            VolumeControl.SetSEVolumeFromLoad(data.seVolume);
            return data;
        }
        catch
        {
            return new SaveData();
        }
    }
    public static bool IsCharacterAvailable(int index)
    {
        if (index == 0) return true;
        return ((1 << index) & data.unlockedCharacters) != 0;
    }
    public static void UnlockCharacter(int index)
    {
        data.unlockedCharacters |= 1 << index;
        Save();
    }
    public static void DebugAddStar(int count)
    {
        data.spentStars -= count;
        Save();
    }
    public static bool SpendStar(int count)
    {
        if (data.spentStars + count > data.clearRecord.Values.Sum(c => c.stars)) return false;
        data.spentStars += count;
        Save();
        return true;
    }
    public static bool BuyCharacter(int index)
    {
        if (!SpendStar(DB.characterDB[index].price)) return false;
        UnlockCharacter(index);
        return true;
    }
    public static void Save()
    {
        PlayerPrefs.SetString("data", AesEncryptor.Encrypt(LitJson.JsonMapper.ToJson(_data)));
    }
}
