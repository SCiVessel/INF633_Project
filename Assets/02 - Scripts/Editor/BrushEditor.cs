using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Brush), true)]
public class BrushEditor : Editor {

	private static GUIStyle ToggleButtonStyleNormal = null;
	private static GUIStyle ToggleButtonStyleToggled = null;

	public static bool button_active = false;

	public override void OnInspectorGUI() {
		Brush myBrush = (Brush) target;
		if (myBrush.isActive())
			DrawDefaultInspector();

		if (ToggleButtonStyleNormal == null) {
			ToggleButtonStyleNormal = "Button";
			ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
			ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
		}

		GUIStyle style = myBrush.isActive() ? ToggleButtonStyleToggled : ToggleButtonStyleNormal;
		if (GUILayout.Button("Use", style)) {
			myBrush.toggle();
		}

		if (myBrush.isActive())
		{
			button_active = true;
		}
		else
		{
			button_active = false;
		}
	}
}
