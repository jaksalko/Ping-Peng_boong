using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandGenerator))]
public class IslandEditor : Editor
{
    string myString = "";

    public override void OnInspectorGUI()
    {
        IslandGenerator generator = (IslandGenerator)target;

        base.OnInspectorGUI();

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);


        if (GUILayout.Button("Edit"))
        {
            generator.GenerateMap(myString);
        }
    }
}
