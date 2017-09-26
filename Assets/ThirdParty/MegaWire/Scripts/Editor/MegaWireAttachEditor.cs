
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MegaWireAttach))]
public class MegaWireAttachEditor : Editor
{
	private     MegaWireUndo		undoManager;

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWireAttach)target, "MegaWire Attach Param");
	}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4640");
	}

	public override void OnInspectorGUI()
	{
		MegaWireAttach mod = (MegaWireAttach)target;

		undoManager.CheckUndo();

#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		MegaWire wire = (MegaWire)EditorGUILayout.ObjectField("Wire", mod.wire, typeof(MegaWire), true);
		if ( wire != mod.wire )
		{
			mod.wire = wire;
			mod.parent = null;
			if ( wire  )
			{
				mod.parent = wire.transform.parent;
			}
		}

		mod.alpha = EditorGUILayout.Slider("Alpha", mod.alpha, 0.0f, 1.0f);

		int cons = 0;
		if ( mod.wire )
			cons = mod.wire.spans[0].connections.Count - 1;

		mod.connection = EditorGUILayout.IntSlider("Strand", mod.connection, 0, cons);
		mod.offset = EditorGUILayout.Vector3Field("Offset", mod.offset);

		mod.align = EditorGUILayout.BeginToggleGroup("Align", mod.align);
		mod.rotate = EditorGUILayout.Vector3Field("Rotate", mod.rotate);
		EditorGUILayout.EndToggleGroup();

		if ( GUI.changed )
			EditorUtility.SetDirty(target);

		undoManager.CheckDirty();
	}
}