using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DB
{
    public static StageDB StageDB = new StageDB();
    public static ScoreDB ScoreDB = new ScoreDB();
    public static MessageDB MessageDB = new MessageDB();
}

public class StageDB : ListData<StageDataLoader.StageData, StageDB>
{
    protected override List<StageDataLoader.StageData> CSVToTValue(string csv)
    {
        return CSVLoader.ToList<StageDataLoader.StageMiddleData>(csv).Select(s => new StageDataLoader.StageData(s)).ToList();
    }
    public StageDB() { dataPath = "stageDB.txt"; Load(419959845); }
}

public class ScoreDB : DictionaryData<int, ScoreDB>
{
    public ScoreDB() { dataPath = "scoreDB.txt"; Load(495693467); }
}

public class MessageDB : DictionaryData<string, MessageDB>
{
    public MessageDB() { dataPath = "messageDB.txt"; Load(1681149424); }
}