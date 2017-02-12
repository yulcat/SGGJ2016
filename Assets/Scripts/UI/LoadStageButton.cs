using UnityEngine;


public class LoadStageButton : MonoBehaviour {
	public int index;

	void OnMouseDown()
	{
		if(!enabled) return;
		if(WindowManager.instance.isWindowOpen) return;
		var toLoad = index+1;
		// StageManager.LoadStage(toLoad);
		if(SaveDataManager.clearRecord.ContainsKey(toLoad.ToString()))
		{
			var startWindow = GameObject.Find("Canvas").GetComponentInChildren<WindowStartWithScore>(true);
			startWindow.OpenStartWindow(toLoad);
		}
		else
		{
			var startWindow = GameObject.Find("Canvas").GetComponentInChildren<WindowStart>(true);
			startWindow.OpenStartWindow(toLoad);
		}
	}
}
