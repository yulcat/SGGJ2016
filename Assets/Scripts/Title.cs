using UnityEngine;
using System.Collections;

public class Title : VRListener {
	IEnumerator Start ()
	{
		yield return new WaitForSeconds(2);
		while(true)
		{
			if(Input.GetMouseButtonDown(0))
				SceneLoader.LoadScene(1);
			yield return null;
		}
	}
	public override void OnClick()
	{
		SceneLoader.LoadScene(1);
	}
}
