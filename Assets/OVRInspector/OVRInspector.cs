/************************************************************************************

Copyright   :   Copyright 2017 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.4.1 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

https://developer.oculus.com/licenses/sdk-3.4.1


Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

using System.Runtime.InteropServices; // required for DllImpor


public class OVRInspector : MonoBehaviour
{
    // Singleton instance
    public static OVRInspector instance { get; private set; }

    /// <summary>
    /// fader object for screen in and out
    /// </summary>
    public OVRScreenFade fader
    {
        get;

        private set;
    }

    // OVR SDK Objects for convenience
    public OVRPlayerController playerController { get; private set; }
    static public OVRCameraRig cameraRig 
    {
        get
        {
            return GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        }
    }
    public GameObject leftCamera { get; private set; }
    public GameObject rightCamera { get; private set; }
    public OVRManager manager { get; private set; }
    public Transform centerEyeTransform { get; private set; }

    // Input module
    //private OVRInputModule inputModule;

    // Prefabs
    //private EventSystem eventSystemPrefab;

	// Unity layer containing the player collider
	//private int playerLayer;


	#region StartUpFunctions 
	void Awake()
    {
		if (instance != null)
        {
            Debug.LogError("Existing OVRInspector");
            GameObject.Destroy(gameObject);
            return;
        }
        instance = this;
		
        //Find prefabs
        //eventSystemPrefab = (EventSystem)Resources.Load("Prefabs/EventSystem", typeof(EventSystem));
		
        // Pre-level stuff
        OnAwakeOrLevelLoad();
		
    }

    void OnAwakeOrLevelLoad()
    {
        if (instance != this)
            return;

        OVRManager.display.RecenterPose();

        AssignCameraRig();
        AssignFader();
    }

    public void AssignFader()
    {
        // make sure we have a new fader object
        fader = cameraRig.GetComponentInChildren<OVRScreenFade>();

        if (fader == null)
			fader = cameraRig.centerEyeAnchor.gameObject.AddComponent<OVRScreenFade>();

        // Make sure legacy fader objects are not present
        if (cameraRig.leftEyeAnchor.GetComponent<OVRScreenFade>() != null ||
            cameraRig.rightEyeAnchor.GetComponent<OVRScreenFade>() != null)
        {
            Debug.LogError("Camera rig has ScreenFade objects");
        }
    }
    
    public void AssignCameraRig()
    {
        FindPlayerAndCamera();
        // There has to be an event system for the GUI to work
   //     EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
   //     if (eventSystem == null)
   //     {
   //         Debug.Log("Creating EventSystem");
			//eventSystem = (EventSystem)GameObject.Instantiate(eventSystemPrefab);
   //     }
   //     else
   //     {
   //         //and an OVRInputModule
   //         if (eventSystem.GetComponent<OVRInputModule>() == null)
   //         {
   //             eventSystem.gameObject.AddComponent<OVRInputModule>();
   //         }
   //     }
   //     inputModule = eventSystem.GetComponent<OVRInputModule>();

        playerController = FindObjectOfType<OVRPlayerController>();
        
        cameraRig.EnsureGameObjectIntegrity();
    }

    void FindPlayerAndCamera()
    {
        playerController = FindObjectOfType<OVRPlayerController>();
        //if (playerController && playerController.gameObject.layer != playerLayer)
        //{
        //    Debug.LogError("PlayerController should be layer \"Player\"");
        //}


        if (cameraRig)
        {
            Transform t = cameraRig.transform.Find("TrackingSpace");
            centerEyeTransform = t.Find("CenterEyeAnchor");
        }

        manager = FindObjectOfType<OVRManager>();
    }
	#endregion
	
    // Update is called once per frame
    void Update()
    {
		OVRInput.Controller activeController = OVRInput.GetActiveController();
		Transform activeTransform = cameraRig.centerEyeAnchor;

		if ((activeController == OVRInput.Controller.LTouch) || (activeController == OVRInput.Controller.LTrackedRemote))
			activeTransform = cameraRig.leftHandAnchor;

		if ((activeController == OVRInput.Controller.RTouch) || (activeController == OVRInput.Controller.RTrackedRemote))
			activeTransform = cameraRig.rightHandAnchor;

		if (activeController == OVRInput.Controller.Touch)
			activeTransform = cameraRig.rightHandAnchor;
        
		//OVRGazePointer.instance.rayTransform = activeTransform;
		//inputModule.rayTransform = activeTransform;
    }
	
    #region Top Level Menu Features
    /// <summary>
    /// Fade out and quit. Unless in the editor in which case just stop playing
    /// </summary>
    public void DoQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        StartCoroutine(QuitApp());
#endif
    }

    /// <summary>
    /// Fade out and quit
    /// </summary>
    /// <returns></returns>
    IEnumerator QuitApp()
    {
        fader.SetUIFade(1);
        yield return null;
        Application.Quit();

    }
    
    void Recenter()
    {
		OVRManager.display.RecenterPose();
    }
    #endregion Top Level Menu Features 
	
    #region UI Graphical Effects
    public IEnumerator FadeOutCameras()
    {
        fader.FadeOut();
        while (fader.currentAlpha < 1)
            yield return null;
    }
    #endregion UI Graphical Effects
}
