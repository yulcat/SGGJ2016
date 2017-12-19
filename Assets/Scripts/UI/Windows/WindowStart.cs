﻿using JetBrains.Annotations;
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

    [UsedImplicitly]
    public void StartStage()
    {
        if (HeartManager.HeartLeft < GameState.GetHeartCost())
        {
            WindowHeartInsufficient.Open();
            return;
        }
        HeartManager.SpendHeart();
        StageManager.LoadStage(toLoad);
        PlaySound("startStage");
    }
}