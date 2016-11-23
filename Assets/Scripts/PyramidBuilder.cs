using UnityEngine;
using System.Linq;
using LitJson;
using System.Collections.Generic;

[System.SerializableAttribute]
public enum BlockType { Character=0, FlagBalloon, Empty, Wood, Sand, Iron, Balloon }
public struct BlockData
{
	public int[] data;
	public BlockType GetBlockType()
	{
		return (BlockType)data[0];
	}
	public void SetBlockType(BlockType t)
	{
		data[0] = (int)t;
	}
	public XY GetXY()
	{
		return new XY(data[1],data[2]);
	}
	public BlockData(BlockType _type, int _x, int _y)
	{
		data = new int[3];
		data[0] = (int)_type;
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
public class PyramidBuilder : MonoBehaviour {
	public int stageToLoad;
	Dictionary<BlockType,GameObject> resource = new Dictionary<BlockType,GameObject>();
	// Use this for initialization
	void Start () {
		var stage = Resources.Load<TextAsset>("Stages/" + stageToLoad);
		if(!stage) return;
		// JsonMapper.ToObject(stage);
	}
	GameObject GetBlock(BlockType blockType)
	{
		if(resource.ContainsKey(blockType)) return resource[blockType];
		var obj = Resources.Load<GameObject>("Blocks/"+blockType.ToString());
		resource.Add(blockType, obj);
		return obj;
	}
	public void LoadStage()
	{
		var stage = Resources.Load<TextAsset>("Stages/" + stageToLoad);
		if(!stage) return;
		Build(JsonMapper.ToObject<List<int[]>>(stage.text).Select(i => new BlockData(i)));
	}
	public void Build(IEnumerable<BlockData> blockData)
	{
		var childCount = transform.childCount;
		for(int i=0; i<childCount; i++)
		{
			var c = transform.GetChild(0);
			c.SetParent(null);
			DestroyImmediate(c.gameObject);
		}
		List<PyramidComponent> instantiated = new List<PyramidComponent>();
		foreach(var block in blockData)
		{
			if(block.GetBlockType() == BlockType.Empty) continue;
			var newObj = Instantiate<GameObject>(GetBlock(block.GetBlockType()));
			newObj.transform.SetParent(transform);
			newObj.transform.localPosition = block.GetXY().ToVector3();
			instantiated.Add(newObj.GetComponent<PyramidComponent>());
		}
		GetComponent<Pyramid>().EnlistBlocks(instantiated);
	}
}
