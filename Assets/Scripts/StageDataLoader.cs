using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class StageDataLoader
{
    public struct StageMiddleData
    {
        public int stageNumber;
        public string theme;
        public string mission;
    }

    public struct StageData
    {
        public int stageNumber;
        public StageManager.Theme theme;
        public Dictionary<string, int> mission;

        public StageData(StageMiddleData input)
        {
            stageNumber = input.stageNumber;
            theme = (StageManager.Theme) System.Enum.Parse(typeof(StageManager.Theme), input.theme);
            if (string.IsNullOrEmpty(input.mission))
                mission = null;
            else
                mission = JsonMapper.ToObject<Dictionary<string, int>>(input.mission);
        }
    }

    public static StageData GetStageData(int stage)
    {
        return DB.StageDB.First(d => d.stageNumber == stage);
    }

    public static int GetMaxStage()
    {
        return DB.StageDB.Max(l => l.stageNumber);
    }
}

public static class ScoreDataLoader
{
    public static int GetScore(string key)
    {
        if (DB.ScoreDB.ContainsKey(key))
            return DB.ScoreDB[key];
        else
            return 0;
    }
}