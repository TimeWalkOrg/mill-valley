
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class MegaWireHandles
{
	public static void DotCap(int id, Vector3 pos, Quaternion rot, float size)
	{
#if UNITY_5_6 || UNITY_2017
		Handles.DotHandleCap(id, pos, rot, size, EventType.Repaint);
#else
		// TODO: commented out line below for MegaWire compatibility issue.  Add back?
		//Handles.DotCap(id, pos, rot, size);
#endif
	}

	public static void SphereCap(int id, Vector3 pos, Quaternion rot, float size)
	{
#if UNITY_5_6 || UNITY_2017
		Handles.SphereHandleCap(id, pos, rot, size, EventType.Repaint);
#else

		// TODO: commented out line below for MegaWire compatibility issue.  Add back?
		//Handles.SphereCap(id, pos, rot, size);

#endif
	}
}

[CustomEditor(typeof(MegaWire))]
public class MegaWireEditor : Editor
{
	private     MegaWireUndo		undoManager;

	List<string> layers = new List<string>();
	List<int> layerNumbers = new List<int>();

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWire)target, "MegaWire Param");

		for ( int i = 0; i < 32; i++ )
		{
			string layerName = LayerMask.LayerToName(i);
			if ( layerName != "" )
			{
				layers.Add(layerName);
				layerNumbers.Add(i);
			}
		}
	}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4642");
	}

	LayerMask LayerMaskField(string label, LayerMask layerMask)
	{
		int maskWithoutEmpty = 0;
		for ( int i = 0; i < layerNumbers.Count; i++ )
		{
			if ( ((1 << layerNumbers[i]) & layerMask.value) > 0 )
				maskWithoutEmpty |= (1 << i);
		}
		maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers.ToArray());
		int mask = 0;
		for ( int i = 0; i < layerNumbers.Count; i++ )
		{
			if ( (maskWithoutEmpty & (1 << i)) > 0 )
				mask |= (1 << layerNumbers[i]);
		}
		layerMask.value = mask;
		return layerMask;
	}

	public override void OnInspectorGUI()
	{
		MegaWire mod = (MegaWire)target;

		undoManager.CheckUndo();

#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		MegaWire.DisableAll = EditorGUILayout.Toggle("Disable All", MegaWire.DisableAll);

		if ( GUILayout.Button("Rebuild") )
		{
			mod.Rebuild = true;
			mod.RebuildWire();
		}

		mod.warmPhysicsTime = EditorGUILayout.FloatField("Warm Physics Time", mod.warmPhysicsTime);
		if ( GUILayout.Button("Run Physics") )
		{
			mod.RunPhysics(mod.warmPhysicsTime);
		}

		if ( GUILayout.Button("Open Select Window") )
		{
		}

		if ( GUILayout.Button("Add Wire") )
		{
			MegaWireConnectionDef last = mod.connections[mod.connections.Count - 1];
			MegaWireConnectionDef cdef = new MegaWireConnectionDef();
			cdef.inOffset = last.inOffset;
			cdef.outOffset = last.outOffset;
			cdef.radius = last.radius;
			mod.connections.Add(cdef);
			mod.RebuildWire();
			mod.Rebuild = true;
		}

		mod.Enabled = EditorGUILayout.Toggle("Enabled", mod.Enabled);

		bool ShowWire = EditorGUILayout.Toggle("Show Wire", mod.ShowWire);
		if ( ShowWire != mod.ShowWire )
		{
			mod.ShowWire = ShowWire;
			mod.SetWireVisible(ShowWire);
		}
		// Lod params
		mod.disableOnDistance = EditorGUILayout.BeginToggleGroup("Disable On Dist", mod.disableOnDistance);
		mod.disableDist = EditorGUILayout.FloatField("Disable Dist", mod.disableDist);
		EditorGUILayout.EndToggleGroup();

		mod.disableOnNotVisible = EditorGUILayout.Toggle("Disable On InVisible", mod.disableOnNotVisible);

		// Physics data
		mod.showphysics = EditorGUILayout.Foldout(mod.showphysics, "Physics Params");

		if ( mod.showphysics )
		{
			EditorGUILayout.BeginVertical("box");

			int points = EditorGUILayout.IntSlider("Masses", mod.points, 2, 20);
			if ( points != mod.points )
			{
				mod.points = points;
				mod.RebuildWire();
			}

			float Mass = EditorGUILayout.FloatField("Mass", mod.Mass);
			if ( Mass != mod.Mass )
			{
				mod.Mass = Mass;
				mod.RebuildWire();
			}

			float massrnd = EditorGUILayout.FloatField("Mass Random", mod.massRand);
			if ( massrnd != mod.massRand )
			{
				mod.massRand = massrnd;
				mod.RebuildWire();
			}

			float spring = EditorGUILayout.FloatField("Spring", mod.spring);
			if ( spring != mod.spring )
			{
				mod.spring = spring;
				mod.RebuildWire();
			}

			float damp = EditorGUILayout.FloatField("Damp", mod.damp);
			if ( damp != mod.damp )
			{
				mod.damp = damp;
				mod.RebuildWire();
			}

			float stretch = EditorGUILayout.FloatField("Stretch", mod.stretch);
			if ( stretch != mod.stretch )
			{
				mod.stretch = stretch;
				mod.ChangeStretch(stretch);
			}

			mod.gravity = EditorGUILayout.Vector3Field("Gravity", mod.gravity);
			mod.airdrag = EditorGUILayout.Slider("Aero Drag", mod.airdrag, 0.0f, 1.0f);

			// These require a rebuild
			bool lencon = EditorGUILayout.Toggle("Length Constraints", mod.lengthConstraints);
			if ( lencon != mod.lengthConstraints )
			{
				mod.lengthConstraints = lencon;
				mod.RebuildWire();
			}

			bool stiff = EditorGUILayout.BeginToggleGroup("Stiff Springs", mod.stiffnessSprings);
			if ( stiff != mod.stiffnessSprings )
			{
				mod.stiffnessSprings = stiff;
				mod.RebuildWire();
			}

			float stiffrate = EditorGUILayout.FloatField("Stiff Rate", mod.stiffrate);
			if ( stiffrate != mod.stiffrate )
			{
				mod.stiffrate = stiffrate;
				mod.RebuildWire();
			}

			float stiffdamp = EditorGUILayout.FloatField("Stiff Damp", mod.stiffdamp);
			if ( stiffdamp != mod.stiffdamp )
			{
				mod.stiffdamp = stiffdamp;
				mod.RebuildWire();
			}
			EditorGUILayout.EndToggleGroup();

			mod.doCollisions = EditorGUILayout.BeginToggleGroup("Do Collisions", mod.doCollisions);
			mod.useraycast = EditorGUILayout.Toggle("Use RayCast", mod.useraycast);

			if ( mod.useraycast )
			{
				mod.collisionoff = EditorGUILayout.FloatField("Collision Offset", mod.collisionoff);
				mod.collisiondist = EditorGUILayout.FloatField("Collision Dist", mod.collisiondist);
				mod.collisionmask = LayerMaskField("Mask", mod.collisionmask);
			}
			else
				mod.floor = EditorGUILayout.FloatField("Floor", mod.floor);

			EditorGUILayout.EndToggleGroup();

			mod.showWindParams = EditorGUILayout.Foldout(mod.showWindParams, "Wind Params");
			if ( mod.showWindParams )
			{
				mod.wind = (MegaWireWind)EditorGUILayout.ObjectField("Wind Src", mod.wind, typeof(MegaWireWind), true);
				MegaWire.windDir = EditorGUILayout.Vector3Field("Wind Dir", MegaWire.windDir);
				MegaWire.windFrc = EditorGUILayout.FloatField("Wind Frc", MegaWire.windFrc);
				mod.windEffect = EditorGUILayout.FloatField("Wind Effect", mod.windEffect);
			}

			mod.showPhysicsAdv = EditorGUILayout.Foldout(mod.showPhysicsAdv, "Advanced Params");
			if ( mod.showPhysicsAdv )
			{
				mod.timeStep = EditorGUILayout.FloatField("Time Step", mod.timeStep);
				mod.fudge = EditorGUILayout.FloatField("Time Mult", mod.fudge);
				mod.startTime = EditorGUILayout.FloatField("Start Time", mod.startTime);
				mod.awakeTime = EditorGUILayout.FloatField("Awake Time", mod.awakeTime);
				mod.frameWait = EditorGUILayout.IntField("Frame Wait", mod.frameWait);
				mod.frameNum = EditorGUILayout.IntField("Frame Num", mod.frameNum);

				mod.iters = EditorGUILayout.IntSlider("Constraint Iters", mod.iters, 1, 8);
			}

			EditorGUILayout.EndVertical();
		}

		// Meshing options
		mod.showmeshparams = EditorGUILayout.Foldout(mod.showmeshparams, "Mesh Params");
		if ( mod.showmeshparams )
		{
			EditorGUILayout.BeginVertical("box");

			Material mat = (Material)EditorGUILayout.ObjectField("Material", mod.material, typeof(Material), true);
			if ( mat != mod.material )
			{
				mod.material = mat;
				for ( int i = 0; i < mod.spans.Count; i++ )
				{
					Renderer rend = mod.spans[i].GetComponent<Renderer>();
					if ( rend )
						rend.sharedMaterial = mat;
				}
			}
			mod.strandedMesher.sides = EditorGUILayout.IntSlider("Sides", mod.strandedMesher.sides, 2, 32);
			mod.strandedMesher.segments = EditorGUILayout.IntSlider("Segments", mod.strandedMesher.segments, 1, 64);
			mod.strandedMesher.SegsPerUnit = EditorGUILayout.FloatField("Segs Per Unit", mod.strandedMesher.SegsPerUnit);
			mod.strandedMesher.strands = EditorGUILayout.IntSlider("Strands", mod.strandedMesher.strands, 1, 8);
			mod.strandedMesher.offset = EditorGUILayout.FloatField("Offset", mod.strandedMesher.offset);
			mod.strandedMesher.strandRadius = EditorGUILayout.FloatField("Strand Radius", mod.strandedMesher.strandRadius);

			mod.strandedMesher.Twist = EditorGUILayout.FloatField("Twist", mod.strandedMesher.Twist);
			mod.strandedMesher.TwistPerUnit = EditorGUILayout.FloatField("Twist Per Unit", mod.strandedMesher.TwistPerUnit);

			bool genuv = EditorGUILayout.BeginToggleGroup("Gen UV", mod.strandedMesher.genuv);
			if ( genuv != mod.strandedMesher.genuv )
			{
				mod.strandedMesher.genuv = genuv;
				mod.builduvs = true;
			}

			float uvtwist = EditorGUILayout.FloatField("UV Twist", mod.strandedMesher.uvtwist);
			if ( uvtwist != mod.strandedMesher.uvtwist )
			{
				mod.strandedMesher.uvtwist = uvtwist;
				mod.builduvs = true;
			}
			
			float uvtilex = EditorGUILayout.FloatField("UV Tile X", mod.strandedMesher.uvtilex);
			if ( uvtilex != mod.strandedMesher.uvtilex )
			{
				mod.strandedMesher.uvtilex = uvtilex;
				mod.builduvs = true;
			}

			float uvtiley = EditorGUILayout.FloatField("UV Tile Y", mod.strandedMesher.uvtiley);
			if ( uvtiley != mod.strandedMesher.uvtiley )
			{
				mod.strandedMesher.uvtiley = uvtiley;
				mod.builduvs = true;
			}
			EditorGUILayout.EndToggleGroup();

			mod.strandedMesher.linInterp = EditorGUILayout.Toggle("Linear Interp", mod.strandedMesher.linInterp);
			mod.strandedMesher.calcBounds = EditorGUILayout.Toggle("Calc Bounds", mod.strandedMesher.calcBounds);
			mod.strandedMesher.calcTangents = EditorGUILayout.Toggle("Calc Tangents", mod.strandedMesher.calcTangents);
		
			int vcount = mod.GetVertexCount();
			EditorGUILayout.LabelField("Vertex Count: " + vcount);
			EditorGUILayout.EndVertical();
		}

		mod.showconnections = EditorGUILayout.Foldout(mod.showconnections, "Connections");

		if ( mod.showconnections )
		{
			for ( int i = 0; i < mod.connections.Count; i++ )
			{
				MegaWireConnectionDef con = mod.connections[i];
				EditorGUILayout.BeginVertical("box");

				float radius = EditorGUILayout.FloatField("Radius", con.radius);
				if ( radius != con.radius )
				{
					con.radius = radius;
				}

				Vector3 outOffset = EditorGUILayout.Vector3Field("Out Offset", con.outOffset);
				if ( outOffset != con.outOffset )
				{
					con.outOffset = outOffset;

					mod.Rebuild = true;
				}

				Vector3 inOffset = EditorGUILayout.Vector3Field("In Offset", con.inOffset);
				if ( inOffset != con.inOffset )
				{
					con.inOffset = inOffset;
					mod.Rebuild = true;
				}

				if ( GUILayout.Button("Delete") )
				{
					if ( mod.connections.Count > 1 )
					{
						mod.connections.RemoveAt(i);
						mod.RebuildWire();
						mod.Rebuild = true;
					}
				}

				EditorGUILayout.EndVertical();
			}
		}

		bool hidespans = EditorGUILayout.Toggle("Hide Spans", mod.hidespans);
		if ( hidespans != mod.hidespans )
		{
			mod.hidespans = hidespans;
			mod.SetHidden(mod.hidespans);
			EditorApplication.RepaintHierarchyWindow();
		}

		mod.displayGizmo = EditorGUILayout.BeginToggleGroup("Show Gizmos", mod.displayGizmo);
		mod.gizmoColor = EditorGUILayout.ColorField("Gizmo Color", mod.gizmoColor);
		EditorGUILayout.EndToggleGroup();

		mod.showAttach = EditorGUILayout.Foldout(mod.showAttach, "Span Connections");

		if ( mod.showAttach )
		{
			EditorGUILayout.BeginVertical("Box");
			for ( int i = 0; i < mod.spans.Count; i++ )
			{
				if ( i > 0 )
					EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Start", GUILayout.MaxWidth(40.0f));
				for ( int c = 0; c < mod.spans[i].connections.Count; c++ )
				{
					bool active = EditorGUILayout.Toggle(mod.spans[i].connections[c].constraints[0].active, GUILayout.MaxWidth(10.0f));
					if ( active != mod.spans[i].connections[c].constraints[0].active )
						mod.spans[i].connections[c].SetEndConstraintActive(0, active, 2.0f);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("End", GUILayout.MaxWidth(40.0f));
				for ( int c = 0; c < mod.spans[i].connections.Count; c++ )
				{
					bool active = EditorGUILayout.Toggle(mod.spans[i].connections[c].constraints[1].active, GUILayout.MaxWidth(10.0f));
					if ( active != mod.spans[i].connections[c].constraints[1].active )
						mod.spans[i].connections[c].SetEndConstraintActive(1, active, 2.0f);
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}

		if ( GUI.changed )
			EditorUtility.SetDirty(target);

		undoManager.CheckDirty();
	}

	public void OnSceneGUI()
	{
		MegaWire mod = (MegaWire)target;

		if ( mod.displayGizmo )
		{
			Handles.color = mod.gizmoColor;

			for ( int i = 0; i < mod.spans.Count; i++ )
			{
				MegaWireSpan span = mod.spans[i];

				for ( int c = 0; c < span.connections.Count; c++ )
				{
					MegaWireConnection con = span.connections[c];

					Vector3 p = con.masspos[1];
					MegaWireHandles.DotCap(0, p, Quaternion.identity, con.radius);

					for ( int m = 2; m < con.masspos.Length - 1; m++ )
					{
						p = con.masspos[m];
						MegaWireHandles.DotCap(0, con.masspos[m], Quaternion.identity, con.radius);
					}

					// Draw springs
					int scount = con.springs.Count;
					if ( mod.stiffnessSprings )
						scount = con.masses.Count - 1;

					for ( int s = 0; s < scount; s++ )
					{
						Vector3 p1 = con.masses[con.springs[s].p1].pos;
						Vector3 p2 = con.masses[con.springs[s].p2].pos;

						float w = ((con.springs[s].len - con.springs[s].restlen) / con.springs[s].restlen) * con.springs[s].ks;	//con.springs[s].restlen;

						if ( w >= 0.0f )
							Handles.color = Color.Lerp(Color.green, Color.red, w);
						else
							Handles.color = Color.Lerp(Color.green, Color.blue, -w);

						Handles.DrawLine(p1, p2);
					}
				}
			}

			if ( mod.disableOnDistance )
			{
				Handles.color = mod.gizmoColor;
				for ( int s = 0; s < mod.spans.Count; s++ )
				{
					Vector3 mp = (mod.spans[s].connections[0].masses[0].pos + mod.spans[s].connections[0].masses[mod.spans[s].connections[0].masses.Count - 1].pos) * 0.5f;
					MegaWireHandles.SphereCap(0, mp, Quaternion.identity, mod.disableDist * 2.0f);
				}
			}
		}
	}
}