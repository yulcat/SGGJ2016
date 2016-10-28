using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pyramid : MonoBehaviour {
	List<Block> blocks;
	// Use this for initialization
	void Start () {
		blocks = GetComponentsInChildren<Block>().ToList();
		blocks.ForEach(b => b.SetPyramid(this));
	}

	public void RemoveBlock(Block block)
	{
		blocks.Remove(block);
		blocks.ForEach(b => b.RefreshPosition());
	}

	public List<Block> FindFeet(XY pos)
	{
		return blocks.Where(b => CheckFeet(pos, b.position)).ToList();
	}

	public bool HasFeet(XY pos)
	{
		return blocks.Any(b => CheckFeet(pos, b.position));
	}

	public bool CheckFeet(XY pos, XY check)
	{
		return (check.y != pos.y - 2)
			&& (check.x >= pos.x - 1)
			&& (check.x <= pos.x + 1); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
