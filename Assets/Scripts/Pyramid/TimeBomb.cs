using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TimeBomb : Block, IFeetDetect, IOverlapLister
{
    bool disarmed = false;
    public TextMesh textMesh;
    public int explodeTick;
    Sequence sequence;

    public override void SetPyramid(Pyramid m)
    {
        base.SetPyramid(m);
        StartBombTick();
    }

    public override void ClickListener()
    {
        if (disarmed) base.ClickListener();
    }

    public override void Remove()
    {
        pyramid.RemoveBlock(this);
        transform.DOKill();
        withPhysics = true;
        body.constraints = RigidbodyConstraints.None;
        body.velocity = transform.TransformVector(Vector3.forward * 12f);
        if (!disarmed)
            Explode();
    }

    void StartBombTick()
    {
        sequence = DOTween.Sequence();
        textMesh.color = Color.black;
        foreach (var countdown in Enumerable.Range(0, explodeTick).Reverse())
        {
            sequence
                .AppendCallback(() =>
                {
                    textMesh.text = countdown.ToString();
                    textMesh.color = Color.red;
                })
                .Append(DOTween
                    .To(() => textMesh.color, color => textMesh.color = color, Color.black, 1f)
                    .SetEase(Ease.InCirc));
        }
        sequence.AppendCallback(Remove);
        sequence.Play();
    }

    void Explode()
    {
        foreach (var block in pyramid.GetBlocks(b => CheckNearBlock(position, b)).ToArray())
        {
            ThrowAway(block);
        }
        var effect = EffectSpawner.GetEffect("Effects/Explosion");
        effect.transform.position = transform.position;
        effect.SetActive(true);
        Destroy(gameObject);
    }

    void DisarmBomb()
    {
        if (disarmed) return;
        disarmed = true;
        StopAllCoroutines();
        sequence.Kill();
        textMesh.text = "--";
        textMesh.color = Color.green;
    }

    void ThrowAway(PyramidComponent target)
    {
        var characterTarget = target as CharacterControl;
        if (characterTarget != null)
        {
            characterTarget.Kill(GameState.LoseCause.Boomed);
        }
        else if (target is Block)
        {
            var blockTarget = (Block) target;
            blockTarget.Remove();
        }
    }

    bool CheckNearBlock(XY pos, PyramidComponent target)
    {
        XY check = new XY();
        var block = target as Block;
        if (block != null)
        {
            check = block.position;
        }
        else if (target is CharacterControl)
        {
            check = new XY(target.transform.localPosition);
        }
        var upOrDown = (Mathf.Abs(check.y - pos.y) == 2)
                       && (check.x >= pos.x - 2)
                       && (check.x <= pos.x + 2);
        var leftOrRight = (check.y == pos.y)
                          && (check.x >= pos.x - 3 && check.x < pos.x || check.x <= pos.x + 3 && check.x > pos.x);
        return upOrDown || leftOrRight;
    }

    public void OnStepOn() => DisarmBomb();
    public void Overlap(CharacterControl character) => DisarmBomb();
}