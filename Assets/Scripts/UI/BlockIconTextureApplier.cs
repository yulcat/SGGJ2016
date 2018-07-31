using System;
using UnityEngine;
using UnityEngine.UI;

public class BlockIconTextureApplier : MonoBehaviour
{
    [NonSerialized]
    public BlockIconPreset[] blockIconPresetList;

    public int toLoad;
    Image img;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        img = GetComponent<Image>();
        blockIconPresetList = BlockIconTextureScriptable.Instance.blockIconPresetList;
    }

    public void LoadIcon()
    {
        LoadIcon(blockIconPresetList[toLoad]);
    }

    public void LoadIcon(BlockIconPreset preset)
    {
        img = GetComponent<Image>();
        var material = new Material(img.material);
        material.SetTexture("_MainTex", preset.texture);
        material.SetTexture("_UVTex", preset.uvTexture);
        img.material = material;
    }
}

[Serializable]
public struct BlockIconPreset
{
    public StageManager.Theme theme;
    public BlockType type;
    public Texture texture;
    public Texture uvTexture;
}