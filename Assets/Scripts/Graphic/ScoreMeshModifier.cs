using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMeshModifier : BaseMeshEffect
{
    [Serializable]
    public struct MeshModifySetting
    {
        public float offset;
        public Color outlineColor;
        public Gradient gradient;
    }
    public MeshModifySetting[] modifySettings;
    public int selected;
    [Range(0f, 1f)] public float transparency = 1f;

    MeshModifySetting SelectedSetting => modifySettings[selected];

    IEnumerable<Vector3> OffsetVectors()
    {
        yield return new Vector3(SelectedSetting.offset, 0, 0);
        yield return new Vector3(-SelectedSetting.offset, 0, 0);
        yield return new Vector3(0, SelectedSetting.offset, 0);
        yield return new Vector3(0, -SelectedSetting.offset, 0);
        yield return new Vector3(SelectedSetting.offset, SelectedSetting.offset, 0);
        yield return new Vector3(-SelectedSetting.offset, -SelectedSetting.offset, 0);
        yield return new Vector3(SelectedSetting.offset, -SelectedSetting.offset, 0);
        yield return new Vector3(-SelectedSetting.offset, SelectedSetting.offset, 0);
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        var originalVerts = new List<UIVertex>();
        vh.GetUIVertexStream(originalVerts);
        var vertsRect = originalVerts.Select(v => v.position)
            .Aggregate(new Rect(originalVerts[0].position, Vector2.zero), ReduceVert);
        var offsetApplied = OffsetVectors().Select(v => originalVerts.Select(ApplyOffset(v)));
        var verts = new List<UIVertex>();
        foreach (var modified in offsetApplied)
        {
            verts.AddRange(modified);
        }
        verts.AddRange(originalVerts.Select(ApplyGradient(vertsRect)));
        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }

    Func<UIVertex, UIVertex> ApplyGradient(Rect vertsRect)
        => vert =>
        {
            var key = Rect.PointToNormalized(vertsRect, vert.position).y;
            var color = SelectedSetting.gradient.Evaluate(key);
            color.a = transparency;
            vert.color = color;
            return vert;
        };

    Func<UIVertex, UIVertex> ApplyOffset(Vector3 offset)
        => vert =>
        {
            vert.position += offset;
            var color = SelectedSetting.outlineColor;
            color.a = transparency;
            vert.color = color;
            return vert;
        };

    Rect ReduceVert(Rect rect, Vector3 position)
    {
        rect.xMax = Mathf.Max(rect.xMax, position.x);
        rect.xMin = Mathf.Min(rect.xMin, position.x);
        rect.yMax = Mathf.Max(rect.yMax, position.y);
        rect.yMin = Mathf.Min(rect.yMin, position.y);
        return rect;
    }
}