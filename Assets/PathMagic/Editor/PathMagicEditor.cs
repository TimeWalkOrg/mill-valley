using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace Jacovone
{
    [CustomEditor (typeof(Jacovone.PathMagic))]
    [CanEditMultipleObjects]
    /// <summary>
    /// Path magic editor. This i sthe editor class for PathMagic instances. It defines the inspector behavios to
    /// configure a complete PathMagic instance.
    /// </summary>
    public class PathMagicEditor : Editor
    {
        /// <summary>
        /// Store the state of waypoint foldout.
        /// </summary>
        private bool waypointsFoldout = true;

        /// <summary>
        /// Stores the state of preview section foldout.
        /// </summary>
        private bool previewFoldout = true;

        /// <summary>
        /// Stores the state of events section foldout.
        /// </summary>
        private bool eventsFoldout = true;

        /// <summary>
        /// Stores the state of utility section foldout.
        /// </summary>
        private bool utilityFoldout = true;

        /// <summary>
        /// Stores the visibility of path direction in the scene view.
        /// </summary>
        private bool showPathDirections = false;

        /// <summary>
        /// Stores the visibility of path samples when path sampling is enabled
        /// </summary>
        private bool showPathSamples = true;

        /// <summary>
        /// Store the visibility of bezier tangents
        /// </summary>
        private bool showTangents = true;

        /// <summary>
        /// The bake number of samples.
        /// </summary>
        private int bakeNumberOfSamples = 100;

        /// <summary>
        /// The duration of the baked animation.
        /// </summary>
        private float bakeAnimationDuration = 5f;

        /// <summary>
        /// The waypoints reorderable list.
        /// </summary>
        [SerializeField]
        private ReorderableList
            wl;

        /// <summary>
        /// Internally stores the current selected waypoint for either inspector and scene view.
        /// </summary>
        private int currentSelectedWaypoint = -1;

        /// <summary>
        /// A style for bold foldouts
        /// </summary>
        private GUIStyle boldFoldoutStyle;

        /// <summary>
        /// The action button style for buttons like "Play", "Rewind", ...
        /// </summary>
        private GUIStyle actionButtonStyleLeft;

        /// <summary>
        /// The action button style for buttons like "Play", "Rewind", ...
        /// </summary>
        private GUIStyle actionButtonStyleRight;

        /// <summary>
        /// A style for bold mini buttons in the inspector.
        /// </summary>
        private GUIStyle rightMiniButton;

        /// <summary>
        /// The path magic logo. Loaded from resources.
        /// </summary>
        private Texture2D pathMagicLogo = null;

        /// <summary>
        /// Raises the enable event. Sets up the reorderable waypoint list.
        /// </summary>
        void OnEnable ()
        {
            pathMagicLogo = Resources.Load ("pathmagiclogo") as Texture2D;

            showPathDirections = EditorPrefs.GetBool ("PathMagic.ShowPathDirections", false);
            showPathSamples = EditorPrefs.GetBool ("PathMagic.ShowPathSamples", false);
            showTangents = EditorPrefs.GetBool ("PathMagic.ShowTangents", true);

            SerializedProperty waypoints = serializedObject.FindProperty ("waypoints");

            wl = new ReorderableList (serializedObject, waypoints);

            wl.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

                if (index > waypoints.arraySize - 1) {
                    return;
                }

                rect.y += 2;
                EditorGUIUtility.labelWidth = 20;

                if (GUI.Button (new Rect (rect.x, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), "\u2023")) {
                    PathMagic pm = (PathMagic)target;
                    pm.CurrentPos = ComputePosForWaypoint (index);

                }

                EditorGUI.PropertyField (
                    new Rect (rect.x + EditorGUIUtility.singleLineHeight, rect.y, rect.width - 120 - EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight),
                    waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("position"),
                    new GUIContent ("" + (index + 1)));

                EditorGUIUtility.labelWidth = 30;

                if (serializedObject.FindProperty ("disableOrientation").boolValue ||
                    serializedObject.FindProperty ("globalFollowPath").boolValue ||
                    serializedObject.FindProperty ("globalLookAt").objectReferenceValue != null)
                    GUI.enabled = false;

                EditorGUI.PropertyField (
                    new Rect (rect.x + rect.width - 120, rect.y, 120, EditorGUIUtility.singleLineHeight),
                    waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("lookAt"),
                    new GUIContent ("Look", "If specified, on the waypoint the target will look at this transform. Otherwise it will look at the specified rotation"));

                GUI.enabled = true;
            };

            wl.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField (rect, "Waypoints");
            };

            wl.onRemoveCallback = (ReorderableList l) => {
                if (EditorUtility.DisplayDialog ("Warning!",
                        "Are you sure you want to delete the waypoint?", "Yes", "No")) {

                    waypoints.DeleteArrayElementAtIndex (l.index);
                    if (currentSelectedWaypoint >= waypoints.arraySize)
                        currentSelectedWaypoint = -1;
                }
            };

            wl.onCanRemoveCallback = (ReorderableList l) => {
                return true;
            };

            wl.onSelectCallback = (ReorderableList l) => {
                currentSelectedWaypoint = l.index;
                SceneView.RepaintAll ();
            };

            wl.onAddCallback = (ReorderableList l) => {
                if (currentSelectedWaypoint == -1) {
                    InsertWaypointAt (waypoints.arraySize, true);
                } else {
                    InsertWaypointAt (currentSelectedWaypoint + 1, true);
                }
            };

            wl.onReorderCallback = (ReorderableList l) => {
                // Does nothing since now we are use SerializedProperties
            };
        }

        /// <summary>
        /// Raises the OnInspectorGUI event. Defines the instactor behavior for configuring a PathMagic instance.
        /// </summary>
        public override void OnInspectorGUI ()
        {
            serializedObject.Update ();

            boldFoldoutStyle = new GUIStyle (EditorStyles.foldout);
            rightMiniButton = new GUIStyle (EditorStyles.miniButton);

            boldFoldoutStyle.fontStyle = FontStyle.Bold;
            rightMiniButton.fixedWidth = 100;

            actionButtonStyleLeft = new GUIStyle (EditorStyles.miniButtonLeft);
            if (EditorGUIUtility.isProSkin)
                actionButtonStyleLeft.normal.textColor = Color.yellow;
            else
                actionButtonStyleLeft.normal.textColor = Color.black;
            actionButtonStyleLeft.fontStyle = FontStyle.Bold;
            actionButtonStyleLeft.fontSize = 11;
            actionButtonStyleRight = new GUIStyle (EditorStyles.miniButtonRight);
            if (EditorGUIUtility.isProSkin)
                actionButtonStyleRight.normal.textColor = Color.yellow;
            else
                actionButtonStyleRight.normal.textColor = Color.black;
            actionButtonStyleRight.fontStyle = FontStyle.Bold;
            actionButtonStyleRight.fontSize = 11;

            SerializedProperty waypoints = serializedObject.FindProperty ("waypoints");

            // Set the coordinate space matrix
            EditorGUIUtility.labelWidth = 120;

            if (wl == null)
                OnEnable ();

            wl.index = currentSelectedWaypoint;

            EditorGUILayout.LabelField (new GUIContent (pathMagicLogo), GUILayout.Width (142), GUILayout.Height (28));

            EditorGUILayout.BeginVertical ("Box");

            if (!serializedObject.isEditingMultipleObjects)
                EditorGUILayout.PropertyField (serializedObject.FindProperty ("target"), new GUIContent ("Target transform", "The transform that will be animated"));

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("updateMode"), new GUIContent ("Update transform", "Defines when actually update the transform"));

            EditorGUIUtility.labelWidth = 90;
            EditorGUILayout.BeginHorizontal ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("autoStart"), new GUIContent ("Start on play", "If true, the path will be animated at the start"));

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("loop"), new GUIContent ("Loop path", "If true, the path will continue indefinitely to loop, otherwise it stop at the end"));

            EditorGUILayout.EndHorizontal ();

            EditorGUILayout.Slider (serializedObject.FindProperty ("velocityBias"), -5f, 5f, new GUIContent ("Velocity Bias", "Global adjustment of the animation velocity"));

            EditorGUIUtility.labelWidth = 120;

            EditorGUILayout.BeginHorizontal (GUILayout.Width (300f));

            EditorGUILayout.BeginVertical (GUILayout.Width (150f));
            EditorGUILayout.PropertyField (
                serializedObject.FindProperty ("disableOrientation"), new GUIContent ("Disable Orientation", "If set, this path does not modify target rotation"));
            EditorGUILayout.PropertyField (
                serializedObject.FindProperty ("disablePosition"), new GUIContent ("Disable Position", "If set, this path does not change the target position"));
            
            EditorGUILayout.EndVertical ();
            EditorGUILayout.BeginVertical (GUILayout.Width (150f));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("presampledPath"), new GUIContent ("Pre-sampled path", "If set, the path will be pre-sampled and velocity along path will be constant"), GUILayout.Width (150f));

            if (serializedObject.FindProperty ("presampledPath").boolValue) {
                EditorGUILayout.PropertyField (serializedObject.FindProperty ("samplesNum"), new GUIContent ("Number of samples", "Number of samples for pre-sempling. A greater value results in a better precision of animation"), GUILayout.Width (150f));
                if (serializedObject.FindProperty ("samplesNum").intValue <= 5)
                    serializedObject.FindProperty ("samplesNum").intValue = 5;
                if (serializedObject.FindProperty ("samplesNum").intValue >= 10000)
                    serializedObject.FindProperty ("samplesNum").intValue = 10000;
            }

            EditorGUILayout.EndVertical ();
            EditorGUILayout.EndHorizontal ();

            if (serializedObject.FindProperty ("disableOrientation").boolValue)
                GUI.enabled = false;

            EditorGUILayout.BeginHorizontal (GUILayout.Width (300f));

            EditorGUILayout.BeginVertical (GUILayout.Width (150f));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("globalFollowPath"), new GUIContent ("Global Follow Path", "If set, the target will always follow the path"), GUILayout.Width (150f));
            EditorGUILayout.EndVertical ();

            EditorGUILayout.BeginVertical (GUILayout.Width (150f));
            if (serializedObject.FindProperty ("globalFollowPath").boolValue) {
                EditorGUILayout.LabelField (new GUIContent ("Follow bias", "Smaller values give more precise follow, greater values give more soft follow. User greater values for camera follows"), GUILayout.Width (150f));
                EditorGUILayout.Slider (serializedObject.FindProperty ("globalFollowPathBias"), 0.001f, 0.1f, new GUIContent ("", "Smaller values give more precise follow, greater values give more soft follow. User greater values for camera follows"), GUILayout.Width (150f));
            }
            EditorGUILayout.EndVertical ();
            EditorGUILayout.EndHorizontal ();

            if (serializedObject.FindProperty ("globalFollowPath").boolValue)
                GUI.enabled = false;

            EditorGUIUtility.labelWidth = 90;

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("globalLookAt"), new GUIContent ("Global Look At", "If set, the target transform will lok at that at each frame"));

            GUI.enabled = true;

            EditorGUILayout.EndVertical (); // Box

            EditorGUILayout.Separator ();

            if (!waypoints.hasMultipleDifferentValues) {
                // Actually draw the list
                wl.DoLayoutList ();
            } else {
                currentSelectedWaypoint = -1;
                EditorGUILayout.HelpBox ("You can't edit waypoints data because waypoints of selected paths are not the same.", MessageType.Info);
            }

            if (currentSelectedWaypoint > waypoints.arraySize - 1)
                currentSelectedWaypoint = -1;

            if (currentSelectedWaypoint != -1) {

                EditorGUIUtility.labelWidth = 60;
                EditorGUILayout.BeginHorizontal ();

                waypointsFoldout = EditorGUILayout.Foldout (waypointsFoldout, "Waypoint " + (currentSelectedWaypoint + 1), boldFoldoutStyle);
                if (GUILayout.Button (new GUIContent ("Reveal", "Center this point on scene view"), EditorStyles.miniButtonLeft, GUILayout.Width (60))) {
                    if (SceneView.lastActiveSceneView != null) {
                        SceneView.lastActiveSceneView.pivot = ((PathMagic)serializedObject.targetObject).transform.TransformPoint (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("position").vector3Value);
                    }
                }
                if (GUILayout.Button (new GUIContent ("Add after", "Add a new waypoint just after this one"), EditorStyles.miniButtonMid, GUILayout.Width (60))) {
                    if (currentSelectedWaypoint == -1) {
                        InsertWaypointAt (waypoints.arraySize, true);
                    } else {
                        InsertWaypointAt (currentSelectedWaypoint + 1, true);
                    }
                }
                if (GUILayout.Button (new GUIContent ("Remove", "Removes this waypoint"), EditorStyles.miniButtonRight, GUILayout.Width (60))) {
                    RemoveWaypointAt (currentSelectedWaypoint);
                    currentSelectedWaypoint = (currentSelectedWaypoint - 1) % (waypoints.arraySize);
                }

                EditorGUILayout.EndHorizontal ();

                if (waypointsFoldout) {

                    EditorGUILayout.BeginVertical ("Box");
                    EditorGUILayout.BeginHorizontal ();

                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("position"),
                        new GUIContent ("Position", "Position of the waypoint in 3D space"));

                    GUI.enabled = currentSelectedWaypoint > 0;
                    if (GUILayout.Button (new GUIContent ("Align", "Align this waypoint between the previous and the next one"), rightMiniButton)) {
                        RemoveWaypointAt (currentSelectedWaypoint);
                        currentSelectedWaypoint = ((currentSelectedWaypoint - 1) % waypoints.arraySize);

                        // It's crucial
                        serializedObject.ApplyModifiedProperties ();

                        if (currentSelectedWaypoint == -1) {
                            InsertWaypointAt (waypoints.arraySize, true);
                        } else {
                            InsertWaypointAt (currentSelectedWaypoint + 1, true);
                        }
                    }
                    GUI.enabled = true;
                    EditorGUILayout.EndHorizontal ();

                    EditorGUILayout.BeginHorizontal ();

                    if (serializedObject.FindProperty ("disableOrientation").boolValue ||
                        serializedObject.FindProperty ("globalFollowPath").boolValue ||
                        serializedObject.FindProperty ("globalLookAt").objectReferenceValue != null)
                        GUI.enabled = false;

                    if (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("lookAt").objectReferenceValue != null)
                        GUI.enabled = false;

                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("rotation"),
                        new GUIContent ("Rotation", "The rotation to which the waypoint will look during the animation"));

                    if (GUILayout.Button (new GUIContent ("Face forward", "Modify the rotation so that the waypoint will look forward in the path during the animation"), rightMiniButton)) {
                        FaceForward (currentSelectedWaypoint);
                    }

                    GUI.enabled = true;

                    EditorGUILayout.EndHorizontal ();

                    EditorGUILayout.BeginHorizontal ();
                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("velocity"), new GUIContent ("Velocity", "The velocity of the animation at this waypoint"));

                    EditorGUIUtility.labelWidth = 20;

                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("inVariation"), new GUIContent ("In", "Speed at which interpolator reaches the waypoint speed"));

                    EditorGUIUtility.labelWidth = 30;

                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("outVariation"), new GUIContent ("Out", "Speed at which interpolator leaves the waypoint speed"));

                    EditorGUIUtility.labelWidth = 60;

                    GUI.enabled = (currentSelectedWaypoint > 0 && currentSelectedWaypoint < (waypoints.arraySize - 1));
                    if (GUILayout.Button (new GUIContent ("Path average", "Sets the velocity at the average value between the previous and the next in the path"), rightMiniButton)) {
                        waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("velocity").floatValue =
                        (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint - 1).FindPropertyRelative ("velocity").floatValue +
                        waypoints.GetArrayElementAtIndex (currentSelectedWaypoint + 1).FindPropertyRelative ("velocity").floatValue) / 2f;
                    }
                    GUI.enabled = true;
                    EditorGUILayout.EndHorizontal ();

                    // Symmetrical waypoint?
                    EditorGUIUtility.labelWidth = 130;
                    //bool symTgt = 
                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("symmetricTangents"), new GUIContent ("Symmetric tangents", "If set bezier tangents are forced to be symmetric"));
                    EditorGUIUtility.labelWidth = 80;

                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("inTangent"),
                        new GUIContent ("In Tangent", "The IN tangent vector relative to the waypoint position"));
                    if (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("symmetricTangents").boolValue)
                        waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("outTangent").vector3Value = -waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("inTangent").vector3Value;


                    if (!waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("symmetricTangents").boolValue) {
                        EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("outTangent"),
                            new GUIContent ("Out Tangent", "The OUT tangent vector relative to the waypoint position"));
                        if (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("symmetricTangents").boolValue)
                            waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("inTangent").vector3Value = -waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("outTangent").vector3Value;
                    }

                    EditorGUIUtility.labelWidth = 60;

                    EditorGUILayout.Separator ();

                    // The reached event
                    EditorGUILayout.PropertyField (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("reached"));

                    EditorGUILayout.EndVertical (); // Box
                }

                EditorGUILayout.Separator ();
            }

            eventsFoldout = EditorGUILayout.Foldout (eventsFoldout, "Events", boldFoldoutStyle);
            if (eventsFoldout) {
                EditorGUILayout.PropertyField (serializedObject.FindProperty ("waypointChanged"));
            }

            EditorGUILayout.BeginHorizontal ();
            previewFoldout = EditorGUILayout.Foldout (previewFoldout, "Preview", boldFoldoutStyle);
            if (!((PathMagic)serializedObject.targetObject).isPlaying) {
                if (GUILayout.Button (new GUIContent ("Rewind", "Go to the begin of the animation"), actionButtonStyleLeft)) {
                    for (int i = 0; i < targets.Length; i++)
                        ((PathMagic)targets [i]).Rewind ();
                }
            } else {
                if (GUILayout.Button (new GUIContent ("Stop", "Turn off animation simulation and go to the begin"), actionButtonStyleLeft)) {
                    for (int i = 0; i < targets.Length; i++)
                        ((PathMagic)targets [i]).Stop ();
                }
            }
            if (!((PathMagic)serializedObject.targetObject).isPlaying) {
                if (GUILayout.Button (new GUIContent ("Start", "Starts the animation simulation"), actionButtonStyleRight)) {
                    for (int i = 0; i < targets.Length; i++)
                        ((PathMagic)targets [i]).Play ();
                }
            } else {
                if (GUILayout.Button (new GUIContent ("Pause", "Pause the animation simulation"), actionButtonStyleRight)) {
                    for (int i = 0; i < targets.Length; i++)
                        ((PathMagic)targets [i]).Pause ();
                }
            }

            EditorGUILayout.EndHorizontal ();
            if (previewFoldout) {

                EditorGUILayout.BeginVertical ("Box");
                EditorGUILayout.BeginHorizontal ();

                EditorGUIUtility.labelWidth = 70;
                EditorGUILayout.Slider (serializedObject.FindProperty ("currentPos"), 0f, 1f, new GUIContent ("Position", "Seek a specific point in the path"));

                EditorGUIUtility.labelWidth = 120;
                EditorGUILayout.PropertyField (serializedObject.FindProperty ("updateTransform"), new GUIContent ("Update Transform", "Update transform during animation?"));
                EditorGUIUtility.labelWidth = 70;

                for (int i = 0; i < targets.Length; i++) {
                    if (!((PathMagic)targets [i]).isPlaying && ((PathMagic)targets [i]).updateTransform && ((PathMagic)targets [i]).target != null) {

                        if (((PathMagic)targets [i]).presampledPath) {
                            PathMagic pmo = (PathMagic)targets [i];
                            Vector3 position = Vector3.zero;
                            Quaternion rotation = Quaternion.identity;
                            float velocity = 0f;
                            int waypoint = 0;
                            pmo.sampledPositionAndRotationAndVelocityAndWaypointAtPos (pmo.currentPos, out position, out rotation, out velocity, out waypoint);
                            pmo.UpdateTarget (position, rotation);
                        } else {
                            ((PathMagic)targets [i]).UpdateTarget (
                                ((PathMagic)targets [i]).computePositionAtPos (((PathMagic)targets [i]).currentPos),
                                ((PathMagic)targets [i]).computeRotationAtPos (((PathMagic)targets [i]).currentPos)
                            );
                        }
                    }
                }

                EditorGUILayout.EndHorizontal ();
                EditorGUILayout.EndVertical (); //box
            }

            // Enable other utility functions if is editing a single path
            utilityFoldout = EditorGUILayout.Foldout (utilityFoldout, "Utility", boldFoldoutStyle);
            if (utilityFoldout) {

                EditorGUILayout.BeginVertical ("Box");
                if (serializedObject.isEditingMultipleObjects) {


                } else {

                    EditorGUILayout.BeginHorizontal ();
                    if (GUILayout.Button (new GUIContent ("Normalize path transform", "Normalize this transform and update all waypoints"), GUILayout.Width ((EditorGUIUtility.currentViewWidth - 60f) / 2f))) {
                        for (int i = 0; i < waypoints.arraySize; i++) {
                            waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value = ((PathMagic)serializedObject.targetObject).transform.TransformPoint (waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value);
                            waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("rotation").vector3Value = (((PathMagic)serializedObject.targetObject).transform.rotation * Quaternion.Euler (waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("rotation").vector3Value)).eulerAngles;
                            waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("inTangent").vector3Value = ((PathMagic)serializedObject.targetObject).transform.TransformVector (waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("inTangent").vector3Value);
                            waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("outTangent").vector3Value = ((PathMagic)serializedObject.targetObject).transform.TransformVector (waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("outTangent").vector3Value);
                        }

                        Undo.RecordObject (((PathMagic)serializedObject.targetObject).transform, "Normalize");
                        ((PathMagic)serializedObject.targetObject).transform.position = Vector3.zero;
                        ((PathMagic)serializedObject.targetObject).transform.localScale = new Vector3 (1f, 1f, 1f);
                        ((PathMagic)serializedObject.targetObject).transform.rotation = Quaternion.identity;
                        Undo.FlushUndoRecordObjects ();
                    }

                    if (GUILayout.Button (new GUIContent ("Center on waypoints", "Updates this transform position to center of its waypoints and update all waypoints"), GUILayout.Width ((EditorGUIUtility.currentViewWidth - 60f) / 2f))) {

                        Undo.RecordObject (target, "Center transform on waypoints");
                        Vector3 center = Vector3.zero;
                        for (int i = 0; i < waypoints.arraySize; i++) {
                            center += waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value;
                        }

                        center /= waypoints.arraySize;

                        for (int i = 0; i < waypoints.arraySize; i++) {
                            waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value -= center;
                        }

                        Undo.RecordObject (((PathMagic)serializedObject.targetObject).transform, "Center on waypoints");
                        ((PathMagic)serializedObject.targetObject).transform.position += center;
                        Undo.FlushUndoRecordObjects ();
                    }

                    EditorGUILayout.EndHorizontal ();

                    EditorGUILayout.BeginHorizontal ();
                    if (GUILayout.Button (new GUIContent ("Look to forward", "Updates all waypoints to look forward in the path"), GUILayout.Width ((EditorGUIUtility.currentViewWidth - 60f) / 2f))) {

                        Undo.RecordObject (target, "Face all waypoints to path");
                        for (int i = 0; i < waypoints.arraySize; i++) {
                            FaceForward (i);
                        }
                    }

                    if (GUILayout.Button (new GUIContent ("Look to center", "Updates all waypoints to look at the center of the path"),
                            GUILayout.Width ((EditorGUIUtility.currentViewWidth - 60f) / 2f))) {

                        Undo.RecordObject (target, "Face all waypoints to center");
                        Vector3 center = Vector3.zero;
                        for (int i = 0; i < waypoints.arraySize; i++) {
                            center += waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value;
                        }

                        center /= waypoints.arraySize;
                        for (int i = 0; i < waypoints.arraySize; i++) {
                            waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("rotation").vector3Value = Quaternion.LookRotation (center - waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value).eulerAngles;
                        }
                    }

                    EditorGUILayout.EndHorizontal ();
                }

                EditorGUIUtility.labelWidth = 120;

                EditorGUILayout.PropertyField (serializedObject.FindProperty ("pathColor"), new GUIContent ("Path color", "The color of the path in the editor"));

                EditorGUILayout.BeginHorizontal ();

                EditorGUI.BeginChangeCheck ();
                showPathDirections = EditorGUILayout.Toggle (new GUIContent ("Show path directions", "Show direction along entire path"), showPathDirections);
                if (EditorGUI.EndChangeCheck ()) {
                    EditorPrefs.SetBool ("PathMagic.ShowPathDirections", showPathDirections);
                    ((SceneView)SceneView.sceneViews [0]).Repaint ();
                }

                if (serializedObject.FindProperty ("presampledPath").boolValue) {
                    EditorGUI.BeginChangeCheck ();
                    showPathSamples = EditorGUILayout.Toggle (new GUIContent ("Show path samples", "Show samples along path"), showPathSamples);
                    if (EditorGUI.EndChangeCheck ()) {
                        EditorPrefs.SetBool ("PathMagic.ShowPathSamples", showPathSamples);
                        ((SceneView)SceneView.sceneViews [0]).Repaint ();
                    }
                }

                EditorGUILayout.EndHorizontal ();

                EditorGUI.BeginChangeCheck ();
                showTangents = EditorGUILayout.Toggle (new GUIContent ("Show tangents", "Show tangents of waypoints"), showTangents);
                if (EditorGUI.EndChangeCheck ()) {
                    EditorPrefs.SetBool ("PathMagic.ShowTangents", showTangents);
                    ((SceneView)SceneView.sceneViews [0]).Repaint ();
                }
                EditorGUILayout.EndVertical (); // box


                // Bake animation

                EditorGUIUtility.labelWidth = 60;
                EditorGUILayout.BeginVertical ("Box");

                EditorGUILayout.HelpBox ("If you want, you can bake the entire animation into a legacy Unity animation asset. Specify a number of samples and a duration for the animation, then press the Bake Animation button.",
                    MessageType.Info,
                    true);

                EditorGUILayout.BeginHorizontal ();

                bakeNumberOfSamples = EditorGUILayout.IntField (new GUIContent ("Samples", "Number of samples of the final animation"), bakeNumberOfSamples);
                if (bakeNumberOfSamples < 5)
                    bakeNumberOfSamples = 5;

                bakeAnimationDuration = EditorGUILayout.FloatField (new GUIContent ("Duration", "Duration of the final animation in seconds"), bakeAnimationDuration);
                if (bakeAnimationDuration < 0.2f)
                    bakeAnimationDuration = 0.2f;

                if (GUILayout.Button (new GUIContent ("Bake Animation", "Bake the complete animation on the target"),
                        GUILayout.Width (150f))) {
                    BakeAnimation (serializedObject, bakeNumberOfSamples, bakeAnimationDuration);
                }

                EditorGUILayout.EndHorizontal ();
                EditorGUILayout.EndVertical (); // box
            }

            EditorGUILayout.Separator ();

            EditorGUILayout.BeginVertical ("Box");
            EditorGUIUtility.labelWidth = 90;

            EditorGUILayout.LabelField ("Version", PathMagicVersion.Version);

            EditorGUILayout.Separator ();
            EditorGUILayout.LabelField ("Shortcuts:", "");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField ("\",\"", "Select previous waypoint\t");
            EditorGUILayout.LabelField ("\".\"", "Select next waypoint\t");
            EditorGUILayout.LabelField ("\"Enter\"", "Focus on selected waypoint\t");
            EditorGUILayout.LabelField ("\"Esc\"", "Clear waypoint selection\t");
            EditorGUILayout.Separator ();
            EditorGUILayout.LabelField ("CTRL/CMD", "Append new waypoint at mouse position\t");
            EditorGUILayout.LabelField("+ Click", "");

            EditorGUILayout.EndVertical (); // Box

            serializedObject.ApplyModifiedProperties ();
            if (serializedObject.FindProperty ("presampledPath").boolValue)
                ((PathMagic)serializedObject.targetObject).UpdatePathSamples ();

            ManageKeyboardEvents ();
        }

        /// <summary>
        /// Raises the OnSceneGUI event. Define the interaction between user and PathMagic waypoints in the scene view.
        /// </summary>
        void OnSceneGUI ()
        {
            // Global/Local section
            bool isGlobalMode = Tools.pivotRotation == PivotRotation.Global;

            SerializedObject pm = new SerializedObject (target);
            PathMagic pmo = (PathMagic)target;
            SerializedProperty waypoints = pm.FindProperty ("waypoints");
            Handles.matrix = ((PathMagic)pm.targetObject).transform.localToWorldMatrix;

            ManageKeyboardEvents ();

            ManageMouseEvents (waypoints);
            pm.ApplyModifiedProperties();

            for (int i = 0; i < waypoints.arraySize; i++) {

                SerializedProperty wp = waypoints.GetArrayElementAtIndex (i);

                float handleSize = HandleUtility.GetHandleSize (wp.FindPropertyRelative ("position").vector3Value);

                // Selection handle
                if (pmo.Waypoints[i].Reached.GetPersistentEventCount() > 0) {
                    Handles.color = new Color (1f, .4f, 0f);
                } else {
                    Handles.color = Color.white;
                }

                if (Handles.Button (
#if UNITY_5_5_OR_NEWER
                        wp.FindPropertyRelative ("position").vector3Value, 
                        Quaternion.identity, 
                        HandleUtility.GetHandleSize (wp.FindPropertyRelative ("position").vector3Value) / 10f,
                        HandleUtility.GetHandleSize (wp.FindPropertyRelative ("position").vector3Value) / 5f,
                        Handles.DotHandleCap)) {
#else
                        wp.FindPropertyRelative("position").vector3Value,
                        Quaternion.identity,
                        HandleUtility.GetHandleSize(wp.FindPropertyRelative("position").vector3Value) / 10f,
                        HandleUtility.GetHandleSize(wp.FindPropertyRelative("position").vector3Value) / 5f,
                        Handles.DotCap))
                {
#endif
                    currentSelectedWaypoint = i;
                    EditorUtility.SetDirty (target);
                }

                if (currentSelectedWaypoint == i) {

                    // Position
                    wp.FindPropertyRelative ("position").vector3Value = PositionHandle (wp.FindPropertyRelative ("position").vector3Value, isGlobalMode ? Quaternion.identity : Quaternion.Euler (wp.FindPropertyRelative ("rotation").vector3Value), false);

                    if (showTangents) {
                        // In tangent
                        wp.FindPropertyRelative ("inTangent").vector3Value = PositionHandle (
                            wp.FindPropertyRelative ("inTangent").vector3Value + wp.FindPropertyRelative ("position").vector3Value,
                            isGlobalMode ? Quaternion.identity : Quaternion.Euler (wp.FindPropertyRelative ("rotation").vector3Value), true) - wp.FindPropertyRelative ("position").vector3Value;
                        if (wp.FindPropertyRelative ("symmetricTangents").boolValue)
                            wp.FindPropertyRelative ("outTangent").vector3Value = -wp.FindPropertyRelative ("inTangent").vector3Value;

                        // Out tangent
                        wp.FindPropertyRelative ("outTangent").vector3Value = PositionHandle (
                            wp.FindPropertyRelative ("outTangent").vector3Value + wp.FindPropertyRelative ("position").vector3Value,
                            isGlobalMode ? Quaternion.identity : Quaternion.Euler (wp.FindPropertyRelative ("rotation").vector3Value), true) - wp.FindPropertyRelative ("position").vector3Value;

                        if (wp.FindPropertyRelative ("symmetricTangents").boolValue)
                            wp.FindPropertyRelative ("inTangent").vector3Value = -wp.FindPropertyRelative ("outTangent").vector3Value;

                        // Draw tangent         
                        Handles.color = Color.green;
                        Handles.DrawLine (wp.FindPropertyRelative ("inTangent").vector3Value + wp.FindPropertyRelative ("position").vector3Value, wp.FindPropertyRelative ("position").vector3Value);
                        Handles.DrawLine (wp.FindPropertyRelative ("outTangent").vector3Value + wp.FindPropertyRelative ("position").vector3Value, wp.FindPropertyRelative ("position").vector3Value);
                        Handles.color = Color.white;
                    }

                    // Rotation
                    if (pm.FindProperty ("globalFollowPath").boolValue) {

                        // Don't draw

                    } else if (pm.FindProperty ("globalLookAt").objectReferenceValue != null) {

                        // Global look at
                        Handles.color = Color.green;
                        Handles.DrawLine (wp.FindPropertyRelative ("position").vector3Value,
                            ((PathMagic)pm.targetObject).transform.InverseTransformPoint (((Transform)pm.FindProperty ("globalLookAt").objectReferenceValue).position));
                        Handles.color = Color.white;

                        // Direction
                        DrawWaypointDirection (wp);

                    } else if (wp.FindPropertyRelative ("lookAt").objectReferenceValue != null) {

                        // Target look at
                        Handles.color = Color.blue;
                        Handles.DrawLine (wp.FindPropertyRelative ("position").vector3Value,
                            ((PathMagic)pm.targetObject).transform.InverseTransformPoint (((Transform)wp.FindPropertyRelative ("lookAt").objectReferenceValue).position));
                        Handles.color = Color.white;

                        // Direction
                        DrawWaypointDirection (wp);

                    } else {
                        // Rotation handles
                        Vector3 rot = wp.FindPropertyRelative ("rotation").vector3Value;
                        Quaternion rotQuaternion = Quaternion.Euler (rot);

                        EditorGUI.BeginChangeCheck ();

                        rotQuaternion = Handles.RotationHandle (rotQuaternion, wp.FindPropertyRelative ("position").vector3Value);

                        if (EditorGUI.EndChangeCheck ()) {
                            wp.FindPropertyRelative ("rotation").vector3Value = rotQuaternion.eulerAngles;
                        }

                        // Direction
                        DrawWaypointDirection (wp);
                    }

                    EditorGUI.BeginChangeCheck ();

                    // Velocity
#if UNITY_5_5_OR_NEWER
                    wp.FindPropertyRelative ("velocity").floatValue = Mathf.Clamp (Handles.ScaleValueHandle (
                        wp.FindPropertyRelative ("velocity").floatValue, 
                        wp.FindPropertyRelative ("position").vector3Value + (wp.FindPropertyRelative ("velocity").floatValue * Vector3.up + 0.2f * Vector3.right + 0.2f * Vector3.forward), 
                        Quaternion.identity, 
                        handleSize / 3f, 
                        Handles.DotHandleCap, 
                        0.1f), 
                        0.0f, 
                        1000f);
#else
                    wp.FindPropertyRelative("velocity").floatValue = Mathf.Clamp(Handles.ScaleValueHandle(
                        wp.FindPropertyRelative("velocity").floatValue,
                        wp.FindPropertyRelative("position").vector3Value + (wp.FindPropertyRelative("velocity").floatValue * Vector3.up + 0.2f * Vector3.right + 0.2f * Vector3.forward),
                        Quaternion.identity,
                        handleSize / 3f,
                        Handles.DotCap,
                        0.1f),
                        0.0f,
                        1000f);
#endif
                    Handles.DrawSolidRectangleWithOutline (new Vector3[] {
                        wp.FindPropertyRelative ("position").vector3Value,
                        wp.FindPropertyRelative ("position").vector3Value + (wp.FindPropertyRelative ("velocity").floatValue * Vector3.up),
                        wp.FindPropertyRelative ("position").vector3Value + (wp.FindPropertyRelative ("velocity").floatValue * Vector3.up + 0.2f * Vector3.right + 0.2f * Vector3.forward),
                        wp.FindPropertyRelative ("position").vector3Value + (0.2f * Vector3.right + 0.2f * Vector3.forward)
                    },
                        new Color (1f, .8f, 0f, 0.1f),
                        new Color (1f, 1f, 0f, 1f)
                    );
                }

                if (i > 0) {
                    // Connect with previous Waypoint
                    Handles.DrawBezier (
                        waypoints.GetArrayElementAtIndex (i - 1).FindPropertyRelative ("position").vector3Value,
                        waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value,
                        waypoints.GetArrayElementAtIndex (i - 1).FindPropertyRelative ("position").vector3Value +
                        waypoints.GetArrayElementAtIndex (i - 1).FindPropertyRelative ("outTangent").vector3Value,
                        waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("position").vector3Value +
                        waypoints.GetArrayElementAtIndex (i).FindPropertyRelative ("inTangent").vector3Value,
                        pmo.pathColor, null, 5f);

                    // Draw in-path directions
                    if (showPathDirections && !pmo.disableOrientation)
                        DrawRotationsForSegment (pm, i);
                }

                if (!pm.FindProperty ("updateTransform").boolValue || pm.FindProperty ("target").objectReferenceValue == null) {

                    Vector3 pos;
                    Quaternion rot;
                    float vel;
                    int way;

                    if (pmo.presampledPath) {
                        pmo.sampledPositionAndRotationAndVelocityAndWaypointAtPos (pm.FindProperty ("currentPos").floatValue, out pos, out rot, out vel, out way);
                    } else {
                        pos = pmo.computePositionAtPos (pm.FindProperty ("currentPos").floatValue);
                    }

                    Handles.color = Color.red;
#if UNITY_5_5_OR_NEWER
                    Handles.DotHandleCap (0, pos, Quaternion.identity, handleSize / 10f, EventType.Repaint);   
#else
                    Handles.DotCap(0, pos, Quaternion.identity, handleSize / 10f);
#endif
                    Handles.color = Color.white;
                }
            }

            if (pm.FindProperty ("loop").boolValue && waypoints.arraySize > 1) {
                // Draw last segment
                Handles.DrawBezier (

                    waypoints.GetArrayElementAtIndex (waypoints.arraySize - 1).FindPropertyRelative ("position").vector3Value,
                    waypoints.GetArrayElementAtIndex (0).FindPropertyRelative ("position").vector3Value,
                    waypoints.GetArrayElementAtIndex (waypoints.arraySize - 1).FindPropertyRelative ("position").vector3Value + waypoints.GetArrayElementAtIndex (waypoints.arraySize - 1).FindPropertyRelative ("outTangent").vector3Value,
                    waypoints.GetArrayElementAtIndex (0).FindPropertyRelative ("position").vector3Value + waypoints.GetArrayElementAtIndex (0).FindPropertyRelative ("inTangent").vector3Value,
                    pmo.pathColor, null, 2f);

                // Draw in-path directions
                if (showPathDirections && !pmo.disableOrientation)
                    DrawRotationsForSegment (pm, 0);
            }

            // If it is a sampled path, draw sample segments
            if (pmo.presampledPath) {
                Handles.color = Color.yellow;
                for (int i = 1; i < pmo.samplesNum; i++) {
                    Handles.DrawLine (pmo.positionSamples [i - 1], pmo.positionSamples [i]);
                }
                if (showPathSamples) {
                    Handles.color = Color.cyan;
                    for (int i = 1; i < pmo.samplesNum; i++) {
                        float handleSize = HandleUtility.GetHandleSize (pmo.positionSamples [i - 1]);

#if UNITY_5_5_OR_NEWER
                        Handles.DotHandleCap (0, pmo.positionSamples [i - 1], Quaternion.identity, handleSize / 20f, EventType.Repaint);
#else
                        Handles.DotCap(0, pmo.positionSamples[i - 1], Quaternion.identity, handleSize / 20f);
#endif

                    }
                }
            }

            pm.ApplyModifiedProperties ();
        }

        /// <summary>
        /// Draws the rotations magenta indicators (arrows) for a specified segment.
        /// </summary>
        /// <param name="pm">PathMagic instance</param>
        /// <param name="index">Segment index</param>
        void DrawRotationsForSegment (SerializedObject pm, int index)
        {
            PathMagic tgt = ((PathMagic)pm.targetObject);

            float startPos = (index == 0 ? CalcPosForWaypointIndex (tgt.waypoints.Length - 1) : CalcPosForWaypointIndex (index - 1));
            float endPos = (index == 0 ? 1 : CalcPosForWaypointIndex (index));

            for (int j = 0; j < 10; j++) {

                float cPos = j * (endPos - startPos) / 10f + startPos;
                Vector3 pos;
                Quaternion rot;
                float vel;
                int way;

                if (tgt.presampledPath) {
                    tgt.sampledPositionAndRotationAndVelocityAndWaypointAtPos (cPos, out pos, out rot, out vel, out way);
                } else {
                    pos = tgt.computePositionAtPos (cPos);
                    rot = tgt.computeRotationAtPos (cPos);
                }

                float capSize = HandleUtility.GetHandleSize (pos) / 1.5f;
                Handles.color = Color.magenta;
#if UNITY_5_5_OR_NEWER
                Handles.ArrowHandleCap (0, pos, rot, capSize, EventType.Repaint);
#else
                Handles.ArrowCap(0, pos, rot, capSize);
#endif
                Handles.color = Color.white;
            }
        }

        /// <summary>
        /// Draws the waypoint direction. Draws a camera-like symbol to show in what direction 
        /// is oriented the object at waypoint position.
        /// </summary>
        /// <param name="wp">The SerializedProperty representing the waypoint.</param>
        private void DrawWaypointDirection (SerializedProperty wp)
        {
            Vector3 arrowPos = wp.FindPropertyRelative ("position").vector3Value;
            float s = 0.3f * HandleUtility.GetHandleSize (arrowPos);

            Transform t = null;
            if (wp.serializedObject.FindProperty ("globalLookAt").objectReferenceValue != null)
                t = (Transform)wp.serializedObject.FindProperty ("globalLookAt").objectReferenceValue;
            else if (wp.FindPropertyRelative ("lookAt").objectReferenceValue != null)
                t = (Transform)wp.FindPropertyRelative ("lookAt").objectReferenceValue;
            else
                t = null;

            Handles.color = Color.cyan;

            Quaternion arrowRot = ((t == null) ?
            Quaternion.Euler (wp.FindPropertyRelative ("rotation").vector3Value) :
                               Quaternion.LookRotation (((PathMagic)wp.serializedObject.targetObject).transform.InverseTransformPoint (t.position) - arrowPos, Vector3.up));

            Handles.DrawPolyLine (new Vector3[] {
                arrowPos + arrowRot * (new Vector3 (s / 2f, s / 2f, s)),
                arrowPos + arrowRot * (new Vector3 (0, s, s)),
                arrowPos + arrowRot * (new Vector3 (-s / 2f, s / 2f, s)),
                arrowPos + arrowRot * (new Vector3 (-s / 2f, -s / 2f, s)),
                arrowPos + arrowRot * (new Vector3 (s / 2f, -s / 2f, s)),
                arrowPos + arrowRot * (new Vector3 (s / 2f, s / 2f, s)),
                arrowPos + arrowRot * (new Vector3 (0, 0, 3f * s)),
                arrowPos + arrowRot * (new Vector3 (-s / 2f, -s / 2f, s))
            });
            Handles.DrawPolyLine (new Vector3[] {
                arrowPos + arrowRot * (new Vector3 (-s / 2f, s / 2f, s)),
                arrowPos + arrowRot * (new Vector3 (0, 0, 3f * s)),
                arrowPos + arrowRot * (new Vector3 (s / 2f, -s / 2f, s))
            });
            Handles.DrawLine (
                arrowPos + arrowRot * (new Vector3 (0, 0, 3f * s)),
                arrowPos + arrowRot * (new Vector3 (0, s, s))
            );
            Handles.color = Color.white;
        }

        /// <summary>
        /// Inserts the waypoint at index and align it with previous and next waypoint.
        /// </summary>
        /// <param name="index">Index at which insert the waypoint</param>
        /// <param name="align">If set to <c>true</c> align it with the previous and next waypoints.</param>
        private void InsertWaypointAt (int index, bool align)
        {
            SerializedProperty waypoints = serializedObject.FindProperty ("waypoints");

            Waypoint item = new Waypoint ();

            if (align) {
                if (index < waypoints.arraySize) {
                    // In the middle of the path
                    float pos1 = CalcPosForWaypointIndex (index - 1);
                    float pos2 = CalcPosForWaypointIndex (index);
                    float pos = (pos1 + pos2) / 2f;

                    item.position = ((PathMagic)serializedObject.targetObject).computePositionAtPos (pos);
                    item.rotation = ((PathMagic)serializedObject.targetObject).computeRotationAtPos (pos).eulerAngles;
                    item.velocity = (((PathMagic)serializedObject.targetObject).waypoints [index - 1].velocity + ((PathMagic)serializedObject.targetObject).waypoints [index].velocity) / 2f;

                    Quaternion fForward = Quaternion.LookRotation (((PathMagic)serializedObject.targetObject).computePositionAtPos (pos + 0.001f) - ((PathMagic)serializedObject.targetObject).computePositionAtPos (pos), Vector3.up);
                    item.inTangent = -1 * Vector3.forward;
                    item.outTangent = fForward * Vector3.forward;

                } else {
                    // At end of path
                    if (waypoints.arraySize > 0) {
                        item.position = ((PathMagic)serializedObject.targetObject).waypoints [index - 1].position + 5f * (GetFaceForwardForIndex (index - 1) * Vector3.forward);
                        item.rotation = GetFaceForwardForIndex (index - 1).eulerAngles;
                        item.velocity = ((PathMagic)serializedObject.targetObject).waypoints [index - 1].velocity;
                        item.inTangent = -1 * (Quaternion.Euler (item.rotation) * Vector3.forward);
                        item.outTangent = Quaternion.Euler (item.rotation) * Vector3.forward;
                        item.symmetricTangents = true;
                    } else {
                        // First point in the path
                        item.position = new Vector3 (5f, 5f, 0f);
                        item.rotation = new Vector3 (0f, 0f, 0f);
                        item.velocity = 1f;
                        item.inTangent = -1 * (Quaternion.Euler (item.rotation) * Vector3.forward);
                        item.outTangent = Quaternion.Euler (item.rotation) * Vector3.forward;
                        item.symmetricTangents = true;
                    }
                }
            }

            waypoints.InsertArrayElementAtIndex (index);

            waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("position").vector3Value = item.position;
            waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("rotation").vector3Value = item.rotation;
            waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("velocity").floatValue = item.velocity;
            waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("inTangent").vector3Value = item.inTangent;
            waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("outTangent").vector3Value = item.outTangent;
            waypoints.GetArrayElementAtIndex (index).FindPropertyRelative ("symmetricTangents").boolValue = true;

            // Select the inserted index
            currentSelectedWaypoint = index;

            // Reveal it on scene view
            if (SceneView.lastActiveSceneView != null) {
                SceneView.lastActiveSceneView.pivot = ((PathMagic)serializedObject.targetObject).transform.TransformPoint (waypoints.GetArrayElementAtIndex (currentSelectedWaypoint).FindPropertyRelative ("position").vector3Value);
            }
        }

        /// <summary>
        /// Removes the waypoint at specified index.
        /// </summary>
        /// <param name="index">Index at which remove waypoint</param>
        private void RemoveWaypointAt (int index)
        {
            serializedObject.FindProperty ("waypoints").DeleteArrayElementAtIndex (index);
        }

        /// <summary>
        /// Calculates the float position ([0..1])of a waypoint related to the whole path. This implementation
        /// does not take account of effective distances from waypoints but only waypoints number.
        /// </summary>
        /// <returns>The position for waypoint index.</returns>
        /// <param name="index">The reference index.</param>
        private float CalcPosForWaypointIndex (int index)
        {
            //return (float)index / (float)(serializedObject.FindProperty ("waypoints").arraySize - (((PathMagic)serializedObject.targetObject).loop ? 0f : 1f));
            return (float)index / (((PathMagic)target).waypoints.Length - (((PathMagic)target).loop ? 0f : 1f));
        }

        /// <summary>
        /// Computes the position for a specific waypoint. This implementation takes account of the effective
        /// distances from waypoints. The implementation takes also account of the fact that the path is
        /// pre-sampled or not.
        /// </summary>
        /// <returns>The position for waypoint.</returns>
        /// <param name="waypoint">Waypoint.</param>
        public float ComputePosForWaypoint (int waypoint)
        {
            PathMagic pm = (PathMagic)target;
            float pos = 0f;
            float step = 0.0001f;

            if (!pm.presampledPath) {
                // Compute the pos to the minWaypoint in non-pre-sampled path
                pos = CalcPosForWaypointIndex (waypoint);
                /*
				while (pm.GetWaypointFromPos (pos) != waypoint)
					pos += step;

				if (pos > 1)
					pos = 1;
				*/
            } else {
                // Compute the pos to the minWaypoint in pre-sampled path
                int i = 0;
                while (pm.WaypointSamples [i] != waypoint) {
                    pos += pm.SamplesDistances [i++];
                }

                pos /= pm.TotalDistance;


                float p = pos;
                Vector3 position;
                Quaternion rotation;
                float vel;
                int wp;
                float lastDistanceFromWaypoint;

                pm.sampledPositionAndRotationAndVelocityAndWaypointAtPos (p, out position, out rotation, out vel, out wp);

                do {
                    lastDistanceFromWaypoint = Vector3.Distance (position, pm.Waypoints [waypoint].Position);

                    p += step;
                    if (p > 1f)
                        p = 1f;

                    pm.sampledPositionAndRotationAndVelocityAndWaypointAtPos (p, out position, out rotation, out vel, out wp);
                } while (Vector3.Distance (position, pm.Waypoints [waypoint].Position) <= lastDistanceFromWaypoint && p < 1);

                pos = p;
            }

            return pos;
        }



        /// <summary>
        /// Calculates the orientation that the object should have when reaches the specified waypoint, to naturally follow the path (face forward).
        /// </summary>
        /// <returns>The face forward orientation for index.</returns>
        /// <param name="index">The index at which calculate the face forward orientation</param>
        private Quaternion GetFaceForwardForIndex (int index)
        {
            Quaternion rot;
            if (((PathMagic)serializedObject.targetObject).waypoints.Length <= 1)
                rot = Quaternion.identity;
            else {
                float pos = CalcPosForWaypointIndex (index);
                if (index < ((PathMagic)serializedObject.targetObject).waypoints.Length - 1) {
                    rot = Quaternion.LookRotation (((PathMagic)serializedObject.targetObject).computePositionAtPos (pos + 0.001f) - ((PathMagic)serializedObject.targetObject).computePositionAtPos (pos), Vector3.up);
                } else
                    rot = Quaternion.LookRotation (((PathMagic)serializedObject.targetObject).computePositionAtPos (pos) - ((PathMagic)serializedObject.targetObject).computePositionAtPos (pos - 0.001f), Vector3.up);
            }

            return rot;
        }

        /// <summary>
        /// Sets the orientation for a waypoint index so that the object will orient in face forward mode
        /// </summary>
        /// <param name="index">The index of the reference waypoint.</param>
        private void FaceForward (int index)
        {
            serializedObject.FindProperty ("waypoints").GetArrayElementAtIndex (index).FindPropertyRelative ("rotation").vector3Value = GetFaceForwardForIndex (index).eulerAngles;
        }

        /// <summary>
        /// Creates a position handle in the scene view. Used by the scene view to draw handles for bezier tangents and waypoints
        /// </summary>
        /// <returns>The position handle.</returns>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        private Vector3 PositionHandle (Vector3 position, Quaternion rotation, bool mini)
        {
            float handleSize = HandleUtility.GetHandleSize (position) / (mini ? 2f : 1f);
            Color color = Handles.color;

            bool xPresent = true;

            if (SceneView.sceneViews.Count > 0)
            if (((SceneView)SceneView.sceneViews [0]).in2DMode)
                xPresent = false;

            if (xPresent) {
                Handles.color = Handles.xAxisColor;
#if UNITY_5_5_OR_NEWER
                position = Handles.Slider (position, rotation * Vector3.right, handleSize, 
                    Handles.ArrowHandleCap, 
                    EditorPrefs.GetFloat ("MoveSnapX"));
#else
                position = Handles.Slider(position, rotation * Vector3.right, handleSize,
                    Handles.ArrowCap,
                    EditorPrefs.GetFloat("MoveSnapX"));
#endif
            }

            Handles.color = Handles.yAxisColor;
#if UNITY_5_5_OR_NEWER
            position = Handles.Slider (position, rotation * Vector3.up, handleSize, Handles.ArrowHandleCap, EditorPrefs.GetFloat ("MoveSnapY"));
#else
            position = Handles.Slider(position, rotation * Vector3.up, handleSize, Handles.ArrowCap, EditorPrefs.GetFloat("MoveSnapY"));
#endif
            Handles.color = Handles.zAxisColor;
#if UNITY_5_5_OR_NEWER
            position = Handles.Slider (position, rotation * Vector3.forward, handleSize, Handles.ArrowHandleCap, EditorPrefs.GetFloat ("MoveSnapZ"));
#else
            position = Handles.Slider(position, rotation * Vector3.forward, handleSize, Handles.ArrowCap, EditorPrefs.GetFloat("MoveSnapZ"));
#endif
            Handles.color = Handles.centerColor;

#if UNITY_5_5_OR_NEWER
            position = Handles.FreeMoveHandle (position, Quaternion.identity, handleSize * 0.15f, new Vector3 (xPresent ? 0.5f : 0f, 0.5f, 0.5f), Handles.RectangleHandleCap);
#else
            position = Handles.FreeMoveHandle(position, Quaternion.identity, handleSize * 0.15f, new Vector3(xPresent ? 0.5f : 0f, 0.5f, 0.5f), Handles.RectangleCap);
#endif
            Handles.color = color;
            return position;
        }

        /// <summary>
        /// Manages the mouse events. Ctrl-left click or CMD-left click to place
        /// the next waypoint.
        /// </summary>
        private void ManageMouseEvents (SerializedProperty wps)
        {
            PathMagic pm = (PathMagic)target;

            if (Event.current != null &&
                Event.current.isMouse &&
                Event.current.button == 0 &&
                Event.current.type == EventType.MouseDown) { // Left mouse button

                int controlId = GUIUtility.GetControlID (FocusType.Passive);

                if (Event.current.modifiers == EventModifiers.Control ||
                    Event.current.modifiers == EventModifiers.Command) {

                    // User has pressed left mouse button with CTRL/CMD modifier,
                    // please place the next waypoint at the mouse position (need to
                    // click over a collider)

                    Vector2 guiPosition = Event.current.mousePosition;
                    Ray ray = HandleUtility.GUIPointToWorldRay (guiPosition);
                    RaycastHit info;
                    if (Physics.Raycast (ray, out info)) {
                        Vector3 impact = info.point;
                        Vector3 localImpact = pm.transform.InverseTransformPoint (impact);

                        int index = wps.arraySize;
                        wps.InsertArrayElementAtIndex (index);

                        wps.GetArrayElementAtIndex (index).FindPropertyRelative ("position").vector3Value = localImpact;
                        wps.GetArrayElementAtIndex (index).FindPropertyRelative ("symmetricTangents").boolValue = true;
                        wps.GetArrayElementAtIndex (index).FindPropertyRelative ("velocity").floatValue = 1f;
                        if (index > 0) {
                            Vector3 pos = localImpact;
                            Vector3 prevPos = pm.Waypoints [index - 1].Position;
                            //Vector3 prevOutTgt = wps.GetArrayElementAtIndex (index - 1).FindPropertyRelative ("outTangent").vector3Value;

                            Vector3 inTgt = 1f / 3f * (prevPos - pos);
                            Vector3 outTgt = -1f * inTgt;

                            wps.GetArrayElementAtIndex (index).FindPropertyRelative ("inTangent").vector3Value = inTgt;
                            wps.GetArrayElementAtIndex (index).FindPropertyRelative ("outTangent").vector3Value = outTgt;


                        } else {
                            wps.GetArrayElementAtIndex (index).FindPropertyRelative ("inTangent").vector3Value = Vector3.zero;
                            wps.GetArrayElementAtIndex (index).FindPropertyRelative ("outTangent").vector3Value = Vector3.zero;
                        }

                        GUIUtility.hotControl = controlId;
                        Event.current.Use ();

                        // TODO: Better in/out tangent computation!!!!


                    } else {
                        Debug.Log ("Please CTRL/CMD-click over a collider!");
                    }
                }
            }
        }

        /// <summary>
        /// Manages the keyboard events for the scene view and the inspector view.
        /// </summary>
        private void ManageKeyboardEvents ()
        {
            Waypoint[] waypoints = ((PathMagic)target).waypoints;
            PathMagic pm = (PathMagic)target;

            if (Event.current != null &&
                Event.current.isKey &&
                Event.current.type.Equals (EventType.KeyDown)) {

                if (Event.current.keyCode == KeyCode.Return) {

                    // Reveal the waypoint if a waypoint is selected
                    if (currentSelectedWaypoint != -1) {
                        for (int i = 0; i < SceneView.sceneViews.Count; i++) {
                            SceneView sceneView = (SceneView)SceneView.sceneViews [i];
                            sceneView.pivot = pm.transform.TransformPoint (waypoints [currentSelectedWaypoint].position);
                        }

                        EditorUtility.SetDirty (target);
                        Event.current.Use ();
                    }
                }

                if (Event.current.keyCode == KeyCode.Period) {

                    // Select next waypoint
                    currentSelectedWaypoint = (currentSelectedWaypoint + 1) % waypoints.Length;
                    for (int i = 0; i < SceneView.sceneViews.Count; i++) {
                        SceneView sceneView = (SceneView)SceneView.sceneViews [i];
                        sceneView.pivot = pm.transform.TransformPoint (waypoints [currentSelectedWaypoint].position);
                    }

                    EditorUtility.SetDirty (target);
                    Event.current.Use ();
                }

                if (Event.current.keyCode == KeyCode.Comma) {

                    // Select next waypoint
                    if (currentSelectedWaypoint == -1)
                        currentSelectedWaypoint = waypoints.Length - 1;
                    else if (currentSelectedWaypoint == 0)
                        currentSelectedWaypoint = waypoints.Length - 1;
                    else
                        currentSelectedWaypoint--;

                    for (int i = 0; i < SceneView.sceneViews.Count; i++) {
                        SceneView sceneView = (SceneView)SceneView.sceneViews [i];
                        sceneView.pivot = pm.transform.TransformPoint (waypoints [currentSelectedWaypoint].position);
                    }

                    EditorUtility.SetDirty (target);
                    Event.current.Use ();
                }

                if (Event.current.keyCode == KeyCode.Escape) {

                    // Add a new waypoint after
                    currentSelectedWaypoint = -1;
                    EditorUtility.SetDirty (target);
                    //wl.ReleaseKeyboardFocus ();
                    Event.current.Use ();
                }
            }
        }

        [MenuItem ("GameObject/PathMagic/Create new PathMagic")]
        /// <summary>
        /// Creates A new instance of PathMagic in the Hierarchy view.
        /// </summary>
        /// <param name="menuCommand">Menu command.</param>
        public static void CreateNewPathMagic (MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject ("PathMagic Path");
            go.transform.rotation = Quaternion.Euler (0f, 90f, 0f);
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);

            go.AddComponent<PathMagic> ();
            Selection.activeObject = go;

            Undo.RegisterCreatedObjectUndo (go, "Create new PathMagic");
        }

        /// <summary>
        /// Bakes the animation. Creates a legacy animation with the exact path animation.
        /// The user can specify the number of samples (precision of the animation) and
        /// the total duration of the animation. The animation will animate localRotation
        /// and localPosition properties, so you can reparent the target object in a safe way.
        /// </summary>
        /// <param name="pmo">The SerializedObject of the PathMagic to bake</param>
        /// <param name="numberOfSamples">Number of samples, so the number of keyframes of the animation.</param>
        /// <param name="totalTime">Total duration time of the final created animation.</param>
        public void BakeAnimation (SerializedObject pmo, int numberOfSamples, float totalTime)
        {
            PathMagic pm = (PathMagic)pmo.targetObject;
            Animation anim;

            // Attach an Animation script if there is no one
            if ((anim = pm.Target.gameObject.GetComponent<Animation> ()) == null) {
                anim = pm.Target.gameObject.AddComponent<Animation> ();
            }

            // Create animation clip
            AnimationClip clip = new AnimationClip ();
            clip.legacy = true;

            // Declare arrays of keypoints for position and rotation
            Keyframe[] localPositionXKeys;
            Keyframe[] localPositionYKeys;
            Keyframe[] localPositionZKeys;

            Keyframe[] localRotationXKeys;
            Keyframe[] localRotationYKeys;
            Keyframe[] localRotationZKeys;
            Keyframe[] localRotationWKeys;

            // Initialize keypoints
            localPositionXKeys = new Keyframe[numberOfSamples];
            localPositionYKeys = new Keyframe[numberOfSamples];
            localPositionZKeys = new Keyframe[numberOfSamples];
            localRotationXKeys = new Keyframe[numberOfSamples];
            localRotationYKeys = new Keyframe[numberOfSamples];
            localRotationZKeys = new Keyframe[numberOfSamples];
            localRotationWKeys = new Keyframe[numberOfSamples];

            // cycle on samples
            for (int i = 0; i < numberOfSamples; i++) {

                float currentPos = 1f / (numberOfSamples - 1) * (float)i;

                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                float velocity = 1.0f;
                int waypoint = 0;

                if (pm.presampledPath) {
                    pm.sampledPositionAndRotationAndVelocityAndWaypointAtPos (currentPos, out position, out rotation, out velocity, out waypoint);
                } else {
                    position = pm.computePositionAtPos (currentPos);
                    rotation = pm.computeRotationAtPos (currentPos);
                }

                pm.UpdateTarget (position, rotation);

                localPositionXKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localPosition.x);
                localPositionYKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localPosition.y);
                localPositionZKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localPosition.z);

                localRotationXKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localRotation.x);
                localRotationYKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localRotation.y);
                localRotationZKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localRotation.z);
                localRotationWKeys [i] = new Keyframe (currentPos * totalTime, pm.Target.transform.localRotation.w);
            }

            // Define curves
            AnimationCurve localPositionXCurve = new AnimationCurve (localPositionXKeys);
            AnimationCurve localPositionYCurve = new AnimationCurve (localPositionYKeys);
            AnimationCurve localPositionZCurve = new AnimationCurve (localPositionZKeys);

            AnimationCurve localRotationXCurve = new AnimationCurve (localRotationXKeys);
            AnimationCurve localRotationYCurve = new AnimationCurve (localRotationYKeys);
            AnimationCurve localRotationZCurve = new AnimationCurve (localRotationZKeys);
            AnimationCurve localRotationWCurve = new AnimationCurve (localRotationWKeys);

            clip.SetCurve ("", typeof(Transform), "localPosition.x", localPositionXCurve);
            clip.SetCurve ("", typeof(Transform), "localPosition.y", localPositionYCurve);
            clip.SetCurve ("", typeof(Transform), "localPosition.z", localPositionZCurve);

            clip.SetCurve ("", typeof(Transform), "localRotation.x", localRotationXCurve);
            clip.SetCurve ("", typeof(Transform), "localRotation.y", localRotationYCurve);
            clip.SetCurve ("", typeof(Transform), "localRotation.z", localRotationZCurve);
            clip.SetCurve ("", typeof(Transform), "localRotation.w", localRotationWCurve);

            if (pm.Loop) {
                clip.wrapMode = WrapMode.Loop;
            } else {
                clip.wrapMode = WrapMode.Once;
            }

            string path = EditorUtility.SaveFilePanelInProject (
                              "Save Animation",
                              pm.name + "_" + pm.target.name + ".anim",
                              "anim",
                              "");

            if (path.Length != 0) {
                AssetDatabase.CreateAsset (clip, path);
                AssetDatabase.SaveAssets ();
                anim.AddClip (clip, "Path Animation");
                anim.clip = anim.GetClip ("Path Animation");
            } else {
                if (pm.Target.gameObject.GetComponent<Animation> () != null)
                    DestroyImmediate (pm.Target.gameObject.GetComponent<Animation> ());
            }
        }
    }
}