using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CC_LookupFilter))]
public class CC_LookupFilterEditor : Editor
{
	SerializedProperty p_lookupTexture;

	void OnEnable()
	{
		p_lookupTexture = serializedObject.FindProperty("lookupTexture");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(p_lookupTexture);
		EditorGUILayout.LabelField("Read the documentation for more information about this effect.", EditorStyles.boldLabel);

		serializedObject.ApplyModifiedProperties();
    }
}
