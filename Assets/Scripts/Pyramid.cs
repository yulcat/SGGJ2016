using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Pyramid : MonoBehaviour
{
    List<PyramidComponent> blocks;
    CharacterControl character;
    float angularVelocity = 0f;
    public float angularDamp = 0.1f;
    public float torqueMultiplier = 5f;
    public float returnTorque = 10f;
    public float torqueSum;
	public bool calculate = false;
    // Use this for initialization
    void Start()
    {
        blocks = GetComponentsInChildren<PyramidComponent>().ToList();
        blocks.ForEach(b => b.SetPyramid(this));
        character = FindObjectOfType<CharacterControl>();
        character.SetPyramid(this);
    }

    public void RemoveBlock(PyramidComponent block)
    {
        blocks.Remove(block);
        RefreshBlocks();
    }
    public void RefreshBlocks()
    {
        blocks.ForEach(b => b.RefreshPosition());
    }

    public bool HasBlocks(Func<PyramidComponent, bool> func)
    {
        return blocks.Any(func);
    }

    void OnDrawGizmos()
    {
		if(calculate && !Application.isPlaying)
		{
			calculate = false;
			Start();
			FixedUpdate();
		}
    }

    void FixedUpdate()
    {
        torqueSum = -blocks.Sum(b => b.torque) * torqueMultiplier;
        var currentRot = transform.rotation.eulerAngles.z;
        var returning = -returnTorque * Mathf.Sin(currentRot * Mathf.Deg2Rad);
        angularVelocity += (returning + torqueSum) * Time.fixedDeltaTime;
        angularVelocity *= 1 - angularDamp;
        transform.rotation = Quaternion.Euler(0, 0, currentRot + angularVelocity);
        if (Mathf.Cos(currentRot * Mathf.Deg2Rad) < 0.9f)
        {
            transform.DetachChildren();
            blocks.ForEach(b => b.FallOff());
        }
    }
}
