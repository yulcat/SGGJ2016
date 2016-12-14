using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lose : Window {
	public Text text;
	public GameState.LoseCause cause;
	public string[] messages;
	public void ReloadCurrentStage()
	{
		StageManager.ReloadCurrentStage();
	}
	public void ToStageSelect()
	{
		StageManager.LoadStageSelectScene();
	}
}
