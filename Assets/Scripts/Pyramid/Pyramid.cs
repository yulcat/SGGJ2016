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
    public float inertia;
    public bool calculate = false;
    private Plane inputPlane;
    private Vector3 recentClick;
    private bool initializedByBuilder = false;
    // Use this for initialization
    void Start()
    {
        if(!initializedByBuilder)
        {
            blocks = GetComponentsInChildren<PyramidComponent>().ToList();
            blocks.ForEach(b => b.SetPyramid(this));
        }
        inputPlane = new Plane(Vector3.forward, transform.position + (Vector3.back * 0.5f));
    }
    public void EnlistBlocks(IEnumerable<PyramidComponent> newBlocks)
    {
        // blocks.ForEach(b => Destroy(b.gameObject));
        blocks = new List<PyramidComponent>();
        blocks.AddRange(newBlocks);
        blocks.ForEach(b => b.SetPyramid(this));
        initializedByBuilder = true;
    }
    void Update()
    {
        foreach(var t in Input.touches)
        {
            if(t.phase != TouchPhase.Began) continue;
            RayCheck(t.position);
        }
        if (!Input.touchSupported && Input.GetMouseButtonDown(0))
        {
            RayCheck(Input.mousePosition);
        }
    }
    void RayCheck(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        float rayDistance;
        if (inputPlane.Raycast(ray, out rayDistance))
        {
            recentClick = ray.GetPoint(rayDistance);
            PushBlock(transform.InverseTransformPoint(recentClick));
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
            inertia = blocks.Aggregate(0f,GetInertiaSum) * 0.01f;
        }
    }

    void FixedUpdate()
    {
        if(blocks.Count == 0) return;
        inertia = blocks.Aggregate(0f,GetInertiaSum) * 0.01f;
        torqueSum = -blocks.Sum(b => b.torque) * torqueMultiplier;
        var currentRot = transform.rotation.eulerAngles.z;
        var returning = -returnTorque * Mathf.Sin(currentRot * Mathf.Deg2Rad);
        ApplyMomentum(torqueSum + returning);
        ApplyAngularVelocity();
        angularVelocity *= 1 - angularDamp;
    }
    float GetInertiaSum(float inertia, PyramidComponent comp)
    {
        var mass = comp.GetComponent<Rigidbody>().mass;
        var r = comp.transform.localPosition.magnitude;
        var multiplier = comp is Balloon? 0 : 1;
        return (mass * r * r * multiplier) + inertia;
    }
    public void ApplyMomentum(float momentum)
    {
        angularVelocity += momentum * Time.deltaTime / inertia;
    }
    void ApplyAngularVelocity()
    {
        var currentRot = transform.rotation.eulerAngles.z;
        var dc = angularVelocity * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, 0, currentRot + dc);
        if (Mathf.Cos((currentRot + dc) * Mathf.Deg2Rad) < 0.9f)
        {
            GameState.Lose(GameState.LoseCause.Collapsed);
            CollapseAll();
        }
    }
}
