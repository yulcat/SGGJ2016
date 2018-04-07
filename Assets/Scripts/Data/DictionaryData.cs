using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDataStorage
{
    string Path { get; }
    int Id { get; }
    void SetData(string value);
}

public abstract class DictionaryData<TValue, TSelf> :
    IDataStorage,
    IEnumerable<KeyValuePair<string, TValue>> where TSelf : DictionaryData<TValue, TSelf>
{
    protected Dictionary<string, TValue> dic;
    protected string dataPath;
    public int Id { get; private set; }
    public string Path => $"Data/{dataPath.Split('.')[0]}";

    public TValue this[string key]
    {
        get
        {
            if (dic != null) return dic[key];
            var unused = Application.persistentDataPath;
            var read = Resources.Load<TextAsset>(Path).text;
            dic = CSVToDictionary(read);
            return dic[key];
        }
    }

    public bool ContainsKey(string key)
    {
        if (dic != null) return dic.ContainsKey(key);
        var read = Resources.Load<TextAsset>(Path).text;
        dic = CSVToDictionary(read);
        return dic.ContainsKey(key);
    }

    public void SetData(string value) => dic = CSVToDictionary(value);

    protected virtual Dictionary<string, TValue> CSVToDictionary(string csv) => CSVLoader.ToDictionary<TValue>(csv);

    protected void SetId(int sheetId) => Id = sheetId;

    public void Load() => SetData(Resources.Load<TextAsset>(Path).text);

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        if (dic == null)
        {
            var read = Resources.Load<TextAsset>(Path).text;
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
    List<TValue> list;
    protected string dataPath;
    public int Id { get; private set; }
    public string Path => $"Data/{dataPath.Split('.')[0]}";

    public TValue this[int index]
    {
        get
        {
            if (list != null) return list[index];
            var read = Resources.Load<TextAsset>(Path).text;
            list = CSVToTValue(read);
            return list[index];
        }
    }

    public void SetData(string value) => list = CSVToTValue(value);

    protected virtual List<TValue> CSVToTValue(string csv) => CSVLoader.ToList<TValue>(csv);

    protected void SetId(int sheetId) => Id = sheetId;

    public void Load() => SetData(Resources.Load<TextAsset>(Path).text);

    public IEnumerator<TValue> GetEnumerator()
    {
        if (list == null)
        {
            var read = Resources.Load<TextAsset>(Path).text;
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