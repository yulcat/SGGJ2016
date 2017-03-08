using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionViewStageSelect : MonoBehaviour {
	public GameObject missionIconPrefab;
	List<MissionIcon> icons = new List<MissionIcon>();

	// Use this for initialization
	public void SetIcons (int stage) {
		foreach(var icon in icons)
		{
			Destroy(icon.gameObject);
		}
		icons.Clear();
		var stageData = StageDataLoader.GetStageData(stage);
		if(stageData.mission != null)
		{
			foreach(var kvp in stageData.mission)
			{
				InstanciateIcon(kvp.Key, stageData.theme, kvp.Value.ToString());
			}
		}
		InstanciateIcon("FlagBalloon",stageData.theme,"1");
	}
	void InstanciateIcon(string blockType, StageManager.Theme theme, string textToSet)
	{
		var instanciated = Instantiate<GameObject>(missionIconPrefab);
		instanciated.transform.SetParent(transform,false);
		var icon = instanciated.GetComponent<MissionIcon>();
		icons.Add(icon);
		icon.SetSelectIcon(blockType,theme,textToSet);
	}
}
