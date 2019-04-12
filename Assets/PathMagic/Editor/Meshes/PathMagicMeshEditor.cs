using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Jacovone.Meshes
{
	/// <summary>
	/// Path magic mesh editor base class. All PathMagic mesh classes editors have
	/// to subclass this one.
	/// </summary>
	[CustomEditor (typeof(PathMagicMesh))]
	[CanEditMultipleObjects]
	public abstract class PathMagicMeshEditor : Editor
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI ()
		{
			PathMagicMesh m = (PathMagicMesh)target;
            
			serializedObject.Update ();
            
			EditorGUIUtility.labelWidth = 80;
			EditorGUI.BeginChangeCheck ();
            
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("path"),
				new GUIContent ("Path", "The PathMagic"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("sections"),
				new GUIContent ("Sections", "The number of longitudinal sections"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("pieces"),
				new GUIContent ("Pieces", "Number of mesh pieces"));

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

			EditorGUILayout.PropertyField (serializedObject.FindProperty ("flipped"),
				new GUIContent ("Flipped", "Is the resulting mesh flipped?"));

			EditorGUILayout.BeginVertical ("Box");
			GenerateInspectorGUI ();
			EditorGUILayout.EndVertical ();

			EditorGUIUtility.labelWidth = 160;
			EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.PropertyField (serializedObject.FindProperty ("autoUpdateMesh"), 
				new GUIContent ("Auto update mesh", "Auto update mesh every frame in edit mode"));
			EditorGUILayout.Space ();

			if (GUILayout.Button ("Generate")) {
				m.Generate ();

				// Important for lightmapiing of the mesh
				Unwrapping.GenerateSecondaryUVSet (((PathMagicMesh)target).mesh);
			}

			EditorGUILayout.EndHorizontal ();
			EditorGUIUtility.labelWidth = 80;

			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();

			if (EditorGUI.EndChangeCheck ()) {
				m.Generate ();
			}

			EditorGUILayout.BeginVertical ("Box");

			Mesh mesh = m.GetComponent<MeshFilter> ().sharedMesh;
			if (mesh != null)
				EditorGUILayout.LabelField ("Info: " + mesh.vertexCount + " vertices, " + mesh.triangles.Length / 3 + " triangles.");

			EditorGUILayout.EndVertical ();
		}

		/// <summary>
		/// Abstract method that subclasses editors have to override
		/// to create a specific inspector configuration.
		/// </summary>
		public abstract void GenerateInspectorGUI ();
	}
}