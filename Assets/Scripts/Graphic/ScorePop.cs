using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScorePop : MonoBehaviour
{
    static GameObject original;
    static CanvasScaler canvas;
    static List<GameObject> pool = new List<GameObject>();
    Text text;
    RectTransform rect;
    ScoreMeshModifier modifier;
    public float moveDistance;

    public static GameObject OriginalPop
    {
        get
        {
            if (original != null) return original;
            original = Resources.Load<GameObject>("Effects/ScorePop");
            return original;
        }
    }

    static CanvasScaler Canvas
    {
        get
        {
            if (canvas != null) return canvas;
            canvas = FindObjectOfType<CanvasScaler>();
            return canvas;
        }
    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        modifier = GetComponent<ScoreMeshModifier>();
    }

    public static void ShowScore(int score, Vector3 position)
    {
        var toSpawn = pool.FirstOrDefault(p => !p.activeSelf);
        if (toSpawn == null)
        {
            toSpawn = Instantiate(OriginalPop);
            pool.Add(toSpawn);
        }
        toSpawn.SetActive(true);
        toSpawn.transform.SetParent(Canvas.transform);
        var scorePop = toSpawn.GetComponent<ScorePop>();
        scorePop.StartPop(score, position);
    }

    void StartPop(int score, Vector3 position)
    {
        SetPosition(position);
        SetScore(score);
        rect.DOAnchorPos(Vector2.up * moveDistance + rect.anchoredPosition, 0.7f);
        rect.DOPunchScale(Vector3.one * 1.5f, 0.3f, 4);
        modifier.transparency = 1f;
        DOTween.To(() => modifier.transparency, value =>
            {
                modifier.transparency = value;
                text.SetVerticesDirty();
            }, 0f, 0.5f)
            .SetEase(Ease.InCubic)
            .SetDelay(0.2f)
            .OnComplete(() => gameObject.SetActive(false))
            .Play();
    }

    void SetScore(int score)
    {
        text.text = $"+{score}";
    }

    void SetPosition(Vector3 position)
    {
        var screenPosition = Camera.main.WorldToScreenPoint(position);
        var relative = Canvas.referenceResolution.y / Screen.height;
        var yPosition = screenPosition.y * relative;
        var xFromCenter = screenPosition.x - Screen.width * 0.5f;
        var xPosition = Canvas.referenceResolution.x * 0.5f + xFromCenter * relative;
        rect.anchoredPosition = new Vector2(xPosition, yPosition);
    }

    void OnDestroy()
    {
        pool.Remove(gameObject);
    }
}