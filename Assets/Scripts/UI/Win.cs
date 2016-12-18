using UnityEngine.UI;

public class Win : Window {
	public Text stageNumber;
	override protected void OnEnable()
	{
		base.OnEnable();
		stageNumber.text = FindObjectOfType<PyramidBuilder>().stageToLoad.ToString();
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
