using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

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
	public bool IsVR { get { return (false); } }
	//	public bool IsVR { get { return (XRDevice.isPresent); } }

	public ControlType testingControlType;

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

		// TODO: Reinstate XR code below
		/*
		if (UnityEngine.XR.XRSettings.enabled)
		{
			OVRInput.Button oculusTouchButtonA = OVRInput.Button.One;
			OVRInput.Button oculusTouchButtonB = OVRInput.Button.Two;
			OVRInput.Button oculusTouchButtonC = OVRInput.Button.Three;
			OVRInput.Button oculusTouchButtonD = OVRInput.Button.Four;

			OVRInput.Button oculusTouchButtonE = OVRInput.Button.PrimaryThumbstick;


			OVRInput.Controller activeController = OVRInput.GetActiveController();

			if (OVRInput.GetUp(oculusTouchButtonA))
			{
				ToggleCredits();
			}

			if (OVRInput.GetUp(oculusTouchButtonB))
			{
				ToggleNight();
			}

			if (OVRInput.GetUp(oculusTouchButtonC))
			{
				ToggleYear(-2);
			}

			if (OVRInput.GetUp(oculusTouchButtonD))
			{
				ToggleYear();
			}

			if (OVRInput.GetUp(oculusTouchButtonE))
			{
				// not used
			}
		}
		*/

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
	#endregion

	#region control type
	private void OnControlSelect(ControlSelectMissive missive)
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);

		currentControlType = missive.controlType;
		
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
	}

	public void EnableTestingControlType()
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);

		currentControlType = testingControlType;

		// check for actual VR isPresent
		// TODO: Reinstate XRDevice test below
		//if (currentControlType == ControlType.VR && !XRDevice.isPresent)
		//	currentControlType = ControlType.FPS;
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
	}

	public void EnableVR()
	{
		if (currentControlGO != null)
			currentControlGO.SetActive(false);
		if (currentControlUI != null)
			currentControlUI.SetActive(false);

		currentControlType = ControlType.VR;
		
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