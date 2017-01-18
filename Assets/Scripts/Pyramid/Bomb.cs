using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Bomb : Block {
	public override void ClickListener()
    {
		if(pyramid == null || GameState.instance.isGameEnd) return;
		pyramid.RemoveBlock(this);
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = transform.TransformVector(Vector3.forward * 12f);
    }
	void Explode()
	{
		var onLeft = pyramid.GetBlock(b => CheckSide(position, b, -1));
		var onRight = pyramid.GetBlock(b => CheckSide(position, b, 1));
	}
	bool CheckSide(XY pos, PyramidComponent target, int direction)
	{
		XY check = new XY();
		if(target is Block)
		{
			check = (target as Block).position;
		}
		else if(target is CharacterControl)
		{
			check = new XY(target.transform.localPosition);
		}
		if(check.y == pos.y) return false;
		if(direction == 1) return (check.x >= pos.x - 3);
		else if (direction == -1) return (check.x <= pos.x + 3);
		else throw new System.Exception("Finding wierd direction");
	}
}
