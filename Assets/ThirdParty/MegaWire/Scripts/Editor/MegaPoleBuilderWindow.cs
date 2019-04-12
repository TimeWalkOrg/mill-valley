
#if false
// Naming convention for parts

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// Options for parts of pole
// pole - type
// cross member - type, num, pos
// cross supports - type, pos
// connections - type, num, spacing
// pegs - type, pos, num, spacing, angle
// rings - type, pos, angle
// posters - type, pos, angle
// top - type
// transformers - type, pos, angle


public class MegaPoleBuilderWindow : EditorWindow
{
	Vector2	scroll = Vector2.zero;

	[MenuItem("GameObject/Mega Pole Builder")]
	static void Init()
	{
		EditorWindow.GetWindow(typeof(MegaPoleBuilderWindow), false, "Mega Pole Builder");
	}

	void OnFocus()
	{
	}

	void OnDestroy()
	{
	}

	void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);

		EditorGUILayout.EndScrollView();
	}
}
#endif