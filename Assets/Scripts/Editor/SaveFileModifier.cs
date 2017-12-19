using UnityEngine;
using UnityEditor;

public class SaveFileModifier : Editor
{
    [MenuItem("Tools/Edit Save File/Clear All Stages")]
    static void ClearAll()
    {
        var max = StageDataLoader.GetMaxStage();
        for (int i = 0; i < max; i++)
        {
            if (SaveDataManager.clearRecord.ContainsKey(i.ToString()))
                continue;
            var clear = new SaveDataManager.ClearData();
            SaveDataManager.clearRecord.Add(i.ToString(), clear);
        }
        SaveDataManager.Save();
    }

    [MenuItem("Tools/Edit Save File/Delete Save Data")]
    static void Delete()
    {
        SaveDataManager.clearRecord.Clear();
        SaveDataManager.Save();
    }

    [MenuItem("Tools/Edit Hearts/Zero Heart")]
    static void ZeroHeart()
    {
        var hearts = SaveDataManager.data.heartLeft;
        HeartManager.SpendHeart(hearts);
    }

    [MenuItem("Tools/Edit Save File/Enable all Characters")]
    static void EnableCharacters()
    {
        SaveDataManager.UnlockCharacter(1);
        SaveDataManager.UnlockCharacter(2);
    }

    [MenuItem("Tools/Edit Save File/Disable all Characters")]
    static void DisableCharacters()
    {
        SaveDataManager.data.unlockedCharacters = 0;
        SaveDataManager.Save();
    }

    [MenuItem("Tools/Edit Save File/Add Stars")]
    static void AddStars()
    {
        SaveDataManager.DebugAddStar(30);
    }

    [MenuItem("Tools/Edit Save File/Reset Stars")]
    static void ResetStars()
    {
        SaveDataManager.data.spentStars = 0;
        SaveDataManager.Save();
    }

    [MenuItem("Tools/Edit Save File/Reload DB")]
    static void ReloadDB()
    {
        PlayerPrefs.DeleteKey("downloadTime");
    }
}