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
                text.text = MessageData.dictionary["fail_balloonLost"];
                break;
            case GameState.LoseCause.CharacterLost:
                text.text = MessageData.dictionary["fail_bearFalled"];
                break;
            case GameState.LoseCause.Collapsed:
                text.text = MessageData.dictionary["fail_nablaFalled"];
                break;
            case GameState.LoseCause.Crushed:
                text.text = MessageData.dictionary["fail_crushed"];
                break;
            case GameState.LoseCause.Objective:
                text.text = MessageData.dictionary["fail_objective"];
                break;
            case GameState.LoseCause.Boomed:
                text.text = MessageData.dictionary["fail_boomed"];
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