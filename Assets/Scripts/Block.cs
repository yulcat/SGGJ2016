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
}
public class Block : MonoBehaviour
{
	List<Block> Feet;
	Pyramid pyramid;
	public XY position;

	public void SetPyramid(Pyramid m)
	{
		pyramid = m;
		var floatPos = transform.position * 2f;
		position = new XY(Mathf.RoundToInt(floatPos.x), Mathf.RoundToInt(floatPos.y));
	}

	public void RefreshPosition()
	{
		RefreshPositionSelf(position);
	}

    void RefreshPositionSelf(XY targetPosition)
    {
		if(targetPosition.x == 0 && targetPosition.y == 1) return;
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
			targetPosition.y -= 2;
			RefreshPositionSelf(targetPosition);
		}
    }
	void FallTo(XY targetPosition)
	{
		transform.DOMove(targetPosition.ToVector3(), 0.5f).SetEase(Ease.InQuint);
		position = targetPosition;
	}
	void FallOff()
	{
		transform.DOMoveY(-1, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => Destroy(gameObject));
	}
    void OnMouseDown()
    {

    }
}
