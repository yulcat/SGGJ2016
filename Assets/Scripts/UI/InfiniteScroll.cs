using UnityEngine;
using System.Collections;
using System.Linq;

public class InfiniteScroll : MonoBehaviour
{
	public float deltaX = 500;
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
        var pos = rect.anchoredPosition;
        if (pos.x > deltaX && buttons[0].index > 0)
        {
            pos.x -= deltaX;
            rect.anchoredPosition = pos;
			foreach(var b in buttons)
			{
				b.index--;
			}
        }
        else if (pos.x < -deltaX && buttons[buttons.Length-1].index < 20)
        {
            pos.x += deltaX;
            rect.anchoredPosition = pos;
			foreach(var b in buttons)
			{
				b.index++;
			}
        }
    }
}
