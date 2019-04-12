using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Jacovone.Meshes
{
	/// <summary>
	/// A specific editor subclass to configure a PathMagic torus mesh class.
	/// </summary>
	[CustomEditor (typeof(PathMagicTorus))]
	[CanEditMultipleObjects]
	public class PathMagicTorusEditor : PathMagicMeshEditor
	{
		/// <summary>
		/// Allow to configure specific torus attributes in the inspector.
		/// </summary>
		public override void GenerateInspectorGUI ()
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("radiusType"),
				new GUIContent ("Radius type", "The type of the radius"));

			if (serializedObject.FindProperty ("radiusType").enumValueIndex == 0) {
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("radius"),
					new GUIContent ("Radius", "The radius of the mesh"));
			} else {
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("radiusCurve"),
					new GUIContent ("Radius Curve", "The radius curve of the mesh"));
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("angleOffset"),
				new GUIContent ("Angle Offset", "The offset added to each section"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("twist"),
				new GUIContent ("Twist", "The twist of the generated mesh"));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("closeFront"),
				new GUIContent ("Close Front", "Close front end point"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("closeBack"),
				new GUIContent ("Close Back", "Close back end point"));
			EditorGUILayout.EndHorizontal ();
		}


		[MenuItem ("GameObject/PathMagic/Meshes/New Torus")]
		/// <summary>
		/// Creates A new instance of Torus in the Hierarchy view.
		/// </summary>
		/// <param name="menuCommand">Menu command.</param>
		public static void CreateNewTorus (MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject go = new GameObject ("Torus");
			go.transform.rotation = Quaternion.Euler (0f, 00f, 0f);
			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);

			PathMagic path = go.AddComponent<PathMagic> ();

			Waypoint wp1 = new Waypoint ();
			wp1.Position = Vector3.zero;
			wp1.InTangent = Vector3.back;
			wp1.OutTangent = Vector3.forward;
			wp1.SymmetricTangents = true;

			Waypoint wp2 = new Waypoint ();
			wp2.Position = new Vector3 (0, 0, 10);
			wp2.InTangent = Vector3.back;
			wp2.OutTangent = Vector3.forward;
			wp2.SymmetricTangents = true;

			path.Waypoints = new Waypoint[] {
				wp1,
				wp2
			};

			path.Loop = false;
			path.GlobalFollowPath = true;

			PathMagicTorus torus = go.AddComponent<PathMagicTorus> ();
			torus.path = path;
			torus.closeFront = true;
			torus.closeBack = true;
			torus.Generate ();

			MeshRenderer mr = torus.GetComponent<MeshRenderer> ();
			mr.materials = new Material[] { new Material (Shader.Find ("Standard")), 
				new Material (Shader.Find ("Standard"))
			};

			// Register the creation in the undo system
			Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);

			Selection.activeObject = go;
		}
	}
}
