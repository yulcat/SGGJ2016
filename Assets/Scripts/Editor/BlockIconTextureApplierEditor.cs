using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BlockIconTextureApplier))]
public class BlockIconTextureApplierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BlockIconTextureApplier applier = target as BlockIconTextureApplier;

        if (GUILayout.Button("Load Icon Image"))
        {
            applier.LoadIcon();
        }

        if (GUILayout.Button("Save Settings into Asset"))
        {
            BlockIconScriptableEditor.Create(applier.blockIconPresetList);
        }
    }
}