using UnityEngine;
using System.Collections;
using UnityEditor;


namespace Jacovone
{

	[CustomEditor (typeof(Jacovone.DistributeOnPath))]
	[CanEditMultipleObjects]
	public class DistributeOnPathEditor : Editor
	{

		public override void OnInspectorGUI ()
		{
			DistributeOnPath p = (DistributeOnPath)target;

			serializedObject.Update ();

			EditorGUIUtility.labelWidth = 80;
			EditorGUI.BeginChangeCheck ();

			EditorGUILayout.PropertyField (serializedObject.FindProperty ("path"),
				new GUIContent ("Path", "The PathMagic"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("target"),
				new GUIContent ("Target", "The object to distribute"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("count"),
				new GUIContent ("Count", "Number of objects"));

			float _startingFrom = serializedObject.FindProperty ("startingFrom").floatValue;
			float _endTo = serializedObject.FindProperty ("endTo").floatValue;
			EditorGUILayout.MinMaxSlider (new GUIContent ("From/To", "From to [0,1]"), ref _startingFrom, ref _endTo, 0f, 1f);

			serializedObject.FindProperty ("startingFrom").floatValue = _startingFrom;
			serializedObject.FindProperty ("endTo").floatValue = _endTo;

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("startingFrom"),
				new GUIContent ("Start", "The starting point on the path"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("endTo"),
				new GUIContent ("End", "The end point in the path"));
			EditorGUILayout.EndHorizontal ();

			if (EditorGUI.EndChangeCheck ()) {
				p.Generate ();
			}


			if (GUILayout.Button ("Generate")) {
				p.Generate ();
			}

			serializedObject.ApplyModifiedProperties ();


		}


	}
}
