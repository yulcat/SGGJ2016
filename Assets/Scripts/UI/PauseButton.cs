using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {
	Button button;
    private void BackButton()
    {
        if(WindowManager.instance.isWindowOpen || Pause.paused) return;
		OpenPauseWindow();
    }

    public void OpenPauseWindow()
    {
		if(Pause.paused) return;
        Pause.Open();
    }

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		button = GetComponent<Button>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(Pause.paused) button.interactable = false;
		else button.interactable = true;
		if(Input.GetKeyDown(KeyCode.Backspace) && button.interactable)
		{
			OpenPauseWindow();
		}
	}
}
