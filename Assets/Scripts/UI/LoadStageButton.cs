using UnityEngine;


public class LoadStageButton : MonoBehaviour {
	public int index;

	void OnMouseDown()
	{
		var toLoad = index+1;
		StageManager.LoadStage(toLoad);
	}
}
