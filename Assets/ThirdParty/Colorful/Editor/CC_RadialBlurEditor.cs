using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CC_RadialBlur))]
public class CC_RadialBlurEditor : Editor
{
	SerializedProperty p_amount;
	SerializedProperty p_center;
	SerializedProperty p_quality;

	static string[] qualities = { "Low", "Medium", "High" };

	void OnEnable()
	{
		p_amount = serializedObject.FindProperty("amount");
		p_center = serializedObject.FindProperty("center");
		p_quality = serializedObject.FindProperty("quality");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		p_quality.intValue = EditorGUILayout.Popup("Quality", p_quality.intValue, qualities);

		if (p_quality.intValue == 2)
			EditorGUILayout.LabelField("High quality will only be available on SM3.0 compatible GPUs !", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField(p_amount);
		EditorGUILayout.PropertyField(p_center, new GUIContent("Center Point"));

		serializedObject.ApplyModifiedProperties();
    }
}
