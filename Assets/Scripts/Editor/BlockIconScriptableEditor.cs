using UnityEditor;
using UnityEngine;

public class BlockIconScriptableEditor
{
    public static void Create(BlockIconPreset[] presets)
    {
        var inst = ScriptableObject.CreateInstance<BlockIconTextureScriptable>();
        inst.blockIconPresetList = presets;
        AssetDatabase.CreateAsset(inst, "Assets/Resources/BlockIcons.asset");
    }
}