
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MegaWireWind))]
public class MegaWireWindEditor : Editor
{
	private     MegaWireUndo		undoManager;

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWireWind)target, "MegaWire Wind Param");
	}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4646");
	}

	[MenuItem("GameObject/Create Other/MegaWire/Wind")]
	static void CreatePageMesh()
	{
		Vector3 pos = Vector3.zero;
		if ( UnityEditor.SceneView.lastActiveSceneView )
			pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

		GameObject go = new GameObject("Wind");

		go.AddComponent<MegaWireWind>();
		go.transform.position = pos;
		Selection.activeObject = go;
	}

	public override void OnInspectorGUI()
	{
		MegaWireWind mod = (MegaWireWind)target;

		undoManager.CheckUndo();
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		mod.dir = EditorGUILayout.Vector3Field("Direction", mod.dir);

		mod.decay = EditorGUILayout.FloatField("Decay", mod.decay);
		mod.strength = EditorGUILayout.FloatField("Strength", mod.strength);
		mod.type = (MegaWindType)EditorGUILayout.EnumPopup("Type", mod.type);
		mod.turb = EditorGUILayout.FloatField("Turbelance", mod.turb);
		mod.freq = EditorGUILayout.FloatField("Frequency", mod.freq);
		mod.scale = EditorGUILayout.FloatField("Scale", mod.scale);

		mod.strengthnoise = EditorGUILayout.BeginToggleGroup("Strength Noise", mod.strengthnoise);
		mod.strengthscale = EditorGUILayout.FloatField("Strength Scale", mod.strengthscale);
		mod.strengthfreq = EditorGUILayout.FloatField("Strength Freq", mod.strengthfreq);
		EditorGUILayout.EndToggleGroup();

		mod.dirnoise = EditorGUILayout.BeginToggleGroup("Dir Noise", mod.dirnoise);
		mod.dirscale = EditorGUILayout.Vector3Field("Dir Scale", mod.dirscale);
		mod.dirfreq = EditorGUILayout.FloatField("Dir Freq", mod.dirfreq);
		EditorGUILayout.EndToggleGroup();

		mod.displayGizmo = EditorGUILayout.BeginToggleGroup("Display Gizmo", mod.displayGizmo);

		mod.gizmoSize = EditorGUILayout.Vector2Field("Gizmo Size", mod.gizmoSize);
		mod.divs = EditorGUILayout.IntField("Divs", mod.divs);
		mod.gizscale = EditorGUILayout.Slider("Giz Scale", mod.gizscale, 0.0f, 1.0f);
		mod.gizmocol = EditorGUILayout.ColorField("Gizmo Col", mod.gizmocol);

		EditorGUILayout.EndToggleGroup();

		if ( GUI.changed )
			EditorUtility.SetDirty(target);

		undoManager.CheckDirty();
	}

	public void OnSceneGUI()
	{
		MegaWireWind mod = (MegaWireWind)target;

		Matrix4x4 tm = mod.transform.localToWorldMatrix;

		if ( mod.displayGizmo )
		{
			if ( mod.divs < 1 )
				mod.divs = 1;

			Handles.matrix = tm;

			Vector3 p = Vector3.zero;

			Vector3 frc = mod.Force(tm.MultiplyPoint(p)) * mod.gizscale;

			Handles.color = mod.gizmocol;
			Handles.DrawLine(p, p + frc);

			int xdivs = 0;
			int ydivs = 0;
			
			if ( mod.gizmoSize.x > mod.gizmoSize.y )
			{
				xdivs = mod.divs;
				ydivs = (int)((float)mod.divs / (mod.gizmoSize.x / mod.gizmoSize.y));
			}
			else
			{
				ydivs = mod.divs;
				xdivs = (int)((float)mod.divs / (mod.gizmoSize.y / mod.gizmoSize.x));
			}

			for ( int y = 0; y < 1; y++ )
			{
				p.y = mod.gizmopos.y;
				for ( int x = 0; x <= xdivs; x++ )
				{
					p.x = (-mod.gizmoSize.x * 0.5f) + (((float)x / (float)xdivs) * mod.gizmoSize.x) + mod.gizmopos.x;
					for ( int z = 0; z <= ydivs; z++ )
					{
						p.z = (-mod.gizmoSize.y * 0.5f) + (((float)z / (float)ydivs) * mod.gizmoSize.y) + mod.gizmopos.z;

						frc = mod.Force(tm.MultiplyPoint(p));

						frc *= mod.gizscale;

						MegaWireHandles.DotCap(0, p, Quaternion.identity, (mod.gizmoSize.x + mod.gizmoSize.y) * 0.5f * 0.002f);
						Handles.DrawLine(p, p + frc);
					}
				}
			}

			Vector3 gpos = mod.gizmopos;
			gpos.x += mod.gizmoSize.x * 0.5f;
			gpos.z += mod.gizmoSize.y * 0.5f;
			gpos = Handles.PositionHandle(gpos, Quaternion.identity);
		
			gpos.x -= mod.gizmoSize.x * 0.5f;
			gpos.z -= mod.gizmoSize.y * 0.5f;
			mod.gizmopos = gpos;
			//mod.gizmopos = Vector3.zero;
		}
	}
}