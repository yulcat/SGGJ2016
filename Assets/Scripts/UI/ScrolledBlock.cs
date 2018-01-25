using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrolledBlock : ScrolledGameObject
{
    TextMesh stageNumberText;
    public GameObject[] blocks;
    int prevIndex = -1;
    static Color inactiveGrey = new Color(0.42f, 0.42f, 0.42f, 1f);

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        stageNumberText = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        var x = scroll.xPosition * scrollRatio + initialX;
        var pos = transform.position;
        pos.x = x % deltaX;
        var index = localIndex - ((int) (x / deltaX) * 6);
        if (index != prevIndex)
            ResetBlocks(index);
        stageNumberText.text = (index + 1).ToString();
        pos.z = Mathf.Sin(index) * 0.5f;
        transform.localPosition = pos;
        prevIndex = index;
    }

    void ResetBlocks(int index)
    {
        var stage = index + 1;
        var loadStageButton = GetComponent<LoadStageButton>();
        loadStageButton.index = index;
        var maxClearedStage = 0;
        if (SaveDataManager.clearRecord.Count > 0)
            maxClearedStage = SaveDataManager.clearRecord.Max(kvp => System.Convert.ToInt32(kvp.Key));
        if (stage > maxClearedStage + 1)
        {
            loadStageButton.enabled = false;
            SetBlock(4);
            stageNumberText.color = inactiveGrey;
        }
        else if (stage == maxClearedStage + 1)
        {
            loadStageButton.enabled = true;
            SetBlock(0);
            stageNumberText.color = Color.white;
        }
        else
        {
            loadStageButton.enabled = true;
            try
            {
                SetBlock(SaveDataManager.clearRecord[stage.ToString()].stars);
            }
            catch
            {
                SetBlock(0);
            }
            stageNumberText.color = Color.white;
        }
    }

    void SetBlock(int blockNum)
    {
        for (var i = 0; i < blocks.Length; i++)
        {
            blocks[i].SetActive(i == blockNum);
        }
    }
}