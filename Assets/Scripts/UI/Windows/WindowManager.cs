﻿using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    static WindowManager _instance;

    public static WindowManager instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = new GameObject("WindowManager");
            _instance = obj.AddComponent<WindowManager>();
            return _instance;
        }
    }

    public bool isWindowOpen
    {
        get { return windows.Count != 0; }
    }

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (windows.Count > 0)
            {
                windows.Peek().BackToPrevWindow();
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StageSelect")
            {
                WindowYNPop.OpenYNPop(
                    DB.MessageDB["exitGame"],
                    () => Application.Quit(),
                    null);
            }
            else
            {
                Pause.Open();
            }
        }
    }

    Stack<Window> windows = new Stack<Window>();

    public void OpenWindow(Window newWindow)
    {
        if (windows.Count != 0)
            windows.Peek().gameObject.SetActive(false);
        if (windows.Contains(newWindow))
        {
            var tempWindows = new Stack<Window>();
            while (windows.Peek() != newWindow)
            {
                tempWindows.Push(windows.Pop());
            }
            windows.Pop();
            while (tempWindows.Count > 0)
            {
                windows.Push(tempWindows.Pop());
            }
        }
        windows.Push(newWindow);
        newWindow.gameObject.SetActive(true);
        Debug.Log(newWindow.gameObject.name + " : window open : " + newWindow.gameObject.activeSelf);
    }

    public void OpenWindow<T>() where T : Window
    {
        var newWindow = Resources.FindObjectsOfTypeAll<T>()[0];
        OpenWindow(newWindow);
    }

    public void BackToPrevWindow()
    {
        windows.Pop().gameObject.SetActive(false);
        if (windows.Count != 0)
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