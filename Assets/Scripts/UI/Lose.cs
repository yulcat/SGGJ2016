using System;
using JetBrains.Annotations;
using UnityEngine.UI;

public class Lose : Window
{
    public Text text;

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

    public void SetMessage(GameState.LoseCause cause)
    {
        var loseCause = GetLoseCauseMessage(cause);
        text.text = FormatMessage(loseCause);
    }

    string GetLoseCauseMessage(GameState.LoseCause cause)
    {
        switch (cause)
        {
            case GameState.LoseCause.BalloonLost:
                return DB.MessageDB["fail_balloonLost"];
            case GameState.LoseCause.CharacterLost:
                return DB.MessageDB["fail_bearFalled"];
            case GameState.LoseCause.Collapsed:
                return DB.MessageDB["fail_nablaFalled"];
            case GameState.LoseCause.Crushed:
                return DB.MessageDB["fail_crushed"];
            case GameState.LoseCause.Objective:
                return DB.MessageDB["fail_objective"];
            case GameState.LoseCause.Boomed:
                return DB.MessageDB["fail_boomed"];
            default:
                throw new ArgumentOutOfRangeException(nameof(cause), cause, null);
        }
    }

    string FormatMessage(string messageToShow)
    {
        return $"{messageToShow}\n\n<size=50>{DB.MessageDB.GetTip()}</size>";
    }

    [UsedImplicitly]
    public void ToStageSelect()
    {
        StageManager.LoadStageSelectScene();
    }
}