using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterControl : PyramidComponent {
	Vector3 moveTarget;
	int currentFloor;
	public float thickness = 0.25f;
	public float moveSpeed = 1f;
	public GameObject crushEffect;
	Animator anim;

	public override void SetPyramid(Pyramid m)
	{
		base.SetPyramid(m);
		var floatPos = transform.localPosition * 2f;
		currentFloor = Mathf.RoundToInt(transform.localPosition.y * 2f);
		anim = GetComponentInChildren<Animator>(true);
	}

	public override void RefreshPosition()
	{
		RefreshPositionSelf(transform.localPosition.x, currentFloor);
	}

	void RefreshPositionSelf(float x, int y)
    {
		if(y <= 1)
		{
			pyramid.RemoveBlock(this);
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
			return GetTorque(transform.localPosition, body.mass);
			// return transform.localPosition.x * body.mass;
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
	public bool BlockFallTest(Block target)
	{
		return CheckOverlap(transform.localPosition.x, currentFloor, target);
	}

	bool CheckFeet(float x, int y, PyramidComponent target, int dy = -2)
	{
		if(target is Block && !(target is FlagBalloon))
		{
			var check = (target as Block).position;
			return (check.y == y - 2)
				&& (check.x + 1 >= (x - thickness)*2f)
				&& (check.x - 1 <= (x + thickness)*2f);
		}
		return false;
	}

	bool CheckOverlap(float x, int y, PyramidComponent target)
	{
		if(target is Block)
		{
			var check = (target as Block).position;
			return (check.y == y)
				&& (check.x + 1 >= (x - thickness)*2f)
				&& (check.x - 1 <= (x + thickness)*2f);
		}
		return false;
	}

	bool CheckFlag(float x, int y, PyramidComponent target)
	{
		if(target is FlagBalloon) return CheckOverlap(x,y,target);
		else return false;
	}

	IEnumerator Start () {
		while(true)
		{
			yield return null;
			if(pyramid == null || floating) continue;
			var currentX = transform.localPosition.x;
			var direction = Input.GetAxis("Horizontal");
			if(Mathf.Abs(direction) < 0.3f)
			{
				anim.SetBool("IsTrace",false);
				continue;
			}
			var rotation = direction>0? 120 : -120;
			anim.transform.localRotation = Quaternion.Euler(0,rotation,0);
			anim.SetBool("IsTrace",true);
			float dx = direction * moveSpeed * Time.deltaTime;
			float destination = currentX + dx;
			var flag = pyramid.GetBlock(c => CheckFlag(destination,currentFloor,c));
			if(flag != null)
			{
				(flag as FlagBalloon).Launch(this);
				yield break; //Reached Goal
			}
			if(pyramid.HasBlocks(c => CheckOverlap(destination,currentFloor,c)))
				continue; //Blocked by block
			
			transform.Translate(dx, 0, 0);
			if(!pyramid.HasBlocks(c => CheckFeet(destination,currentFloor,c)))
			{
				RefreshPosition();
				anim.SetBool("IsTrace",false);
				yield return StartCoroutine(WaitForLanding());
				//Jump off
			}
		}
	}
	public override void FallOff(bool refresh = true)
	{
		base.FallOff();
		StopAllCoroutines();
		GameState.Lose(GameState.LoseCause.CharacterLost);
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
