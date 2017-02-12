using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WindowStartWithScore : WindowStart {
	public Text highScore;
	public Text rank;
	public GameObject[] stars;
	int stageToLoad;
	public new void OpenStartWindow(int _stageToLoad)
	{
		stageToLoad = _stageToLoad;
		base.OpenStartWindow(stageToLoad);
		if(!SaveDataManager.clearRecord.ContainsKey(stageToLoad.ToString()))
			throw new System.Exception("WindowStartWithScore is trying to load stage without clear record");
		StartCoroutine(ApplyClearData());
	}
	IEnumerator ApplyClearData()
	{
		var clearData = SaveDataManager.clearRecord[stageToLoad.ToString()];
		highScore.text = clearData.score.ToString();
		StartCoroutine(LoadWorldRank());
		yield return new WaitForSeconds(timeToOpenWindow);
		foreach(var star in stars.Take(clearData.stars))
		{
			star.SetActive(true);
			yield return 0.2f;
		}
	}
	IEnumerator LoadWorldRank()
	{
		var clearData = SaveDataManager.clearRecord[stageToLoad.ToString()];
		var builder = new System.Text.StringBuilder("http://52.78.26.149/api/values/");
		builder.Append("stage");
		builder.Append(stageToLoad.ToString());
		builder.Append(".");
		builder.Append(clearData.scoreGuid);
		var www = new WWW(builder.ToString());
		yield return www;
		float ranking = (float)System.Convert.ToDouble(www.text) * 100;
		rank.text = ranking.ToString("0.0") + "%";
	}
}
