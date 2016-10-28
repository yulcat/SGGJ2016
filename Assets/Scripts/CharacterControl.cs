using UnityEngine;
using System.Collections;

public class CharacterControl : PyramidComponent {
	Vector3 moveTarget;
	int currentFloor;
	public float thickness = 0.25f;

	public override void SetPyramid(Pyramid m)
	{
		base.SetPyramid(m);
		var floatPos = transform.localPosition * 2f;
		currentFloor = Mathf.RoundToInt(transform.localPosition.y * 2f);
	}

	public override void RefreshPosition()
	{
		RefreshPositionSelf(transform.localPosition.x, currentFloor);
	}

	void RefreshPositionSelf(float x, int y)
    {
		if(y <= 1)
		{
			FallOff();
		}
		else if(pyramid.HasBlocks(c => CheckFeet(x,y,c)))
		{
			if(currentFloor == y) return;
			FallTo(currentFloor, y);
			currentFloor = y;
			pyramid.RefreshBlocks();
		}
		else
		{
			RefreshPositionSelf(x, y-2);
		}
    }

	public override float torque
	{
		get
		{
			return transform.localPosition.x * body.mass;
		}
	}

	void OnDrawGizmos()
	{
		if(!Application.isPlaying) return;
		Gizmos.color = Color.red;
        Gizmos.DrawSphere(pyramid.transform.TransformPoint(moveTarget), 0.52f);
	}

	Plane GetPyramidPlane()
	{
		return new Plane(pyramid.transform.forward, pyramid.transform.position);
	}

	Vector3 GetClickPosition()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var plane = GetPyramidPlane();
		float dist;
		plane.Raycast(ray, out dist);
		return pyramid.transform.InverseTransformPoint(ray.GetPoint(dist));
	}

	bool CheckFeet(float x, int y, PyramidComponent target)
	{
		if(target is Block)
		{
			var check = (target as Block).position;
			return (check.y == y - 2)
				&& (check.x + 1 >= (x - thickness)*2f)
				&& (check.x - 1 <= (x + thickness)*2f);
		}
		return false;
	}

	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			var pos = GetClickPosition();
			pos.y = Mathf.Floor(pos.y) + 0.5f;
			moveTarget = pos;
		}
	}
}
