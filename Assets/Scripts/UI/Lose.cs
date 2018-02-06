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
        switch (cause)
        {
            case GameState.LoseCause.BalloonLost:
                text.text = DB.MessageDB["fail_balloonLost"];
                break;
            case GameState.LoseCause.CharacterLost:
                text.text = DB.MessageDB["fail_bearFalled"];
                break;
            case GameState.LoseCause.Collapsed:
                text.text = DB.MessageDB["fail_nablaFalled"];
                break;
            case GameState.LoseCause.Crushed:
                text.text = DB.MessageDB["fail_crushed"];
                break;
            case GameState.LoseCause.Objective:
                text.text = DB.MessageDB["fail_objective"];
                break;
            case GameState.LoseCause.Boomed:
                text.text = DB.MessageDB["fail_boomed"];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cause), cause, null);
        }
    }

    [UsedImplicitly]
    public void ToStageSelect()
    {
        StageManager.LoadStageSelectScene();
    }
}