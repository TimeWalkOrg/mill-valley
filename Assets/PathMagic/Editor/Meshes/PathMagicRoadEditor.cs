using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Jacovone.Meshes
{
	/// <summary>
	/// A specific editor subclass to configure a PathMagic road mesh class.
	/// </summary>
	[CustomEditor (typeof(PathMagicRoad))]
	[CanEditMultipleObjects]
	public class PathMagicRoadEditor : PathMagicMeshEditor
	{
		/// <summary>
		/// Allow to configure specific road attributes in the inspector.
		/// </summary>
		public override void GenerateInspectorGUI ()
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("widthType"),
				new GUIContent ("Width type", "The type of the width"));

			if (serializedObject.FindProperty ("widthType").enumValueIndex == 0) {
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("width"),
					new GUIContent ("Width", "The width of the road"));
			} else {
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("widthCurve"),
					new GUIContent ("Width Curve", "The width curve of the road"));
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("height"),
				new GUIContent ("Height", "The height of the generated road"));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("closeFront"),
				new GUIContent ("Close Front", "Close front end point"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("closeBack"),
				new GUIContent ("Close Back", "Close back end point"));
			EditorGUILayout.EndHorizontal ();
		}

		[MenuItem ("GameObject/PathMagic/Meshes/New Road")]
		/// <summary>
		/// Creates A new instance of Road in the Hierarchy view.
		/// </summary>
		/// <param name="menuCommand">Menu command.</param>
		public static void CreateNewTorus (MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject go = new GameObject ("Road");
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

			PathMagicRoad road = go.AddComponent<PathMagicRoad> ();
			road.path = path;
			road.closeFront = true;
			road.closeBack = true;
			road.Generate ();

			MeshRenderer mr = road.GetComponent<MeshRenderer> ();
			mr.materials = new Material[] { new Material (Shader.Find ("Standard")), 
				new Material (Shader.Find ("Standard")), 
				new Material (Shader.Find ("Standard"))
			};

			// Register the creation in the undo system
			Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);

			Selection.activeObject = go;
		}
	}
}
