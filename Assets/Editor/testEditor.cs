using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(test))]
public class testEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        test t = (test)target;

        if (GUILayout.Button("rotate global(left)"))
        {
            t.RotateFromLeft();
        }

        if (GUILayout.Button("rotate local(right)"))
        {
            t.RotateFromRight();
        }
    }
}
