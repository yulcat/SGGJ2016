﻿using UnityEngine;
using DG.Tweening;

public class FlagBalloon : Balloon {
	BalloonLine line;
	void Awake()
	{
		line = GetComponentInChildren<BalloonLine>(true);
	}
	public void Launch(CharacterControl character)
	{
		GameState.Win();
		var joint = gameObject.AddComponent<SpringJoint>();
		var charBody = character.GetComponent<Rigidbody>();
		joint.anchor = Vector3.up * 0.5f;
		joint.autoConfigureConnectedAnchor = false;
		joint.connectedBody = charBody;
		joint.connectedAnchor = Vector3.up * 0.5f;
		joint.damper = 0.8f;
		character.TurnToCamera();
		line.target = character.GetComponentInChildren<BearHandMarker>().transform;
		line.gameObject.SetActive(true);
		transform.SetParent(null);
		pyramid.RemoveBlock(this, false);
		pyramid.CollapseAll();
		charBody.velocity = Vector3.zero;
		GetComponent<Collider>().enabled = false;
		character.GetComponent<Collider>().enabled = false;
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = Vector3.forward * -4f;
	}
	public override void FallOff(bool refresh = true)
	{
		base.FallOff(refresh);
		GameState.Lose(GameState.LoseCause.BalloonLost);
	}
	protected override void OnMouseDown()
    {
    }
}