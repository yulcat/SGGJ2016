using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;
using JetBrains.Annotations;

public class Win : Window
{
    public Text score;
    public Text stageNumber;
    public Text stageRanking;
    [NonSerializedAttribute] public Score finalScore;
    public GameObject[] stars;
    int scoreShow;
    double? rankingToShow;

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var star in stars)
        {
            star.SetActive(false);
        }
        stageNumber.text = FindObjectOfType<PyramidBuilder>().stageToLoad.ToString();
    }

    public void WinGame()
    {
        StartCoroutine(ShowScoreUp());
        StartCoroutine(ShowStars(finalScore.stars));
        // HeartManager.AddHeart();
    }

    IEnumerator ShowStars(int starCount)
    {
        foreach (var star in stars.Take(starCount))
        {
            star.SetActive(true);
            star.transform.DOScale(Vector3.one * 0.2f, 0.4f).SetEase(Ease.OutBack).From();
            yield return new WaitForSeconds(0.1333f);
        }
    }

    IEnumerator ShowScoreUp()
    {
        scoreShow = 0;
        DOTween.To(() => (float) scoreShow, x => scoreShow = Mathf.RoundToInt(x), finalScore.score, 2)
            .SetEase(Ease.OutExpo);
        while (scoreShow != finalScore.score)
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

    [UsedImplicitly]
    public void ToNextStage()
    {
        StageManager.LoadNextStageSelectScene();
    }

    [UsedImplicitly]
    public void ReloadCurrentStage()
    {
        if (HeartManager.HeartLeft < GameState.GetHeartCost())
        {
            WindowHeartInsufficient.Open();
            return;
        }
        HeartManager.SpendHeart();
        StageManager.ReloadCurrentStage();
    }

    [UsedImplicitly]
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