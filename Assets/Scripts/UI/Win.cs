using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Win : Window {
	public Text score;
	public Text stageNumber;
	public int finalScore;
	int scoreShow;
	override protected void OnEnable()
	{
		base.OnEnable();
		StartCoroutine(ShowScoreUp());
		stageNumber.text = FindObjectOfType<PyramidBuilder>().stageToLoad.ToString();
	}
	IEnumerator ShowScoreUp()
	{
		scoreShow = 0;
		DOTween.To(()=>(float)scoreShow, x=>scoreShow = Mathf.RoundToInt(x), finalScore, 2)
			.SetEase(Ease.OutExpo);
		while(scoreShow != finalScore)
		{
			score.text = scoreShow.ToString();
			yield return null;
		}
		score.text = scoreShow.ToString();
	}
	public void ToNextStage()
	{
		StageManager.LoadNextStageSelectScene();
	}
	public void ReloadCurrentStage()
	{
		StageManager.ReloadCurrentStage();
	}
	public void ToStageSelect()
	{
		StageManager.LoadStageSelectScene();
	}
}
