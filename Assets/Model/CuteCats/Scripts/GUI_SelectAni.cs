using UnityEngine;
using System.Collections;

public class GUI_SelectAni : MonoBehaviour {

	public GameObject Cat01;
	public GameObject Cat02;
	public GameObject Cat03;
	public GameObject Cat04;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		//Animation
		int startY = 100;
		int offsetY = 50;
		int ButtonWidth = 150;
		string aniName = "Ani";
		

		GUI.Label (new Rect(20,50,500,50),"-  You can see by rotating the mouse");
		GUI.Label (new Rect(20,80,200,50),"-  Animations");


		aniName = "Idle01";
		if (GUI.Button(new Rect(20, startY, ButtonWidth, offsetY),aniName)){
			Cat01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat01.GetComponent<Animation>().CrossFade(aniName);	
			Cat02.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat02.GetComponent<Animation>().CrossFade(aniName);	
			Cat03.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat03.GetComponent<Animation>().CrossFade(aniName);	
			Cat04.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat04.GetComponent<Animation>().CrossFade(aniName);	
		}	
		aniName = "Idle02";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Cat01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat01.GetComponent<Animation>().CrossFade(aniName);	
			Cat02.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat02.GetComponent<Animation>().CrossFade(aniName);	
			Cat03.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat03.GetComponent<Animation>().CrossFade(aniName);	
			Cat04.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat04.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Idle03";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Cat01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat01.GetComponent<Animation>().CrossFade(aniName);	
			Cat02.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat02.GetComponent<Animation>().CrossFade(aniName);	
			Cat03.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat03.GetComponent<Animation>().CrossFade(aniName);	
			Cat04.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat04.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Walk";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Cat01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat01.GetComponent<Animation>().CrossFade(aniName);	
			Cat02.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat02.GetComponent<Animation>().CrossFade(aniName);	
			Cat03.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat03.GetComponent<Animation>().CrossFade(aniName);	
			Cat04.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat04.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Run";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Cat01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat01.GetComponent<Animation>().CrossFade(aniName);	
			Cat02.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat02.GetComponent<Animation>().CrossFade(aniName);	
			Cat03.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat03.GetComponent<Animation>().CrossFade(aniName);	
			Cat04.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat04.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Dance";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Cat01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat01.GetComponent<Animation>().CrossFade(aniName);	
			Cat02.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat02.GetComponent<Animation>().CrossFade(aniName);	
			Cat03.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat03.GetComponent<Animation>().CrossFade(aniName);	
			Cat04.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Cat04.GetComponent<Animation>().CrossFade(aniName);	
		}
	}
}
