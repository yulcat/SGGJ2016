using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using System;

public class Win : Window
{
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
        stageNumber.text = FindObjectOfType<PyramidBuilder>().stageToLoad.ToString();
    }
    public void WinGame()
    {
        StartCoroutine(ShowScoreUp());
        // HeartManager.AddHeart();
    }
    IEnumerator ShowScoreUp()
    {
        scoreShow = 0;
        DOTween.To(() => (float)scoreShow, x => scoreShow = Mathf.RoundToInt(x), finalScore, 2)
            .SetEase(Ease.OutExpo);
        while (scoreShow != finalScore)
        {
            score.text = scoreShow.ToString();
            if (!rankingToShow.HasValue)
                stageRanking.text = UnityEngine.Random.Range(0f, 100f).ToString("0.0") + "%";
            yield return null;
        }
        score.text = scoreShow.ToString();
        if (!rankingToShow.HasValue)
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
        if (HeartManager.heartLeft <= 0)
        {
            WindowHeartInsufficient.Open();
            return;
        }
        HeartManager.SpendHeart();
        StageManager.ReloadCurrentStage();
    }
    public void ToStageSelect()
    {
        StageManager.LoadStageSelectScene();
    }

    public void SetRanking(double v)
    {
        rankingToShow = v;
        stageRanking.text = (v * 100).ToString("0.0") + "%";
    }
}
