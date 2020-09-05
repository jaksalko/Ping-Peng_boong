using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandGenerator))]
public class IslandEditor : Editor
{
    string myString = "";
    string title = "";
    int island_num = 0;
    public override void OnInspectorGUI()
    {
        IslandGenerator generator = (IslandGenerator)target;

        base.OnInspectorGUI();

        GUILayout.Label("Input Data", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        GUILayout.Label("Title", EditorStyles.boldLabel);
        title = EditorGUILayout.TextField("Text Field", title);

        GUILayout.Label("Input Data", EditorStyles.boldLabel);
        island_num = EditorGUILayout.IntField("Island Number", island_num);

        if (GUILayout.Button("Edit"))
        {
            generator.GenerateMap(myString ,title, island_num);
        }
    }
}
