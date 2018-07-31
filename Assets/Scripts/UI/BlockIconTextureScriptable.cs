using UnityEngine;

public class BlockIconTextureScriptable : ScriptableObject
{
    public BlockIconPreset[] blockIconPresetList;
    static BlockIconTextureScriptable _instance;

    public static BlockIconTextureScriptable instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<BlockIconTextureScriptable>("BlockIcons");
            return _instance;
        }
    }
}