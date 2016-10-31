﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lose : VRListener {
	public Text text;
	public GameState.LoseCause cause;
	public string[] messages;
	bool active;
	void Start()
	{
		Invoke("Activate",1f);
		text.text = messages[(int)cause];
	}
	void Activate()
	{
		active = true;
	}
	// Update is called once per frame
	void Update () {
		if(!active) return;
		if(Input.GetMouseButtonDown(0))
		{
			Reload();
		}
	}

    public override void OnClick()
    {
		if(!active || !gameObject.activeSelf) return;
        Reload();
    }

	void Reload()
	{
		var current = SceneManager.GetActiveScene().buildIndex;
		SceneLoader.LoadScene(current);
	}
}
