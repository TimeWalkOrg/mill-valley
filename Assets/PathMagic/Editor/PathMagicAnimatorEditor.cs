using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace Jacovone
{
    [CanEditMultipleObjects]
    [CustomEditor (typeof(Jacovone.PathMagicAnimator))]
    /// <summary>
/// Path magic editor. This i sthe editor class for PathMagic instances. It defines the inspector behavios to
/// configure a complete PathMagic instance.
/// </summary>
	public class PathMagicAnimatorEditor : Editor
    {
        /// <summary>
        /// Stores the state of preview section foldout.
        /// </summary>
        private bool previewFoldout = true;
    
        /// <summary>
        /// Stores the state of events section foldout.
        /// </summary>
        private bool eventsFoldout = true;

        /// <summary>
        /// A style for bold foldouts
        /// </summary>
        GUIStyle boldFoldoutStyle;
    
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
        GUIStyle rightMiniButton;

        /// <summary>
        /// The path magic logo. Loaded from resources.
        /// </summary>
        private Texture2D pathMagicLogo = null;

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        void OnEnable ()
        {
            pathMagicLogo = Resources.Load ("pathmagiclogo") as Texture2D;
        }

        /// <summary>
        /// Raises the OnInspectorGUI event.
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
			
            EditorGUILayout.LabelField (new GUIContent (pathMagicLogo), GUILayout.Width (142), GUILayout.Height (28));

            // Set the coordinate space matrix
            EditorGUIUtility.labelWidth = 120;
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("pathMagic"), new GUIContent ("PathMagic Path", "The reference PathMagic path"));
            serializedObject.ApplyModifiedProperties ();

            if (serializedObject.FindProperty ("pathMagic").objectReferenceValue == null) {
                EditorGUILayout.HelpBox ("You have to connect a PathMagic instance to animate this transform.", MessageType.Info);
                return;
            }

            EditorGUILayout.Separator ();

            EditorGUILayout.BeginVertical ("Box");
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("updateMode"), new GUIContent ("Update Transform", "Defines when actually update the transform"));

            EditorGUIUtility.labelWidth = 90;
        
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("autoStart"), new GUIContent ("Start on play", "If true, the path will be animated at the start"));

            EditorGUILayout.Slider (serializedObject.FindProperty ("velocityBias"), -5f, 5f, new GUIContent ("Velocity Bias", "Global adjustment of the animation velocity"));

            EditorGUIUtility.labelWidth = 160;

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("disableOrientation"), new GUIContent ("Disable Orientation", "If true, the object's rotation will not be modified"));
            EditorGUILayout.PropertyField (
                serializedObject.FindProperty ("disablePosition"), new GUIContent ("Disable Position", "If set, the object's position will not be modified"));
            
            if (serializedObject.FindProperty ("disableOrientation").boolValue) {
                GUI.enabled = false;
            }

            // Disable override global follow path if the path is pre-sampled
            if (((PathMagicAnimator)target).pathMagic.presampledPath) {
                GUI.enabled = false;
                serializedObject.FindProperty ("globalFollowPath").boolValue = false;
            }

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("globalFollowPath"), new GUIContent ("Override Global Follow Path", "Global Follow Path only for this animator"));

            GUI.enabled = true;

            if (((PathMagicAnimator)target).pathMagic.presampledPath) {
                EditorGUILayout.HelpBox ("Override global follow path on pre-sampled path is not supported.", MessageType.Info);
            }

            if (serializedObject.FindProperty ("globalFollowPath").boolValue || serializedObject.FindProperty ("disableOrientation").boolValue)
                GUI.enabled = false;
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("globalLookAt"), new GUIContent ("Override Global Look At", "Global Look At only for this animator"));
            EditorGUIUtility.labelWidth = 90;

            GUI.enabled = true;

            EditorGUILayout.EndVertical (); // Box

            EditorGUILayout.Separator ();

            if (!serializedObject.isEditingMultipleObjects) {
                eventsFoldout = EditorGUILayout.Foldout (eventsFoldout, "Events", boldFoldoutStyle);
                if (eventsFoldout) {
                    EditorGUILayout.PropertyField (serializedObject.FindProperty ("waypointChanged"));
                }
            }

            EditorGUILayout.BeginHorizontal ();
            previewFoldout = EditorGUILayout.Foldout (previewFoldout, "Preview", boldFoldoutStyle);
            if (!((PathMagicAnimator)serializedObject.targetObject).isPlaying) {
                if (GUILayout.Button (new GUIContent ("Rewind", "Go to the begin of the animation"), actionButtonStyleLeft)) {
                    for (int i = 0; i < targets.Length; i++) {
                        ((PathMagicAnimator)targets [i]).Rewind ();
                        UpdateTarget ((PathMagicAnimator)targets [i]);
                    }
                }
            } else {
                if (GUILayout.Button (new GUIContent ("Stop", "Turn off animation simulation and go to the begin"), actionButtonStyleLeft)) {
                    for (int i = 0; i < targets.Length; i++) {
                        ((PathMagicAnimator)targets [i]).Stop ();
                        UpdateTarget ((PathMagicAnimator)targets [i]);
                    }
                }
            }
            if (!((PathMagicAnimator)serializedObject.targetObject).isPlaying) {
                if (GUILayout.Button (new GUIContent ("Start", "Starts the animation simulation"), actionButtonStyleRight)) {
                    for (int i = 0; i < targets.Length; i++) {
                        ((PathMagicAnimator)targets [i]).Play ();
                        UpdateTarget ((PathMagicAnimator)targets [i]);
                    }
                }
            } else {
                if (GUILayout.Button (new GUIContent ("Pause", "Pause the animation simulation"), actionButtonStyleRight)) {
                    for (int i = 0; i < targets.Length; i++) {
                        ((PathMagicAnimator)targets [i]).Pause ();
                        UpdateTarget ((PathMagicAnimator)targets [i]);
                    }
                }
            }
            EditorGUILayout.EndHorizontal ();
            if (previewFoldout) {
        
                EditorGUILayout.BeginVertical ("Box");
                EditorGUILayout.BeginHorizontal ();
            
                EditorGUIUtility.labelWidth = 70;

                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.Slider (serializedObject.FindProperty ("currentPos"), 0f, 1f, new GUIContent ("Position", "Seek a specific point in the path"));
                if (EditorGUI.EndChangeCheck ()) {
                    for (int i = 0; i < targets.Length; i++) {
                        UpdateTarget ((PathMagicAnimator)targets [i]);
                    }
                }
        
                EditorGUILayout.EndHorizontal ();
                EditorGUILayout.EndVertical (); //box
            }
        
            serializedObject.ApplyModifiedProperties ();
        }

        void UpdateTarget (PathMagicAnimator pathMagicAnimator)
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float velocity = 1.0f;
            int waypoint = 0;

            if (pathMagicAnimator.pathMagic.presampledPath) {
                pathMagicAnimator.pathMagic.sampledPositionAndRotationAndVelocityAndWaypointAtPos (pathMagicAnimator.currentPos, out position, out rotation, out velocity, out waypoint);
            } else {
                position = pathMagicAnimator.pathMagic.computePositionAtPos (pathMagicAnimator.currentPos);
                rotation = pathMagicAnimator.pathMagic.computeRotationAtPos (pathMagicAnimator.currentPos);
                velocity = pathMagicAnimator.pathMagic.computeVelocityAtPos (pathMagicAnimator.currentPos);
                waypoint = pathMagicAnimator.pathMagic.GetWaypointFromPos (pathMagicAnimator.currentPos);
            }

            if (pathMagicAnimator.globalFollowPath) {
                // Global follow path override
                rotation = pathMagicAnimator.pathMagic.GetFaceForwardForPos (pathMagicAnimator.currentPos);
            } else if (pathMagicAnimator.globalLookAt != null) {
                // Global look at override
                rotation = Quaternion.LookRotation (pathMagicAnimator.pathMagic.transform.InverseTransformPoint (pathMagicAnimator.globalLookAt.position) - position);
            }

            pathMagicAnimator.UpdateTarget (position, rotation);
        }

        /// <summary>
        /// Raises the OnSceneGUI event.
        /// </summary>
        void OnSceneGUI ()
        { 
        }
    }
}