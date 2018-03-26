using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace Jacovone.CameraTools
{
	[CustomEditor (typeof(PathMagicFollower))]
	/// <summary>
	/// Path magic follower editor. This is the editor class for PathMagicFollower instances. It defines the inspector behavios to
	/// configure a complete PathMagicFollower instance.
	/// </summary>
	public class PathMagicFollowerEditor : Editor
	{
		/// <summary>
		/// The path magic logo. Loaded from resources.
		/// </summary>
		private Texture2D pathMagicLogo = null;

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		void OnEnable ()
		{
			pathMagicLogo = Resources.Load ("pathmagiclogo") as Texture2D;
		}

		/// <summary>
		/// Raises the OnInspectorGUI event.
		/// </summary>
		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			PathMagicFollower pmf = (PathMagicFollower)target;

			if (pmf.GetComponent<PathMagic> () == null && pmf.GetComponent<PathMagicAnimator> () == null) {
				EditorGUILayout.HelpBox ("Please attach this script to a PathMagic or PathMagicAnimator instance.", MessageType.Warning);
			} else {

				EditorGUILayout.LabelField (new GUIContent (pathMagicLogo), GUILayout.Width (142), GUILayout.Height (28));

				EditorGUILayout.BeginVertical ("Box");
				EditorGUIUtility.labelWidth = 80;

				EditorGUILayout.PropertyField (serializedObject.FindProperty ("target"), new GUIContent ("Target", "The target to follow"));
			
				if (serializedObject.FindProperty ("target").objectReferenceValue == null) {
					EditorGUILayout.HelpBox ("Please connect a target to follow.", MessageType.Warning);
				}


				EditorGUIUtility.labelWidth = 120;

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("precision"), new GUIContent ("Precision", "The number of possible points on which the path will set the current pos to follow the target"));
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("waypointsOnly"), new GUIContent ("Only Waypoints", "The object will move only from a waypoint to another"));
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.PropertyField (serializedObject.FindProperty ("accurate"), new GUIContent ("Accurate", "If set, give more accurate result on nearest point search."));

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("lerpPosition"), new GUIContent ("Lerp Position", "If set, positions will Lerp"));

				if (!serializedObject.FindProperty ("lerpPosition").boolValue) {
					GUI.enabled = false;
				}

				EditorGUILayout.PropertyField (serializedObject.FindProperty ("lerpFactor"), new GUIContent ("Lerp Factor", "The factor at which the CurrentPos of the path will Lerp to"));

				GUI.enabled = true;

				EditorGUILayout.EndHorizontal ();

				EditorGUIUtility.labelWidth = 80;

				EditorGUILayout.EndVertical ();
			}

			serializedObject.ApplyModifiedProperties ();
		}

		/// <summary>
		/// Raises the OnSceneGUI event.
		/// </summary>
		void OnSceneGUI ()
		{ 
			PathMagicFollower pmf = (PathMagicFollower)target;

			if (pmf == null)
				return;
			if (pmf.target == null)
				return;
			
			Handles.color = Color.magenta;
			Handles.DrawDottedLine (pmf.PointOfView, pmf.target.position, 2f);
		}
	}
}