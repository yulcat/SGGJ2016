using UnityEngine;
using DG.Tweening;

public class Balloon : Block {
    public float buoyancy;
    public override float torque
	{
		get
		{
			return -position.x * 0.5f * body.mass;
		}
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
    protected override void OnMouseDown()
    {
		pyramid.RemoveBlock(this);
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = Vector3.forward * 8f;
		var col = GetComponent<Collider>();
		Invoke("DestroySelf",5f);
    }
}
