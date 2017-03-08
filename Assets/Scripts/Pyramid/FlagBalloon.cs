using UnityEngine;
using DG.Tweening;
using System;

public class FlagBalloon : Balloon, IOverlapLister {
	BalloonLine line;
	void Awake()
	{
		line = GetComponentInChildren<BalloonLine>(true);
	}
	public override bool CollideResult
    {
        get
        {
            return false;
        }
    }
	public override void FallOff(bool refresh = true)
	{
		base.FallOff(refresh);
		GameState.Lose(GameState.LoseCause.BalloonLost);
	}
	public override void ClickListener()
    {
    }

    public void Overlap(CharacterControl character)
    {
		character.anim.SetBool("GetBalloon",true);
		character.enabled = false;
        GameState.EndGame();
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
		GetComponent<Collider>().enabled = false;
		character.FlyWithBalloon();
		transform.DOKill();
		withPhysics = true;
		body.constraints = RigidbodyConstraints.None;
		body.velocity = transform.TransformVector(Vector3.forward * -4f);
    }
}
