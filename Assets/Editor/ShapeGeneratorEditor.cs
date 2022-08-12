using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeGenerator))]
public class ShapeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //GUILayout.

        ShapeGenerator shapeGenerator = (ShapeGenerator)target;
        if(GUILayout.Button("Generate Shape"))
        {
            shapeGenerator.GenerateShape();
        }
    }

    private void OnSceneGUI()
    {
        ShapeGenerator shapeGenerator = (ShapeGenerator)target;

        Vector3 size = shapeGenerator.getSize();
        Tuple<Vector2, Vector2, Vector2> intervals = shapeGenerator.getInterval();

        Vector3 position = shapeGenerator.transform.position;
        position.x += (intervals.Item1.x + intervals.Item1.y) / 2;
        position.y += (intervals.Item2.x + intervals.Item2.y) / 2;
        position.z += (intervals.Item3.x + intervals.Item3.y) / 2;

        Handles.DrawWireCube(position, size);
    }
}
