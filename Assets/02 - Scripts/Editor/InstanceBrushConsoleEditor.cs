using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InstanceBrushConsole))]
[CanEditMultipleObjects]
public class InstanceBrushConsoleEditor : BrushEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InstanceBrushConsole instanceBrush = (InstanceBrushConsole)target;
        serializedObject.Update();

        bool useButtonClicked = button_active;
        if (useButtonClicked)
        {
            if (instanceBrush.type == InstanceBrushConsole.BrushType.Tree)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("random_mode"));
                if (instanceBrush.random_mode == true)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("tree_number"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("height_control"));
                if (instanceBrush.height_control == true)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("min_height"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("max_height"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("steepness_control"));
                if (instanceBrush.steepness_control == true)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("max_steepness"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("distance_control"));
                if (instanceBrush.distance_control == true)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("min_distance"));
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
