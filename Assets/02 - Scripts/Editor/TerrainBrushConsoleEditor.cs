using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainBrushConsole))]
[CanEditMultipleObjects]
public class TerrainBrushConsoleEditor : BrushEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainBrushConsole terrainBrush = (TerrainBrushConsole)target;
        serializedObject.Update();

        if (terrainBrush.type == TerrainBrushConsole.BrushType.Gaussian)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("standard_deviation"));
        }
        else if (terrainBrush.type == TerrainBrushConsole.BrushType.Smooth)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("smoothing_factor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("kernel_size"));
        }
        else if (terrainBrush.type == TerrainBrushConsole.BrushType.Noise)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("frequency"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
