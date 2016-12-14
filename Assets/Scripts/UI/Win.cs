using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : Window {
	public void ToNextStage()
	{
		StageManager.LoadNextStage();
	}
}
