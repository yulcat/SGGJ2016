using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Block
{
	public float rotateSpeed = 1f;
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(withPhysics) return;
		float yRotation = transform.localRotation.eulerAngles.y;
		yRotation += rotateSpeed * Time.deltaTime;
		transform.localRotation = Quaternion.Euler(0,yRotation,0);
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
