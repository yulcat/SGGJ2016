using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType
{
    Bear = 0,
    Owl,
    Cat
}

public class GameState : MonoBehaviour
{
    public enum LoseCause
    {
        CharacterLost = 0,
        BalloonLost,
        Crushed,
        Collapsed,
        Objective,
        Boomed
    }

    public bool isGameEnd;
    [NonSerialized] public Win winMessage;
    [NonSerialized] public Lose loseMessage;
    public Dictionary<string, int> mission;
    Dictionary<string, int> accomplished = new Dictionary<string, int>();
    static GameState instance;
    Pyramid pyramid;
    Score scoreToSend;
    public Action<Dictionary<string, int>> accomplishedListener;
    public static CharacterType selectedCharacter;

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.FindObjectsOfTypeAll<GameState>().First();
                instance.Initialize();
            }
            return instance;
        }
    }

    public static void Win()
    {
        if (Instance.isGameEnd) return;
        Instance.CalculateScore();
        Instance.StartCoroutine(Instance.SendScoreToServer());
        Instance.isGameEnd = true;
        Instance.Invoke(nameof(ShowWinGameMessage), 3f);
    }

    public static void Lose(LoseCause cause)
    {
        if (Instance.isGameEnd) return;
        Instance.isGameEnd = true;
        Instance.Invoke(nameof(ShowLoseGameMessage), 3f);
        Instance.loseMessage.SetMessage(cause);
    }

    void Initialize()
    {
        winMessage = GetComponentInChildren<Win>(true);
        loseMessage = GetComponentInChildren<Lose>(true);
        var builder = FindObjectOfType<PyramidBuilder>();
        int stage;
        stage = StageManager.IsInitialized() ? StageManager.instance.stageToLoad : builder.stageToLoad;
        Debug.Log("stage : " + stage);
        var stageData = StageDataLoader.GetStageData(stage);
        mission = stageData.mission;
        pyramid = FindObjectOfType<Pyramid>();
    }

    public static void EndGame()
    {
        if (Instance.isGameEnd) return;

        if (Instance.mission == null)
        {
            Win();
            return;
        }
        if (IsMissionComplete()) Win();
        else Lose(LoseCause.Objective);
    }

    static bool IsMissionComplete()
    {
        foreach (var kvp in Instance.mission)
        {
            Debug.Log($"Mission : {kvp.Key} x {kvp.Value}");
            if (kvp.Value == 0 && kvp.Key != "Less") continue;
            var sum = Instance.accomplished.Where(ac => ac.Key != "Coin").Sum(ac => ac.Value);
            switch (kvp.Key)
            {
                case "More":
                    if (sum >= kvp.Value) continue;
                    else return false;
                case "Less":
                    if (sum <= kvp.Value) continue;
                    else return false;
                default:
                    if (!Instance.accomplished.ContainsKey(kvp.Key)) return false;
                    if (Instance.accomplished[kvp.Key] >= kvp.Value) continue;
                    else return false;
            }
        }
        return true;
    }

    public static void Accomplished(string key, int value)
    {
        if (!Instance.accomplished.ContainsKey(key))
            Instance.accomplished.Add(key, value);
        else
            Instance.accomplished[key] += value;
        Instance.accomplishedListener?.Invoke(Instance.accomplished);
    }

    public static int GetHeartCost()
        => DB.characterDB[(int) selectedCharacter].heartCost;

    void CalculateScore()
    {
        Debug.Log("MaxRotation : " + pyramid.maxRotation);
        var rotationMultiplier = Mathf.Cos(pyramid.maxRotation * Mathf.Deg2Rad) - 0.9f;
        var primeScore = ScoreDataLoader.GetScore("size" + pyramid.maxY);
        var rotationScore = Mathf.FloorToInt(primeScore * rotationMultiplier);
        Debug.Log($"{"size" + pyramid.maxY} : {rotationScore}");
        var blockScore = accomplished.Sum(a => ScoreDataLoader.GetScore(a.Key) * a.Value);
        var totalScore = rotationScore + blockScore;
        var gotCoin = !accomplished.ContainsKey("Coin") || accomplished["Coin"] == pyramid.coinCount;
        var scoreOverPrime = primeScore < totalScore;
        var stars = 3;
        if (!gotCoin) stars--;
        if (!scoreOverPrime) stars--;
        scoreToSend = new Score(rotationScore + blockScore, stars);
    }

    void ShowWinGameMessage()
    {
        winMessage.finalScore = scoreToSend;
        WindowManager.instance.OpenWindow(winMessage);
        winMessage.WinGame();
    }

    void ShowLoseGameMessage()
    {
        WindowManager.instance.OpenWindow(loseMessage);
    }

    IEnumerator SendScoreToServer()
    {
        string id = Guid.NewGuid().ToString();
        int stage = FindObjectOfType<PyramidBuilder>().stageToLoad;
        CheckAndUpdateHighscore(id, stage);
        var form = new WWWForm();
        form.AddField("stage", "stage" + stage);
        form.AddField("id", id);
        form.AddField("score", scoreToSend.score);
        var www = new WWW("http://13.124.225.49/api/values", form);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogWarning(www.error);
        }
        Debug.Log(www.text);
        winMessage.SetRanking(Convert.ToDouble(www.text));
    }

    void CheckAndUpdateHighscore(string id, int stage)
    {
        var clearData = new SaveDataManager.ClearData
        {
            score = scoreToSend.score,
            scoreGuid = id,
            stars = scoreToSend.stars
        };
        var stageString = stage.ToString();
        if (SaveDataManager.clearRecord.ContainsKey(stage.ToString()))
        {
            var record = SaveDataManager.clearRecord[stageString];
            if (record.score > scoreToSend.score)
            {
                return;
            }
            SaveDataManager.clearRecord[stageString] = clearData;
        }
        else
        {
            SaveDataManager.clearRecord.Add(stageString, clearData);
        }
        SaveDataManager.Save();
    }
}

public class Score
{
    public readonly int score;
    public readonly int stars;

    public Score(int score, int stars)
    {
        this.score = score;
        this.stars = stars;
    }
}