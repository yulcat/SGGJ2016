using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(PyramidBuilder))]
public class PyramidBuilderEditor : Editor
{
    SerializedProperty toLoad;
    SerializedProperty currentTheme;

    void OnEnable()
    {
        toLoad = serializedObject.FindProperty("stageToLoad");
        currentTheme = serializedObject.FindProperty("currentTheme");
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        PyramidBuilder builder = target as PyramidBuilder;
        EditorGUILayout.PropertyField(currentTheme);
        var loaded = Resources.LoadAll<TextAsset>("Stages");
        var intName = loaded.Select(asset => System.Convert.ToInt32(asset.name));
        if (intName.Count() == 0) return;
        EditorGUILayout.IntSlider(toLoad, intName.Min(), intName.Max(), new GUIContent("Stage"));
        if (GUILayout.Button("Load Stage"))
        {
            builder.LoadStage();
        }
        if (GUILayout.Button("Clear Stage"))
        {
            builder.ClearStage();
        }
        serializedObject.ApplyModifiedProperties();
    }
}