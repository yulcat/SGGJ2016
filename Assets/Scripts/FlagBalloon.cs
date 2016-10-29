﻿using UnityEngine;
using DG.Tweening;

public class FlagBalloon : Balloon {
	public void Launch(CharacterControl character)
	{
		var joint = gameObject.AddComponent<SpringJoint>();
		joint.anchor = Vector3.down * 0.5f;
		joint.autoConfigureConnectedAnchor = false;
		joint.connectedBody = character.GetComponent<Rigidbody>();
		joint.connectedAnchor = Vector3.up * 0.5f;
		joint.damper = 0.5f;
		transform.SetParent(null);
		pyramid.RemoveBlock(this, false);
		pyramid.CollapseAll();
		GetComponent<Collider>().enabled = false;
		character.GetComponent<Collider>().enabled = false;
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = Vector3.forward * -5f;
		Invoke("DestroySelf",5f);
		character.Invoke("DestroySelf",5f);
	}
	public override void FallOff(bool refresh = true)
	{
		base.FallOff(refresh);
	}
	protected override void OnMouseDown()
    {
    }
}
