using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    public Material material;

//    void OnEnable()
//    {
//        material = new Material(Shader.Find("Hidden/GrayscaleFromPoint"));
//    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }
}