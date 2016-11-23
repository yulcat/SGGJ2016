using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StageMaker))]
public class StageMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StageMaker maker = target as StageMaker;

        if (GUILayout.Button("Make New Stage"))
        {
			maker.MakeStage();
        }
        if (GUILayout.Button("Save Current Stage"))
        {
			maker.SaveStage();
        }
    }
}
