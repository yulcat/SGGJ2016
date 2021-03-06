﻿using System.Linq;
using UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextLocalizer))]
public class TextLocalizerEditor : Editor
{
    string[] strings;
    string prevKey;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var localizer = (TextLocalizer) target;
        if (!string.IsNullOrEmpty(localizer.textKey) && MessageDB.HasKey(localizer.textKey))
        {
            var text = DB.MessageDB[localizer.textKey];
            GUILayout.Label(text);
        }
        else
        {
            var labelStyle = EditorStyles.label;
            labelStyle.richText = true;
            GUILayout.Label("<color=red>Key is Not Found</color>", labelStyle);
            if (string.IsNullOrEmpty(localizer.textKey)) return;
            if (prevKey != localizer.textKey)
            {
                var candidates = MessageDB.korMessageDB.Where(p => p.Key.Contains(localizer.textKey))
                    .Select(p => p.Key);
                strings = new[] {"--"}.Concat(candidates).ToArray();
                prevKey = localizer.textKey;
            }
            var selected = EditorGUILayout.Popup(0, strings);
            if (selected != 0) localizer.textKey = strings[selected];
        }
    }
}