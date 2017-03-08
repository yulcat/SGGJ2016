using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using System;

public class Win : Window {
	public Text score;
	public Text stageNumber;
	public Text stageRanking;
	[System.NonSerializedAttribute]
	public int finalScore;
	int scoreShow;
	double? rankingToShow;
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
			if(!rankingToShow.HasValue)
				stageRanking.text = UnityEngine.Random.Range(0,99).ToString() + "%";
			yield return null;
		}
		score.text = scoreShow.ToString();
		if(!rankingToShow.HasValue)
		{
			stageRanking.text = "??%";
		}
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

    public void SetRanking(double v)
    {
        rankingToShow = v;
		stageRanking.text = ((int)(v * 100)).ToString() + "%";
    }
}
