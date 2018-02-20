using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    List<PyramidComponent> blocks;
    float angularVelocity;
    public float angularDamp = 0.1f;
    public float torqueMultiplier = 5f;
    public float returnTorque = 10f;
    public float torqueSum;
    public float inertia;
    public bool calculate;
    [NonSerialized] public float maxRotation;
    [NonSerialized] public int maxY;
    [NonSerialized] public int coinCount;
    Plane inputPlane;
    Vector3 recentClick;

    bool initializedByBuilder;

    // Use this for initialization
    void Start()
    {
        if (!initializedByBuilder)
        {
            blocks = GetComponentsInChildren<PyramidComponent>().ToList();
            blocks.ForEach(b => b.SetPyramid(this));
        }
        inputPlane = new Plane(transform.forward, transform.position + (transform.forward * -0.5f));
        maxY = GetMaxY();
        coinCount = GetCoinCount();
        StartCoroutine(FixedUpdateCoroutine());
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
        if (Pause.Paused) return;

        foreach (var t in Input.touches)
        {
            if (t.phase != TouchPhase.Began) continue;
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
        if (block != null)
        {
            var tabEffect = EffectSpawner.GetEffect("Effects/tabEffect");
            tabEffect.transform.SetParent(transform);
            var targetPosition = block.transform.localPosition;
            targetPosition.z = -0.55f;
            tabEffect.transform.localPosition = targetPosition;
            tabEffect.transform.localRotation = Quaternion.identity;
            tabEffect.gameObject.SetActive(true);
            block.ClickListener();
        }
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
        if (block is Block && !(block is Coin))
        {
            GameState.Accomplished(block.BlockType.ToString(), 1);
        }
        blocks.Remove(block);
        if (refresh)
            RefreshBlocks();
    }

    public void RefreshBlocks()
    {
        blocks.Do(b => b.RefreshPosition());
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
            inertia = blocks.Aggregate(0f, GetInertiaSum) * 0.01f;
        }
    }

    IEnumerator FixedUpdateCoroutine()
    {
        while (true)
        {
            if (blocks.Count == 0) yield break;
            yield return new WaitForFixedUpdate();
            inertia = blocks.Aggregate(0f, GetInertiaSum) * 0.01f;
            torqueSum = -blocks.Sum(b => b.torque) * torqueMultiplier;
            var currentRot = transform.localRotation.eulerAngles.z;
            var returning = -returnTorque * Mathf.Sin(currentRot * Mathf.Deg2Rad);
            ApplyMomentum(torqueSum + returning);
            var stuck = ApplyAngularVelocity();
            if (stuck)
            {
                Time.timeScale = 0.5f;
                var first = blocks.OfType<CharacterControl>().FirstOrDefault();
                if (first != null) GrayscaleEffect.SetGrayscale(first.transform.position);
                yield return new WaitForSeconds(DB.characterDB[(int) CharacterType.Gummy].special);
                Time.timeScale = 1f;
                GrayscaleEffect.UnsetGraysclae();
            }
            angularVelocity *= 1 - angularDamp;
        }
    }

    float GetInertiaSum(float inert, PyramidComponent comp)
    {
        var mass = comp.GetComponent<Rigidbody>().mass;
        var r = comp.transform.localPosition.magnitude;
        var multiplier = comp is Balloon ? 0 : 1;
        return (mass * r * r * multiplier) + inert;
    }

    public void ApplyMomentum(float momentum)
    {
        angularVelocity += momentum * Time.deltaTime / inertia;
    }

    bool everStucked;

    bool ApplyAngularVelocity()
    {
        var currentRot = transform.localRotation.eulerAngles.z;
        var dc = angularVelocity * Time.fixedDeltaTime * GameState.PyramidRotateSpeed;
        var nextRot = float.IsNaN(dc) ? currentRot : currentRot + dc;
        transform.localRotation = Quaternion.Euler(0, 0, nextRot);
        maxRotation = Mathf.Max(maxRotation, Mathf.Abs(Mathf.DeltaAngle(0, nextRot)));
        if (!(Mathf.Cos(nextRot * Mathf.Deg2Rad) < 0.9f)) return false;
        if (GameState.selectedCharacter == CharacterType.Gummy && !everStucked)
        {
            everStucked = true;
            return true;
        }
        GameState.Lose(GameState.LoseCause.Collapsed);
        CollapseAll();
        return false;
    }

    int GetMaxY()
    {
        return blocks
            .Where(block => block.BlockType != BlockType.Character)
            .Max(block => (new XY(block.transform.localPosition).y + 1) / 2);
    }

    int GetCoinCount()
    {
        return blocks.Count(block => block.BlockType == BlockType.Coin);
    }
}