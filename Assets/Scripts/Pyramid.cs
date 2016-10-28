using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pyramid : MonoBehaviour {
	List<Block> blocks;
	float angularVelocity = 0f;
	public float angularDamp = 0.1f;
	public float torqueMultiplier = 5f;
	public float returnTorque = 10f;
	// Use this for initialization
	void Start () {
		blocks = GetComponentsInChildren<Block>().ToList();
		blocks.ForEach(b => b.SetPyramid(this));
	}

	public void RemoveBlock(Block block)
	{
		blocks.Remove(block);
		RefreshBlocks();
	}
	public void RefreshBlocks()
	{
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
		return (check.y == pos.y - 2)
			&& (check.x >= pos.x - 1)
			&& (check.x <= pos.x + 1); 
	}
	
	void FixedUpdate () {
		var torqueSum = - blocks.Sum(b => b.torque) * torqueMultiplier;
		var currentRot = transform.rotation.eulerAngles.z;
		var returning = - returnTorque * Mathf.Sin(currentRot * Mathf.Deg2Rad);
		angularVelocity += returning + torqueSum;
		angularVelocity *= 1 - angularDamp;
		transform.rotation = Quaternion.Euler(0,0,currentRot + (angularVelocity * Time.fixedDeltaTime));
		Debug.Log(Mathf.Cos(currentRot * Mathf.Deg2Rad));
		if(Mathf.Cos(currentRot * Mathf.Deg2Rad) < 0.9f)
		{
			blocks.ForEach(b => b.FallOff());
		}
	}
}
