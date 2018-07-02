using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Snow : Block, IOverlapLister
{
    public void Overlap(CharacterControl character)
    {
        var effect = EffectSpawner.GetEffect("Effects/SnowSplash");
        effect.transform.position = transform.position;
        effect.SetActive(true);
        pyramid.RemoveBlock(this);
        Destroy(gameObject);
    }

    public override void Remove()
    {
        pyramid.RemoveBlock(this);
        transform.DOKill();
        withPhysics = true;
        body.constraints = RigidbodyConstraints.None;
        body.velocity = transform.TransformVector(Vector3.forward * 12f);
        var effect = EffectSpawner.GetEffect("Effects/SnowSplash");
        effect.transform.position = transform.position;
        effect.SetActive(true);
        Destroy(gameObject);
    }

    protected override void FallResult()
    {
        floating = false;
        var character = pyramid.GetBlock(c => c is CharacterControl) as CharacterControl;
        if (character.BlockFallTest(this))
        {
            Overlap(character);
        }
    }

    public override bool CollideResult => false;
}