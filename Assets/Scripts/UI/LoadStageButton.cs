using UnityEngine;
using UnityEngine.UI;

public class LoadStageButton : MonoBehaviour {
	int _index;
	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			SetIndex(value);
		}
	}
	void Start()
	{
		SetIndex(transform.GetSiblingIndex());
	}

	void SetIndex(int i)
	{
		_index = i;
		GetComponentInChildren<Text>().text = (_index+1).ToString();
	}

	public void LoadStage()
	{
		var toLoad = index+1;
		StageManager.LoadStage(toLoad);
	}
}
