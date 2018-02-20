using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    public Material material;

    const float AnimationTime = 1f;
    static GrayscaleEffect instance;

    void Awake()
    {
        if (material == null)
        {
            material = Resources.Load<Material>("Grayscale");
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }

    void SetGrayscaleInstance(Vector2 pos)
    {
        StartCoroutine(SetGrayscaleCoroutine(pos));
    }

    IEnumerator SetGrayscaleCoroutine(Vector2 position)
    {
        material.SetVector("_GrayscaleCenter", new Vector4(position.x, position.y, 0, 0));
        material.SetFloat("_GrayscaleRadius", 0);
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.To(() => material.GetFloat("_GrayscaleRadius"), f => material.SetFloat("_GrayscaleRadius", f), 2,
            AnimationTime).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(AnimationTime);
    }

    public static void SetGrayscale(Vector3 position)
    {
        if (instance == null)
        {
            instance = Camera.main.gameObject.AddComponent<GrayscaleEffect>();
        }
        var pos = Camera.main.WorldToViewportPoint(position);
        instance.SetGrayscaleInstance(pos);
    }

    public static void UnsetGraysclae()
    {
        Destroy(instance);
    }
}