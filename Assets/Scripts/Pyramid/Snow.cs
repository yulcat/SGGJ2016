using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : Block, IOverlapLister
{
    public void Overlap(CharacterControl character)
    {
		GameState.Accomplished("Snow",1);
        pyramid.RemoveBlock(this);
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
	public override bool CollideResult
    {
        get
        {
            return false;
        }
    }
}
