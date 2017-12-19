using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ScreenShot))]
public class ScreenShotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Take a shot"))
        {
            foreach (var t in targets)
            {
                (t as ScreenShot).TakeAShot();
            }
        }
    }
}