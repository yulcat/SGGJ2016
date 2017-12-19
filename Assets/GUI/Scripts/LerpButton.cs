using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LerpButton : MonoBehaviour
{
    public void LerpColor(Color color)
    {
        StartCoroutine(YLerpColor(color));
    }

    private IEnumerator YLerpColor(Color color)
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            GetComponent<Image>().color = Color.Lerp(Color.white, color, t);
            yield return null;
            if (t >= 1)
                break;
        }
    }
}