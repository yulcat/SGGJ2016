using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	IEnumerator Start () {
		// QualitySettings.vSyncCount = 0;
		// Application.targetFrameRate = 30;
		// yield return new WaitForSeconds(2);
		while(true)
		{
			if(Input.GetMouseButtonDown(0))
				SceneLoader.LoadScene(1);
			yield return null;
		}
	}
}
