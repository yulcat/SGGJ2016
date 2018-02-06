using System.Collections.Generic;
using System.Linq;
using Data;

public static class DB
{
    public static readonly StageDB StageDB = new StageDB();
    public static readonly ScoreDB ScoreDB = new ScoreDB();
    public static readonly MessageDB MessageDB = new MessageDB();
    public static readonly CharacterDB characterDB = new CharacterDB();
}

public class StageDB : ListData<StageDataLoader.StageData, StageDB>
{
    protected override List<StageDataLoader.StageData> CSVToTValue(string csv)
    {
        return CSVLoader.ToList<StageDataLoader.StageMiddleData>(csv).Select(s => new StageDataLoader.StageData(s))
            .ToList();
    }

    public StageDB()
    {
        dataPath = "stageDB.txt";
        SetId(419959845);
    }
}

public class ScoreDB : DictionaryData<int, ScoreDB>
{
    public ScoreDB()
    {
        dataPath = "scoreDB.txt";
        SetId(495693467);
    }
}

public class MessageDB
{
    public static LanguageDB korMessageDB = new LanguageDB("messageDB.txt", 1681149424);
    public static LanguageDB engMessageDB = new LanguageDB("engMessage.txt", 332871737);

    public string this[string key] => LanguageManager.GetLang() == LanguageManager.Language.English
        ? engMessageDB[key]
        : korMessageDB[key];

    public static bool HasKey(string localizerTextKey) =>
        engMessageDB.HasKey(localizerTextKey) && korMessageDB.HasKey(localizerTextKey);
}

public class LanguageDB : DictionaryData<string, LanguageDB>
{
    public LanguageDB(string path, int id)
    {
        dataPath = path;
        SetId(id);
    }

    public bool HasKey(string key) => ContainsKey(key);
}

public class CharacterDB : ListData<CharacterData, CharacterDB>
{
    public CharacterDB()
    {
        dataPath = "characterDB.txt";
        SetId(167980508);
    }
}

public class CharacterData
{
    public string name;
    public int price;
    public int heartCost;
    public float special;
}