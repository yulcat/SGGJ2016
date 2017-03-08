using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class MissionIcon : MonoBehaviour {
	public Color incompleteColor;
	public Color completeColor;
	public Color fontIncompleteColor;
	public Color fontCompleteColor;
	[System.NonSerializedAttribute]
	public string iconType;
	Text text;
	Image bgImg;
	BlockIconTextureApplier blockIcon;
	int maxCount;
	

	// Use this for initialization
	void Awake () {
		bgImg = GetComponent<Image>();
		text = GetComponentInChildren<Text>();
		blockIcon = GetComponentInChildren<BlockIconTextureApplier>();
	}
	public void SetIcon(string blockType, StageManager.Theme theme, int maxCountToSet)
	{
		var icons = blockIcon.blockIconPresetList.Where(b => b.type.ToString() == blockType);
		if(icons.Any(b => b.theme == theme))
		{
			blockIcon.LoadIcon(icons.First(b => b.theme == theme));
		}
		else if(icons.Count() > 0)
		{
			blockIcon.LoadIcon(icons.First(b => b.theme == StageManager.Theme.Common));
		}
		else
		{

			throw new System.Exception("MissionIcon Cannot Load Icon : "+blockType.ToString());
		}
		maxCount = maxCountToSet;
		text.text = "0/"+maxCount;
		iconType = blockType;
	}
	public void SetSelectIcon(string blockType, StageManager.Theme theme,  string textToSet)
	{
		var icons = blockIcon.blockIconPresetList.Where(b => b.type.ToString() == blockType);
		if(icons.Any(b => b.theme == theme))
		{
			blockIcon.LoadIcon(icons.First(b => b.theme == theme));
		}
		else if(icons.Count() > 0)
		{
			blockIcon.LoadIcon(icons.First(b => b.theme == StageManager.Theme.Common));
		}
		else
		{

			throw new System.Exception("MissionIcon Cannot Load Icon : "+blockType.ToString());
		}
		text.text = textToSet;
		iconType = blockType;
	}
	public void SetCount(int count)
	{
		text.text = string.Format("{0}/{1}",Mathf.Min(maxCount,count),maxCount);
		if(count >= maxCount)
		{
			SetIconComplete();
		}
		Debug.Log("Updated!");
	}

    private void SetIconComplete()
    {
        bgImg.color = completeColor;
		text.color = fontCompleteColor;
    }
}
