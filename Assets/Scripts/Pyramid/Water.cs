using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Block, IOverlapLister {
    public void Overlap(CharacterControl character)
	{
		character.Kill();
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
