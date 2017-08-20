using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class GameState : MonoBehaviour
{
    public enum LoseCause { CharacterLost = 0, BalloonLost, Crushed, Collapsed, Objective, Boomed }
    public bool isGameEnd = false;
    [System.NonSerializedAttribute]
    public Win winMessage;
    [System.NonSerializedAttribute]
    public Lose loseMessage;
    public Dictionary<string, int> mission;
    Dictionary<string, int> accomplished = new Dictionary<string, int>();
    static GameState _instance;
    Pyramid pyramid;
    int scoreToSend;
    int starCount;
    public Action<Dictionary<string, int>> AccomplishedListener;

    public static GameState instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.FindObjectsOfTypeAll<GameState>().First();
                _instance.Initialize();
            }
            return _instance;
        }
    }
    public static void Win()
    {
        if (instance.isGameEnd) return;
        instance.calculateScore();
        instance.StartCoroutine(instance.SendScoreToServer());
        instance.isGameEnd = true;
        instance.Invoke("ShowWinGameMessage", 3f);
    }
    public static void Lose(LoseCause _cause)
    {
        if (instance.isGameEnd) return;
        instance.isGameEnd = true;
        instance.Invoke("ShowLoseGameMessage", 3f);
        instance.loseMessage.SetMessage(_cause);
    }
    void Initialize()
    {
        winMessage = GetComponentInChildren<Win>(true);
        loseMessage = GetComponentInChildren<Lose>(true);
        var builder = FindObjectOfType<PyramidBuilder>();
        int stage;
        if (StageManager.IsInitialized())
            stage = StageManager.instance.stageToLoad;
        else
            stage = builder.stageToLoad;
        Debug.Log("stage : " + stage);
        var stageData = StageDataLoader.GetStageData(stage);
        mission = stageData.mission;
        pyramid = FindObjectOfType<Pyramid>();
    }
    public static void EndGame()
    {
        if (instance.isGameEnd) return;

        if (instance.mission == null)
        {
            Win();
            return;
        }
        if (IsMissionComplete()) Win();
        else Lose(LoseCause.Objective);
    }
    static bool IsMissionComplete()
    {
        foreach (var kvp in instance.mission)
        {
            Debug.Log(string.Format("Mission : {0} x {1}", kvp.Key, kvp.Value));
            if (kvp.Value == 0 && kvp.Key != "Less") continue;
            var sum = instance.accomplished.Where(ac => ac.Key != "Coin").Sum(ac => ac.Value);
            switch (kvp.Key)
            {
                case "More":
                    if (sum >= kvp.Value) continue;
                    else return false;
                case "Less":
                    if (sum <= kvp.Value) continue;
                    else return false;
                default:
                    if (!instance.accomplished.ContainsKey(kvp.Key)) return false;
                    if (instance.accomplished[kvp.Key] >= kvp.Value) continue;
                    else return false;
            }
        }
        return true;
    }
    public static void Accomplished(string key, int value)
    {
        if (!instance.accomplished.ContainsKey(key))
            instance.accomplished.Add(key, value);
        else
            instance.accomplished[key] += value;
        if (instance.AccomplishedListener != null)
        {
            instance.AccomplishedListener(instance.accomplished);
        }
    }
    void calculateScore()
    {
        Debug.Log("MaxRotation : " + pyramid.maxRotation);
        var rotationScore = Mathf.Cos(pyramid.maxRotation * Mathf.Deg2Rad) - 0.9f;
        var primeScore = Mathf.FloorToInt(ScoreDataLoader.GetScore("size" + pyramid.maxY) * rotationScore);
        Debug.Log(string.Format("{0} : {1}", "size" + pyramid.maxY, primeScore));
        var blockScore = accomplished.Sum(a => ScoreDataLoader.GetScore(a.Key) * a.Value);
        scoreToSend = primeScore + blockScore;
        starCount = 3;
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
        string id = System.Guid.NewGuid().ToString();
        int stage = FindObjectOfType<PyramidBuilder>().stageToLoad;
        CheckAndUpdateHighscore(id, stage);
        var form = new WWWForm();
        form.AddField("stage", "stage" + stage.ToString());
        form.AddField("id", id);
        form.AddField("score", scoreToSend);
        var www = new WWW("http://52.78.26.149/api/values", form);
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
        var clearData = new SaveDataManager.ClearData();
        clearData.score = scoreToSend;
        clearData.scoreGuid = id;
        clearData.stars = starCount;
        var stageString = stage.ToString();
        if (SaveDataManager.clearRecord.ContainsKey(stage.ToString()))
        {
            var record = SaveDataManager.clearRecord[stageString];
            if (record.score > scoreToSend)
            {
                return;
            }
            else
            {
                SaveDataManager.clearRecord[stageString] = clearData;
            }
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
    public Score(int _score, int _stars)
    {
        score = _score;
        stars = _stars;
    }
}