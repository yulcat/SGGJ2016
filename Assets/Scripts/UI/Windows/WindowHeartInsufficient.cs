using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHeartInsufficient : Window {
	public static void Open()
	{
		var canvas = FindObjectOfType<Canvas>();
		var instanciated = canvas.GetComponentInChildren<WindowHeartInsufficient>(true);
		if(instanciated != null)
		{
			instanciated.OpenWindow();
			return;
		}
		var heartWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_notice_Heart"));
		heartWindow.transform.SetParent(canvas.transform,false);
		heartWindow.GetComponent<WindowHeartInsufficient>().OpenWindow();
	}
}
