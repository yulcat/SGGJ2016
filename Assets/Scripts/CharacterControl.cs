using UnityEngine;
using System.Collections;

public class CharacterControl : PyramidComponent {
	Vector3 moveTarget;
	int currentFloor;
	public float thickness = 0.25f;
	public float moveSpeed = 1f;

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

	bool CheckFeet(float x, int y, PyramidComponent target, int dy = -2)
	{
		if(target is Block)
		{
			var check = (target as Block).position;
			return (check.y == y + dy)
				&& (check.x + 1 >= (x - thickness)*2f)
				&& (check.x - 1 <= (x + thickness)*2f);
		}
		return false;
	}

	bool CheckOverlap(float x, int y, PyramidComponent target)
	{
		return CheckFeet(x,y,target,0);
	}

	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			var pos = GetClickPosition();
			var y = Mathf.FloorToInt(pos.y) * 2 + 1;
			if(pyramid.HasBlocks(c => CheckFeet(pos.x, y, c))
				&& !pyramid.HasBlocks(c => CheckOverlap(pos.x, y, c)))
			{
				moveTarget = pos;
				StopAllCoroutines();
				StartCoroutine(MoveToTarget());
			}
		}
	}
	public override void FallOff()
	{
		base.FallOff();
		StopAllCoroutines();
	}
	IEnumerator MoveToTarget()
	{
		while(true)
		{
			var currentX = transform.localPosition.x;
			var toward = moveTarget.x - currentX;
			var abs = Mathf.Abs(toward);
			if(abs < 0.1f)
				yield break;

			int direction = (int)(toward / abs);
			float dx = direction * moveSpeed * Time.fixedDeltaTime;
			float destination = currentX + dx;
			if(pyramid.HasBlocks(c => CheckOverlap(destination,currentFloor,c)))
				yield break;
			
			transform.Translate(dx, 0, 0);
			if(!pyramid.HasBlocks(c => CheckFeet(destination,currentFloor,c)))
			{
				RefreshPosition();
				yield return StartCoroutine(WaitForLanding());
			}
			else
			{
				yield return new WaitForFixedUpdate();
			}
		}
	}
	IEnumerator WaitForLanding()
	{
		while(true)
		{
			if(floating) yield return null;
			else yield break;
		}
	}
}
