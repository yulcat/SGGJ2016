using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public struct XY
{
	public int x;
	public int y;
	public XY(int newX, int newY)
	{
		x = newX;
		y = newY;
	}
	public XY(Vector3 vec)
	{
		vec.y = Mathf.Floor(vec.y);
		vec = vec * 2f;
		vec.y ++;
		x = Mathf.RoundToInt(vec.x);
		y = Mathf.RoundToInt(vec.y);
	}
	public Vector3 ToVector3()
	{
		var vec = new Vector3(x,y,0);
		return vec/2f;
	}
	public static bool operator == (XY one, XY other)
	{
		return one.x == other.x && one.y == other.y;
	}
	public static bool operator != (XY one, XY other)
	{
		return !(one == other);
	}
}
public class Block : PyramidComponent
{
	List<Block> Feet;
	public XY position;

	public override void SetPyramid(Pyramid m)
	{
		base.SetPyramid(m);
		var floatPos = transform.localPosition * 2f;
		position = new XY(Mathf.RoundToInt(floatPos.x), Mathf.RoundToInt(floatPos.y));
	}
	public override void RefreshPosition()
	{
		RefreshPositionSelf(position);
	}

	public override float torque
	{
		get
		{
			return position.x * 0.5f * body.mass;
		}
	}

	void RefreshPositionSelf(XY targetPosition)
    {
		if(targetPosition.x == 0 && targetPosition.y == 1)
		{
			return;
		}
		if(targetPosition.y == 1)
		{
			FallOff();
		}
		else if(pyramid.HasBlocks(c => CheckFeet(targetPosition, c)))
		{
			if(position == targetPosition) return;
			FallTo(position.y, targetPosition.y);
			position = targetPosition;
			pyramid.RefreshBlocks();
		}
		else
		{
			RefreshPositionSelf(new XY(targetPosition.x, targetPosition.y-2));
		}
    }
	
	bool CheckFeet(XY pos, PyramidComponent target)
	{
		if(target is CharacterControl)
		{
			//Check if character is crashed
			return false;
		}
		if(target is Block)
		{
			var check = (target as Block).position;
			return (check.y == pos.y - 2)
				&& (check.x >= pos.x - 1)
				&& (check.x <= pos.x + 1);
		}
		return false;
	}
    void OnMouseDown()
    {
		pyramid.RemoveBlock(this);
		Destroy(gameObject);
    }
}
