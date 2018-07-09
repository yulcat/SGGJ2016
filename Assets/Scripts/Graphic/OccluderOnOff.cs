using UnityEngine;

public class OccluderOnOff : MonoBehaviour
{
    Pyramid nabla;

    Renderer occluderRenderer;
    float defaultAlpha;

    // Use this for initialization
    void Start()
    {
        nabla = FindObjectOfType<Pyramid>();
        occluderRenderer = GetComponentInChildren<Renderer>();
        defaultAlpha = occluderRenderer.material.GetFloat("_Alpha");
    }

    // Update is called once per frame
    void Update()
    {
        occluderRenderer.material.SetFloat("_Alpha", nabla.IsCollapsed() ? 0f : 0.681f);
    }
}