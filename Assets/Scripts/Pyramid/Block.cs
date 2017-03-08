using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

[System.SerializableAttribute]
public struct XY
{
    public int x;
    public int y;
    public XY(int newX, int newY)
    {
        x = newX;
        y = newY;
    }
    public XY(Vector3 vec)
    {
        vec.y = Mathf.Floor(vec.y);
        vec = vec * 2f;
        vec.y++;
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
    }
    public Vector3 ToVector3()
    {
        var vec = new Vector3(x, y, 0);
        return vec / 2f;
    }
    public static bool operator ==(XY one, XY other)
    {
        return one.x == other.x && one.y == other.y;
    }
    public static bool operator !=(XY one, XY other)
    {
        return !(one == other);
    }
}
public class Block : PyramidComponent, ICollidable
{
    List<Block> Feet;
    public int ClickCount = 3;
    int currentClickCount = 0;
    public XY position;

    public override void SetPyramid(Pyramid m)
    {
        base.SetPyramid(m);
        var floatPos = transform.localPosition * 2f;
        position = new XY(Mathf.RoundToInt(floatPos.x), Mathf.RoundToInt(floatPos.y));
    }
    public override void RefreshPosition()
    {
        RefreshPositionSelf(position);
    }

    public override float torque
    {
        get
        {
            return GetTorque(position.ToVector3(), body.mass);
        }
    }

    public virtual bool CollideResult
    {
        get
        {
            return true;
        }
    }

    protected override void FallResult()
    {
        base.FallResult();
        var character = pyramid.GetBlock(c => c is CharacterControl) as CharacterControl;
        if (character.BlockFallTest(this))
        {
            character.Kill();
        }
    }

    void RefreshPositionSelf(XY targetPosition)
    {
        if (pyramid.HasBlocks(c => CheckFeet(targetPosition, c)))
        {
            if (position == targetPosition) return;
            FallTo(position.y, targetPosition.y);
            position = targetPosition;
            pyramid.RefreshBlocks();
        }
        else if (targetPosition.y == 1)
        {
            pyramid.RemoveBlock(this);
            FallOff();
        }
        else
        {
            RefreshPositionSelf(new XY(targetPosition.x, targetPosition.y - 2));
        }
    }

    bool CheckFeet(XY pos, PyramidComponent target)
    {
        if (pos.x == 0 && pos.y == 1)
        {
            return true;
        }
        if (target is CharacterControl)
        {
            return false;
        }
        if (target is Block)
        {
            var check = (target as Block).position;
            return (check.y == pos.y - 2)
                && (check.x >= pos.x - 1)
                && (check.x <= pos.x + 1);
        }
        return false;
    }
    public virtual void ClickListener()
    {
        if (pyramid == null || GameState.instance.isGameEnd) return;
        if (++currentClickCount == ClickCount)
        {
            Remove();
        }
        else
        {
            transform.DOLocalMoveZ((float)currentClickCount / ClickCount, 0.3f)
                .SetEase(Ease.OutQuint);
        }
        var audio = GetComponent<AudioList>();
        if (audio != null) audio.Play("Push");
    }
    public virtual void Remove()
    {
        pyramid.RemoveBlock(this);
        FallOff();
        body.velocity = transform.TransformVector(Vector3.forward * 12f);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Ground") return;
        var contact = col.contacts[0].point;
        var effect = EffectSpawner.GetEffect("Dust");
        effect.transform.position = contact;
        effect.SetActive(true);
    }
}
