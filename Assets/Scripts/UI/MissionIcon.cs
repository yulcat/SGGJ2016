using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissionIcon : MonoBehaviour
{
    public Color completeColor;
    public Color fontCompleteColor;

    [NonSerialized]
    public string iconType;

    Text text;
    Image bgImg;
    BlockIconTextureApplier blockIcon;
    int maxCount;


    // Use this for initialization
    void Awake()
    {
        bgImg = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        blockIcon = GetComponentInChildren<BlockIconTextureApplier>();
    }

    public void SetIcon(string blockType, StageManager.Theme theme, int maxCountToSet)
    {
        var icons = blockIcon.blockIconPresetList.Where(b => b.type.ToString() == blockType);
        var blockIconPresets = icons as BlockIconPreset[] ?? icons.ToArray();
        if (blockIconPresets.Any(b => b.theme == theme))
        {
            blockIcon.LoadIcon(blockIconPresets.First(b => b.theme == theme));
        }
        else if (blockIconPresets.Any())
        {
            blockIcon.LoadIcon(blockIconPresets.First(b => b.theme == StageManager.Theme.Common));
        }
        else
        {
            throw new Exception("MissionIcon Cannot Load Icon : " + blockType);
        }

        maxCount = maxCountToSet;
        text.text = "0/" + maxCount;
        iconType = blockType;
    }

    public void SetSelectIcon(string blockType, StageManager.Theme theme, string textToSet)
    {
        var icons = blockIcon.blockIconPresetList.Where(b => b.type.ToString() == blockType);
        var blockIconPresets = icons as BlockIconPreset[] ?? icons.ToArray();
        if (blockIconPresets.Any(b => b.theme == theme))
        {
            blockIcon.LoadIcon(blockIconPresets.First(b => b.theme == theme));
        }
        else if (blockIconPresets.Any())
        {
            blockIcon.LoadIcon(blockIconPresets.First(b => b.theme == StageManager.Theme.Common));
        }
        else
        {
            throw new Exception("MissionIcon Cannot Load Icon : " + blockType);
        }

        text.text = textToSet;
        iconType = blockType;
    }

    public void SetCount(int count)
    {
        text.text = string.Format("{0}/{1}", Mathf.Min(maxCount, count), maxCount);
        if (count >= maxCount)
        {
            SetIconComplete();
        }
    }

    void SetIconComplete()
    {
        bgImg.color = completeColor;
        text.color = fontCompleteColor;
    }
}