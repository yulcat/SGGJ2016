using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarter : MonoBehaviour {
	public GameObject[] tutorials;

	// Use this for initialization
	void Start () {
		if(StageManager.IsInitialized() && StageManager.instance.stageToLoad == 1)
		{
			StartCoroutine(Tutorial());
			StartCoroutine(GameEndCheck());
		}
	}

	IEnumerator WaitForDisable(GameObject objectToWait)
	{
		objectToWait.SetActive(true);
		while(objectToWait.activeSelf)
		{
			yield return null;
		}
	}

	IEnumerator GameEndCheck()
	{
		while(!GameState.instance.isGameEnd)
		{
			yield return null;
		}
		StopAllCoroutines();
		foreach(var tutorial in tutorials)
		{
			tutorial.SetActive(false);
		}
	}

	IEnumerator Tutorial()
	{
		FindObjectOfType<Pyramid>().torqueMultiplier = 0f;
		yield return new WaitForSeconds(1.5f);
		yield return StartCoroutine(WaitForDisable(tutorials[0]));
		yield return new WaitForSeconds(3f);
		yield return StartCoroutine(WaitForDisable(tutorials[1]));
		yield return new WaitForSeconds(3f);
		yield return StartCoroutine(WaitForDisable(tutorials[2]));
	}
}
