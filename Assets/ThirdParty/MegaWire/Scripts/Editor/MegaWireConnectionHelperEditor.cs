
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MegaWireConnectionHelper))]
public class MegaWireConnectionHelperEditor : Editor
{
	private     MegaWireUndo		undoManager;

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWireConnectionHelper)target, "MegaWire Connection Param");
	}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4641");
	}

	public override void OnInspectorGUI()
	{
		MegaWireConnectionHelper mod = (MegaWireConnectionHelper)target;

		undoManager.CheckUndo();

#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		mod.showgizmo = EditorGUILayout.Toggle("Show Connections", mod.showgizmo);
		if ( GUILayout.Button("Add Wire") )
		{
			MegaWireConnectionDef cdef = new MegaWireConnectionDef();

			if ( mod.connections.Count > 0 )
			{
				MegaWireConnectionDef last = mod.connections[mod.connections.Count - 1];
				cdef.inOffset = last.inOffset;
				cdef.outOffset = last.outOffset;
				cdef.radius = last.radius;
			}
			mod.connections.Add(cdef);
		}

		for ( int i = 0; i < mod.connections.Count; i++ )
		{
			MegaWireConnectionDef con = mod.connections[i];
			EditorGUILayout.BeginVertical("box");

			con.radius = EditorGUILayout.FloatField("Radius", con.radius);
			con.outOffset = EditorGUILayout.Vector3Field("Out Offset", con.outOffset);
			con.inOffset = EditorGUILayout.Vector3Field("In Offset", con.inOffset);

			if ( GUILayout.Button("Delete") )
			{
				if ( mod.connections.Count > 1 )
				{
					mod.connections.RemoveAt(i);
				}
			}
			EditorGUILayout.EndVertical();
		}

		if ( GUI.changed )
			EditorUtility.SetDirty(target);

		undoManager.CheckDirty();
	}

	public void OnSceneGUI()
	{
		MegaWireConnectionHelper mod = (MegaWireConnectionHelper)target;

		if ( mod.showgizmo )
		{
			Handles.matrix = mod.transform.localToWorldMatrix;

			for ( int i = 0; i < mod.connections.Count; i++ )
			{
				MegaWireConnectionDef con = mod.connections[i];

				con.inOffset = Handles.PositionHandle(con.inOffset, Quaternion.identity);
				con.outOffset = Handles.PositionHandle(con.outOffset, Quaternion.identity);
				Handles.Label(con.inOffset, "in " + i);
				Handles.Label(con.outOffset, "out " + i);
			}
		}
	}
}