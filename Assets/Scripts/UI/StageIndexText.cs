using UnityEngine;
using UnityEngine.UI;

public class StageIndexText : MonoBehaviour {
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
	void SetIndex(int i)
	{
		_index = i;
		GetComponentInChildren<Text>().text = (_index+1).ToString();
	}
}
