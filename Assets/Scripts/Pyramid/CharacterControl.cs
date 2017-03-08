using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class CharacterControl : PyramidComponent {
	Vector3 moveTarget;
	int currentFloor;
	public float thickness = 0.25f;
	public float moveSpeed = 1f;
	[RangeAttribute(0f,1f)]
	public float counterTorqueMultiplier = 1f;
	public GameObject crushEffect;
	[System.NonSerializedAttribute]
	public Animator anim;

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
			anim.SetTrigger("Fail");
			pyramid.RemoveBlock(this);
			FallOff();
		}
		else if(pyramid.HasBlocks(c => CheckFeet(x,y,c)) || OverlapTest(x,y) != null)
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

	public bool BlockFallTest(Block target)
	{
		return CheckOverlap(transform.localPosition.x, currentFloor, target);
	}

	bool CheckFeet(float x, int y, PyramidComponent target, int dy = -2)
	{
		var blockTarget = target as Block;
		if(blockTarget && blockTarget.CollideResult)
		{
			var check = blockTarget.position;
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

	bool CheckCollideOverlap(float x, int y, PyramidComponent target)
	{
		var blockTarget = target as Block;
		if(blockTarget != null && blockTarget.CollideResult)
		{
			return CheckOverlap(x,y,target);
		}
		else return false;
	}

	bool CheckFlag<T>(float x, int y, PyramidComponent target)
	{
		if(target is T) return CheckOverlap(x,y,target);
		else return false;
	}

	IEnumerator Start () {
		var bodies = GetComponentsInChildren<Rigidbody>();
		foreach(var childBody in bodies)
		{
			childBody.constraints = RigidbodyConstraints.FreezeAll;
		}
		while(true)
		{
			yield return null;
			if(pyramid == null) continue;
			if(floating) yield return StartCoroutine(WaitForLanding());
			var currentX = transform.localPosition.x;
			var direction = Input.GetAxis("Horizontal");
			var touchDirection = InControl
				.InputManager
				.ActiveDevice
				.GetControl(InControl.InputControlType.LeftStickX)
				.Value;
			if(Mathf.Abs(direction) < 0.3f) direction = touchDirection;
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
			var overlap = OverlapTest(destination, currentFloor);
			if(overlap != null)
				overlap.Overlap(this);
			if(!enabled) yield break;
			if(pyramid.HasBlocks(c => CheckCollideOverlap(destination,currentFloor,c)))
				continue; //Blocked by block
			
			transform.Translate(dx, 0, 0);
			pyramid.ApplyMomentum(GetMoveMomentum(direction * moveSpeed) * counterTorqueMultiplier);
			if(!pyramid.HasBlocks(c => CheckFeet(destination,currentFloor,c)))
			{
				RefreshPosition();
				anim.SetBool("IsTrace",false);
				//Jump off
			}
		}
	}
	IOverlapLister OverlapTest(float x, int floor)
	{
		var overlap = pyramid.GetBlock(c => CheckFlag<IOverlapLister>(x,floor,c));
		if(overlap != null)
		{
			return overlap as IOverlapLister;
		}
		return null;
	}
	float GetMoveMomentum(float vx)
	{
		var mv = transform.right * vx * body.mass;
		var r = transform.localPosition;
		return -Vector3.Cross(r,mv).z;
	}
	public void TurnToCamera()
	{
		anim.transform.localRotation = Quaternion.Euler(0,180,0);
	}
	public override void FallOff(bool refresh = true)
	{
		transform.SetParent(null);
		FreeBodyRagdoll();
		GameState.Lose(GameState.LoseCause.CharacterLost);
	}
	public void FreeBodyRagdoll()
	{
		transform.DOKill();
		withPhysics = true;
		var bodies = GetComponentsInChildren<Rigidbody>();
		foreach(var childBody in bodies)
		{
			childBody.constraints = RigidbodyConstraints.None;
			childBody.velocity = deltaPosition / Time.fixedDeltaTime;
		}
		StopAllCoroutines();
		anim.enabled = false;
	}
	public void FlyWithBalloon()
	{
		body.velocity = Vector3.zero;
		body.constraints = RigidbodyConstraints.None;
		anim.enabled = true;
		body.isKinematic = false;
		body.useGravity = true;
		var cols = GetComponentsInChildren<Collider>();
		foreach(var col in cols)
		{
			col.enabled = false;
		}
		var bodies = GetComponentsInChildren<Rigidbody>();
		foreach(var childBody in bodies)
		{
			if(body != childBody) childBody.isKinematic = true;
		}
	}
	public void Kill(GameState.LoseCause cause = GameState.LoseCause.Crushed)
	{
		crushEffect.SetActive(true);
		anim.gameObject.SetActive(false);
		GameState.Lose(cause);
	}
	IEnumerator WaitForLanding()
	{
		while(true)
		{
			if(floating) yield return null;
			else
			{
				GetComponent<AudioList>().Play("step");
				anim.SetTrigger("Land");
				var overlap = OverlapTest(transform.localPosition.x, currentFloor);
				if(overlap != null)
					overlap.Overlap(this);
				pyramid.RefreshBlocks();
				yield break;
			}
		}
	}
	public void SetFloating(bool isFloating)
	{
		floating = isFloating;
	}
}
