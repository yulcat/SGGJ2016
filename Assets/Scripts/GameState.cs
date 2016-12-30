using UnityEngine;
using System.Collections;
using System.Linq;

public class GameState : MonoBehaviour {
	public enum LoseCause { CharacterLost = 0, BalloonLost, Crushed, Collapsed }
	public bool isGameEnd = false;
	public Win winMessage;
	public Lose loseMessage;
	static GameState _instance;
	LoseCause cause;
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
		instance.StartCoroutine(instance.SendScoreToServer());
		instance.isGameEnd = true;
		instance.Invoke("WinGame",2f);
	}
	public static void Lose(LoseCause _cause)
	{
		if(instance.isGameEnd) return;
		instance.isGameEnd = true;
		instance.Invoke("LoseGame",2f);
		instance.cause = _cause;
	}
	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
		}
	}
	void WinGame()
	{
		WindowManager.instance.OpenWindow(winMessage);
	}
	void LoseGame()
	{
		WindowManager.instance.OpenWindow(loseMessage);
		loseMessage.text.text = loseMessage.messages[(int)cause];
	}
	IEnumerator SendScoreToServer()
	{
		var form = new WWWForm();
		form.AddField("stage","stage"+FindObjectOfType<PyramidBuilder>().stageToLoad.ToString());
		form.AddField("id",SystemInfo.deviceUniqueIdentifier);
		form.AddField("score",Random.Range(0,9000000));
		var www = new WWW("http://52.78.26.149/api/values",form);
		yield return www;
		Debug.Log(www.text);
	}
}
