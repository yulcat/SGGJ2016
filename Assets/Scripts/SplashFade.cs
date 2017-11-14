using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SplashFade : MonoBehaviour
{
    static bool isFirstTime = true;
    // Use this for initialization
    IEnumerator Start()
    {
        if (!isFirstTime)
        {
            this.gameObject.SetActive(false);
            yield break;
        }
        isFirstTime = false;
        var child = transform.GetChild(0);
        child.gameObject.SetActive(true);
        var img = child.GetComponent<Image>();
        yield return new WaitForSeconds(0.1f);
        img.DOColor(Color.clear, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
