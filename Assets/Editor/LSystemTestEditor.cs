using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LSystemTest))]
public class LSystemTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LSystemTest lsystemTest = (LSystemTest)target;
        /*if (GUILayout.Button("Parse CSLY"))
        {
            lsystemTest.Parse();
        }*/
        if (GUILayout.Button("Parse"))
        {
            lsystemTest.Parse();
        }
        if (GUILayout.Button("Iterate"))
        {
            lsystemTest.Iterate();
        }
        if (GUILayout.Button("Draw"))
        {
            lsystemTest.Draw();
        }
        if (GUILayout.Button("Clear"))
        {
            lsystemTest.Clear();
        }
    }
}
