using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncText : MonoBehaviour {
	public Text childText;
	Text text;
	string lastText;
	void Start()
	{
		text = GetComponent<Text>();
		Debug.Assert(childText!=null, "childText is null!");
	}
	// Update is called once per frame
	void LateUpdate () {
		if(lastText != text.text)
		{
			lastText = text.text;
			childText.text = lastText;
		}
	}
}
