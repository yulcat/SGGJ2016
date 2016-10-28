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
	protected void FallTo(int originalY, int y)
	{
		floating = true;
		transform.DOLocalMoveY(y/2f, 0.5f)
			.SetEase(Ease.InCubic)
			.OnComplete(() => floating = false);
	}
	public void FallOff()
	{
		transform.DOKill();
		withPhysics = true;
		pyramid.RemoveBlock(this);
		body.constraints = RigidbodyConstraints.None;
		body.velocity = deltaPosition / Time.fixedDeltaTime;
		Invoke("DestroySelf",5f);
	}
	protected void DestroySelf()
	{
		Destroy(gameObject);
	}
	protected bool withPhysics = false;
	Vector3 prevPosition;
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
