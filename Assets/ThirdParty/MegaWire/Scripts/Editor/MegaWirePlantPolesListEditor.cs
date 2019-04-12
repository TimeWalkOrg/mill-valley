
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MegaWirePlantPolesList))]
public class MegaWirePlantPolesListEditor : Editor
{
	private     MegaWireUndo		undoManager;

	private void OnEnable()
	{
		undoManager = new MegaWireUndo((MegaWirePlantPolesList)target, "MegaWire Plant Poles List Param");
	}

	bool	addingpoint = false;

	//public float arrowwidth = 0.1f;
	//public float arrowlength = 1.0f;
	//public float vertStart = 0.2f;
	//public float vertLength = 1.3f;
	//public float arrowoff = 1.0f;

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=4645");
	}

	[MenuItem("GameObject/Create Other/MegaWire/Plant Poles")]
	static void CreatePoles()
	{
		Vector3 pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

		GameObject go = new GameObject("Plant Poles");

		MegaWirePlantPolesList poles = go.AddComponent<MegaWirePlantPolesList>();
		go.transform.position = pos;
		Selection.activeObject = go;

		poles.waypoints.Add(Vector3.zero);
		poles.waypoints.Add(new Vector3(10.0f, 0.0f, 0.0f));
		poles.waypoints.Add(new Vector3(20.0f, 0.0f, 0.0f));
		poles.waypoints.Add(new Vector3(30.0f, 0.0f, 0.0f));
	}

	public override void OnInspectorGUI()
	{
		bool rebuild = false;
		MegaWirePlantPolesList mod = (MegaWirePlantPolesList)target;

		undoManager.CheckUndo();
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		if ( GUILayout.Button("Add Waypoint") )
		{
			Vector3 p = Vector3.zero;

			if ( mod.waypoints.Count > 0 )
			{
				if ( mod.waypoints.Count > 1 )
				{
					p = mod.waypoints[mod.waypoints.Count - 1];
					Vector3 p1 = mod.waypoints[mod.waypoints.Count - 2];
					p += p - p1;
				}
				else
				{
					p = mod.waypoints[mod.waypoints.Count - 1];
					p.x += 4.0f;
				}
			}
 
			mod.waypoints.Add(p);
			rebuild = true;
		}

		for ( int i = 0; i < mod.waypoints.Count; i++ )
		{
			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("-", GUILayout.MaxWidth(20.0f)) )
			{
				mod.waypoints.RemoveAt(i);
				rebuild = true;
			}
			else
				mod.waypoints[i] = EditorGUILayout.Vector3Field("Waypoint " + i, mod.waypoints[i]);
			EditorGUILayout.EndHorizontal();
		}

		if ( mod.waypoints.Count > 0 )
		{
			if ( GUILayout.Button("Delete") )
			{
				mod.waypoints.RemoveAt(mod.waypoints.Count - 1);
				rebuild = true;
			}
		}

		mod.start = EditorGUILayout.Slider("Start", mod.start, 0.0f, 1.0f);
		mod.length = EditorGUILayout.Slider("Length", mod.length, 0.0f, 1.0f);
		mod.spacing = EditorGUILayout.FloatField("Spacing", mod.spacing);
		if ( mod.spacing < 1.0f )
			mod.spacing = 1.0f;
		mod.closed = EditorGUILayout.Toggle("Closed", mod.closed);

		mod.pole = (GameObject)EditorGUILayout.ObjectField("Pole Obj", mod.pole, typeof(GameObject), true);

		mod.offset = EditorGUILayout.FloatField("Offset", mod.offset);
		mod.rotate = EditorGUILayout.Vector3Field("Rotate", mod.rotate);

		mod.conform = EditorGUILayout.BeginToggleGroup("Conform", mod.conform);
		mod.upright = EditorGUILayout.Slider("Upright", mod.upright, 0.0f, 1.0f);
		EditorGUILayout.EndToggleGroup();

		mod.material = (Material)EditorGUILayout.ObjectField("Wire Material", mod.material, typeof(Material), true);
		mod.copyfrom = (MegaWire)EditorGUILayout.ObjectField("Copy Wire", mod.copyfrom, typeof(MegaWire), true);
		mod.addwires = EditorGUILayout.BeginToggleGroup("Add Wires", mod.addwires);
		mod.reverseWire = EditorGUILayout.Toggle("Reverse Wire", mod.reverseWire);
		mod.wireSizeMult = EditorGUILayout.FloatField("Wire Size Mult", mod.wireSizeMult);
		mod.stretch = EditorGUILayout.Slider("Stretch", mod.stretch, 0.0f, 1.5f);
		EditorGUILayout.EndToggleGroup();

		mod.seed = EditorGUILayout.IntField("Seed", mod.seed);
		mod.positionVariation = EditorGUILayout.Vector3Field("Position Variation", mod.positionVariation);
		mod.rotateVariation = EditorGUILayout.Vector3Field("Rotate Variation", mod.rotateVariation);
		mod.spacingVariation = EditorGUILayout.Slider("Spacing Variation", mod.spacingVariation, 0.0f, 1.0f);

		//mod.realtime = EditorGUILayout.Toggle("Realtime", mod.realtime);
		//mod.watch = (GameObject)EditorGUILayout.ObjectField("Watch", mod.watch, typeof(GameObject), true);

		if ( GUI.changed || rebuild )
		{
			EditorUtility.SetDirty(target);
			mod.Rebuild();
		}

		GUI.changed = false;
		mod.showgizmoparams = EditorGUILayout.Foldout(mod.showgizmoparams, "Gizmo Params");

		if ( mod.showgizmoparams )
		{
			mod.showgizmo = EditorGUILayout.BeginToggleGroup("Show Gizmo", mod.showgizmo);
			mod.gizmoType = (MegaWireGizmoType)EditorGUILayout.EnumPopup("Show Type", mod.gizmoType);

			mod.units = (MegaWireUnits)EditorGUILayout.EnumPopup("Units", mod.units);
			mod.unitsScale = EditorGUILayout.FloatField("Units Scale", mod.unitsScale);

			mod.arrowwidth = EditorGUILayout.FloatField("Arrow Width", mod.arrowwidth);
			mod.arrowlength = EditorGUILayout.FloatField("Arrow Length", mod.arrowlength);
			mod.arrowoff = EditorGUILayout.Slider("Arrow Offset", mod.arrowoff, 0.0f, 1.0f);
			mod.vertStart = EditorGUILayout.FloatField("Vert Start", mod.vertStart);
			mod.vertLength = EditorGUILayout.FloatField("Vert Length", mod.vertLength);

			mod.dashdist = EditorGUILayout.FloatField("Dash Dist", mod.dashdist);
			mod.lineCol = EditorGUILayout.ColorField("Line Color", mod.lineCol);
			mod.arrowCol = EditorGUILayout.ColorField("Arrow Color", mod.arrowCol);
			mod.otherCol = EditorGUILayout.ColorField("Other Color", mod.otherCol);
			mod.dashCol = EditorGUILayout.ColorField("Dash Color", mod.dashCol);
			EditorGUILayout.EndToggleGroup();
		}

		if ( GUI.changed )
		{
			EditorUtility.SetDirty(target);
		}

		undoManager.CheckDirty();
	}

	bool	addmode = false;
	int		seg = -1;

	MegaArrow	arrow = new MegaArrow();

	void OnSceneGUI()
	{
		MegaWirePlantPolesList mod = (MegaWirePlantPolesList)target;

		arrow.arrowlength = mod.arrowlength;
		arrow.arrowoff = mod.arrowoff;
		arrow.arrowwidth = mod.arrowwidth;
		arrow.vertStart = mod.vertStart;
		arrow.vertLength = mod.vertLength;
		arrow.lineCol = mod.lineCol;
		arrow.otherCol = mod.otherCol;
		arrow.arrowcol = mod.arrowCol;
		arrow.dashCol = mod.dashCol;
		arrow.units = MegaArrow.GetUnitsString(mod.units);
		arrow.unitsscale = MegaArrow.GetUnitsScale(mod.units, mod.unitsScale);
		arrow.dashdist = mod.dashdist;

		if ( mod.showgizmo && (mod.gizmoType == MegaWireGizmoType.Waypoint || mod.gizmoType == MegaWireGizmoType.Both) )
		{
			switch ( Event.current.type )
			{
				case EventType.KeyDown:
					if ( Event.current.keyCode == KeyCode.A )
					{
						if ( !addmode )
						{
							Debug.Log("Start Add Mode");
							addmode = true;
							Event.current.Use();
						}
					}
					break;

				case EventType.KeyUp:
					if ( Event.current.keyCode == KeyCode.A )
					{
						if ( addmode )
						{
							Debug.Log("End Add mode");
							addmode = false;
							addingpoint = false;
							Event.current.Use();
						}
					}
					break;
			}

			if ( mod.showgizmo && (mod.gizmoType == MegaWireGizmoType.Waypoint || mod.gizmoType == MegaWireGizmoType.Both) )
			{
				Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;

				if ( cam )
				{
					Vector3 camwp = Event.current.mousePosition;

					camwp.z = 0.0f;

					float dist = 100000.0f;

					seg = -1;

					// Just do distance from mid point, and use closest
					for ( int i = 0; i < mod.waypoints.Count - 1; i++ )
					{
						Vector3 p = mod.waypoints[i];
						Vector3 p1 = mod.waypoints[i + 1];

						Vector3 wp = mod.transform.TransformPoint(p);
						Vector3 wp1 = mod.transform.TransformPoint(p1);

						Vector3 sp = cam.WorldToScreenPoint(wp);
						Vector3 sp1 = cam.WorldToScreenPoint(wp1);

						sp.z = 0.0f;
						sp1.z = 0.0f;

						Vector3 cp2 = (sp + sp1) * 0.5f;

						float d = Vector3.Distance(camwp, cp2);
						if ( d < dist )
						{
							dist = d;
							seg = i;
						}
					}

					if ( seg >= 0 )
					{
						Vector3 pos = Vector3.Lerp(mod.waypoints[seg], mod.waypoints[seg + 1], 0.5f);

						Handles.color = Color.green;
						MegaWireHandles.DotCap(1066, mod.transform.TransformPoint(pos), Quaternion.identity, 0.2f);
						
					}
				}
			}

			if ( addmode )
			{
				int controlID = GUIUtility.GetControlID(FocusType.Passive);

				Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;

				if ( cam )
				{
					switch ( Event.current.GetTypeForControl(controlID) )
					{
						case EventType.MouseDown:
							GUIUtility.hotControl = controlID;
							Event.current.Use();

							if ( !addingpoint )
							{
								addingpoint = true;

								if ( seg >= 0 )
								{
									Vector3 cp = (mod.waypoints[seg] + mod.waypoints[seg +1]) * 0.5f;
									mod.waypoints.Insert(seg + 1, cp);
								}
							}

							break;

						case EventType.MouseUp:
							addingpoint = false;
							GUIUtility.hotControl = 0;
							Event.current.Use();
							break;
					}
				}
			}
		}

		// Draw the fancier lines
		// Show waypoints and or pole spacing
		// option to control units
		//string units = MegaArrow.GetUnitsString(mod.units);
		//float unitsscale = MegaArrow.GetUnitsScale(mod.units, mod.unitsScale);

		GUIStyle style = new GUIStyle();
		style.normal.textColor = mod.lineCol;

		if ( mod.waypoints.Count > 1 && mod.showgizmo )
		{
			float arrowoff = mod.vertStart + (mod.vertLength * mod.arrowoff);
			arrow.arrowoff = arrowoff;
			
			if ( mod.gizmoType == MegaWireGizmoType.Waypoint || mod.gizmoType == MegaWireGizmoType.Both )
			{
				for ( int i = 0; i < mod.waypoints.Count; i++ )
				{
					Vector3 lp = mod.transform.TransformPoint(mod.waypoints[i]);
					lp = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(lp);

					Handles.color = mod.lineCol;
					if ( lp.z > 0.0f )
						Handles.Label(mod.transform.TransformPoint(mod.waypoints[i]), "" + i, style);

					if ( i > 0 )
					{
						Vector3 p1 = mod.transform.TransformPoint(mod.waypoints[i - 1]);
						Vector3 p2 = mod.transform.TransformPoint(mod.waypoints[i]);
						arrow.DrawArrow(i, p1, p2, style);
					}

					Vector3 p = mod.transform.worldToLocalMatrix.MultiplyPoint(Handles.PositionHandle(mod.transform.localToWorldMatrix.MultiplyPoint(mod.waypoints[i]), Quaternion.identity));
					if ( p != mod.waypoints[i] )
					{
						mod.waypoints[i] = p;
						mod.Rebuild();
					}
				}

				if ( mod.closed )
					Handles.DrawLine(mod.waypoints[mod.waypoints.Count - 1], mod.waypoints[0]);
			}
			// Poles

			if ( mod.gizmoType == MegaWireGizmoType.Pole || mod.gizmoType == MegaWireGizmoType.Both )
			{
				Handles.matrix = Matrix4x4.identity;

				for ( int i = 0; i < mod.poles.Count; i++ )
				{
					Vector3 lp = mod.transform.TransformPoint(mod.poles[i].transform.position);
					lp = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(lp);

					if ( i > 0 )
					{
						Vector3 p1 = mod.poles[i - 1].transform.position;
						Vector3 p2 = mod.poles[i].transform.position;
						arrow.DrawArrow(i, p1, p2, style);
					}
				}
			}
		}
	}

	// can return the alpha or t value here
	Vector2 GetClosetPoint(Vector2 A, Vector2 B, Vector2 P, bool segmentClamp, ref float alpha)
	{
		Vector2 AP = P - A;
		Vector2 AB = B - A;
		float ab2 = AB.x * AB.x + AB.y * AB.y;
		float ap_ab = AP.x * AB.x + AP.y * AB.y;
		float t = ap_ab / ab2;

		if ( segmentClamp )
		{
			 if ( t < 0.0f )
				 t = 0.0f;
			 else
			 {
				 if ( t > 1.0f )
					 t = 1.0f;
			 }
		}

		alpha = t;
		Vector2 Closest = A + AB * t;
		return Closest;
	}

	Vector3 GetClosestPointOnLineSegment(Vector3 LinePointStart, Vector3 LinePointEnd, Vector3 testPoint)
	{
		Vector3 LineDiffVect = LinePointEnd - LinePointStart;
		float lineSegSqrLength = LineDiffVect.sqrMagnitude;
 
		Vector3 LineToPointVect = testPoint - LinePointStart;
		float dotProduct = Vector3.Dot(LineDiffVect, LineToPointVect);
 
		float percAlongLine = dotProduct / lineSegSqrLength;
 
		if (  percAlongLine  < 0.0f ||  percAlongLine  > 1.0f )
		{
			// Point isn't within the line segment
			return Vector3.zero;
		}
 
		return LinePointStart + (percAlongLine * (LinePointEnd - LinePointStart));
	}

	public float FindNearestPoint(Vector2 p1, Vector2 p2, Vector2 p, int iterations)
	{
		float positiveInfinity = float.PositiveInfinity;
		float num2 = 0.0f;
		iterations = Mathf.Clamp(iterations, 0, 5);
		//int kt = 0;

		for ( float i = 0.0f; i <= 1.0f; i += 0.01f )
		{
			//Vector3 vector = this.GetPositionOnSpline(i) - p;
			//Vector3 vector = InterpCurve3D(0, i, true) - p;	//this.GetPositionOnSpline(i) - p;
			Vector2 vector = Vector2.Lerp(p1, p2, i);
			float sqrMagnitude = vector.sqrMagnitude;
			if ( positiveInfinity > sqrMagnitude )
			{
				positiveInfinity = sqrMagnitude;
				num2 = i;
			}
		}

		for ( int j = 0; j < iterations; j++ )
		{
			float num6 = 0.01f * Mathf.Pow(10.0f, -((float)j));
			float num7 = num6 * 0.1f;
			for ( float k = Mathf.Clamp01(num2 - num6); k <= Mathf.Clamp01(num2 + num6); k += num7 )
			{
				//Vector3 vector2 = InterpCurve3D(0, k, true) - p;	//this.GetPositionOnSpline(k) - p;
				Vector3 vector2 = Vector2.Lerp(p1, p2, k) - p;	//this.GetPositionOnSpline(k) - p;
				float num9 = vector2.sqrMagnitude;

				if ( positiveInfinity > num9 )
				{
					positiveInfinity = num9;
					num2 = k;
				}
			}
		}

		return num2;	//Vector2.Lerp(p1, p2, num2);
		//return np;
	}

#if false
	public string GetUnitsString(MegaWireUnits units)
	{
		switch ( units )
		{
			case MegaWireUnits.Meters:
				return "m";

			case MegaWireUnits.Centimeters:
				return "cm";

			case MegaWireUnits.Feet:
				return "ft";

			case MegaWireUnits.Inches:
				return "in";

			case MegaWireUnits.Yards:
				return "yds";
		}

		return "m";
	}

	float GetUnitsScale(MegaWireUnits units, float scale)
	{
		switch ( units )
		{
			case MegaWireUnits.Meters:
				return 1.0f * scale;

			case MegaWireUnits.Centimeters:
				return 100.0f * scale;

			case MegaWireUnits.Feet:
				return 3.28084f * scale;

			case MegaWireUnits.Inches:
				return 39.37f * scale;

			case MegaWireUnits.Yards:
				return 1.0936133f * scale;
		}

		return scale;
	}
#endif
}

[System.Serializable]
public class MegaArrow
{
	public float	arrowlength;
	public float	arrowoff;
	public float	arrowwidth;
	public float	vertStart;
	public float	vertLength;
	public Color	lineCol;
	public Color	otherCol;
	public Color	arrowcol;
	public Color	dashCol;
	public string	units;
	public float	unitsscale;
	public float	dashdist;

	public void DrawArrow(int i, Vector3 p1, Vector3 p2, GUIStyle style)
	{
		//Vector3 p1 = mod.poles[i - 1].transform.position;
		//Vector3 p2 = mod.poles[i].transform.position;

		float dist = Vector3.Distance(p1, p2);
		Vector3 dir = (p2 - p1).normalized;

		Vector3 cross = Vector3.Cross(dir, Vector3.up);

		if ( (i & 1) == 1 )
			cross = -cross;

		Vector3 outline = cross * arrowoff;
		Vector3 outline1 = cross * vertStart;
		Vector3 outline2 = cross * (vertStart + vertLength);

		Vector3 p3 = p1 + outline;
		Vector3 p4 = p2 + outline;

		Handles.color = lineCol;	//new Color(0.75f, 0.75f, 0.75f, 0.75f);
		Handles.DrawLine(p1, p2);
		Vector3 mid = (p3 + p4) * 0.5f;
		Vector3 lp = mid;
		lp = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(lp);

		if ( lp.z > 0.0f )
			Handles.Label(mid, "" + (dist * unitsscale).ToString("0.00") + units, style);

		Handles.color = otherCol;	//new Color(0.35f, 0.35f, 0.35f, 0.75f);
		Handles.DrawLine(p1 + outline1, p1 + outline2);
		Handles.DrawLine(p2 + outline1, p2 + outline2);

		// Arrow heads
		Vector3 ap1 = p1 + (dir * arrowlength);
		Vector3 ap2 = p2 - (dir * arrowlength);

		Vector3 apl = ap1 + (cross * (arrowoff - arrowwidth));
		Vector3 apr = ap1 + (cross * (arrowoff + arrowwidth));

		Vector3 apl1 = ap2 + (cross * (arrowoff - arrowwidth));
		Vector3 apr1 = ap2 + (cross * (arrowoff + arrowwidth));

		Handles.color = otherCol;
		ap1 = p3 + (dir * arrowlength);
		ap2 = p4 - (dir * arrowlength);

		int dcount = 1;
		float dca = 1.0f;

		if ( dashdist > 0.01f )
		{
			dca = Vector3.Distance(ap1, ap2) / dashdist;
			dcount = (int)(Vector3.Distance(ap1, ap2) / dashdist);
		}

		Vector3 delta = (ap2 - ap1) / dca;	//(float)dcount;	//21.0f;
		for ( int d = 0; d <= dcount; d++ )
		{
			Vector3 pp = ap1 + ((float)d * delta);
			Vector3 pp1 = ap1 + ((float)(d + 1) * delta);

			if ( d == dcount )
				pp1 = ap2;

			if ( (d & 1) == 0 )
			{
				Handles.color = otherCol;
				Handles.DrawLine(pp, pp1);
			}
			else
			{
				Handles.color = dashCol;
				Handles.DrawLine(pp, pp1);
			}
		}

		Handles.color = arrowcol;
		GL.PushMatrix();
		GL.Color(arrowcol);
		GL.Begin(GL.TRIANGLES);
		GL.Vertex3(p3.x, p3.y, p3.z);
		GL.Vertex3(apl.x, apl.y, apl.z);
		GL.Vertex3(apr.x, apr.y, apr.z);

		GL.Vertex3(p4.x, p4.y, p4.z);
		GL.Vertex3(apl1.x, apl1.y, apl1.z);
		GL.Vertex3(apr1.x, apr1.y, apr1.z);

		GL.End();
		GL.PopMatrix();
		Handles.DrawLine(p3, apl);
		Handles.DrawLine(p3, apr);
		Handles.DrawLine(apl, apr);

		Handles.DrawLine(p4, apl1);
		Handles.DrawLine(p4, apr1);
		Handles.DrawLine(apl1, apr1);
	}

	public static string GetUnitsString(MegaWireUnits units)
	{
		switch ( units )
		{
			case MegaWireUnits.Meters:
				return "m";

			case MegaWireUnits.Centimeters:
				return "cm";

			case MegaWireUnits.Feet:
				return "ft";

			case MegaWireUnits.Inches:
				return "in";

			case MegaWireUnits.Yards:
				return "yds";
		}

		return "m";
	}

	public static float GetUnitsScale(MegaWireUnits units, float scale)
	{
		switch ( units )
		{
			case MegaWireUnits.Meters:
				return 1.0f * scale;

			case MegaWireUnits.Centimeters:
				return 100.0f * scale;

			case MegaWireUnits.Feet:
				return 3.28084f * scale;

			case MegaWireUnits.Inches:
				return 39.37f * scale;

			case MegaWireUnits.Yards:
				return 1.0936133f * scale;
		}

		return scale;
	}
}
