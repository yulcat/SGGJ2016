using UnityEngine;


public class LoadStageButton : MonoBehaviour {
	public int index;

	void OnMouseDown()
	{
		if(WindowManager.instance.isWindowOpen) return;
		var toLoad = index+1;
		// StageManager.LoadStage(toLoad);
		var startWindow = GameObject.Find("Canvas").GetComponentInChildren<WindowStart>(true);
		startWindow.OpenStartWindow(toLoad);
	}
}
