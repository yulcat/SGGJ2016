using System;
using System.Collections;
using DG.Tweening;
using InControl;
using UnityEngine;

public class CharacterControl : PyramidComponent
{
    [NonSerialized]
    public Animator anim;

    public bool automatic;
    FlagBalloon balloon;
    public Animator[] characterAnimators;

    [Range(0f, 1f)]
    public float counterTorqueMultiplier = 1f;

    public GameObject crushEffect;
    int currentFloor;
    public float moveSpeed = 1f;
    Vector3 moveTarget;
    public float thickness = 0.25f;
    public float MoveSpeedFinal => moveSpeed * (GameState.selectedCharacter == CharacterType.Panda ? 1.3f : 1f);

    public override float torque => GetTorque(transform.localPosition, body.mass);

    public override void SetPyramid(Pyramid m)
    {
        base.SetPyramid(m);
        currentFloor = Mathf.RoundToInt(transform.localPosition.y * 2f);
        try
        {
            anim = characterAnimators[(int) GameState.selectedCharacter];
        }
        catch
        {
            return;
        }
        foreach (var a in characterAnimators)
            a.gameObject.SetActive(a == anim);
    }

    public override void RefreshPosition()
    {
        RefreshPositionSelf(transform.localPosition.x, currentFloor);
    }

    void RefreshPositionSelf(float x, int y)
    {
        if (y <= 1)
        {
            anim.SetTrigger("Fail");
            pyramid.RemoveBlock(this);
            FallOff();
        }
        else if (pyramid.HasBlocks(c => CheckFeet(x, y, c)) || OverlapTest(x, y) != null)
        {
            var feet = pyramid.GetBlock(c => CheckFeet(x, y, c));
            if (feet != null) (feet as IFeetDetect)?.OnStepOn();
            if (currentFloor == y) return;
            FallTo(currentFloor, y);
            currentFloor = y;
            pyramid.RefreshBlocks();
        }
        else
        {
            RefreshPositionSelf(x, y - 2);
        }
    }

    public bool BlockFallTest(Block target)
    {
        return CheckOverlap(transform.localPosition.x, currentFloor, target);
    }

    bool CheckFeet(float x, int y, PyramidComponent target)
    {
        var blockTarget = target as Block;
        if (blockTarget && blockTarget.CollideResult)
        {
            var check = blockTarget.position;
            return check.y == y - 2
                   && check.x + 1 >= (x - thickness) * 2f
                   && check.x - 1 <= (x + thickness) * 2f;
        }
        return false;
    }

    bool CheckOverlap(float x, int y, PyramidComponent target)
    {
        var block = target as Block;
        if (block == null) return false;
        var check = block.position;
        return check.y == y
               && check.x + 1 >= (x - thickness) * 2f
               && check.x - 1 <= (x + thickness) * 2f;
    }

    bool CheckCollideOverlap(float x, int y, PyramidComponent target)
    {
        var blockTarget = target as Block;
        if (blockTarget != null && blockTarget.CollideResult)
            return CheckOverlap(x, y, target);
        return false;
    }

    bool CheckFlag<T>(float x, int y, PyramidComponent target)
    {
        if (target is T) return CheckOverlap(x, y, target);
        return false;
    }

    IEnumerator Start()
    {
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var childBody in bodies)
            childBody.constraints = RigidbodyConstraints.FreezeAll;
        while (true)
        {
            yield return null;
            if (Pause.Paused) continue;
            if (pyramid == null) continue;
            if (floating) yield return StartCoroutine(WaitForLanding());
            var currentX = transform.localPosition.x;
            var direction = Input.GetAxis("Horizontal");
            var touchDirection = InputManager
                .ActiveDevice
                .GetControl(InputControlType.LeftStickX)
                .Value;
            if (Mathf.Abs(direction) < 0.3f) direction = touchDirection;
            if (automatic && Mathf.Abs(touchDirection) < 0.3f) direction = GetAutomaticDirection();
            if (Mathf.Abs(direction) < 0.3f)
            {
                anim.SetBool("IsTrace", false);
                continue;
            }
            var rotation = direction > 0 ? 120 : -120;
            anim.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            anim.SetBool("IsTrace", true);
            var dx = direction * MoveSpeedFinal * Time.deltaTime;
            var destination = currentX + dx;
            var overlap = OverlapTest(destination, currentFloor);
            overlap?.Overlap(this);
            if (!enabled) yield break;
            if (pyramid.HasBlocks(c => CheckCollideOverlap(destination, currentFloor, c)))
                continue; //Blocked by block

            transform.Translate(dx, 0, 0);
            pyramid.ApplyMomentum(GetMoveMomentum(direction * MoveSpeedFinal) * counterTorqueMultiplier);
            if (!pyramid.HasBlocks(c => CheckFeet(destination, currentFloor, c)))
            {
                RefreshPosition();
                anim.SetBool("IsTrace", false);
                //Jump off
            }
            else
            {
                var feet = pyramid.GetBlock(c => CheckFeet(destination, currentFloor, c));
                (feet as IFeetDetect)?.OnStepOn();
            }
        }
    }

    float GetAutomaticDirection()
    {
        if (balloon == null)
            balloon = FindObjectOfType<FlagBalloon>();
        if (pyramid.transform.parent == null)
        {
            var balloonPos = balloon.transform.position;
            var myPos = transform.position;
            if (Mathf.Abs(myPos.x - balloonPos.x) < 0.1f) return 0;
            return Mathf.Clamp(balloonPos.x - myPos.x, -1f, 1f);
        }
        else
        {
            var balloonPos = pyramid.transform.parent.InverseTransformPoint(balloon.transform.position);
            var myPos = pyramid.transform.parent.InverseTransformPoint(transform.position);
            if (Mathf.Abs(myPos.x - balloonPos.x) < 0.1f) return 0;
            return Mathf.Clamp(balloonPos.x - myPos.x, -1f, 1f);
        }
    }

    IOverlapLister OverlapTest(float x, int floor)
    {
        var overlap = pyramid.GetBlock(c => CheckFlag<IOverlapLister>(x, floor, c));
        if (overlap != null)
            return overlap as IOverlapLister;
        return null;
    }

    float GetMoveMomentum(float vx)
    {
        var mv = transform.right * vx * body.mass;
        var r = transform.localPosition;
        return -Vector3.Cross(r, mv).z;
    }

    public void TurnToCamera()
    {
        anim.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public override void FallOff(bool refresh = true)
    {
        transform.SetParent(null);
        FreeBodyRagdoll();
        GameState.Lose(GameState.LoseCause.CharacterLost);
    }

    void FreeBodyRagdoll()
    {
        transform.DOKill();
        withPhysics = true;
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var childBody in bodies)
        {
            childBody.constraints = RigidbodyConstraints.None;
            childBody.velocity = deltaPosition / Time.fixedDeltaTime;
        }
        StopAllCoroutines();
        anim.enabled = false;
    }

    public void FlyWithBalloon()
    {
        body.velocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.None;
        anim.enabled = true;
        body.isKinematic = false;
        body.useGravity = true;
        var cols = GetComponentsInChildren<Collider>();
        foreach (var col in cols)
            col.enabled = false;
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var childBody in bodies)
            if (body != childBody) childBody.isKinematic = true;
    }

    public void Kill(GameState.LoseCause cause = GameState.LoseCause.Crushed)
    {
        crushEffect.SetActive(true);
        anim.gameObject.SetActive(false);
        GameState.Lose(cause);
    }

    IEnumerator WaitForLanding()
    {
        while (true)
            if (floating)
            {
                yield return null;
            }
            else
            {
                GetComponent<AudioList>().Play("step");
                anim.SetTrigger("Land");
                var overlap = OverlapTest(transform.localPosition.x, currentFloor);
                overlap?.Overlap(this);
                pyramid.RefreshBlocks();
                yield break;
            }
    }

    public void SetFloating(bool isFloating)
    {
        floating = isFloating;
    }
}