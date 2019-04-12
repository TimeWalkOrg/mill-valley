using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CC_Vintage))]
public class CC_VintageEditor : Editor
{
	SerializedProperty p_filter;

	void OnEnable()
	{
		p_filter = serializedObject.FindProperty("filter");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(p_filter);

		serializedObject.ApplyModifiedProperties();
	}
}
