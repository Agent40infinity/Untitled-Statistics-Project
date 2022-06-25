using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueLoading))]
public class DialogueInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueLoading dialogue = (DialogueLoading)target;

        if (GUILayout.Button("Create New Level File"))
        {
            dialogue.GenerateLevel();
        }
    }
}