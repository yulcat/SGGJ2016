using UnityEngine;
using System.Linq;
using LitJson;
using System.Collections.Generic;
using System;

[System.SerializableAttribute]
public enum BlockType
{
    Character = 0,
    FlagBalloon,
    Empty,
    Light,
    Medium,
    Heavy,
    Balloon,
    Fixed,
    Coin,
    Water,
    Barrel,
    Gift,
    Wood,
    More,
    Snow,
    TutorialBase,
    Landmine,
    TimeBomb,
}

public struct BlockData
{
    public int[] data;

    public BlockType GetBlockType()
    {
        return (BlockType) data[0];
    }

    public void SetBlockType(BlockType t)
    {
        data[0] = (int) t;
    }

    public XY GetXY()
    {
        return new XY(data[1], data[2]);
    }

    public BlockData(BlockType _type, int _x, int _y)
    {
        data = new int[3];
        data[0] = (int) _type;
        data[1] = _x;
        data[2] = _y;
    }

    public BlockData(int[] newData)
    {
        data = newData;
    }
}

public struct StageData
{
    public List<BlockData> data;
    public int number;
}

public class PyramidBuilder : MonoBehaviour
{
    public string currentTheme;
    public int stageToLoad;
    Dictionary<string, GameObject[]> loadedBlocks = new Dictionary<string, GameObject[]>();

    public GameObject GetBlock(BlockType blockType)
    {
        if (!string.IsNullOrEmpty(currentTheme))
        {
            if (!loadedBlocks.ContainsKey(currentTheme))
            {
                var loaded = Resources.LoadAll<GameObject>("Blocks/" + currentTheme);
                loadedBlocks.Add(currentTheme, loaded);
            }
            var foundBlock = FindProperBlock(loadedBlocks[currentTheme], blockType);
            if (foundBlock) return foundBlock;
        }
        if (!loadedBlocks.ContainsKey("Common"))
        {
            var loaded = Resources.LoadAll<GameObject>("Blocks/Common");
            loadedBlocks.Add("Common", loaded);
        }
        var foundCommonBlock = FindProperBlock(loadedBlocks["Common"], blockType);
        if (foundCommonBlock) return foundCommonBlock;
        throw new System.Exception("no proper block found");
    }

    GameObject FindProperBlock(GameObject[] objs, BlockType blockType)
    {
        return objs
            .Where(o => o.GetComponent<PyramidComponent>() != null)
            .FirstOrDefault(o => o.GetComponent<PyramidComponent>().BlockType == blockType);
    }

    public void LoadStage()
    {
        var stage = Resources.Load<TextAsset>("Stages/" + stageToLoad);
        if (!stage) throw new System.Exception(stageToLoad + " is not valid stage");
        var blockData = JsonMapper.ToObject<List<int[]>>(stage.text).Select(i => new BlockData(i));
        Build(blockData);
        SetCameraAndBackground(blockData);
    }

    private void SetCameraAndBackground(IEnumerable<BlockData> blockData)
    {
        int maxY = blockData
            .Where(block => block.GetBlockType() != BlockType.Character)
            .Max(block => (block.GetXY().y + 1) / 2);
        Resources.FindObjectsOfTypeAll<CameraSetter>()[0].SetMainCamera(maxY);
    }

    public void Build(IEnumerable<BlockData> blockData)
    {
        transform.localRotation = Quaternion.identity;
        FindObjectsOfType<PyramidComponent>().ToList().ForEach(p => DestroyImmediate(p.gameObject));
        var childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var c = transform.GetChild(0);
            c.SetParent(null);
            DestroyImmediate(c.gameObject);
        }
        List<PyramidComponent> instantiated = new List<PyramidComponent>();
        foreach (var block in blockData)
        {
            if (block.GetBlockType() == BlockType.Empty) continue;
            var newObj = Instantiate<GameObject>(GetBlock(block.GetBlockType()));
            newObj.transform.SetParent(transform, false);
            newObj.transform.localPosition = block.GetXY().ToVector3();
            newObj.transform.localRotation = Quaternion.identity;
            instantiated.Add(newObj.GetComponent<PyramidComponent>());
        }
        GetComponent<Pyramid>().EnlistBlocks(instantiated);
    }

    public void ClearStage()
    {
        var childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}