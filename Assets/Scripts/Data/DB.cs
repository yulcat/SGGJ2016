using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DB
{
    public readonly static StageDB StageDB = new StageDB();
    public readonly static ScoreDB ScoreDB = new ScoreDB();
    public readonly static MessageDB MessageDB = new MessageDB();

    internal static void LoadAll()
    {
        Debug.Log("do nothing");
    }

    public readonly static CharacterDB characterDB = new CharacterDB();
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

public class CharacterDB : ListData<CharacterData, CharacterDB>
{
    public CharacterDB() { dataPath = "characterDB.txt"; Load(167980508); }
}

public class CharacterData
{
    public string name;
    public int price;
}