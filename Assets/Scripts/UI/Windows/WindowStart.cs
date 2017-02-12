using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : Window {
	public Text levelNumber;
	protected int toLoad;
	public void OpenStartWindow(int stageToLoad)
	{
		levelNumber.text = stageToLoad.ToString();
		toLoad = stageToLoad;
		OpenWindow();
	}
	public void StartStage()
	{
		StageManager.LoadStage(toLoad);
	}
}
