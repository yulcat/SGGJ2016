using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrolledBlock : ScrolledGameObject
{
    StageIndexText button;
    public GameObject[] blocks;
	int prevIndex = -1;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        var buttonObj = Instantiate<GameObject>(Resources.Load<GameObject>("StageButton"));
        buttonObj.transform.SetParent(GameObject.Find("IndexCanvas").transform);
        // buttonObj.transform.localScale = Vector3.one;
        button = buttonObj.GetComponent<StageIndexText>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        float x = scroll.xPosition * scrollRatio + initialX;
        var pos = transform.position;
        pos.x = x % deltaX;
        var index = localIndex - ((int)(x / deltaX) * 6);
		if(index != prevIndex)
			ResetBlocks(index);
        button.index = index;
        var rect = button.GetComponent<RectTransform>();
        pos.z = Mathf.Sin(index) * 0.5f;
        transform.localPosition = pos;
        button.transform.position = transform.position;
		prevIndex = index;
    }

    private void ResetBlocks(int index)
    {
		int stage = index + 1;
		var loadStageButton = GetComponent<LoadStageButton>();
		loadStageButton.index = index;
		int maxClearedStage = 0;
		if(SaveDataManager.clearRecord.Count > 0)
			maxClearedStage = SaveDataManager.clearRecord.Max(kvp => System.Convert.ToInt32(kvp.Key));
		if(stage > maxClearedStage + 1)
		{
			loadStageButton.enabled = false;
			SetBlock(4);
			button.GetComponent<Button>().interactable = false;
		}
		else if(stage == maxClearedStage + 1)
		{
			loadStageButton.enabled = true;
			SetBlock(0);
			button.GetComponent<Button>().interactable = true;
		}
		else
		{
			loadStageButton.enabled = true;
			SetBlock(SaveDataManager.clearRecord[stage.ToString()].stars);
			button.GetComponent<Button>().interactable = true;
		}
    }
	private void SetBlock(int blockNum)
	{
		for(int i=0; i<blocks.Length; i++)
		{
			blocks[i].SetActive(i == blockNum);
		}
	}
}
