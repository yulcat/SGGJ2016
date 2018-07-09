using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;

public static class DataLoader
{
    public static void DownloadAll()
    {
        RequestLoad(DB.characterDB);
        RequestLoad(MessageDB.engMessageDB);
        RequestLoad(MessageDB.korMessageDB);
        RequestLoad(DB.ScoreDB);
        RequestLoad(DB.StageDB);
    }

    static void RequestLoad(IDataStorage db)
        => LoadFromWeb(db, db.Id);

    static async void LoadFromWeb(IDataStorage db, int sheetId)
    {
        var www = new WWW(
            $@"https://docs.google.com/spreadsheets/d/1gXAkxrZYqdVe9ALyeMdLY1Umd1jW3QWtPiA9FBF40GA/export?format=csv&gid={
                    sheetId
                }");
        while (!www.isDone)
        {
            await Task.Delay(1);
        }
        WriteFile(db, www.text);
        Debug.Log($"Loaded from web : {sheetId} ({DateTime.UtcNow})");
    }

    static void WriteFile(IDataStorage storage, string value)
    {
        var writePath = Path.Combine(Application.dataPath, $"Resources/{storage.Path}.txt");
        var directory = Path.GetDirectoryName(writePath);
        Debug.Log($"Writing File By : {storage.GetType().Name} At : {writePath}");
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(writePath, value + '\n');
        storage.SetData(value);
    }
}