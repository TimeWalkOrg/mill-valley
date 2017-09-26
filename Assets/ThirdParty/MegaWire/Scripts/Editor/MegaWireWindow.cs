
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// TODO: Option to close loop
public class MegaWireWindow : EditorWindow
{
	static bool				picking = false;
	static GameObject		lastsel = null;
	static List<GameObject>	selection = new List<GameObject>();
	public MegaWire			copyfrom;
	public string			wirename = "Wire";
	public Material			material;
	Vector2					scroll = Vector2.zero;

	public float				arrowwidth = 0.2f;
	public float				arrowlength = 1.1f;
	public float				vertStart = 0.2f;
	public float				vertLength = 1.5f;
	public float				arrowoff = 0.8f;
	public float				dashdist = 2.0f;

	public Color				arrowCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color				lineCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color				otherCol = new Color(0.75f, 0.75f, 0.75f, 1.0f);
	public Color				dashCol = new Color(0.5f, 0.5f, 0.5f, 1.0f);

	public MegaWireUnits		units = MegaWireUnits.Meters;
	public float				unitsScale = 1.0f;

	[MenuItem("GameObject/Mega Wire")]
	static void Init()
	{
		EditorWindow.GetWindow(typeof(MegaWireWindow), false, "MegaWire");
	}

	void Awake()
	{
		arrowwidth = EditorPrefs.GetFloat("MArrowWidth", 0.2f);
		arrowlength = EditorPrefs.GetFloat("MArrowLength", 1.1f);
		vertStart = EditorPrefs.GetFloat("MArrowStart", 0.2f);
		vertLength = EditorPrefs.GetFloat("MArrowvLength", 1.5f);
		arrowoff = EditorPrefs.GetFloat("MArrowOff", 0.8f);
		dashdist = EditorPrefs.GetFloat("MArrowDashDist", 2.0f);
		arrowCol.r = EditorPrefs.GetFloat("MArrowColR", 1.0f);
		arrowCol.g = EditorPrefs.GetFloat("MArrowColG", 1.0f);
		arrowCol.b = EditorPrefs.GetFloat("MArrowColB", 1.0f);
		arrowCol.a = EditorPrefs.GetFloat("MArrowColA", 1.0f);


		lineCol.r = EditorPrefs.GetFloat("MArrowLineColR", 1.0f);
		lineCol.g = EditorPrefs.GetFloat("MArrowLineColG", 1.0f);
		lineCol.b = EditorPrefs.GetFloat("MArrowLineColB", 1.0f);
		lineCol.a = EditorPrefs.GetFloat("MArrowLineColA", 1.0f);

		otherCol.r = EditorPrefs.GetFloat("MArrowOtherColR", 0.75f);
		otherCol.g = EditorPrefs.GetFloat("MArrowOtherColG", 0.75f);
		otherCol.b = EditorPrefs.GetFloat("MArrowOtherColB", 0.75f);
		otherCol.a = EditorPrefs.GetFloat("MArrowOtherColA", 1.0f);

		dashCol.r = EditorPrefs.GetFloat("MArrowDashColR", 0.5f);
		dashCol.g = EditorPrefs.GetFloat("MArrowDashColG", 0.5f);
		dashCol.b = EditorPrefs.GetFloat("MArrowDashColB", 0.5f);
		dashCol.a = EditorPrefs.GetFloat("MArrowDashColA", 1.0f);

		units = (MegaWireUnits)EditorPrefs.GetInt("MArrowUnits", (int)MegaWireUnits.Meters);
		unitsScale = EditorPrefs.GetFloat("MArrowLength", 1.1f);
	}

	void OnDestroy()
	{
		EditorPrefs.SetFloat("MArrowWidth", arrowwidth);
		EditorPrefs.SetFloat("MArrowLength", arrowlength);
		EditorPrefs.SetFloat("MArrowStart", vertStart);
		EditorPrefs.SetFloat("MArrowvLength", vertLength);
		EditorPrefs.SetFloat("MArrowOff", arrowoff);
		EditorPrefs.SetFloat("MArrowDashDist", dashdist);
		EditorPrefs.SetFloat("MArrowColR", arrowCol.r);
		EditorPrefs.SetFloat("MArrowColG", arrowCol.g);
		EditorPrefs.SetFloat("MArrowColB", arrowCol.b);
		EditorPrefs.SetFloat("MArrowColA", arrowCol.a);

		EditorPrefs.SetFloat("MArrowLineColR", lineCol.r);
		EditorPrefs.SetFloat("MArrowLineColG", lineCol.g);
		EditorPrefs.SetFloat("MArrowLineColB", lineCol.b);
		EditorPrefs.SetFloat("MArrowLineColA", lineCol.a);

		EditorPrefs.SetFloat("MArrowOtherColR", otherCol.r);
		EditorPrefs.SetFloat("MArrowOtherColG", otherCol.g);
		EditorPrefs.SetFloat("MArrowOtherColB", otherCol.b);
		EditorPrefs.SetFloat("MArrowOtherColA", otherCol.a);

		EditorPrefs.SetFloat("MArrowDashColR", dashCol.r);
		EditorPrefs.SetFloat("MArrowDashColG", dashCol.g);
		EditorPrefs.SetFloat("MArrowDashColB", dashCol.b);
		EditorPrefs.SetFloat("MArrowDashColA", dashCol.a);

		EditorPrefs.SetInt("MArrowUnits", (int)units);
		EditorPrefs.SetFloat("MArrowLength", unitsScale);
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}

	void OnFocus()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	MegaArrow arrow = new MegaArrow();
	public bool showarrows = false;

	void OnSceneGUI(SceneView sceneView)
	{
		Handles.BeginGUI();

		arrow.arrowlength = arrowlength;
		arrow.arrowoff = arrowoff;
		arrow.arrowwidth = arrowwidth;
		arrow.vertStart = vertStart;
		arrow.vertLength = vertLength;
		arrow.lineCol = lineCol;
		arrow.otherCol = otherCol;
		arrow.arrowcol = arrowCol;
		arrow.dashCol = dashCol;
		arrow.units = MegaArrow.GetUnitsString(units);
		arrow.unitsscale = MegaArrow.GetUnitsScale(units, unitsScale);
		arrow.dashdist = dashdist;

		GUIStyle style = new GUIStyle();
		style.normal.textColor = lineCol;

		for ( int i = 0; i < selection.Count; i++ )
		{
			Handles.Label(selection[i].transform.position, " " + i);

			if ( i > 0 )
			{
				//Handles.DrawLine(selection[i - 1].transform.position, selection[i].transform.position);
				Vector3 p1 = selection[i - 1].transform.position;
				Vector3 p2 = selection[i].transform.position;
				arrow.DrawArrow(i, p1, p2, style);
			}
		}

		Handles.EndGUI();
	}

	void OnSelectionChange()
	{
		if ( picking )
		{
			if ( Selection.activeGameObject != null && Selection.activeGameObject != lastsel )
			{
				if ( !selection.Contains(Selection.activeGameObject) )
				{
					selection.Add(Selection.activeGameObject);
					lastsel = Selection.activeGameObject;
					Repaint();
				}
			}
		}
	}

	void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);

		if ( picking )
		{
			if ( GUILayout.Button("Stop Picking") )
				picking = false;
		}
		else
		{
			if ( GUILayout.Button("Start Picking") )
			{
				picking = true;
				lastsel = null;
				// Ask if clear or add

				if ( selection.Count > 0 )
				{
					bool opt = EditorUtility.DisplayDialog("Add or Replace", "Do you want to Add to or Replace selection", "Add", "Replace");

					if ( opt == false )
						selection.Clear();
				}
			}
		}

		// If we have a wire object selected then replace the selection
		if ( GUILayout.Button("Create Wire") )
		{
			picking = false;
			MegaWire wire = MegaWire.Create(null, selection, material, wirename, copyfrom, 1.0f, 1.0f);

			copyfrom = wire;
			selection.Clear();
		}

		wirename = EditorGUILayout.TextField("Wire Name", wirename);
		material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material), true);
		copyfrom = (MegaWire)EditorGUILayout.ObjectField("Copy From", copyfrom, typeof(MegaWire), true);

		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.LabelField("Current Selection");
		for ( int i = 0; i < selection.Count; i++ )
			EditorGUILayout.LabelField(i + ": " + selection[i].name);

		EditorGUILayout.EndVertical();

		showarrows = EditorGUILayout.Foldout(showarrows, "Arrow Params");

		if ( showarrows )
		{
			units = (MegaWireUnits)EditorGUILayout.EnumPopup("Units", units);
			unitsScale = EditorGUILayout.FloatField("Units Scale", unitsScale);

			arrowwidth = EditorGUILayout.FloatField("Arrow Width", arrowwidth);
			arrowlength = EditorGUILayout.FloatField("Arrow Length", arrowlength);
			arrowoff = EditorGUILayout.Slider("Arrow Offset", arrowoff, 0.0f, 1.0f);
			vertStart = EditorGUILayout.FloatField("Vert Start", vertStart);
			vertLength = EditorGUILayout.FloatField("Vert Length", vertLength);

			dashdist = EditorGUILayout.FloatField("Dash Dist", dashdist);
			lineCol = EditorGUILayout.ColorField("Line Color", lineCol);
			arrowCol = EditorGUILayout.ColorField("Arrow Color", arrowCol);
			otherCol = EditorGUILayout.ColorField("Other Color", otherCol);
			dashCol = EditorGUILayout.ColorField("Dash Color", dashCol);
		}

		EditorGUILayout.EndScrollView();
	}
}
// 202