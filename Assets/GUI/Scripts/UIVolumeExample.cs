using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIVolumeExample : MonoBehaviour
{
    public RectTransform[] points;
    public Color onColor;
    public Color offColor;
    public int index;

    private void Start()
    {
        index = 0;
        ChangeValue(index);
    }

    public void OnClick(int val)
    {
        index = Mathf.Clamp(index + val, 0, points.Length);
        ChangeValue(index);
    }

    private void ChangeValue(int index)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i < index)
            {
                points[i].GetComponent<Image>().color = onColor;
            }
            else
            {
                points[i].GetComponent<Image>().color = offColor;
            }
        }
    }
}