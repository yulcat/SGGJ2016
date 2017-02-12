using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour {
	public void TakeAShot()
	{
		Application.CaptureScreenshot(Application.persistentDataPath + "/" + transform.parent.gameObject.name + transform.GetSiblingIndex() + ".PNG",2);
		Debug.Log("Captured in "+ Application.persistentDataPath);
	}
}
