using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Jacovone
{
	/// <summary>
	/// This class implements an editor window that allows user to generate paths starting from
	/// parametric templates
	/// </summary>
	public class CreatePathEditorWindow : EditorWindow
	{
		/// <summary>
		/// Path template type.
		/// </summary>
		public enum PathTemplateType
		{
			Circular,
			Linear
		}

		/// <summary>
		/// Tangent mode type.
		/// </summary>
		public enum TangentMode
		{
			Tangent,
			InPath
		}

		/// <summary>
		/// The create gameobject.
		/// </summary>
		private GameObject _go = null;

		/// <summary>
		/// The template type.
		/// </summary>
		private PathTemplateType _type = PathTemplateType.Circular;

		/// <summary>
		/// The number of samples.
		/// </summary>
		private int _samples;

		/// <summary>
		/// The same XY radius. Only for circular templates
		/// </summary>
		private bool _sameXYRadius;

		/// <summary>
		/// The y advance. Used by circular path to create spirals
		/// </summary>
		private float _yAdvance;

		/// <summary>
		/// The number of cycles. Used by circular path to create spirals
		/// </summary>
		private float _periods;

		/// <summary>
		/// The X radius in case of circular paths, or the length in case of linear path
		/// </summary>
		private float _radiusX;

		/// <summary>
		/// The Y radius. Only for circular templates
		/// </summary>
		private float _radiusY;

		/// <summary>
		/// The start angle. Only for circular templates
		/// </summary>
		private float _startAngle;

		/// <summary>
		/// The end angle. Only for circular templates
		/// </summary>
		private float _endAngle;

		/// <summary>
		/// If set, the generated template path will be closed
		/// </summary>
		private bool _closed;

		/// <summary>
		/// The tangent mode.
		/// </summary>
		private TangentMode _tangentMode;

		/// <summary>
		/// The in tangent radius.
		/// </summary>
		private float _inTangentRadius;

		/// <summary>
		/// The out tangent radius.
		/// </summary>
		private float _outTangentRadius;

		/// <summary>
		/// If set, in and out tangents are forced to have the same radius.
		/// </summary>
		private bool _sameTangentRadius;

		/// <summary>
		/// The path magic logo. Loaded from resources.
		/// </summary>
		private Texture2D pathMagicLogo = null;

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		public void OnEnable ()
		{
			pathMagicLogo = Resources.Load ("pathmagiclogo") as Texture2D;
		}

		/// <summary>
		/// Initialize this instance. This is a menu command
		/// </summary>
		[MenuItem ("GameObject/PathMagic/Create new path from template")]
		static void Initialize ()
		{
			CreatePathEditorWindow window = (CreatePathEditorWindow)EditorWindow.GetWindow (typeof(CreatePathEditorWindow));

#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2
			window.titleContent = new GUIContent ("Template path");
#else
			window.title = "Template path";
#endif
			window.InitializePath ();
			window.Show ();
		}

		/// <summary>
		/// Initializes the new path.
		/// </summary>
		public void InitializePath ()
		{
			_type = PathTemplateType.Linear;
			_sameXYRadius = true;

			_yAdvance = 0f;
			_periods = 1f;

			_radiusX = 20f;
			_radiusY = 20f;
			_samples = 5;
			_closed = true;
			_startAngle = 0f;
			_endAngle = 360f;
			_tangentMode = TangentMode.Tangent;
			_inTangentRadius = .5f;
			_outTangentRadius = .5f;
			_sameTangentRadius = true;

			_go = null;
			CreateThePath ();
		}

		/// <summary>
		/// Raises the GUI event.
		/// </summary>
		void OnGUI ()
		{
			EditorGUILayout.LabelField (new GUIContent (pathMagicLogo), GUILayout.Width (142), GUILayout.Height (28));

			if (_go == null) {
				EditorGUILayout.HelpBox ("Create a new parameter path by pressing the New button", MessageType.Info);
			} else {
				EditorGUI.BeginChangeCheck ();

				EditorGUILayout.LabelField (new GUIContent ("General"));
				EditorGUILayout.BeginVertical ("Box");
				_type = (PathTemplateType)EditorGUILayout.EnumPopup (new GUIContent ("Path template", "Select a template path"), _type);

				_samples = EditorGUILayout.IntField (new GUIContent ("Samples", "Number of samples to generate"), _samples);

				if (_samples < 3)
					_samples = 3;
				_closed = EditorGUILayout.Toggle (new GUIContent ("Closed", "Generate a loop-ed path"), _closed);
				EditorGUILayout.EndVertical ();

				EditorGUILayout.LabelField ("Prperties");
				EditorGUILayout.BeginVertical ("Box");

				if (_type == PathTemplateType.Circular)
					_sameXYRadius = EditorGUILayout.Toggle (new GUIContent ("Same X/Y radius", "Circle have same X/Y radius?"), _sameXYRadius);

				if (_sameXYRadius || _type == PathTemplateType.Linear) {
					_radiusX = _radiusY = EditorGUILayout.FloatField (new GUIContent ("Radius", "The radius of the circle"), _radiusX);
				} else {
					_radiusX = EditorGUILayout.FloatField (new GUIContent ("Radius X", "The X radius of the circle"), _radiusX);
					_radiusY = EditorGUILayout.FloatField (new GUIContent ("Radius Y", "The Y radius of the circle"), _radiusY);
				}

				if (_type == PathTemplateType.Circular) {
					_yAdvance = EditorGUILayout.FloatField (new GUIContent ("Y Advance", "Eache waypoint Y is increased by this value (Spiral template)"), _yAdvance);
					_periods = EditorGUILayout.FloatField (new GUIContent ("Cycles", "Number of cycles around the circle"), _periods);

					EditorGUILayout.MinMaxSlider (new GUIContent ("Min/Max angles", "Start angle and end angle"), ref _startAngle, ref _endAngle, 0f, 360f);
					_tangentMode = (TangentMode)EditorGUILayout.EnumPopup (new GUIContent ("Tangents align", "Type of generated bezier tangents"), _tangentMode);
				}

				_sameTangentRadius = EditorGUILayout.Toggle (new GUIContent ("Same tangent radius", "Tangents are symmetric in radius?"), _sameTangentRadius);
				if (_sameTangentRadius) {
					_inTangentRadius = _outTangentRadius = EditorGUILayout.FloatField (new GUIContent ("Tangents radius", "In and out tangents radius"), _inTangentRadius);
				} else {
					_inTangentRadius = EditorGUILayout.FloatField (new GUIContent ("In tangent radius", "In tangent radius"), _inTangentRadius);
					_outTangentRadius = EditorGUILayout.FloatField (new GUIContent ("Out tangent radius", "Out tangent radius"), _outTangentRadius);
				}
				EditorGUILayout.EndVertical ();

				if (EditorGUI.EndChangeCheck ()) {
					CreateThePath ();
				}
			}

			if (GUILayout.Button (new GUIContent ("New", "Leaves current generated path (if there is one) and create a new one"))) {
				InitializePath ();
			}
		}

		/// <summary>
		/// Creates the path. The path is generated based on the path template type.
		/// Each time we create the path, reuse the same gamobject (_go) but destroy
		/// and create a new PathMagic instance (and attach it)
		/// </summary>
		private void CreateThePath ()
		{
			// Create gameobject if there is no one
			if (_go == null) {
				// Register the creation in the undo system
				_go = new GameObject ("PathMagic template");

				Undo.RegisterCreatedObjectUndo (_go, "Create " + _go.name);
				_go.transform.rotation = Quaternion.Euler (0f, 90f, 0f);
			}

			// CHeck for PathMagic component
			PathMagic pm = _go.GetComponent<PathMagic> ();
			if (pm == null) {
				pm = _go.AddComponent<PathMagic> ();
			}

			Selection.activeObject = _go;

			if (_type == PathTemplateType.Circular) {
				CreateCircularTemplatePath (pm);
			} else if (_type == PathTemplateType.Linear) {
				CreateLinearTemplatePath (pm);
			}

			SceneView.RepaintAll ();
		}

		/// <summary>
		/// Creates the circular template path.
		/// </summary>
		/// <param name="pm">Pm.</param>
		private void CreateCircularTemplatePath (PathMagic pm)
		{
			Waypoint[] wps = new Waypoint[_samples];

			float angleDistance = _endAngle - _startAngle;
			float yAdvance = 0f;

			float yIncrement = _yAdvance * Mathf.Deg2Rad * angleDistance / _samples;

			for (int i = 0; i < _samples; i++) {
				wps [i] = new Waypoint ();
				wps [i].position = new Vector3 (_radiusX * Mathf.Sin (Mathf.Deg2Rad * _startAngle + _periods * Mathf.Deg2Rad * angleDistance / _samples * i), 
					yAdvance, 
					_radiusY * Mathf.Cos (Mathf.Deg2Rad * _startAngle + _periods * Mathf.Deg2Rad * angleDistance / _samples * i));

				// Compute prev and next index
				int nextIndex = (i + 1) % _samples;
				int prevIndex = ((i - 1) < 0) ? (i + _samples - 1) : (i - 1);

				// Compute prev and next positions
				Vector3 nextPos = new Vector3 (_radiusX * Mathf.Sin (Mathf.Deg2Rad * _startAngle + _periods * Mathf.Deg2Rad * angleDistance / _samples * nextIndex), 
					                  yAdvance + yIncrement, 
					                  _radiusY * Mathf.Cos (Mathf.Deg2Rad * _startAngle + _periods * Mathf.Deg2Rad * angleDistance / _samples * nextIndex));
				Vector3 prevPos = new Vector3 (_radiusX * Mathf.Sin (Mathf.Deg2Rad * _startAngle + _periods * Mathf.Deg2Rad * angleDistance / _samples * prevIndex), 
					                  yAdvance - yIncrement, 
					                  _radiusY * Mathf.Cos (Mathf.Deg2Rad * _startAngle + _periods * Mathf.Deg2Rad * angleDistance / _samples * prevIndex));

				// At end of path clear yAdvance effect
				if (nextIndex == 0)
					nextPos.y = 0;

				// Tangent modes
				if (_tangentMode == TangentMode.InPath) {


					// In this case we create tangents oriented to other waypoints
					// This has a polygon effect
					wps [i].symmetricTangents = false;

					wps [i].inTangent = (prevPos - wps [i].position).normalized * _inTangentRadius;
					wps [i].outTangent = (nextPos - wps [i].position).normalized * _outTangentRadius;

				} else if (_tangentMode == TangentMode.Tangent) {

					// We want to compute tangents to create a circular effect
					// we compute the "normal" for points wp, center and wp + 1up
					wps [i].symmetricTangents = false;

					Vector3 side1 = Vector3.up;
					Vector3 side2 = -wps [i].position;

					wps [i].inTangent = _inTangentRadius * Vector3.Cross (side1, side2).normalized;
					wps [i].outTangent = _outTangentRadius * Vector3.Cross (side2, side1).normalized;

					// Now we consider effect of yAdvance to create spirals
					float dist;
					float xyDistance;
					float tangentAngle;

					// At begin and end of the spiral we want in/out tangents
					// symmetric with counterparts
					if (i > 0) {
						dist = Vector3.Distance (wps [i].position, prevPos);
						xyDistance = Mathf.Sqrt (dist * dist - yIncrement * yIncrement);
						tangentAngle = Mathf.Atan2 (xyDistance, yIncrement);

						wps [i].inTangent -= _inTangentRadius * Mathf.Cos (tangentAngle) * Vector3.up;
					}

					if (i < (_samples - 1)) {
						dist = Vector3.Distance (wps [i].position, nextPos);
						xyDistance = Mathf.Sqrt (dist * dist - yIncrement * yIncrement);
						tangentAngle = Mathf.Atan2 (xyDistance, yIncrement);

						wps [i].outTangent += _outTangentRadius * Mathf.Cos (tangentAngle) * Vector3.up;
					} else {
						wps [i].outTangent = -wps [i].inTangent;
					}

					if (i == 0)
						wps [i].inTangent = -wps [i].outTangent;
				}

				// Increment ascent
				yAdvance += yIncrement;
			}

			pm.loop = _closed;
			pm.waypoints = wps;
		}

		/// <summary>
		/// Creates the linear template path.
		/// </summary>
		/// <param name="pm">Pm.</param>
		private void CreateLinearTemplatePath (PathMagic pm)
		{
			Waypoint[] wps = new Waypoint[_samples];

			for (int i = 0; i < _samples; i++) {
				wps [i] = new Waypoint ();

				wps [i].position = new Vector3 (_radiusX / (_samples - 1) * i, 0, 0);

				wps [i].symmetricTangents = _sameTangentRadius;

				//int nextIndex = (i + 1) % _samples;
				int prevIndex = ((i - 1) < 0) ? (i + _samples - 1) : (i - 1);

				//Vector3 nextPos = new Vector3 (_radiusX / (_samples - 1) * nextIndex, 0, 0);
				Vector3 prevPos = new Vector3 (_radiusX / (_samples - 1) * prevIndex, 0, 0);

				wps [i].inTangent = (prevPos - wps [i].position).normalized * _inTangentRadius;
				wps [i].outTangent = -wps [i].inTangent; //(nextPos - wps [i].position).normalized * _outTangentRadius;
			}

			pm.loop = _closed;
			pm.waypoints = wps;
		}
	}
}