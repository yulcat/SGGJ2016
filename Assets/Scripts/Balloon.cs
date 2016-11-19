using UnityEngine;
using DG.Tweening;

public class Balloon : Block {
    public float buoyancy;
    public override float torque
	{
		get
		{
			return -GetTorque(position.ToVector3(), body.mass);
			// return -position.x * 0.5f * body.mass;
		}
	}
	public override void FallOff(bool refresh = true)
	{
		base.FallOff(refresh);
		var v = body.velocity;
		v.z = -3;
		body.velocity = v;
	}
    protected override void FixedUpdate()
	{
		deltaPosition = transform.position - prevPosition;
		prevPosition = transform.position;
		if(withPhysics)
		{
            body.AddForceAtPosition(
                Vector3.up * buoyancy * Time.fixedDeltaTime, 
                transform.TransformPoint(Vector3.up * -0.5f));
			if(transform.position.y > 0.5f) return;
			body.velocity *= 0.8f;
		}
	}
    public override void ClickListener()
    {
		if(GameState.instance.isGameEnd) return;
		pyramid.RemoveBlock(this);
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = Vector3.forward * 8f;
		var col = GetComponent<Collider>();
    }
}
