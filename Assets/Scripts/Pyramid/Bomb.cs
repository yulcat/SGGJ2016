using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class Bomb : Block
{
    public override void ClickListener()
    {
        if (pyramid == null || GameState.instance.isGameEnd) return;
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

    private void ThrowAway(PyramidComponent target)
    {
        if (target is CharacterControl)
        {
            var characterTarget = target as CharacterControl;
            characterTarget.Kill(GameState.LoseCause.Boomed);
            return;
        }
        else if (target is Block)
        {
            var blockTarget = target as Block;
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
        else if (direction == -1) return (check.x <= pos.x + 3 && check.x > pos.x);
        else throw new System.Exception("Finding wierd direction");
    }
}
