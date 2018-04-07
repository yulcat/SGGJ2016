using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    public Material material;

    Material setTexture;
    const float AnimationTime = 1f;
    static List<GrayscaleEffect> instances;
    Camera cam;
    RenderTextureDescriptor descript;
    RenderTexture target;

    void Awake()
    {
        material = Resources.Load<Material>("Grayscale");
        setTexture = new Material(Shader.Find("Unlit/Unlit"));
        cam = GetComponent<Camera>();
    }

    void OnPreRender()
    {
        descript = new RenderTextureDescriptor()
        {
            width = Screen.width,
            height = Screen.height,
            depthBufferBits = 24,
            dimension = TextureDimension.Tex2D,
            colorFormat = RenderTextureFormat.ARGB32,
            volumeDepth = 1,
            msaaSamples = 1
        };
        target = RenderTexture.GetTemporary(descript);
        cam.targetTexture = target;
        Graphics.SetRenderTarget(target);
        GL.Clear(true, true, Color.clear);
    }

    void OnPostRender()
    {
        var dest = RenderTexture.GetTemporary(descript);
        Graphics.SetRenderTarget(dest.colorBuffer, target.depthBuffer);
        Graphics.Blit(target, material);
        RenderTexture.active = null;
        cam.targetTexture = null;
        Graphics.Blit(dest, setTexture);
        dest.Release();
        target.Release();
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
        if ((instances ?? (instances = new List<GrayscaleEffect>())).Count == 0)
        {
            foreach (var cam in FindObjectsOfType<Camera>())
            {
                instances.Add(cam.gameObject.AddComponent<GrayscaleEffect>());
            }
        }
        var pos = Camera.main.WorldToViewportPoint(position);
        instances.ForEach(instance => instance.SetGrayscaleInstance(pos));
    }

    public static void UnsetGraysclae()
    {
        instances.ForEach(Destroy);
        instances.Clear();
    }
}