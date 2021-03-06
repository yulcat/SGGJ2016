﻿using UnityEngine;

public class WindowCredit : WindowPop
{
    static WindowCredit instanciated;

    public static void OpenCredit()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (instanciated != null)
        {
            instanciated.transform.localScale = Vector3.one;
            instanciated.OpenWindow();
            return;
        }
        var newWindow = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Window_Credit"));
        newWindow.transform.SetParent(canvas.transform, false);
        instanciated = newWindow.GetComponent<WindowCredit>();
        instanciated.OpenWindow();
    }
}