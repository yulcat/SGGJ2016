﻿using UnityEngine;

interface IOverlapLister
{
    void Overlap(CharacterControl character);
}

public class Coin : Block, IOverlapLister
{
    public float rotateSpeed = 1f;
    Transform child;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (child == null)
        {
            child = transform.GetChild(0);
        }
        if (withPhysics) return;
        float yRotation = child.localRotation.eulerAngles.y;
        yRotation += rotateSpeed * Time.deltaTime;
        child.localRotation = Quaternion.Euler(90, yRotation, 0);
    }

    public void Overlap(CharacterControl character)
    {
        GameState.Accomplished("Coin", 1, transform.position);
        pyramid.RemoveBlock(this);
        Destroy(gameObject);
    }

    protected override void FallResult()
    {
        floating = false;
        var character = pyramid.GetBlock(c => c is CharacterControl) as CharacterControl;
        if (character != null && character.BlockFallTest(this))
        {
            Overlap(character);
        }
    }

    public override bool CollideResult => false;

    public override void ClickListener()
    {
    }
}