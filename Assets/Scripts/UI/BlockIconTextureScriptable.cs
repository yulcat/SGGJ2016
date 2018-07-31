using UnityEngine;

public class BlockIconTextureScriptable : ScriptableObject
{
    public BlockIconPreset[] blockIconPresetList;
    static BlockIconTextureScriptable instance;

    public static BlockIconTextureScriptable Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = Resources.Load<BlockIconTextureScriptable>("BlockIcons");
            return instance;
        }
    }
}