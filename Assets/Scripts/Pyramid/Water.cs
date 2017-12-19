using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Water : Block, IOverlapLister
{
    CharacterControl overlapCharacter;

    public void Overlap(CharacterControl character)
    {
        overlapCharacter = character;
        character.anim.SetBool("InWater", true);
        character.SetFloating(true);
        character.transform.DOLocalMove(transform.localPosition, 0.5f);
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

    public override bool CollideResult
    {
        get { return false; }
    }

    public override void Remove()
    {
        base.Remove();
        var effect = EffectSpawner.GetEffect("Effects/FX_Water01");
        effect.transform.position = transform.position;
        effect.SetActive(true);
        if (overlapCharacter != null)
        {
            overlapCharacter.SetFloating(false);
            overlapCharacter.anim.SetBool("InWater", false);
        }
        Destroy(gameObject);
    }
}