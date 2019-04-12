using UnityEngine;
using System.Collections;

namespace Jacovone
{
	[RequireComponent (typeof(Rigidbody))]
	/// <summary>
	/// A Joint to constraint a rigidbody on a PathMagic Path. Simply add this component to
	/// a rigidbody and assign a PathMagic instance
	/// </summary>
	public class PathMagicJoint : MonoBehaviour
	{
		/// <summary>
		/// The connected path.
		/// </summary>
		public PathMagic connectedPath;

		/// <summary>
		/// The RigidBody cached component
		/// </summary>
		Rigidbody _rb;

		/// <summary>
		/// The Transform cached component
		/// </summary>
		Transform _tr;

		/// <summary>
		/// The precision (number of samples in which the path is subdived into).
		/// </summary>
		public int precision1 = 100;

		/// <summary>
		/// The precision (number of samples in which the path is subdived into) for the second sub-search.
		/// </summary>
		public int precision2 = 200;

		/// <summary>
		/// Gets or sets the precision1.
		/// </summary>
		/// <value>The precision1.</value>
		public int Precision1 {
			get {
				return precision1;
			}
			set {
				precision2 = value;
			}
		}

		/// <summary>
		/// Gets or sets the precision2.
		/// </summary>
		/// <value>The precision2.</value>
		public int Precision2 {
			get {
				return precision2;
			}
			set {
				precision2 = value;
			}
		}

		/// <summary>
		/// Follow the path orientation
		/// </summary>
		public bool followPathOrientation = false;

		/// <summary>
		/// Follow the path orientation Property
		/// </summary>
		/// <value><c>true</c> if accurate; otherwise, <c>false</c>.</value>
		public bool FollowPathOrientation {
			get {
				return followPathOrientation;
			}
			set {
				followPathOrientation = value;
			}
		}

		/// <summary>
		/// Apply a constant force along the path
		/// </summary>
		public bool motor = false;

		/// <summary>
		/// Apply a constant force along the path property
		/// </summary>
		/// <value><c>true</c> if accurate; otherwise, <c>false</c>.</value>
		public bool Motor {
			get {
				return motor;
			}
			set {
				motor = value;
			}
		}

		/// <summary>
		/// The amount of motor force
		/// </summary>
		public int motorForce = 10;

		/// <summary>
		/// The amount of motor force
		/// </summary>
		/// <value>The precision.</value>
		public int MotorForce {
			get {
				return motorForce;
			}
			set {
				motorForce = value;
			}
		}

		void Awake ()
		{
			_rb = GetComponent<Rigidbody> ();
			_tr = GetComponent<Transform> ();
		}

		/// <summary>
		/// Physics computation of the joint
		/// </summary>
		void FixedUpdate ()
		{
			if (connectedPath == null)
				return;

			// Re-align transform on path
			float minPos = computePosAtMinDistance ();
			_tr.position = GetPositionForPos (minPos);

			//Debug.Log (GetPositionForPos (minPos));
			//Debug.DrawLine (_tr.position, GetPositionForPos (minPos), Color.cyan);

			if (followPathOrientation) {
				// Align rotation to the path
				Vector3 position = Vector3.zero;
				Quaternion rotation = Quaternion.identity;
				float velocity = 1.0f;
				int waypoint = 0;

				if (connectedPath.presampledPath) {
					connectedPath.sampledPositionAndRotationAndVelocityAndWaypointAtPos (minPos, out position, out rotation, out velocity, out waypoint);
				} else {
					rotation = connectedPath.computeRotationAtPos (minPos);
				}

				if (!connectedPath.disableOrientation) {
					_tr.rotation = connectedPath.transform.rotation * rotation;
				}
			}

			// Computer direction vector
			Quaternion ffVq = connectedPath.GetFaceForwardForPos (minPos);
			Vector3 ffV = ffVq * Vector3.forward;
			ffV = connectedPath.transform.TransformVector (ffV);

			// Constraint the velocity to the path direction
			_rb.velocity = Vector3.Dot (_rb.velocity, ffV) * ffV;

			//Debug.Log (minPos);

			// Apply motor force
			if (motor) {
				_rb.AddForce (ffV * motorForce);
			}
		}

		/// <summary>
		/// Computes the position of the path at minimum distance from the target.
		/// </summary>
		/// <returns>The position at minimum distance.</returns>
		private float computePosAtMinDistance ()
		{
			if (connectedPath == null)
				return 0f;

			float minDistance = float.MaxValue;
			float step = 1f / (float)Precision1;
			float minPos = 0f;

			for (int i = 0; i < precision1; i++) {

				Vector3 p = GetPositionForPos ((float)i * step);
				float pDistance = Vector3.Distance (p, _tr.position);

				if (pDistance < minDistance) {
					minPos = (float)i * step;
					minDistance = pDistance;
				}
			}

			//Debug.DrawLine (_tr.position, GetPositionForPos (minPos - step), Color.red);
			//Debug.DrawLine (_tr.position, GetPositionForPos (minPos + step), Color.yellow);

			// Extra loop for more accurate result
			float foundPos = minPos;
			for (float f = foundPos - step; f < foundPos + step; f += (step / precision2)) {
				Vector3 p = GetPositionForPos (f);
				float pDistance = Vector3.Distance (p, _tr.position);
				if (pDistance < minDistance) {
					minPos = f;
					minDistance = pDistance;
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

			if (pos < 0)
			if (connectedPath.loop)
				pos = 1 + pos;
			else
				pos = 0;

			float velocity = 1.0f;
			int waypoint = 0;

			if (connectedPath.presampledPath) {
				connectedPath.sampledPositionAndRotationAndVelocityAndWaypointAtPos (pos, out position, out rotation, out velocity, out waypoint);
			} else {
				position = connectedPath.computePositionAtPos (pos);
			}

			return connectedPath.transform.TransformPoint (position);
		}
	}
}
