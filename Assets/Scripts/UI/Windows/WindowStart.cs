using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : Window
{
    public Text levelNumber;
    protected int toLoad;

    public void OpenStartWindow(int stageToLoad)
    {
        levelNumber.text = stageToLoad.ToString();
        toLoad = stageToLoad;
        GetComponentInChildren<MissionViewStageSelect>().SetIcons(stageToLoad);
        OpenWindow();
    }

    public void StartStage()
    {
        if (HeartManager.heartLeft <= 0)
        {
            WindowHeartInsufficient.Open();
            return;
        }
        HeartManager.SpendHeart();
        StageManager.LoadStage(toLoad);
        PlaySound("startStage");
    }
}