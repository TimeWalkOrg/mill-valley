using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SpatialTracking;

[System.Serializable]
public class YearData
{
	public int year;
	public string yearLabel;
	public AudioClip audioClip;
}

public enum ControllerType
{
	Right,
	Left
};

public enum ButtonType
{
	Trigger,
	Grip,
	TouchPad,
	ButtonOne,
	ButtonTwo,
	StartMenu
};

public enum ControlType
{
	None = 0,
	FPS,
	VR
};

public class YearDataMissive : Missive
{
	public YearData data;
}

public class HelpMissive : Missive
{
	public bool state;
}

public class AudioMissive : Missive
{
	//
}

public class CreditsMissive : Missive
{
	//
}

public class WebViewMissive : Missive
{
	public string url = "";
}

public class InputDataMissive : Missive
{
	public ControllerType controllerType;
	public ButtonType buttonType;
}

public class ControlSelectMissive : Missive
{
	public ControlType controlType;
}

public class ControlManager : MonoBehaviour
{
	#region Singleton
	private static ControlManager _instance = null;
	public static ControlManager instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<ControlManager>();
			return _instance;
		}
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			_instance = this;
		}
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnApplicationQuit()
	{
		_instance = null;
		DestroyImmediate(gameObject);
	}
	#endregion

	public YearData[] yearData;

	public int currentYear { get; private set; }
	public int currentYearIndex { get; private set; }
	public timeWalkDayNightToggle dayNightRef { get; set; }

	[System.Serializable]
	public struct ControllerData
	{
		public ControlType type;
		public GameObject[] controlObjects;
	}
	public ControllerData[] controls;
	[HideInInspector]
	public GameObject currentControlGO;
	[HideInInspector]
	public GameObject currentControlUI;
	[HideInInspector]
	public ControlType currentControlType;
	public bool IsVR { get { return XRDeviceUtil.isPresent(); } }

	public ControlType testingControlType;
	private Dictionary<string, bool> _prevButtons = new Dictionary<string, bool>();

	// True once the user has actually chosen a control mode. Until then the in-game VR
	// button shortcuts (credits/night/year) must NOT fire, otherwise pressing A/B on the
	// selection menu both picks a mode AND triggers an in-game action. See Update().
	private bool _modeSelected = false;

	#region mono
	private void Start()
	{
		Missive.AddListener<ControlSelectMissive>(OnControlSelect);
		Missive.AddListener<InputDataMissive>(OnInput);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.R))
			ToggleMenu();

		if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Escape))
			ToggleQuit();

//		if (!LoadingManager.instance.IsMainSceneActive())
//			return;

		if (Input.GetKeyUp(KeyCode.Y))
			ToggleYear();

		if (Input.GetKeyUp(KeyCode.N))
			ToggleNight();

		if (Input.GetKeyUp(KeyCode.H))
			ToggleHelp();

		if (Input.GetKeyUp(KeyCode.C))
			ToggleCredits();

		if (_modeSelected && XRDeviceUtil.isPresent())
		{
			InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
			InputDevice left  = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

			if (GetButtonUp(right, CommonUsages.primaryButton,   "right_A")) ToggleCredits();
			if (GetButtonUp(right, CommonUsages.secondaryButton, "right_B")) ToggleNight();
			if (GetButtonUp(left,  CommonUsages.primaryButton,   "left_X"))  ToggleYear(-2);
			if (GetButtonUp(left,  CommonUsages.secondaryButton, "left_Y"))  ToggleYear();
		}

	}

	private void OnDestroy()
	{
		Missive.RemoveListener<ControlSelectMissive>(OnControlSelect);
		Missive.RemoveListener<InputDataMissive>(OnInput);
	}
	#endregion

	#region logic
	public void SetCurrentTime(YearData data)
	{
		for (int i = 0; i < yearData.Length; i++)
		{
			if (data.year == yearData[i].year)
			{
				currentYear = data.year;
				currentYearIndex = i;
				return;
			}
		}
		// default if not correct year
		currentYear = yearData[0].year;
		currentYearIndex = 0;
	}
	#endregion

	#region toggles
	public void ToggleYear(int year = -1)
	{
		// if in a secondary scene disable year changes uncomment if wanted
		//if (!LoadingManager.instance.IsMainSceneActive()) return;

		if (year == -2) // --year
		{
			currentYearIndex = (currentYearIndex > 0) ? currentYearIndex - 1 : yearData.Length - 1;
			currentYear = yearData[currentYearIndex].year;
		}
		else if (year == -1) // ++year
		{
			currentYearIndex = (currentYearIndex >= yearData.Length-1) ? 0 : currentYearIndex + 1;
			currentYear = yearData[currentYearIndex].year;
		}
		else
		{
			// set year
			for (int i = 0; i < yearData.Length; i++)
			{
				if (year == yearData[i].year)
				{
					currentYear = year;
					currentYearIndex = i;
					SendYearDataMissive(yearData[currentYearIndex]);
					return;
				}
			}
			// default if not correct year
			currentYear = yearData[0].year;
			currentYearIndex = 0;
		}

		SendYearDataMissive(yearData[currentYearIndex]);
		SendAudioMissive();
	}

	private void ToggleNight()
	{
		if (dayNightRef == null)
			dayNightRef = FindObjectOfType<timeWalkDayNightToggle>();
		if (dayNightRef != null)
			dayNightRef.ToggleDayNight();
	}

	private void ToggleHelp()
	{
		if (currentControlType != ControlType.FPS)
			return;

		SendHelpMissive();
	}

	private void ToggleMenu()
	{
		LoadingManager.instance.ToggleLoadingScene(true);
		LoadingManager.instance.ToggleMainScene(false);
	}

	private void ToggleCredits()
	{
		SendCreditsMissive();
	}

	private void ToggleQuit()
	{
		Application.Quit();
	}
	#endregion

	#region inputs
	public void OnInput(InputDataMissive missive)
	{
		// Not used atm after VRTK removed
		// OnInput and InputDataMissive code can be deleted
		// Keep if VRTK or you need a missive input system

		if (missive == null) return;
		switch (missive.controllerType)
		{
			case ControllerType.Right:
				switch (missive.buttonType)
				{
					case ButtonType.Trigger:
						break;
					case ButtonType.Grip:
						break;
					case ButtonType.TouchPad:
						break;
					case ButtonType.ButtonOne:
						// Oculus A
						ToggleCredits();
						break;
					case ButtonType.ButtonTwo:
						// Oculus B
						ToggleNight();
						break;
					case ButtonType.StartMenu:
						break;
					default:
						break;
				}
				break;
			case ControllerType.Left:
				switch (missive.buttonType)
				{
					case ButtonType.Trigger:
						break;
					case ButtonType.Grip:
						break;
					case ButtonType.TouchPad:
						break;
					case ButtonType.ButtonOne:
						// Oculus X
						ToggleYear(-2);
						break;
					case ButtonType.ButtonTwo:
						// Oculus Y
						ToggleYear();
						break;
					case ButtonType.StartMenu:
						break;
					default:
						break;
				}
				break;
			default:
				break;
		}
		//Debug.Log("Input received: " + missive.controllerType.ToString() + " / " + missive.buttonType.ToString());
	}

	private bool GetButtonUp(InputDevice device, InputFeatureUsage<bool> usage, string key)
	{
		bool cur = false;
		device.TryGetFeatureValue(usage, out cur);
		bool prev = _prevButtons.ContainsKey(key) && _prevButtons[key];
		_prevButtons[key] = cur;
		return prev && !cur;
	}
	#endregion

	#region control type
	private void OnControlSelect(ControlSelectMissive missive)
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);

		currentControlType = missive.controlType;
		_modeSelected = true;

		// Enable control type
		switch (missive.controlType)
		{
			case ControlType.None:
				currentControlGO = controls[0].controlObjects[0];
				currentControlUI = null;
				break;
			case ControlType.FPS:
				currentControlGO = controls[1].controlObjects[0];
				currentControlUI = controls[1].controlObjects[1];
				break;
			case ControlType.VR:
				currentControlGO = controls[2].controlObjects[0];
				currentControlUI = null;
				break;
			default:
				break;
		}

		if (currentControlGO != null)
			currentControlGO.SetActive(true);
		if (currentControlUI != null)
			currentControlUI.SetActive(true);

		LoadingManager.instance.ToggleLoadingScene(false);
		LoadingManager.instance.ToggleMainScene(true);
		ToggleYear(1920);

		// REGRESSION FIX (OVR->XR migration gap): VR can also be entered through this
		// missive/mouse path (the on-screen VR button), not just EnableVR(). Without this
		// the center-eye camera would again lack a head-pose driver. See ConfigureVRCamera().
		if (missive.controlType == ControlType.VR)
			ConfigureVRCamera();
	}

	public void EnableTestingControlType()
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);

		currentControlType = testingControlType;
		_modeSelected = true;

		if (currentControlType == ControlType.VR && !XRDeviceUtil.isPresent())
			currentControlType = ControlType.FPS;
		// Enable control type
		switch (currentControlType)
		{
			case ControlType.None:
				currentControlGO = controls[0].controlObjects[0];
				currentControlUI = null;
				break;
			case ControlType.FPS:
				currentControlGO = controls[1].controlObjects[0];
				currentControlUI = controls[1].controlObjects[1];
				break;
			case ControlType.VR:
				currentControlGO = controls[2].controlObjects[0];
				currentControlUI = null;
				break;
			default:
				break;
		}

		if (currentControlGO != null)
			currentControlGO.SetActive(true);
		if (currentControlUI != null)
			currentControlUI.SetActive(true);

		LoadingManager.instance.ToggleLoadingScene(false);
		LoadingManager.instance.ToggleMainScene(true);
		ToggleYear(1920);

		// REGRESSION FIX (OVR->XR migration gap): when this testing path selects VR,
		// the center-eye camera still has no head-pose driver, so head tracking is
		// missing/inverted and background buildings z-fight. Repair it now that the
		// rig is active. See ConfigureVRCamera() for details.
		if (currentControlType == ControlType.VR)
			ConfigureVRCamera();
	}

	// REGRESSION FIX (OVR->XR migration, Unity 2020.3 upgrade): the old Oculus OVR
	// SDK was deleted but its head-pose driver was never replaced, so the VR rig's
	// center-eye camera (OVRCameraRig/CenterEyeAnchor) is no longer driven by the
	// HMD. Symptoms: head tracking missing or inverted (BUG 1) because nothing maps
	// the CenterEye XRNode pose onto the camera transform. We add the package-provided
	// UnityEngine.SpatialTracking.TrackedPoseDriver at runtime to restore tracking.
	// We also raise the near clip plane to 0.3 to improve depth-buffer precision and
	// reduce z-fighting/flashing on distant background buildings (BUG 3) caused by the
	// old extreme 0.1/1000 (10000:1) near/far ratio.
	// Idempotent: safe to call multiple times (re-uses an existing driver).
	private void ConfigureVRCamera()
	{
		if (!XRDeviceUtil.isPresent())
			return;

		Camera cam = GetVRCamera();
		if (cam == null)
			return;

		// Ensure a TrackedPoseDriver exists (idempotent) and drives the head pose.
		TrackedPoseDriver tpd = cam.GetComponent<TrackedPoseDriver>();
		if (tpd == null)
			tpd = cam.gameObject.AddComponent<TrackedPoseDriver>();

		tpd.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.Center);
		tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
		tpd.UseRelativeTransform = false;

		// Raise near clip to improve depth precision (only if currently too small).
		if (cam.nearClipPlane < 0.3f)
			cam.nearClipPlane = 0.3f;

		// CRITICAL: disable any OTHER camera that renders to the HMD. MainScene contains a
		// stray root-level stereo camera ("CameraNEW", a Cinemachine-driven camera from the
		// CinemachineNEW object) that is NOT under the VR rig. With two stereo cameras live,
		// that stationary Cinemachine camera dominates the headset view, so the player sees a
		// fixed viewpoint even though the rig (and CenterEyeAnchor) are moving. Leave only the
		// rig's center-eye camera rendering to the headset.
		Camera[] sceneCams = FindObjectsOfType<Camera>();
		foreach (Camera other in sceneCams)
		{
			if (other == null || other == cam)
				continue;
			if (other.targetTexture != null)
				continue; // leave render-to-texture cameras (e.g. UI/portals) alone
			if (other.stereoTargetEye == StereoTargetEyeMask.Both)
			{
				other.enabled = false;
				AudioListener al = other.GetComponent<AudioListener>();
				if (al != null) al.enabled = false; // avoid "2 audio listeners" + keep rig's
				Debug.Log("[MV] Disabled competing HMD camera: " + other.gameObject.name);
			}
		}

		// In VR mode, ensure the rig can be moved with the thumbsticks. VRLocomotion is
		// not attached to the rig prefab, so add it at runtime to the rig root (moving its
		// transform moves the whole rig, camera included). Idempotent. Skipped during the
		// menu preview because currentControlType isn't VR until a mode is chosen.
		if (currentControlType == ControlType.VR && currentControlGO != null)
		{
			if (currentControlGO.GetComponent<VRLocomotion>() == null)
				currentControlGO.AddComponent<VRLocomotion>();
		}
	}

	// Returns the VR rig's center-eye camera (controls[2].controlObjects[0]), or null.
	// We deliberately avoid Camera.main: the rig has multiple GameObjects tagged
	// MainCamera, so Camera.main is ambiguous. Prefer the GameObject named
	// "CenterEyeAnchor" (the only enabled camera in the OVRCameraRig), otherwise fall
	// back to the first camera found. Works whether or not the rig is currently active
	// (GetComponentsInChildren(true) includes inactive children).
	public Camera GetVRCamera()
	{
		if (controls == null || controls.Length < 3)
			return null;
		if (controls[2].controlObjects == null || controls[2].controlObjects.Length < 1)
			return null;

		GameObject rig = controls[2].controlObjects[0];
		if (rig == null)
			return null;

		Camera[] cameras = rig.GetComponentsInChildren<Camera>(true);
		foreach (Camera c in cameras)
		{
			if (c != null && c.gameObject.name == "CenterEyeAnchor")
				return c;
		}
		foreach (Camera c in cameras)
		{
			if (c != null)
				return c;
		}
		return null;
	}

	public void EnableVR()
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);

		currentControlType = ControlType.VR;
		_modeSelected = true;

		// Enable control type
		currentControlGO = controls[2].controlObjects[0];
		currentControlUI = null;

		if (currentControlGO != null)
			currentControlGO.SetActive(true);
		if (currentControlUI != null)
			currentControlUI.SetActive(true);

		LoadingManager.instance.ToggleLoadingScene(false);
		LoadingManager.instance.ToggleMainScene(true);
		ToggleYear(1920);

		// REGRESSION FIX (OVR->XR migration gap): restore head tracking and depth
		// precision on the now-active VR rig. See ConfigureVRCamera() for details.
		ConfigureVRCamera();
	}

	public void DisableAllControlTypes()
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);
		currentControlGO = null;
		currentControlUI = null;
		for (int i = 0; i < controls.Length; i++)
		{
			for (int j = 0; j < controls[i].controlObjects.Length; j++)
			{
				controls[i].controlObjects[j].SetActive(false);
			}
		}
	}
	#endregion

	#region missives
	private void SendYearDataMissive(YearData data)
	{
		YearDataMissive missive = new YearDataMissive();
		missive.data = data;
		Missive.Send(missive);
	}

	private void SendHelpMissive()
	{
		HelpMissive missive = new HelpMissive();
		Missive.Send(missive);
	}

	private void SendAudioMissive()
	{
		AudioMissive missive = new AudioMissive();
		Missive.Send(missive);
	}

	private void SendCreditsMissive()
	{
		CreditsMissive missive = new CreditsMissive();
		Missive.Send(missive);
	}

	public void SendWebViewMissive(string url)
	{
		WebViewMissive missive = new WebViewMissive();
		missive.url = url;
		Missive.Send(missive);
	}
	#endregion
}