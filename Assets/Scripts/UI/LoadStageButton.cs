using UnityEngine;
using UnityEngine.UI;

public class LoadStageButton : MonoBehaviour {
	void Start()
	{
		GetComponentInChildren<Text>().text = (transform.GetSiblingIndex()+1).ToString();
	}

	public void LoadStage()
	{
		var toLoad = transform.GetSiblingIndex()+1;
		StageManager.LoadStage(toLoad);
	}
}
