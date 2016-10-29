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
			return GetTorque(position.ToVector3(), body.mass);
			// return position.x * 0.5f * body.mass;
		}
	}

	protected override void FallResult()
	{
		base.FallResult();
		var character = pyramid.GetBlock(c => c is CharacterControl) as CharacterControl;
		if(character.BlockFallTest(this))
		{
			character.crushEffect.SetActive(true);
			GameState.Lose(GameState.LoseCause.Crushed);
		}
	}

	void RefreshPositionSelf(XY targetPosition)
    {
		if(pyramid.HasBlocks(c => CheckFeet(targetPosition, c)))
		{
			if(position == targetPosition) return;
			FallTo(position.y, targetPosition.y);
			position = targetPosition;
			pyramid.RefreshBlocks();
		}
		else if(targetPosition.y == 1)
		{
			pyramid.RemoveBlock(this);
			FallOff();
		}

		else
		{
			RefreshPositionSelf(new XY(targetPosition.x, targetPosition.y-2));
		}
    }
	
	bool CheckFeet(XY pos, PyramidComponent target)
	{
		if(pos.x == 0 && pos.y == 1)
		{
			return true;
		}
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
    protected virtual void OnMouseDown()
    {
		if(GameState.instance.isGameEnd) return;
		pyramid.RemoveBlock(this);
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = Vector3.forward * 12f;
		var col = GetComponent<Collider>();
		Invoke("DestroySelf",5f);
    }
}
