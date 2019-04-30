using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jacovone.CameraTools
{
	/// <summary>
	/// Path magic follower. Attach this script on a PathMagic or PathMagicAnimator instance. The current
	/// pos of that instance will set frame by frame at a position (CurrentPos) that is the nearest position
	/// respect to the specified target.
	/// </summary>
	[ExecuteInEditMode]
	public class PathMagicFollower : MonoBehaviour
	{
		/// <summary>
		/// The path magic to run over if attached to a PathMagic instance.
		/// </summary>
		public PathMagic pathMagic;

		/// <summary>
		/// The path magic animator to run over if attached to a PathMagic animator instance.
		/// </summary>
		public PathMagicAnimator pathMagicAnimator;

		/// <summary>
		/// The target to follow.
		/// </summary>
		public Transform target;

		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public Transform Target {
			get {
				return target;
			}
			set {
				target = value;
			}
		}

		/// <summary>
		/// The precision (number of samples in which the path is subdived into).
		/// </summary>
		public int precision = 1000;

		/// <summary>
		/// Gets or sets the precision.
		/// </summary>
		/// <value>The precision.</value>
		public int Precision {
			get {
				return precision;
			}
			set {
				precision = value;
			}
		}

		/// <summary>
		/// Give more accurate results, but at cost of more CPU power
		/// </summary>
		public bool accurate = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagicFollower"/> is accurate.
		/// </summary>
		/// <value><c>true</c> if accurate; otherwise, <c>false</c>.</value>
		public bool Accurate {
			get {
				return accurate;
			}
			set {
				accurate = value;
			}
		}

		/// <summary>
		/// The waypoints only flag. If set, the animator will jump from a waypoint to another in the path.
		/// </summary>
		public bool waypointsOnly = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagicFollower"/> waypoints only.
		/// </summary>
		/// <value><c>true</c> if waypoints only; otherwise, <c>false</c>.</value>
		public bool WaypointsOnly {
			get {
				return waypointsOnly;
			}
			set {
				waypointsOnly = value;
			}
		}

		/// <summary>
		/// The lerp position. If set, the object will lerp nearest positions
		/// </summary>
		public bool lerpPosition = true;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagicFollower"/> lerp position.
		/// </summary>
		/// <value><c>true</c> if lerp position; otherwise, <c>false</c>.</value>
		public bool LerpPosition {
			get {
				return lerpPosition;
			}
			set {
				lerpPosition = value;
			}
		}

		/// <summary>
		/// The lerp factor. The currentPos of the connected PathMagic will Lerp to the correct.
		/// CurrentPos at this factor.
		/// </summary>
		public float lerpFactor = 0.1f;

		/// <summary>
		/// Gets or sets the lerp factor.
		/// </summary>
		/// <value>The lerp factor.</value>
		public float LerpFactor {
			get {
				return lerpFactor;
			}
			set {
				lerpFactor = value;
			}
		}

		/// <summary>
		/// Awake this instance. Get the connected instance of type PathMagic or PathMagicAnimator.
		/// </summary>
		void Awake ()
		{
			PathMagic pm = GetComponent<PathMagic> ();
			if (pm != null) {
				pathMagic = pm;
				pathMagicAnimator = null;
			} else {
				PathMagicAnimator pma = GetComponent<PathMagicAnimator> ();
				if (pma != null) {
					pathMagicAnimator = pma;
					pathMagic = null;
				}
			}
		}

		/// <summary>
		/// Raises the OnEnable event.
		/// </summary>
		void OnEnable ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				EditorApplication.update += Update;
			#endif
		}

		/// <summary>
		/// Raises the OnDisable event.
		/// </summary>
		void OnDisable ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				EditorApplication.update -= Update;
			#endif
		}

		/// <summary>
		/// Gets or sets the current position. The value is set or get from connected pathMagic
		/// or PathMagicAnimator instance.
		/// </summary>
		/// <value>The current position.</value>
		public float CurrentPos {
			get {
				if (pathMagic != null)
					return pathMagic.CurrentPos;
				else if (pathMagicAnimator != null)
					return pathMagicAnimator.CurrentPos;
				else
					return 0f;
			}
			set {
				if (pathMagic != null) {
					pathMagic.CurrentPos = value;
					pathMagic.UpdateTarget ();
				} else if (pathMagicAnimator != null) {
					pathMagicAnimator.CurrentPos = value;
					pathMagicAnimator.UpdateTarget ();
				}
			}
		}

		/// <summary>
		/// Gets the referenced path.
		/// </summary>
		/// <value>The path.</value>
		public PathMagic Path {
			get {
				if (pathMagic != null)
					return pathMagic;
				else if (pathMagicAnimator != null)
					return pathMagicAnimator.pathMagic;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the current point of view.
		/// </summary>
		/// <value>The point of view.</value>
		public Vector3 PointOfView {
			get {
				Vector3 position = Vector3.zero;
				Quaternion rotation = Quaternion.identity;
				float velocity = 1.0f;
				int waypoint = 0;

				if (Path.presampledPath) {
					Path.sampledPositionAndRotationAndVelocityAndWaypointAtPos (CurrentPos, out position, out rotation, out velocity, out waypoint);
				} else {
					position = Path.computePositionAtPos (CurrentPos);
				}

				return Path.transform.TransformPoint (position);
			}
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update ()
		{
			// Compute nearest position
			float lerpTo;
			if (waypointsOnly) {
				lerpTo = computeWaypointPosAtMinDistance ();
			} else {
				lerpTo = computePosAtMinDistance ();
			}

			if (Path.Loop) {
				// Prevent the position to turn around on path end/start
				if (Mathf.Abs (CurrentPos - lerpTo) > Mathf.Abs (CurrentPos - (1 + lerpTo)))
					lerpTo = 1 + lerpTo;

				if (Mathf.Abs (CurrentPos - lerpTo) > Mathf.Abs (CurrentPos - (lerpTo - 1)))
					lerpTo = lerpTo - 1;
			}

			float newPos;
			if (lerpPosition) {
				newPos = Mathf.Lerp (CurrentPos, lerpTo, lerpFactor);
			} else {
				newPos = lerpTo;
			}

			while (newPos > 1f)
				newPos -= 1;
			while (newPos < 0f)
				newPos += 1;

			// Set the new pos
			CurrentPos = newPos;
		}

		/// <summary>
		/// Computes the nearest waypoint position to the target.
		/// </summary>
		/// <returns>The waypoint position at minimum distance.</returns>
		private float computeWaypointPosAtMinDistance ()
		{
			float minDistance = float.MaxValue;
			float minPos = 0f;
			int minWaypoint = 0;
			float step = 1f / (float)precision;

			for (int i = 0; i < Path.Waypoints.Length; i++) {

				Vector3 p = Path.transform.TransformPoint (Path.Waypoints [i].position);
				float pDistance = Vector3.Distance (p, target.position);

				if (pDistance < minDistance) {
					minWaypoint = i;
					minDistance = pDistance;
				}
			}

			if (!Path.presampledPath) {
				// Compute the pos to the minWaypoint in non-pre-sampled path
				while (Path.GetWaypointFromPos (minPos) != minWaypoint)
					minPos += step;

				if (minPos > 1)
					minPos = 1;
			} else {
				// Compute the pos to the minWaypoint in pre-sampled path
				int i = 0;
				while (Path.WaypointSamples [i] != minWaypoint) {
					minPos += Path.SamplesDistances [i++];
				}

				minPos /= Path.TotalDistance;
			}

			return minPos;
		}

		/// <summary>
		/// Computes the position of the path at minimum distance from the target.
		/// </summary>
		/// <returns>The position at minimum distance.</returns>
		private float computePosAtMinDistance ()
		{
			if (target == null)
				return 0f;
			if (Path == null)
				return 0f;

			float minDistance = float.MaxValue;
			float step = 1f / (float)precision;
			float minPos = 0f;

			for (int i = 0; i < precision; i++) {

				Vector3 p = GetPositionForPos ((float)i * step);
				float pDistance = Vector3.Distance (p, target.position);

				if (pDistance < minDistance) {
					minPos = (float)i * step;
					minDistance = pDistance;
				}
			}

			// More accurate result
			if (accurate) {
				float foundPos = minPos;
				for (float f = foundPos - step; f < foundPos + step; f += (step / 100f)) {
					Vector3 p = GetPositionForPos (f);
					float pDistance = Vector3.Distance (p, target.position);
					if (pDistance < minDistance) {
						minPos = f;
						minDistance = pDistance;
					}
				}
			}

			return minPos;
		}

		/// <summary>
		/// Gets the position for position whether the path is pre-sampled or not.
		/// </summary>
		/// <returns>The position for position.</returns>
		/// <param name="pos">Position.</param>
		private Vector3 GetPositionForPos (float pos)
		{
			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			float velocity = 1.0f;
			int waypoint = 0;

			if (pos < 0)
			if (Path.loop)
				pos = 1 + pos;
			else
				pos = 0;
			
			if (Path.presampledPath) {
				Path.sampledPositionAndRotationAndVelocityAndWaypointAtPos (pos, out position, out rotation, out velocity, out waypoint);
			} else {
				position = Path.computePositionAtPos (pos);
			}

			return Path.transform.TransformPoint (position);
		}
	}
}
