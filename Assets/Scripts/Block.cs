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
public class Block : MonoBehaviour
{
	List<Block> Feet;
	Pyramid pyramid;
	bool floating;
	Rigidbody body;
	public XY position;

	public void SetPyramid(Pyramid m)
	{
		floating = false;
		pyramid = m;
		body = GetComponent<Rigidbody>();
		var floatPos = transform.localPosition * 2f;
		position = new XY(Mathf.RoundToInt(floatPos.x), Mathf.RoundToInt(floatPos.y));
	}

	public void RefreshPosition()
	{
		RefreshPositionSelf(position);
	}

	public float torque
	{
		get
		{
			return position.x * 0.5f * body.mass;
		}
	}

    void RefreshPositionSelf(XY targetPosition)
    {
		Debug.Log(position.ToVector3());
		if(targetPosition.x == 0 && targetPosition.y == 1)
		{
			return;
		}
		if(targetPosition.y == 1)
		{
			FallOff();
		}
		else if(pyramid.HasFeet(targetPosition))
		{
			FallTo(targetPosition);
		}
		else
		{
			RefreshPositionSelf(new XY(targetPosition.x, targetPosition.y-2));
		}
    }
	void FallTo(XY targetPosition)
	{
		if(position == targetPosition)
		{
			return;
		}
		transform.DOLocalMove(targetPosition.ToVector3(), 0.5f)
			.SetEase(Ease.InCubic)
			.OnComplete(() => floating = false);
		position = targetPosition;
		pyramid.RefreshBlocks();
	}
	public void FallOff()
	{
		withPhysics = true;
		pyramid.RemoveBlock(this);
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		Invoke("DestroySelf",5f);
	}
    void OnMouseDown()
    {
		pyramid.RemoveBlock(this);
		Destroy(gameObject);
    }
	void DestroySelf()
	{
		Destroy(gameObject);
	}
	bool withPhysics = false;
	void FixedUpdate()
	{
		if(withPhysics)
		{
			if(transform.position.y > 0.5f) return;
			GetComponent<Rigidbody>().velocity *= 0.8f;
		}
	}
}
