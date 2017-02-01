using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
	public enum LoseCause { CharacterLost = 0, BalloonLost, Crushed, Collapsed }
	public bool isGameEnd = false;
	public Win winMessage;
	public Lose loseMessage;
	Dictionary<string,int> mission;
	Dictionary<string,int> accomplished = new Dictionary<string,int>();
	static GameState _instance;
	LoseCause cause;
	Pyramid pyramid;
	int scoreToSend;
	public static GameState instance
	{
		get
		{
			if(_instance == null)
				_instance = Resources.FindObjectsOfTypeAll<GameState>().First();
			return _instance;
		}
	}
	public static void Win()
	{
		if(instance.isGameEnd) return;
		instance.calculateScore();
		instance.StartCoroutine(instance.SendScoreToServer());
		instance.isGameEnd = true;
		instance.Invoke("ShowWinGameMessage",2f);
	}
	public static void Lose(LoseCause _cause)
	{
		if(instance.isGameEnd) return;
		instance.isGameEnd = true;
		instance.Invoke("ShowLoseGameMessage",2f);
		instance.cause = _cause;
	}
	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
		}
	}
	void Start()
	{
		var builder = FindObjectOfType<PyramidBuilder>();
		int stage = builder.stageToLoad;
		Debug.Log("stage : " + stage);
		var stageData = StageDataLoader.GetStageData(stage);
		mission = stageData.mission;
		pyramid = FindObjectOfType<Pyramid>();
	}
	public static void EndGame()
	{
		if(instance.isGameEnd) return;

		if(instance.mission == null)
		{
			Win();
			return;
		}
		if(IsMissionComplete()) Win();
		else Lose(LoseCause.BalloonLost);
	}
	static bool IsMissionComplete()
	{
		foreach(var kvp in instance.mission)
		{
			if(kvp.Value == 0 && kvp.Key != "Less") continue;
			var sum = instance.accomplished.Where(ac => ac.Key != "Coin").Sum(ac => ac.Value);
			switch(kvp.Key)
			{
			case "More":
				if(sum >= kvp.Value) continue;
				else return false;
			case "Less":
				if(sum <= kvp.Value) continue;
				else return false;
			default:
				if(!instance.accomplished.ContainsKey(kvp.Key)) return false;
				if(instance.accomplished[kvp.Key] >= kvp.Value) continue;
				else return false;
			}
		}
		return true;
	}
	public static void Accomplished(string key, int value)
	{
		if(!instance.accomplished.ContainsKey(key))
			instance.accomplished.Add(key,value);
		else
			instance.accomplished[key] += value;
	}
	void calculateScore()
	{
		Debug.Log("MaxRotation : " + pyramid.maxRotation);
		var rotationScore = Mathf.Cos(pyramid.maxRotation * Mathf.Deg2Rad) - 0.9f;
		var primeScore = Mathf.FloorToInt(ScoreDataLoader.GetScore("size"+pyramid.maxY) * rotationScore);
		Debug.Log(string.Format("{0} : {1}","size"+pyramid.maxY,primeScore));
		var blockScore = accomplished.Sum(a => ScoreDataLoader.GetScore(a.Key) * a.Value);
		scoreToSend = primeScore + blockScore;
	}
	void ShowWinGameMessage()
	{
		winMessage.finalScore = scoreToSend;
		WindowManager.instance.OpenWindow(winMessage);
	}
	void ShowLoseGameMessage()
	{
		WindowManager.instance.OpenWindow(loseMessage);
		loseMessage.text.text = loseMessage.messages[(int)cause];
	}
	IEnumerator SendScoreToServer()
	{
		var form = new WWWForm();
		form.AddField("stage","stage"+FindObjectOfType<PyramidBuilder>().stageToLoad.ToString());
		form.AddField("id",System.Guid.NewGuid().ToString());
		form.AddField("score",scoreToSend);
		var www = new WWW("http://52.78.26.149/api/values",form);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.LogWarning(www.error);
		}
		Debug.Log(www.text);
	}
}
