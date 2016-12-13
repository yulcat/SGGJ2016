using UnityEngine;
using System.Collections;
using System.Linq;

public class InfiniteScroll : MonoBehaviour
{
	public float deltaX = 500;
    public float xPosition
    {
        get
        {
            return rect.anchoredPosition.x + (delta * deltaX);
        }
    }
    int delta;
    RectTransform rect;
	LoadStageButton[] buttons;

    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
		buttons = GetComponentsInChildren<LoadStageButton>();
    }

    // Update is called once per frame
    void Update()
    {
        var size = rect.sizeDelta;
        size.x = Mathf.Clamp(3000 - rect.anchoredPosition.x, 3000, 20000);
        rect.sizeDelta = size;
    }
}
