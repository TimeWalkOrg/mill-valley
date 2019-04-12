using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jacovone
{
    #if UNITY_EDITOR
    using UnityEditor;

    [ExecuteInEditMode]
    [InitializeOnLoad]
    #endif

		/// <summary>
		/// PathMagic class. Stores an entire path.
		/// </summary>
	public class PathMagic : MonoBehaviour
    {
        [Serializable]
        /// <summary>
	///	The change waypoint event is fired when current pos traverses a waypoint
	/// </summary>
		public class WaypointChangedEvent : UnityEvent<int>
        {
        }

        /// <summary>
        /// Defines when actually move and rotate the target
        ///</summary>
        public enum UpdateModeType
        {
            OnUpdate,
            OnFixedUpdate
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
        /// The color of the path in the editor.
        /// </summary>
        public Color pathColor = Color.white;

        /// <summary>
        /// The waypoints that defines the path. Do not use this field directly, instead use related property.
        /// </summary>
        public Waypoint[] waypoints = new Waypoint[]{ };

        /// <summary>
        /// Gets or sets the waypoints that defines the path.
        /// </summary>
        /// <value>The waypoints.</value>
        public Waypoint[] Waypoints {
            get {
                return waypoints;
            }
            set {
                waypoints = value;
                if (presampledPath)
                    UpdatePathSamples ();
                UpdateTarget ();
            }
        }

        /// <summary>
        /// The target that will follow the path. Do not use this field directly, instead use related property.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Gets or sets the target that will follow the path.
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
        /// The disable orientation. Is set the path don't rotate the target. Do not use this field directly, instead use related property.
        /// </summary>
        public bool disableOrientation = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagic"/> disable orientation.
        /// </summary>
        /// <value><c>true</c> if disable orientation; otherwise, <c>false</c>.</value>
        public bool DisableOrientation {
            get {
                return disableOrientation;
            }
            set {
                disableOrientation = value;
            }
        }

        /// <summary>
        /// The disable position. Is set the path don't move the target. Do not use this field directly, instead use related property.
        /// </summary>
        public bool disablePosition = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagic"/> disable position.
        /// </summary>
        /// <value><c>true</c> if disable position; otherwise, <c>false</c>.</value>
        public bool DisablePosition {
            get {
                return disablePosition;
            }
            set {
                disablePosition = value;
            }
        }

        /// <summary>
        /// If the global look at is set, the target will look at that each frame. Do not use this field directly, instead use related property.
        /// </summary>
        public Transform globalLookAt = null;

        /// <summary>
        /// Gets or sets the global look at. If the global look at is set, the target will look at that each frame.
        /// </summary>
        /// <value>The global look at.</value>
        public Transform GlobalLookAt {
            get {
                return globalLookAt;
            }
            set {
                globalLookAt = value;
            }
        }

        /// <summary>
        /// If enabled, the target will follow the path in direction. Do not use this field directly, instead use related property.
        /// </summary>
        public bool globalFollowPath = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagic"/> 
        /// follows path globally (at each frame).
        /// </summary>
        /// <value><c>true</c> if global follow path; otherwise, <c>false</c>.</value>
        public bool GlobalFollowPath {
            get {
                return globalFollowPath;
            }
            set {
                globalFollowPath = value;
            }
        }

        /// <summary>
        /// The global follow path bias. Smallest values give more precise follow, while bigger values give more soft follow.
        /// User greater values for camera movement. Do not use this field directly, instead use related property.
        /// </summary>
        public float globalFollowPathBias = 0.001f;

        /// <summary>
        /// Gets or sets the global follow path bias. Smallest values give more precise follow, while bigger values give more soft follow.
        /// User greater values for camera movement.
        /// </summary>
        /// <value>The global follow path bias.</value>
        public float GlobalFollowPathBias {
            get {
                return globalFollowPathBias;
            }
            set {
                globalFollowPathBias = value;
            }
        }

        /// <summary>
        /// The update mode. By setting this to OnUpdate or OnFixedUpdate you can control
        /// when the target is actually moved and rotated by the animator. Do not use this field directly, instead use related property.
        /// </summary>
        public UpdateModeType updateMode = UpdateModeType.OnUpdate;

        /// <summary>
        /// Gets or sets the update mode. By setting this to OnUpdate or OnFixedUpdate you can control
        /// when the target is actually moved and rotated by the animator.
        /// </summary>
        /// <value>The update mode.</value>
        public UpdateModeType UpdateMode {
            get {
                return updateMode;
            }
            set {
                updateMode = value;
            }
        }

        /// <summary>
        /// Stop the animator at the end of the path or the path is cyclic? Do not use this field directly, instead use related property.
        /// </summary>
        public bool loop = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagic"/> is loopin.
        /// Stop the animator at the end of the path or the path is cyclic?
        /// </summary>
        /// <value><c>true</c> if th epath is looping; otherwise, <c>false</c>.</value>
        public bool Loop {
            get {
                return loop;
            }
            set {
                loop = value;
            }
        }

        /// <summary>
        /// Start automatically the animation when the player starts? Do not use this field directly, instead use related property.
        /// </summary>
        public bool autoStart = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagic"/> auto start.
        /// Start automatically the animation when the player starts?
        /// </summary>
        /// <value><c>true</c> if the path auto starts; otherwise, <c>false</c>.</value>
        public bool AutoStart {
            get {
                return autoStart;
            }
            set {
                autoStart = value;
            }
        }

        /// <summary>
        /// Bias correction of the global path velocity. Do not use this field directly, instead use related property.
        /// </summary>
        public float velocityBias = .1f;

        /// <summary>
        /// Gets or sets the velocity bias. Bias correction of the global path velocity.
        /// </summary>
        /// <value>The velocity bias.</value>
        public float VelocityBias {
            get {
                return velocityBias;
            }
            set {
                velocityBias = value;
            }
        }

        /// <summary>
        /// Stores the current animation position from 0 to 1. Do not use this field directly, instead use related property.
        /// </summary>
        public float currentPos;

        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>The current position.</value>
        public float CurrentPos {
            get {
                return currentPos;
            }
            set {
                currentPos = value;
                UpdateTarget ();
            }
        }

        /// <summary>
        /// Is the animator actually playing? Do not use this field directly, instead use related property.
        /// </summary>
        public bool isPlaying;

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
        public bool IsPlaying {
            get {
                return isPlaying;
            }
        }

        /// <summary>
        /// The waypoint changed. Fired when the current "last waypoint" has changed. Do not use this field directly, instead use related property.
        /// </summary>
        public WaypointChangedEvent waypointChanged;

        /// <summary>
        /// Gets or sets the waypoint changed event handler.
        /// </summary>
        /// <value>The waypoint changed.</value>
        public WaypointChangedEvent WaypointChanged {
            get {
                return waypointChanged;
            }
            set {
                waypointChanged = value;
            }
        }

        /// <summary>
        /// If true, the path is sampled and animation go through samples, this is a tool to walk on the path at a constant velocity.
        /// Do not use this field directly, instead use related property.
        /// </summary>
        public bool presampledPath = false;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Jacovone.PathMagic"/> presampled path. If true, the path 
        /// is sampled and animation go through samples, this is a tool to walk on the path at a constant velocity
        /// </summary>
        /// <value><c>true</c> if presampled path; otherwise, <c>false</c>.</value>
        public bool PresampledPath {
            get {
                return presampledPath;
            }
        }

        /// <summary>
        /// The samples number at which the path will be sampled. A great number of samples give a better precision of animation.
        /// Do not use this field directly, instead use related property.
        /// </summary>
        public int samplesNum = 100;

        /// <summary>
        /// Gets the samples number. The samples number at which the path will be sampled. 
        /// A great number of samples give a better precision of animation
        /// </summary>
        /// <value>The samples number.</value>
        public int SamplesNum {
            get {
                return samplesNum;
            }
        }

        /// <summary>
        /// The last passed wayponint, for event management. Do not use this field directly, instead use related property.
        /// </summary>
        private int _lastPassedWayponint;

        /// <summary>
        /// Gets the last passed wayponint index.
        /// </summary>
        /// <value>The last passed wayponint index.</value>
        public int LastPassedWayponint {
            get {
                return _lastPassedWayponint;
            }
        }

        /// <summary>
        /// The position samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs. Do not use this field directly, instead use related property.
        /// </summary>
        public Vector3[] positionSamples = null;

        /// <summary>
        /// Gets the position samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs.
        /// </summary>
        /// <value>The position samples.</value>
        public Vector3[] PositionSamples {
            get {
                return positionSamples;
            }
        }

        /// <summary>
        /// The rotation samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs. Do not use this field directly, instead use related property.
        /// </summary>
        public Quaternion[] rotationSamples = null;

        /// <summary>
        /// Gets the rotation samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs.
        /// </summary>
        /// <value>The rotation samples.</value>
        public Quaternion[] RotationSamples {
            get {
                return rotationSamples;
            }
        }

        /// <summary>
        /// The velocity samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs. Do not use this field directly, instead use related property.
        /// </summary>
        public float[] velocitySamples = null;

        /// <summary>
        /// Gets the velocity samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs.
        /// </summary>
        /// <value>The velocity samples.</value>
        public float[] VelocitySamples {
            get {
                return velocitySamples;
            }
        }

        /// <summary>
        /// The waypoint samples for event management. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs. Do not use this field directly, instead use related property.
        /// </summary>
        public int[] waypointSamples = null;

        /// <summary>
        /// Gets the waypoint samples. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs.
        /// </summary>
        /// <value>The waypoint samples.</value>
        public int[] WaypointSamples {
            get {
                return waypointSamples;
            }
        }

        /// <summary>
        /// The samples distances between waypoints. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs. Do not use this field directly, instead use related property.
        /// </summary>
        public float[] samplesDistances = null;

        /// <summary>
        /// Gets the samples distances. Sampling enable constant speed velocity along the entire path and a more precise normal position control.
        /// If the samples number is too low, the path results a bad approximation, if the samples number is too high, a performance 
        /// problem may occurs.
        /// </summary>
        /// <value>The samples distances.</value>
        public float[] SamplesDistances {
            get {
                return samplesDistances;
            }
        }

        /// <summary>
        /// The total distance done by the path. This variable is computed only if the path is pre-sampled.
        /// Do not use this field directly, instead use related property.
        /// </summary>
        public float totalDistance = 0;

        /// <summary>
        /// Gets the total distance of the path in world units. This value is defined only for pre-sampled paths.
        /// </summary>
        /// <value>The total distance.</value>
        public float TotalDistance {
            get {
                return totalDistance;
            }
        }

        /// <summary>
        /// Update or not the transform during animation? Do not use this field directly, instead use related property.
        /// </summary>
        public bool updateTransform = true;

        /// <summary>
        /// Gets or sets a value indicating if update the connected transform during animation.
        /// </summary>
        /// <value><c>true</c> if update transform; otherwise, <c>false</c>.</value>
        public bool UpdateTransform {
            get {
                return updateTransform;
            }
            set {
                updateTransform = value;
            }
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start ()
        {
            if (presampledPath)
                UpdatePathSamples ();

            _lastPassedWayponint = -1;

            if (!IsPlaying) { // Thanks to Mats Nielsen
                if (Application.isPlaying) {
                    isPlaying = autoStart;
                } else {
                    isPlaying = false;
                }
            }
        }

        /// <summary>
        /// Update for this frame. Calls DoUpdate() if PathMagic is in OnUpdate mode
        /// </summary>
        void Update ()
        {
            if (updateMode == UpdateModeType.OnUpdate || !Application.isPlaying) {
                DoUpdate ();
            }	
        }

        /// <summary>
        /// The main FixedUpdate function to update physics. Calls DoUpdate() if PathMagic is in OnFixedUpdate mode
        /// </summary>
        void FixedUpdate ()
        {
            if (updateMode == UpdateModeType.OnFixedUpdate) {
                DoUpdate ();
            }	
        }

        /// <summary>
        /// A simple cache of velocity at previous animation step. We need the velocity to know
        /// how much advace, but velocity is computed together with position and rotation in case
        /// of pre-sampled path. So cache the last velocity.
        /// </summary>
        private float _lastVelocity = 1.0f;

        /// <summary>
        /// Update this instance by advance cursor position.
        /// </summary>
        void DoUpdate ()
        {
            if (waypoints.Length == 0)
                return;

            if (isPlaying) {

                float advance = (0.5f * velocityBias * _lastVelocity *
					// In edit mode update is called at very low FPS, so tweak it!
                                ((Application.isPlaying) ? Time.deltaTime : 0.008f));

                // Advance
                currentPos += advance;

                if (currentPos >= 1f) {
                    if (loop) {
                        currentPos -= 1f;
                    } else {
                        currentPos = 1f;
                        Pause (); // Thanks to Leon
                    }
                } else if (currentPos <= 0f) {
                    if (loop) {
                        currentPos += 1f;
                    } else {
                        currentPos = 0f;
                        Pause (); // Thanks to Leon
                    }
                }

                if (UpdateTransform || Application.isPlaying)
                    UpdateTarget ();
            }
        }

        /// <summary>
        /// Play this instance of path.
        /// </summary>
        public void Play ()
        {
            if (waypoints.Length == 0)
                return;
            //_lastVelocity = waypoints [computeVelocityAtPos (currentPos)].velocity;
            isPlaying = true;
        }

        /// <summary>
        /// Pause this instance of path, don't modify <see cref="currentPos"/>.
        /// </summary>
        public void Pause ()
        {
            if (waypoints.Length == 0)
                return;
            isPlaying = false;
        }

        /// <summary>
        /// Sets <see cref="currentPos"/> to 0 but don't stop playing.
        /// </summary>
        public void Rewind ()
        {
            if (waypoints.Length == 0)
                return;
            currentPos = 0f;
        }

        /// <summary>
        /// Stop this instance ans sets <see cref="currentPos"/> to 0.
        /// </summary>
        public void Stop ()
        {
            if (waypoints.Length == 0)
                return;
            isPlaying = false;
            currentPos = 0f;
		
            UpdateTarget (computePositionAtPos (currentPos), computeRotationAtPos (currentPos));
        }

        /// <summary>
        /// Returns the pre-sampled position, rotation, velocity and reference waypoint at specified position.
        /// </summary>
        /// <param name="pos">The request normalized position on the path (0..1).</param>
        /// <param name="position">The result sampled position on the path.</param>
        /// <param name="rotation">The result sampled rotation on the path.</param>
        /// <param name="velocity">The reference sampled velocity on the path.</param>
        /// <param name="waypoint">The reference sampled waypoint on the path.</param>
        public void sampledPositionAndRotationAndVelocityAndWaypointAtPos (float pos, out Vector3 position, out Quaternion rotation, out float velocity, out int waypoint)
        {
            float refDistance = pos * totalDistance;

            float d = 0f;
            for (int i = 1; i < samplesNum; i++) {
                d += samplesDistances [i];
                if (d >= refDistance) {
                    float interpFactor = 1f - (d - refDistance) / samplesDistances [i];
                    position = Vector3.Lerp (positionSamples [i - 1], positionSamples [i], interpFactor);

                    if (globalLookAt != null) {
                        rotation = Quaternion.LookRotation (transform.InverseTransformPoint (globalLookAt.position) - position);
                    } else {
                        rotation = Quaternion.Lerp (rotationSamples [i - 1], rotationSamples [i], interpFactor);
                    }

                    velocity = Mathf.Lerp (velocitySamples [i - 1], velocitySamples [i], interpFactor);

                    if (pos >= 1) {
                        if (loop)
                            waypoint = 0;
                        else
                            waypoint = waypoints.Length - 1;
                    } else {

                        waypoint = waypointSamples [i - 1];
                    }

                    return;
                }
            }

            position = positionSamples [samplesNum - 1];
            rotation = rotationSamples [samplesNum - 1];
            velocity = velocitySamples [samplesNum - 1];
            waypoint = waypoints.Length - 1;
        }

        /// <summary>
        /// Computes the rotation at specific position [0..1].
        /// </summary>
        /// <returns>The rotation at position.</returns>
        /// <param name="pos">The animation position</param>
        public Quaternion computeRotationAtPos (float pos)
        {
            // In case of global look at, we return a fixed look at
            if (globalFollowPath) {
                return GetFaceForwardForPos (pos);
            } else if (globalLookAt != null) {
                return Quaternion.LookRotation (transform.InverseTransformPoint (globalLookAt.position) - computePositionAtPos (pos));
            }

            if (waypoints.Length < 1)
                return Quaternion.identity;
            else if (waypoints.Length == 1)
                return GetWaypointRotation (0);
            else {
		
                if (pos >= 1) {
                    if (loop) {
                        return GetWaypointRotation (0);
                    } else {
                        return GetWaypointRotation (waypoints.Length - 1);
                    }
                }

                float step = 1f / (float)(waypoints.Length - (loop ? 0 : 1));
                int posWaypoint = GetWaypointFromPos (pos);
                float posOffset = pos - (posWaypoint * step);
                float stepPos = posOffset / step;		
			
                Quaternion p = GetWaypointRotation (posWaypoint);
                Quaternion prevP = GetWaypointRotation (posWaypoint - 1);
                Quaternion nextP = GetWaypointRotation (posWaypoint + 1);
                Quaternion nextNextP = GetWaypointRotation (posWaypoint + 2);
			
                return MathUtils.QuaternionBezier (p, prevP, nextP, nextNextP, stepPos);
            }
        }

        /// <summary>
        /// Pick a rotation from waypoints by checking if the specific waypoint
        /// is facing to a transform or has its own rotation.
        /// </summary>
        /// <returns>The waypoint rotation.</returns>
        /// <param name="index">Index.</param>
        public Quaternion GetWaypointRotation (int index)
        {
            if (index < 0)
                index += (waypoints.Length);
            index %= waypoints.Length;
            return (waypoints [index].lookAt != null ? Quaternion.LookRotation (transform.InverseTransformPoint (waypoints [index].lookAt.position) - waypoints [index].position) : Quaternion.Euler (waypoints [index].rotation));
        }

        /// <summary>
        /// Return the exact interpolated position for a specific animation time [0..1].
        /// </summary>
        /// <returns>The position at animation position [0..1].</returns>
        /// <param name="pos">The animation position [0..1].</param>
        public Vector3 computePositionAtPos (float pos)
        {
            if (waypoints.Length < 1)
                return Vector3.zero;
            else if (waypoints.Length == 1)
                return waypoints [0].position;
            else if (waypoints.Length >= 1 && pos == 0)
                return waypoints [0].position;
            else {

                if (pos >= 1) {
                    if (loop) {
                        return waypoints [0].position;
                    } else {
                        return waypoints [waypoints.Length - 1].position;
                    }
                }
			
                float step = 1f / (float)(waypoints.Length - (loop ? 0 : 1));
                int posWaypoint = GetWaypointFromPos (pos);
                float posOffset = pos - (posWaypoint * step);
                float stepPos = posOffset / step;		
			
                return MathUtils.Vector3Bezier (
                    waypoints [(posWaypoint) % (waypoints.Length)].position, 
                    waypoints [(posWaypoint) % (waypoints.Length)].outTangent + waypoints [(posWaypoint) % (waypoints.Length)].position, 
                    waypoints [(posWaypoint + 1) % (waypoints.Length)].inTangent + waypoints [(posWaypoint + 1) % (waypoints.Length)].position, 
                    waypoints [(posWaypoint + 1) % (waypoints.Length)].position,
                    stepPos);
            }
        }

        /// <summary>
        /// Computes the interpolated velocity at animator position [0..1].
        /// </summary>
        /// <returns>The velocity at position.</returns>
        /// <param name="pos">Animator position [0..1].</param>
        public float computeVelocityAtPos (float pos)
        {
            if (waypoints.Length < 1)
                return 1;
            else if (waypoints.Length == 1)
                return waypoints [0].velocity;
            else {
                if (pos >= 1) {
                    if (loop)
                        return waypoints [0].velocity;
                    else
                        return waypoints [waypoints.Length - 1].velocity;
                }

                float step = 1f / (float)(waypoints.Length - (loop ? 0 : 1));
                int posWaypoint = GetWaypointFromPos (pos);
                float posOffset = pos - (posWaypoint * step);
                float stepPos = posOffset / step;		

                Waypoint wp1 = waypoints [(posWaypoint) % (waypoints.Length)];
                Waypoint wp2 = waypoints [(posWaypoint + 1) % (waypoints.Length)];

                float control1;
                if (wp1.outVariation == Waypoint.VelocityVariation.Fast)
                    control1 = wp2.velocity;
                else if (wp1.outVariation == Waypoint.VelocityVariation.Medium)
                    control1 = Mathf.Lerp (wp1.velocity, wp2.velocity, 0.5f);
                else
                    control1 = wp1.velocity;

                float control2;
                if (wp2.inVariation == Waypoint.VelocityVariation.Fast)
                    control2 = wp1.velocity;
                else if (wp2.inVariation == Waypoint.VelocityVariation.Medium)
                    control2 = Mathf.Lerp (wp1.velocity, wp2.velocity, 0.5f);
                else
                    control2 = wp2.velocity;

                return MathUtils.FloatBezier (
                    wp1.velocity, 
                    control1, 
                    control2, 
                    wp2.velocity,
                    stepPos);
            }
        }

        /// <summary>
        /// Updates the target is the users has specified one.
        /// </summary>
        /// <param name="position">Position to set</param>
        /// <param name="rotation">Rotation to set</param>
        public void UpdateTarget (Vector3 position, Quaternion rotation)
        {
            if (target != null) {

                if (!disablePosition) {
                    target.position = transform.TransformPoint (position);
                }

                if (!disableOrientation) {
                    target.rotation = transform.rotation * rotation;
                }
            }
        }

        /// <summary>
        /// Updates the target using current stored currentPos.
        /// </summary>
        public void UpdateTarget ()
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float velocity = 1.0f;
            int waypoint = 0;

            if (presampledPath) {
                sampledPositionAndRotationAndVelocityAndWaypointAtPos (currentPos, out position, out rotation, out velocity, out waypoint);
            } else {
                position = computePositionAtPos (currentPos);
                rotation = computeRotationAtPos (currentPos);
                velocity = computeVelocityAtPos (currentPos);
                waypoint = GetWaypointFromPos (currentPos);
            }

            _lastVelocity = velocity;

            UpdateTarget (position, rotation);

            // Fire waypointChanged if is the case
            if (waypoint != _lastPassedWayponint) {
                if (waypointChanged != null)
                    waypointChanged.Invoke (waypoint);
                if (waypoints [waypoint].reached != null)
                    waypoints [waypoint].reached.Invoke ();
            }

            _lastPassedWayponint = waypoint;

            #if UNITY_EDITOR
            if (target == null)
                SceneView.RepaintAll ();
            #endif
        }

        /// <summary>
        /// Gets the current waypoint. The current Waypoint is defines as the last waypoint
        /// the animated target has passed.
        /// </summary>
        /// <returns>The current waypoint.</returns>
        public int GetCurrentWaypoint ()
        {
            return GetWaypointFromPos (currentPos);
        }

        /// <summary>
        /// Waypoints from position. Return the waypoint related to a specified position
        /// </summary>
        /// <returns>The waypoint relative to pos.</returns>
        /// <param name="pos">Position.</param>
        public int GetWaypointFromPos (float pos)
        {
            float step = 1f / (float)(waypoints.Length - (loop ? 0 : 1));
            int wp = (Mathf.FloorToInt (pos / step)) % (waypoints.Length);	
            if (wp < 0)
                wp += waypoints.Length;
            return wp;
        }

        /// <summary>
        /// Calculates the orientation that the object should have when reaches the specified pos, to naturally follow the path (face forward).
        /// </summary>
        /// <returns>The face forward orientation for index.</returns>
        /// <param name="index">The pos at which calculate the face forward orientation</param>
        public Quaternion GetFaceForwardForPos (float pos)
        {
            Quaternion rot;
            if (waypoints.Length <= 1)
                rot = Quaternion.identity;
            else {
                if (loop) {

                    if (pos <= 0)
                        pos = 1 + pos;

                    Vector3 p1 = computePositionAtPos ((pos + globalFollowPathBias) % 1f);
                    Vector3 p2 = computePositionAtPos (pos);

                    rot = Quaternion.LookRotation (p1 - p2, Vector3.up);
                } else {

                    float step = Mathf.Clamp01 (pos + globalFollowPathBias);

                    Vector3 p1 = computePositionAtPos (step);
                    Vector3 p2 = computePositionAtPos (pos);

                    if (p1 == p2) {
                        p1 = waypoints [waypoints.Length - 1].outTangent;
                        p2 = waypoints [waypoints.Length - 1].inTangent;					
                    }

                    rot = Quaternion.LookRotation (p1 - p2, Vector3.up);
                }
            }

            return rot;
        }

        /// <summary>
        /// Updates the path samples. Pre-compute all path positions, rotations, velocities and reference (source) waypoint. 
        /// Keep track also of distance of path at predetermined fixed step of approximation. This enable the fixed velocity on the path
        /// and a more precise control of the relation between normalized pos (0..1) and effective position on the path.
        /// </summary>
        public void UpdatePathSamples ()
        {
            totalDistance = 0f;
            float curPos = 0f;

            positionSamples = new Vector3[samplesNum];
            rotationSamples = new Quaternion[samplesNum];
            samplesDistances = new float[samplesNum];
            velocitySamples = new float[samplesNum];
            waypointSamples = new int[samplesNum];

            if (waypoints.Length == 0)
                return;

            for (int i = 0; i < samplesNum - 1; i++) {

                positionSamples [i] = computePositionAtPos (curPos);
                rotationSamples [i] = computeRotationAtPos (curPos);
                velocitySamples [i] = computeVelocityAtPos (curPos);
                waypointSamples [i] = GetWaypointFromPos (curPos);

                if (i == 0)
                    samplesDistances [i] = 0;
                else
                    samplesDistances [i] = Vector3.Distance (positionSamples [i], positionSamples [i - 1]);

                // increment total distance;
                totalDistance += samplesDistances [i];

                // increment pos
                curPos += (1f / ((float)samplesNum - 1));
            }

            positionSamples [samplesNum - 1] = computePositionAtPos (loop ? 0f : 1f);
            rotationSamples [samplesNum - 1] = computeRotationAtPos (loop ? 0f : 1f);
            velocitySamples [samplesNum - 1] = computeVelocityAtPos (loop ? 0f : 1f);
            waypointSamples [samplesNum - 1] = GetWaypointFromPos (loop ? 0f : 1f);

            samplesDistances [samplesNum - 1] = Vector3.Distance (positionSamples [samplesNum - 1], positionSamples [samplesNum - 2]);

            // increment total distance;
            totalDistance += samplesDistances [samplesNum - 1];
        }

        /// <summary>
        /// Draws the path outline for this PathMagic instance
        /// </summary>
        void OnDrawGizmos ()
        {
            #if UNITY_EDITOR
            if (!gameObject.Equals (Selection.activeGameObject)) {
                Matrix4x4 mat = Handles.matrix;  // Thanks to Leon
                Handles.matrix = transform.localToWorldMatrix;

                for (int i = 0; i < waypoints.Length; i++) {

                    if (i > 0) {
                        Handles.DrawBezier (
                            waypoints [i - 1].position, 
                            waypoints [i].position, 
                            waypoints [i - 1].position +
                            waypoints [i - 1].outTangent, 
                            waypoints [i].position +
                            waypoints [i].inTangent, pathColor, null, 2f);
                    } else {
                        if (loop) {
                            Handles.DrawBezier (
                                waypoints [waypoints.Length - 1].position, 
                                waypoints [0].position, 
                                waypoints [waypoints.Length - 1].position +
                                waypoints [waypoints.Length - 1].outTangent, 
                                waypoints [0].position +
                                waypoints [0].inTangent, pathColor, null, 2f);
                        }
                    }
                }

                // Selection button
                if (waypoints.Length > 0) {
                    Gizmos.DrawIcon (transform.TransformPoint (Waypoints [0].position), "path.png", true);
                }

                Handles.matrix = mat;  // Thanks to Leon
            }
            #endif
        }
    }
}