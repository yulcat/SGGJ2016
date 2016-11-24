using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using System.IO;

public class StageMaker : MonoBehaviour
{
	[System.SerializableAttribute]
	public struct BlockTypeSetting
	{
		public BlockType type;
		public int max;
	
	}
    
    public BlockType[] basicBlocks;
    public BlockTypeSetting[] specialBlocks;
	public int height;
	public float maxTorque = 10f;
	public int maxTry = 100;
	Dictionary<BlockType, float> blockMassTable = new Dictionary<BlockType, float>();
	List<BlockData> current;
	public void MakeStage()
	{
		if(height<3) throw new System.Exception("Height > 3 needed");
		int tryCount = 0;
		while(tryCount++ < maxTry)
		{
			var stage = InitializeFloor(height);
			SetFlagBalloon(ref stage);
			stage.ForEach(floor => MakeFloor(ref floor));
			var blockData = ListToBlockData(stage);
			AddSpecialBlocks(blockData);
			if(!SanityCheck(ref blockData)) continue;
			GetComponent<PyramidBuilder>().Build(blockData);
			current = blockData;
			return;
		}
		Debug.LogError("Making Stage Failed");
	}
	List<List<BlockType>> InitializeFloor(int _height)
	{
		List<List<BlockType>> stage = new List<List<BlockType>>();
		for(int i=0; i<_height; i++)
		{
			var newFloor = new List<BlockType>();
			for(int j=0; j<i+1; j++)
			{
				newFloor.Add(BlockType.Empty);
			}
			stage.Add(newFloor);
		}
		return stage;
	}
	void SetFlagBalloon(ref List<List<BlockType>> stage)
	{
		var y = Random.Range(1,height-3);
		var x = Random.Range(0,y+1);
		stage[y][x] = BlockType.FlagBalloon;
	}
	void MakeFloor(ref List<BlockType> blueprint)
	{
		for(int i=0; i<blueprint.Count; i++)
		{
			if(blueprint[i] != BlockType.Empty) continue;
			var picked = PickBasicBlock();
			blueprint[i] = picked;
		}
	}
	BlockType PickBasicBlock()
	{
		var randomNumber = Random.Range(0, basicBlocks.Length);
		return basicBlocks[randomNumber];
	}
	float GetBlockMass(BlockType blockType)
	{
		if(blockType == BlockType.Empty) return 0;
		if(blockMassTable.ContainsKey(blockType)) return blockMassTable[blockType];
		var obj = Resources.Load<GameObject>("Blocks/"+blockType.ToString());
		var body = obj.GetComponent<Rigidbody>();
		float mass = body.mass;
		if(blockType == BlockType.Balloon || blockType == BlockType.FlagBalloon)
			mass *= -1;
		blockMassTable.Add(blockType, mass);
		return body.mass;
	}
	List<BlockData> ListToBlockData(List<List<BlockType>> stage)
	{
		var blockData = new List<BlockData>();
		for(int i=0; i<stage.Count; i++)
		{
			for(int j=0; j<stage[i].Count; j++)
			{
				blockData.Add(new BlockData(stage[i][j], j*2 - i, i*2 + 1));
			}
		}
		return blockData;
	}
	bool SanityCheck(ref List<BlockData> stage)
	{
		var torque = stage.Sum(b => GetBlockMass(b.GetBlockType()) * transform.TransformVector(b.GetXY().ToVector3()).x);
		var charMass = GetBlockMass(BlockType.Character);
		var charX = Mathf.RoundToInt(- torque / charMass * 2);
		var charY = stage.Max(d => d.GetXY().y) + 2;
		if(Mathf.Abs(charX) > Mathf.Abs(charY)/2) return false;
		stage.Add(new BlockData(BlockType.Character,charX,charY));
		return Mathf.Abs(torque) < maxTorque;
	}
	void AddSpecialBlocks(List<BlockData> blockData)
	{
		foreach(var b in specialBlocks)
		{
			for(int i=0; i<b.max; i++)
			{
				var selected = Random.Range(0,blockData.Count);
				if(basicBlocks.Any(basic => basic == blockData[selected].GetBlockType()))
				{
					var modified = blockData[selected];
					modified.SetBlockType(b.type);
					blockData[selected] = modified;
				}
			}
		}
	}
	public void SaveStage()
	{
		if(current == null) return;
		var stages = Resources.LoadAll<TextAsset>("Stages");
		int max = 0;
		if(stages.Length != 0)
		{
			max = stages.Max(s => System.Convert.ToInt32(s.name));
		}
		var pathBuilder = new System.Text.StringBuilder("Assets/Resources/Stages/");
		pathBuilder.Append((max+1).ToString());
		pathBuilder.Append(".json");
		var path = pathBuilder.ToString();
	
		string str = JsonMapper.ToJson(current.Select(b => b.data).ToArray());
		using (FileStream fs = new FileStream(path, FileMode.Create)){
			using (StreamWriter writer = new StreamWriter(fs)){
				writer.Write(str);
			}
		}
		#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh ();
		#endif
	}
}
