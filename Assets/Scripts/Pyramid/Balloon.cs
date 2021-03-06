﻿using UnityEngine;
using DG.Tweening;

public class Balloon : Block
{
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
        v += transform.TransformVector(Vector3.forward * -3);
        body.velocity = v;
    }

    public override void Remove()
    {
        pyramid.RemoveBlock(this);
        FallOff();
        body.velocity = transform.TransformVector(Vector3.forward * 8f);
    }

    protected override void FixedUpdate()
    {
        deltaPosition = transform.position - prevPosition;
        prevPosition = transform.position;
        if (withPhysics)
        {
            body.AddForceAtPosition(
                Vector3.up * buoyancy,
                transform.TransformPoint(Vector3.up * -0.5f));
            if (transform.position.y > 0.5f) return;
            body.velocity *= 0.8f;
        }
    }

    public override void ClickListener()
    {
        if (GameState.Instance.isGameEnd) return;
        Remove();
    }
}