using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIRadioBtnExample : MonoBehaviour
{
    public RectTransform[] points;
    public Color onColor;
    public Color offColor;

    private void Start()
    {
        OnClick(0);
    }

    public void OnClick(int index)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (index == i)
            {
                points[i].Find("color").GetComponent<Image>().color = onColor;
            }
            else
            {
                points[i].Find("color").GetComponent<Image>().color = offColor;
            }
        }
    }
}