using System.IO;
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
//                (t as ScreenShot).TakeAShot();
                TakeShot(t as ScreenShot);
            }
        }
    }

    void TakeShot(ScreenShot screenShot)
    {
        var cam = screenShot.GetComponent<Camera>();
        var temp = RenderTexture.GetTemporary(2048, 1024, 24);
        cam.targetTexture = temp;
        cam.Render();
        RenderTexture.active = temp;
        var texture = new Texture2D(2048, 1024, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, 2048, 1024), 0, 0);
        RenderTexture.active = null;
        cam.targetTexture = null;
        var path = screenShot.transform.parent.gameObject.name + screenShot.transform.GetSiblingIndex() + ".PNG";
        File.WriteAllBytes(Application.dataPath + "/Resources/Screenshots/" + path, texture.EncodeToPNG());
    }
}