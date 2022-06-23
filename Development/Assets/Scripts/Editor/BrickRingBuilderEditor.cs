using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BrickRingBuilder))]
public class BrickRingBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var brickRingBuilder = (BrickRingBuilder)target;

        if (GUILayout.Button("Build"))
        {
            brickRingBuilder.Build();
        }

        if (GUILayout.Button("Undo"))
        {
            brickRingBuilder.Undo();
        }
    }
}