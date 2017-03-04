using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionView : MonoBehaviour {
	public GameObject missionIconPrefab;
	List<MissionIcon> icons = new List<MissionIcon>();

	// Use this for initialization
	void Start () {
		if(GameState.instance.mission == null || GameState.instance.mission.Count == 0)
		{
			gameObject.SetActive(false);
			return;
		}
		foreach(var kvp in GameState.instance.mission)
		{
			var instanciated = Instantiate<GameObject>(missionIconPrefab);
			instanciated.transform.SetParent(transform,false);
			var themeString = FindObjectOfType<PyramidBuilder>().currentTheme;
			var theme = (StageManager.Theme)System.Enum.Parse(typeof(StageManager.Theme),themeString);
			var icon = instanciated.GetComponent<MissionIcon>();
			icons.Add(icon);
			icon.SetIcon(kvp.Key,theme,kvp.Value);
		}
		GameState.instance.AccomplishedListener += UpdateIcon;
	}
	
	void UpdateIcon (string blockType, int count) {
		Debug.Log(blockType);
		if(icons.Any(i => i.iconType == blockType))
			icons.Find(i => i.iconType == blockType).SetCount(count);
	}
}
