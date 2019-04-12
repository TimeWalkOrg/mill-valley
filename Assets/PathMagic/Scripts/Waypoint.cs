using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace Jacovone
{
	[Serializable]
	/// <summary>
/// Stores a complete waypoint.
/// </summary>
	public class Waypoint
	{
		[Serializable]
		/// <summary>
	///	The reached event is fired when current pos traverses this waypoint
	/// </summary>
	public class ReachedEvent : UnityEvent
		{
		}

		/// <summary>
		/// Type of interpolation from a velocity to a new one
		/// </summary>
		public enum VelocityVariation
		{
			Slow,
			Medium,
			Fast
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Waypoint"/> class.
		/// </summary>
		public Waypoint ()
		{
			position = Vector3.zero;
			rotation = Vector3.zero;
			velocity = 1f;
			outTangent = Vector3.forward;
			inTangent = -Vector3.forward;
			symmetricTangents = true;
			inVariation = VelocityVariation.Medium;
			outVariation = VelocityVariation.Medium;
			reached = null;
		}

		/// <summary>
		/// The waypoint position. Do not use this field directly, instead use related property.
		/// </summary>
		public Vector3
			position;

		/// <summary>
		/// Gets or sets the position of the waypoint.
		/// </summary>
		/// <value>The position.</value>
		public Vector3 Position {
			get {
				return position;
			}
			set {
				position = value;
			}
		}

		/// <summary>
		/// The rotation in euler angles. Do not use this field directly, instead use related property.
		/// </summary>
		public Vector3
			rotation;

		/// <summary>
		/// Gets or sets the rotation of the waypoint. This is stored as euler angles of local rotation.
		/// To obtain the object rotation you have to multiply this (Quaternion) by the PathMagic instance rotation.
		/// </summary>
		/// <value>The local rotation.</value>
		public Vector3 Rotation {
			get {
				return rotation;
			}
			set {
				rotation = value;
			}
		}

		/// <summary>
		/// If this is defined, the rotation at this waypoint is computed to face the target. 
		/// Do not use this field directly, instead use related property.
		/// </summary>
		public Transform
			lookAt;

		/// <summary>
		/// Gets or sets the look at. If this is defined, the rotation at this waypoint is computed to face the target.
		/// </summary>
		/// <value>The look at.</value>
		public Transform LookAt {
			get {
				return lookAt;
			}
			set {
				lookAt = value;
			}
		}

		/// <summary>
		/// The input bezier tangent. Do not use this field directly, instead use related property.
		/// </summary>
		public Vector3
			inTangent;

		/// <summary>
		/// Gets or sets the input bezier tangent.
		/// </summary>
		/// <value>The in tangent.</value>
		public Vector3 InTangent {
			get {
				return inTangent;
			}
			set {
				inTangent = value;
				if (symmetricTangents)
					outTangent = -inTangent;
			}
		}

		/// <summary>
		/// The output bezier tangent. Do not use this field directly, instead use related property.
		/// </summary>
		public Vector3
			outTangent;

		/// <summary>
		/// Gets or sets the output bezier tangent.
		/// </summary>
		/// <value>The out tangent.</value>
		public Vector3 OutTangent {
			get {
				return outTangent;
			}
			set {
				outTangent = value;
				if (symmetricTangents)
					inTangent = -outTangent;
			}
		}

		/// <summary>
		/// The in and out tangents are symmetric?. Do not use this field directly, instead use related property.
		/// </summary>
		public bool symmetricTangents;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Jacovone.Waypoint"/> have symmetric tangents.
		/// </summary>
		/// <value><c>true</c> if symmetric tangents; otherwise, <c>false</c>.</value>
		public bool SymmetricTangents {
			get {
				return symmetricTangents;
			}
			set {
				symmetricTangents = value;
			}
		}

		/// <summary>
		/// The waypoint velocity. Do not use this field directly, instead use related property.
		/// </summary>
		public float
			velocity;

		/// <summary>
		/// The in variation. The velocity at which interpolator reaches the waypoint velocity.
		/// Do not use this field directly, instead use related property.
		/// </summary>
		public VelocityVariation inVariation;

		/// <summary>
		/// Gets or sets the in variation. The velocity at which interpolator reaches the waypoint velocity.
		/// </summary>
		/// <value>The in variation.</value>
		public VelocityVariation InVariation {
			get {
				return inVariation;
			}
			set {
				inVariation = value;
			}
		}

		/// <summary>
		/// The out variation. The velocity at which interpolator leaves the waypoint velocity.
		/// Do not use this field directly, instead use related property.
		/// </summary>
		public VelocityVariation outVariation;

		/// <summary>
		/// Gets or sets the out variation. The velocity at which interpolator leaves the waypoint velocity.
		/// </summary>
		/// <value>The in variation.</value>
		public VelocityVariation OutVariation {
			get {
				return outVariation;
			}
			set {
				outVariation = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative velocity in this waypoint.
		/// </summary>
		/// <value>The velocity.</value>
		public float Velocity {
			get {
				return velocity;
			}
			set {
				velocity = value;
			}
		}

		/// <summary>
		/// The reached event. Fired when the current pos traverses this waypoint.
		/// Do not use this field directly, instead use related property.
		/// </summary>
		public ReachedEvent reached;

		/// <summary>
		/// Gets or sets the reached event handler fired when the current pos traverses this waypoint.
		/// </summary>
		/// <value>The reached.</value>
		public ReachedEvent Reached {
			get {
				return reached;
			}
			set {
				reached = value;
			}
		}
	}
}