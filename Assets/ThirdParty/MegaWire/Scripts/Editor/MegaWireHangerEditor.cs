
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MegaWireHanger))]
public class MegaWireHangerEditor : Editor
{
	private     MegaWireUndo		undoManager;

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWireHanger)target, "MegaWire Hanger Param");
	}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4644");
	}

	public override void OnInspectorGUI()
	{
		MegaWireHanger mod = (MegaWireHanger)target;

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
			if ( wire )
			{
				mod.parent = wire.transform.parent;
			}
		}

		mod.alpha = EditorGUILayout.Slider("Alpha", mod.alpha, 0.0f, 1.0f);

		int cons = 0;
		if ( mod.wire )
			cons = mod.wire.spans[0].connections.Count - 1;

		mod.strand = EditorGUILayout.IntSlider("Strand", mod.strand, 0, cons);
		mod.offset = EditorGUILayout.FloatField("Offset", mod.offset);

		mod.align = EditorGUILayout.BeginToggleGroup("Align", mod.align);
		mod.rotate = EditorGUILayout.Vector3Field("Rotate", mod.rotate);
		EditorGUILayout.EndToggleGroup();
		mod.weight = EditorGUILayout.FloatField("Weight", mod.weight);
		//mod.snaptomass = EditorGUILayout.Toggle("Snap To Mass", mod.snaptomass);

		if ( GUI.changed )
			EditorUtility.SetDirty(target);

		undoManager.CheckDirty();
	}
}