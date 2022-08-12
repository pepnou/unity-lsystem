using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lsystem))]
public class LsystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Lsystem lsystem = (Lsystem)target;
        
        /*if (GUILayout.Button("Parse Rules"))
        {
            lsystem.ParseRules();
        }

        if (GUILayout.Button("Parse Axiom"))
        {
            lsystem.ParseAxiom();
        }*/

        if (GUILayout.Button("Generate"))
        {
            lsystem.Generate();
        }
    }
}
