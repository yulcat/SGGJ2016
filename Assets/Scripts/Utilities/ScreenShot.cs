using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour {
	public bool takeAShot = false;
	void OnDrawGizmos()
	{
		if(!takeAShot) return;
		takeAShot = false;
		Application.CaptureScreenshot(Application.persistentDataPath + "/" + transform.parent.gameObject.name + transform.GetSiblingIndex() + ".PNG",2);
		Debug.Log("Captured in "+ Application.persistentDataPath);
	}
}
