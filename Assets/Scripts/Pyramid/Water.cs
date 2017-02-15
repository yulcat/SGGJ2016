using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Block, IOverlapLister {
    public void Overlap(CharacterControl character)
	{
		character.Kill();
	}
    public override bool CollideResult
    {
        get
        {
            return false;
        }
    }
}
