using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class WindowStartWithScore : WindowStart
{
    public Text highScore;
    public Text rank;
    public GameObject[] stars;
    int stageToLoad;

    public new void OpenStartWindow(int _stageToLoad)
    {
        stageToLoad = _stageToLoad;
        base.OpenStartWindow(stageToLoad);
        if (!SaveDataManager.clearRecord.ContainsKey(stageToLoad.ToString()))
            throw new System.Exception("WindowStartWithScore is trying to load stage without clear record");
        StartCoroutine(ApplyClearData());
    }

    IEnumerator ApplyClearData()
    {
        foreach (var star in stars)
        {
            star.SetActive(false);
        }
        var clearData = SaveDataManager.clearRecord[stageToLoad.ToString()];
        highScore.text = clearData.score.ToString();
        StartCoroutine(LoadWorldRank());
        yield return new WaitForSeconds(timeToOpenWindow);
        foreach (var star in stars.Take(clearData.stars))
        {
            star.SetActive(true);
            star.transform.DOScale(Vector3.one * 0.2f, 0.4f).SetEase(Ease.OutBack).From();
            yield return new WaitForSeconds(0.1333f);
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
        try
        {
            var ranking = (float) System.Convert.ToDouble(www.text) * 100;
            rank.text = ranking.ToString("0.0") + "%";
        }
        catch
        {
            Debug.Log("Invalid ranking");
        }
    }
}