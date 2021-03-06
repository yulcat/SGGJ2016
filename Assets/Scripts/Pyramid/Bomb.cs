﻿using System;
using DG.Tweening;
using UnityEngine;

public class Bomb : Block
{
    public override void ClickListener()
    {
        if (pyramid == null || GameState.Instance.isGameEnd) return;
        Remove();
    }

    public override void Remove()
    {
        pyramid.RemoveBlock(this);
        transform.DOKill();
        withPhysics = true;
        body.constraints = RigidbodyConstraints.None;
        body.velocity = transform.TransformVector(Vector3.forward * 12f);
        Explode();
    }

    void Explode()
    {
        var onLeft = pyramid.GetBlock(b => CheckSide(position, b, -1));
        var onRight = pyramid.GetBlock(b => CheckSide(position, b, 1));
        ThrowAway(onLeft);
        ThrowAway(onRight);
        var effect = EffectSpawner.GetEffect("Effects/Explosion");
        effect.transform.position = transform.position;
        effect.SetActive(true);
        Destroy(gameObject);
    }

    void ThrowAway(PyramidComponent target)
    {
        if (target is CharacterControl)
        {
            var characterTarget = target as CharacterControl;
            characterTarget.Kill(GameState.LoseCause.Boomed);
        }
        else if (target is Block)
        {
            var blockTarget = (Block) target;
            blockTarget.Remove();
        }
    }

    bool CheckSide(XY pos, PyramidComponent target, int direction)
    {
        XY check = new XY();
        if (target is Block)
        {
            check = (target as Block).position;
        }
        else if (target is CharacterControl)
        {
            check = new XY(target.transform.localPosition);
        }
        if (check.y != pos.y) return false;
        if (direction == 1) return (check.x >= pos.x - 3 && check.x < pos.x);
        if (direction == -1) return (check.x <= pos.x + 3 && check.x > pos.x);
        throw new Exception("Finding wierd direction");
    }
}