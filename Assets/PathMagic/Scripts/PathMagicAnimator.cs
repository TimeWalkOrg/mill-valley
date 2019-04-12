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
		/// PathMagicAnimator class. Animate a transform along an external Path(Magic).
		/// </summary>
	public class PathMagicAnimator : MonoBehaviour
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

            target = GetComponent<Transform> ();

            // isPlaying = false; //Max B. bug
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
        /// The path magic on which animate.
        /// </summary>
        public PathMagic pathMagic;

        /// <summary>
        /// Gets or sets the path magic instance.
        /// </summary>
        /// <value>The path magic.</value>
        public PathMagic PathMagic {
            get {
                return pathMagic;
            }
            set {
                pathMagic = value;
            }
        }

        /// <summary>
        /// The target to animate.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Gets the target Transform object that is being animated. It is the actual Transform
        /// attached to the game object.
        /// </summary>
        /// <value>The target.</value>
        public Transform Target {
            get {
                return target;
            }
        }

        /// <summary>
        /// The update mode. By setting this to OnUpdate or OnFixedUpdate you can control
        /// when the target is actually moved and rotated by the animator.
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
        /// Start automatically the animation when the player starts?
        /// </summary>
        public bool autoStart = true;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagicAnimator"/> auto starts.
        /// </summary>
        /// <value><c>true</c> if auto start; otherwise, <c>false</c>.</value>
        public bool AutoStart {
            get {
                return autoStart;
            }
            set {
                autoStart = value;
            }
        }

        /// <summary>
        /// Bias correction of the global path velocity.
        /// </summary>
        public float velocityBias = 1f;

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
        /// Stores the current animation position from 0 to 1.
        /// </summary>
        public float currentPos;

        /// <summary>
        /// Gets or sets the current position. The value is normalized between 0f and 1f.
        /// </summary>
        /// <value>The current position.</value>
        public float CurrentPos {
            get {
                return currentPos;
            }
            set {
                currentPos = value;
            }
        }

        /// <summary>
        /// Is the animator actually playing?
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
        /// Override global look at.
        /// </summary>
        public Transform globalLookAt;

        /// <summary>
        /// Gets or sets the global look at. This value overrides the value of global look
        /// at of the related PathMagic instance.
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
        /// The disable orientation. Is set the path don't rotate the target
        /// </summary>
        public bool disableOrientation = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagicAnimator"/> disable orientation.
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
        /// Gets or sets a value indicating whether this <see cref="Jacovone.PathMagicAnimator"/> disable position.
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
        /// Override global path forward.
        /// </summary>
        public bool globalFollowPath;

        /// <summary>
        /// The waypoint changed. Fired when the current "last waypoint" has changed.
        /// </summary>
        public WaypointChangedEvent waypointChanged;

        /// <summary>
        /// Gets or sets the waypoint changed event handler fired when the current "last waypoint" has changed.
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
        /// The last passed waypoint, for event management.
        /// </summary>
        private int _lastPassedWayponint;

        /// <summary>
        /// Gets the last passed waypoint.
        /// </summary>
        /// <value>The last passed waypoint.</value>
        public int LastPassedWayponint {
            get {
                return _lastPassedWayponint;
            }
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start ()
        {
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
        /// Update for this frame. Calls DoUpdate() if animator is in OnUpdate mode
        /// </summary>
        void Update ()
        {
            if (updateMode == UpdateModeType.OnUpdate || !Application.isPlaying) {
                DoUpdate ();
            }	
        }

        /// <summary>
        /// The main FixedUpdate function to update physics. Calls DoUpdate() if animator is in OnFixedUpdate mode
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
        public void DoUpdate ()
        {
            if (pathMagic == null)
                return;

            if (pathMagic.waypoints.Length == 0)
                return;

            Application.targetFrameRate = 60;
            if (isPlaying) {

                float advance = (0.5f * velocityBias * _lastVelocity *
					// In edit mode update is called at very low FPS, so tweak it!
                                ((Application.isPlaying) ? Time.deltaTime : 0.008f));

                // Advance
                currentPos += advance;

                if (currentPos >= 1f) {
                    if (pathMagic.loop) {
                        currentPos -= 1f;
                    } else {
                        currentPos = 1f;
                        Pause (); // Thanks to Leon
                    }
                } else if (currentPos <= 0f) {
                    if (pathMagic.loop) {
                        currentPos += 1f;
                    } else {
                        currentPos = 0f;
                        Pause (); // Thanks to Leon
                    }
                }

                UpdateTarget ();
            }
        }

        /// <summary>
        /// Play this instance of animator.
        /// </summary>
        public void Play ()
        {
            if (pathMagic.waypoints.Length == 0)
                return;
            _lastVelocity = pathMagic.waypoints [0].velocity;
            isPlaying = true;
        }

        /// <summary>
        /// Pause this instance of animator, don't modify <see cref="currentPos"/>.
        /// </summary>
        public void Pause ()
        {
            if (pathMagic.waypoints.Length == 0)
                return;
            isPlaying = false;
        }

        /// <summary>
        /// Sets <see cref="currentPos"/> to 0 but don't stop playing.
        /// </summary>
        public void Rewind ()
        {
            if (pathMagic.waypoints.Length == 0)
                return;
            currentPos = 0f;
        }

        /// <summary>
        /// Stop this instance ans sets <see cref="currentPos"/> to 0.
        /// </summary>
        public void Stop ()
        {
            if (pathMagic.waypoints.Length == 0)
                return;
            isPlaying = false;
            currentPos = 0f;
		
            UpdateTarget (pathMagic.computePositionAtPos (currentPos), pathMagic.computeRotationAtPos (currentPos));
        }

        /// <summary>
        /// Updates the target that is the same transform
        /// </summary>
        /// <param name="position">Position to set</param>
        /// <param name="rotation">Rotation to set</param>
        public void UpdateTarget (Vector3 position, Quaternion rotation)
        {
            if (target != null) {

                if (!disablePosition) {
                    target.position = pathMagic.transform.TransformPoint (position);
                }

                if (!disableOrientation) {
                    target.rotation = pathMagic.transform.rotation * rotation;
                }
            }
        }

        /// <summary>
        /// Updates the target using stored currentPos.
        /// </summary>
        public void UpdateTarget ()
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float velocity = 1.0f;
            int waypoint = 0;

            if (pathMagic.presampledPath) {
                pathMagic.sampledPositionAndRotationAndVelocityAndWaypointAtPos (currentPos, out position, out rotation, out velocity, out waypoint);
            } else {
                position = pathMagic.computePositionAtPos (currentPos);
                rotation = pathMagic.computeRotationAtPos (currentPos);
                velocity = pathMagic.computeVelocityAtPos (currentPos);
                waypoint = pathMagic.GetWaypointFromPos (currentPos);
            }

            if (globalFollowPath) {
                // Global follow path override
                rotation = pathMagic.GetFaceForwardForPos (currentPos);
            } else if (globalLookAt != null) {
                // Global look at override
                rotation = Quaternion.LookRotation (pathMagic.transform.InverseTransformPoint (globalLookAt.position) - position);
            }

            _lastVelocity = velocity;

            UpdateTarget (position, rotation);

            // Fire waypointChanged if is the case
            if (waypoint != _lastPassedWayponint) {
                if (waypointChanged != null)
                    waypointChanged.Invoke (waypoint);
                if (pathMagic.waypoints [waypoint].reached != null)
                    pathMagic.waypoints [waypoint].reached.Invoke ();
            }

            _lastPassedWayponint = waypoint;
        }
    }
}