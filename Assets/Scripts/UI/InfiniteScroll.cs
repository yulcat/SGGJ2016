using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class InfiniteScroll : MonoBehaviour
{
    public float deltaX = 500;
    float max = 1280;

    public float xPosition
    {
        get { return rect.anchoredPosition.x + (delta * deltaX); }
    }

    int delta;
    bool jumpingToStage = false;
    RectTransform rect;
    LoadStageButton[] buttons;

    public void JumpToStage(int index, bool openStartWindow)
    {
        jumpingToStage = true;
        if (openStartWindow)
        {
            GetComponent<RectTransform>().DOAnchorPosX((index - 1) * -500, 2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    GameObject.Find("Canvas").GetComponentInChildren<WindowStart>(true).OpenStartWindow(index);
                    jumpingToStage = false;
                });
        }
        else
        {
            GetComponent<RectTransform>().DOAnchorPosX((index - 1) * -500, 2f)
                .SetEase(Ease.OutCubic).OnComplete(() => jumpingToStage = false);
        }
    }

    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
        buttons = GetComponentsInChildren<LoadStageButton>();
        var maxStage = SaveDataManager.clearRecord.Count > 0
            ? SaveDataManager.clearRecord.Max(kvp => System.Convert.ToInt32(kvp.Key))
            : 0;
        if (SaveDataManager.clearRecord.Count > 0)
            max = maxStage * 500 + 1280;
        if (!jumpingToStage)
            JumpToStage(maxStage + 1, false);
    }

    // Update is called once per frame
    void Update()
    {
        var size = rect.sizeDelta;
        size.x = Mathf.Clamp(3000 - rect.anchoredPosition.x, 3000, max);
        rect.sizeDelta = size;
    }
}