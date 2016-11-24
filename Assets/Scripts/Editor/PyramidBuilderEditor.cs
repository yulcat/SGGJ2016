using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(PyramidBuilder))]
public class PyramidBuilderEditor : Editor {
    SerializedProperty toLoad;
    void OnEnable()
    {
        toLoad = serializedObject.FindProperty("stageToLoad");
    }

	public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        PyramidBuilder builder = target as PyramidBuilder;
        var loaded = Resources.LoadAll<TextAsset>("Stages");
        var intName = loaded.Select(asset => System.Convert.ToInt32(asset.name));
        if(intName.Count() == 0) return;
        EditorGUILayout.IntSlider (toLoad, intName.Min(), intName.Max(), new GUIContent ("Stage"));
        if (GUILayout.Button("Load Stage"))
        {
			builder.LoadStage();
        }
        serializedObject.ApplyModifiedProperties ();
    }
}
