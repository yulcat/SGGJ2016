using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

[System.SerializableAttribute]
public enum BlockType { Character=0, FlagBalloon, Empty, Wood, Sand, Iron, Balloon }
public struct BlockData
{
	public int type;
	public int x;
	public int y;
	public BlockData(BlockType _type, int _x, int _y)
	{
		type = (int)_type;
		x = _x;
		y = _y;
	}
}
public struct StageData
{
	public List<BlockData> data;
	public int number;
}
public class PyramidBuilder : MonoBehaviour {
	public string stageToLoad;
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
		Build(JsonMapper.ToObject<List<BlockData>>(stage.text));
	}
	public void Build(List<BlockData> blockData)
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
			if((BlockType)block.type == BlockType.Empty) continue;
			var newObj = Instantiate<GameObject>(GetBlock((BlockType)block.type));
			newObj.transform.SetParent(transform);
			newObj.transform.localPosition = new Vector2(block.x * 0.5f, block.y * 0.5f);
			instantiated.Add(newObj.GetComponent<PyramidComponent>());
		}
		GetComponent<Pyramid>().EnlistBlocks(instantiated);
	}
}
