using UnityEngine;
using DG.Tweening;

public abstract class PyramidComponent : MonoBehaviour {
	protected Pyramid pyramid;
	protected bool floating;
	protected Rigidbody body;
	public virtual void SetPyramid(Pyramid m)
	{
		floating = false;
		pyramid = m;
		body = GetComponent<Rigidbody>();
	}
	public abstract void RefreshPosition();
	public abstract float torque
	{
		get;
	}
	protected float GetTorque(Vector2 pos, float mass)
	{
		return pyramid.transform.TransformVector(pos).x * mass;
		// return pos.x * mass;
	}
	protected void FallTo(int originalY, int y)
	{
		floating = true;
		transform.DOLocalMoveY(y/2f, 0.5f)
			.SetEase(Ease.InCubic)
			.OnComplete(() => floating = false);
	}
	public virtual void FallOff(bool refresh = true)
	{
		transform.DOKill();
		withPhysics = true;
		var col = GetComponent<Collider>();
		if(col is BoxCollider)
		{
			(col as BoxCollider).size *= 0.9f;
		}
		else if(col is SphereCollider)
		{
			(col as SphereCollider).radius *= 0.9f;
		}
		body.constraints = RigidbodyConstraints.None;
		body.velocity = deltaPosition / Time.fixedDeltaTime;
		Invoke("DestroySelf",5f);
	}
	protected void DestroySelf()
	{
		Destroy(gameObject);
	}
	protected bool withPhysics = false;
	protected Vector3 prevPosition;
	protected Vector3 deltaPosition;
	protected virtual void FixedUpdate()
	{
		deltaPosition = transform.position - prevPosition;
		prevPosition = transform.position;
		if(withPhysics)
		{
			if(transform.position.y > 0.5f) return;
			body.velocity *= 0.8f;
		}
	}
}
