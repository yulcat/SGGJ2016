using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Linq;

[ExecuteInEditMode]
public class DataLoader : MonoBehaviour
{
    Dictionary<string, int> downloadTime = new Dictionary<string, int>();
    static DataLoader _instance;

    static DataLoader instance
    {
        get
        {
            if (_instance != null) return _instance;
            var go = new GameObject("DataLoader");
            _instance = go.AddComponent<DataLoader>();
            return _instance;
        }
    }

    public static void RequestLoad(IDataStorage db, int sheetId)
        => instance.StartCoroutine(instance.LoadFromWeb(db, sheetId));

    IEnumerator LoadFromWeb(IDataStorage db, int sheetId)
    {
        if (!IsDownloadNeeded(sheetId)) yield break;
        var www = new WWW(
            $@"https://docs.google.com/spreadsheets/d/1gXAkxrZYqdVe9ALyeMdLY1Umd1jW3QWtPiA9FBF40GA/export?format=csv&gid={
                    sheetId
                }");
        yield return www;
        db.SetData(www.text);
        downloadTime[sheetId.ToString()] = GetUnixEpoch(DateTime.UtcNow);
        PlayerPrefs.SetString("downloadTime", JsonMapper.ToJson(downloadTime));
        Debug.Log($"Loaded from web : {sheetId} ({DateTime.UtcNow})");
    }

    bool IsDownloadNeeded(int sheetId)
    {
        if (!PlayerPrefs.HasKey("downloadTime"))
            return true;
        if (downloadTime == null)
            downloadTime = JsonMapper.ToObject<Dictionary<string, int>>(PlayerPrefs.GetString("downloadTime"));
        if (!downloadTime.ContainsKey(sheetId.ToString()))
            return true;
        var savedTime = downloadTime[sheetId.ToString()];
        var targetTime = GetUnixEpoch(DateTime.UtcNow + TimeSpan.FromHours(5));
        return savedTime > targetTime;
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    private static int GetUnixEpoch(DateTime dateTime)
    {
        var unixTime = dateTime.ToUniversalTime() -
                       new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return (int) unixTime.TotalSeconds;
    }
}

public interface IDataStorage
{
    void SetData(string value);
}

public abstract class DictionaryData<TValue, TSelf> :
    IDataStorage,
    IEnumerable<KeyValuePair<string, TValue>> where TSelf : DictionaryData<TValue, TSelf>, new()
{
    private Dictionary<string, TValue> dic;
    protected string dataPath;
    private string path => System.IO.Path.Combine(Application.persistentDataPath, dataPath);

    public TValue this[string key]
    {
        get
        {
            if (dic != null) return dic[key];
            var appPath = Application.persistentDataPath;
            var read = System.IO.File.ReadAllText(path);
            dic = CSVToDictionary(read);
            return dic[key];
        }
    }

    public bool ContainsKey(string key)
    {
        if (dic == null)
        {
            var read = System.IO.File.ReadAllText(path);
            dic = CSVToDictionary(read);
        }
        return dic.ContainsKey(key);
    }

    protected virtual Dictionary<string, TValue> CSVToDictionary(string csv)
    {
        return CSVLoader.ToDictionary<TValue>(csv);
    }

    public void SetData(string value)
    {
        dic = CSVToDictionary(value);
        Debug.Log($"Writing File By : {this.GetType().Name} At : {Application.persistentDataPath}");
        if (!System.IO.Directory.Exists(Application.persistentDataPath))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath);
        System.IO.File.WriteAllText(path, value);
    }

    protected void Load(int sheetId)
    {
        DataLoader.RequestLoad(this, sheetId);
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        if (dic == null)
        {
            var read = System.IO.File.ReadAllText(path);
            dic = CSVToDictionary(read);
        }
        foreach (var d in dic)
        {
            yield return d;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public abstract class ListData<TValue, TSelf> : IDataStorage, IEnumerable<TValue>
    where TSelf : ListData<TValue, TSelf>, new()
{
    private List<TValue> list;
    protected string dataPath;
    private string path => System.IO.Path.Combine(Application.persistentDataPath, dataPath);

    public TValue this[int index]
    {
        get
        {
            if (list != null) return list[index];
            var appPath = Application.persistentDataPath;
            var read = System.IO.File.ReadAllText(path);
            list = CSVToTValue(read);
            return list[index];
        }
    }

    protected virtual List<TValue> CSVToTValue(string csv)
    {
        return CSVLoader.ToList<TValue>(csv);
    }

    public void SetData(string value)
    {
        list = CSVToTValue(value);
        Debug.Log($"Writing File By : {this.GetType().Name} At : {Application.persistentDataPath}");
        if (!System.IO.Directory.Exists(Application.persistentDataPath))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath);
        System.IO.File.WriteAllText(path, value);
    }

    protected void Load(int sheetId)
    {
        DataLoader.RequestLoad(this, sheetId);
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        if (list == null)
        {
            var read = System.IO.File.ReadAllText(path);
            list = CSVToTValue(read);
        }
        foreach (var l in list)
        {
            yield return l;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}