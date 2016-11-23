using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PyramidBuilder))]
public class PyramidBuilderEditor : Editor {

	public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PyramidBuilder builder = target as PyramidBuilder;

        if (GUILayout.Button("Load Stage"))
        {
			builder.LoadStage();
        }
    }
}
