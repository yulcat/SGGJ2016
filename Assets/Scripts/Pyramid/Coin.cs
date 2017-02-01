using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Block
{
	public float rotateSpeed = 1f;
	Transform child;
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(child == null)
		{
			child = transform.GetChild(0);
		}
		if(withPhysics) return;
		float yRotation = child.localRotation.eulerAngles.y;
		yRotation += rotateSpeed * Time.deltaTime;
		child.localRotation = Quaternion.Euler(90,yRotation,0);
	}
	public void Activate()
	{
		GameState.Accomplished("Coin",1);
		pyramid.RemoveBlock(this);
		Destroy(gameObject);
	}
    public override bool CollideResult
    {
        get
        {
            return false;
        }
    }
	public override void ClickListener() {}
}
