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
}
