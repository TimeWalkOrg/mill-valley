
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// If you have MegaShapes you can set false to true to use splines to define pole paths
#if false

[CustomEditor(typeof(MegaWirePlantPoles))]
public class MegaWirePlantPolesEditor : Editor
{
	private     MegaWireUndo		undoManager;

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWirePlantPoles)target, "MegaWire Plant Poles Param");
	}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4645");
	}

	[MenuItem("GameObject/Create Other/MegaWire/Plant Poles Spline")]
	static void CreatePoles()
	{
		Vector3 pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

		GameObject go = new GameObject("Plant Poles");

		MegaWirePlantPoles poles = go.AddComponent<MegaWirePlantPoles>();
		go.transform.position = pos;
		Selection.activeObject = go;
	}

	public override void OnInspectorGUI()
	{
		MegaWirePlantPoles mod = (MegaWirePlantPoles)target;

		undoManager.CheckUndo();
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		mod.path = (MegaShape)EditorGUILayout.ObjectField("Path", mod.path, typeof(MegaShape), true);

		if ( mod.path != null && mod.path.splines.Count > 1 )
		{
			mod.curve = EditorGUILayout.IntSlider("Curve", mod.curve, 0, mod.path.splines.Count - 1);
			if ( mod.curve < 0 )
				mod.curve = 0;

			if ( mod.curve > mod.path.splines.Count - 1 )
				mod.curve = mod.path.splines.Count - 1;
		}

		mod.start = EditorGUILayout.Slider("Start", mod.start, 0.0f, 1.0f);
		mod.length = EditorGUILayout.Slider("Length", mod.length, 0.0f, 1.0f);
		mod.spacing = EditorGUILayout.FloatField("Spacing", mod.spacing);

		mod.pole = (GameObject)EditorGUILayout.ObjectField("Pole Obj", mod.pole, typeof(GameObject), true);

		mod.offset = EditorGUILayout.FloatField("Offset", mod.offset);
		mod.rotate = EditorGUILayout.Vector3Field("Rotate", mod.rotate);

		mod.conform = EditorGUILayout.BeginToggleGroup("Conform", mod.conform);
		mod.upright = EditorGUILayout.Slider("Upright", mod.upright, 0.0f, 1.0f);
		EditorGUILayout.EndToggleGroup();

		mod.copyfrom = (MegaWire)EditorGUILayout.ObjectField("Copy Wire", mod.copyfrom, typeof(MegaWire), true);
		mod.material = (Material)EditorGUILayout.ObjectField("Wire Material", mod.material, typeof(Material), true);
		mod.addwires = EditorGUILayout.BeginToggleGroup("Add Wires", mod.addwires);
		mod.reverseWire = EditorGUILayout.Toggle("Reverse Wire", mod.reverseWire);
		mod.wireSizeMult = EditorGUILayout.FloatField("Wire Size Mult", mod.wireSizeMult);
		mod.stretch = EditorGUILayout.FloatField("Stretch", mod.stretch);
		EditorGUILayout.EndToggleGroup();

		mod.seed = EditorGUILayout.IntField("Seed", mod.seed);
		mod.positionVariation = EditorGUILayout.Vector3Field("Position Variation", mod.positionVariation);
		mod.rotateVariation = EditorGUILayout.Vector3Field("Rotate Variation", mod.rotateVariation);
		mod.spacingVariation = EditorGUILayout.Slider("Spacing Variation", mod.spacingVariation, 0.0f, 1.0f);

		if ( GUI.changed )
		{
			EditorUtility.SetDirty(target);
			mod.Rebuild();
		}

		undoManager.CheckDirty();
	}
}
#endif