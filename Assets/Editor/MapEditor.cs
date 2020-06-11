using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGenerator mapGenerator = (MapGenerator)target;

        if(GUILayout.Button(""))
        {
            mapGenerator.Output();
        }
    }

}
