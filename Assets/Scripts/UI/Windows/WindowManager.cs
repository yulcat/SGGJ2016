using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {
	static WindowManager _instance;
	public static WindowManager instance
	{
		get
		{
			if(_instance != null) return _instance;
			var obj = new GameObject("WindowManager");
			_instance = obj.AddComponent<WindowManager>();
			return _instance;
		}
	}
	public bool isWindowOpen
	{
		get
		{
			return windows.Count != 0;
		}
	}
	void Awake()
	{
		if(_instance != null)
		{
			Destroy(_instance.gameObject);
		}
		_instance = this;
	}

	Stack<Window> windows = new Stack<Window>();
	public void OpenWindow(Window newWindow)
	{
		if(windows.Count != 0)
			windows.Peek().gameObject.SetActive(false);
		newWindow.gameObject.SetActive(true);
		windows.Push(newWindow);
		Debug.Log(newWindow.gameObject.name + " : window open");
	}
	public void OpenWindow<T>() where T:Window
	{
		var newWindow = Resources.FindObjectsOfTypeAll<T>()[0];
		OpenWindow(newWindow);
	}
	public void BackToPrevWindow()
	{
		windows.Pop().gameObject.SetActive(false);
		if(windows.Count != 0)
			windows.Peek().gameObject.SetActive(true);
	}
	public void CloseAllWindow()
	{
		var windowToClose = windows.Pop().gameObject;
		windowToClose.SetActive(false);
		windows.Clear();
		Debug.Log(windowToClose.name + " : all window closed");
	}
}
