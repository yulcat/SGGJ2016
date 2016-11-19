using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Pyramid : MonoBehaviour
{
    List<PyramidComponent> blocks;
    float angularVelocity = 0f;
    public float angularDamp = 0.1f;
    public float torqueMultiplier = 5f;
    public float returnTorque = 10f;
    public float torqueSum;
    public bool calculate = false;
    private Plane inputPlane;
    private Vector3 recentClick;
    // Use this for initialization
    void Start()
    {
        blocks = GetComponentsInChildren<PyramidComponent>().ToList();
        blocks.ForEach(b => b.SetPyramid(this));
        inputPlane = new Plane(Vector3.forward, transform.position + (Vector3.back * 0.5f));
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (inputPlane.Raycast(ray, out rayDistance))
            {
                recentClick = ray.GetPoint(rayDistance);
                PushBlock(transform.InverseTransformPoint(recentClick));
            }
        }
    }
    void PushBlock(Vector3 clickPosition)
    {
        var block = GetBlock(b => IsClicked(b, clickPosition)) as Block;
        if (block != null) block.ClickListener();
    }
    bool IsClicked(PyramidComponent obj, Vector3 clickPosition)
    {
        var block = obj as Block;
        if (block == null) return false;
        var y = Mathf.FloorToInt(clickPosition.y) * 2 + 1;
        return block.position.y == y
            && block.position.x + 1 >= clickPosition.x * 2
            && block.position.x - 1 <= clickPosition.x * 2;
    }

    public void RemoveBlock(PyramidComponent block, bool refresh = true)
    {
        blocks.Remove(block);
        if (refresh)
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
    public PyramidComponent GetBlock(Func<PyramidComponent, bool> func)
    {
        return blocks.FirstOrDefault(func);
    }
    public void CollapseAll()
    {
        blocks.ForEach(b => b.FallOff(false));
        transform.DetachChildren();
        blocks.Clear();
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.green;
        // Gizmos.DrawSphere(recentClick,0.2f);
        if (calculate && !Application.isPlaying)
        {
            calculate = false;
            Start();
            torqueSum = -blocks.Sum(b => b.torque) * torqueMultiplier;
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
            GameState.Lose(GameState.LoseCause.Collapsed);
            CollapseAll();
        }
    }
}
