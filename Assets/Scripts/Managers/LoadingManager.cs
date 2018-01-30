using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class LoadingManager : MonoBehaviour
{
	#region Singleton
	private static LoadingManager _instance = null;
	public static LoadingManager instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<LoadingManager>();
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

	public enum ControlTypes
	{
		None = 0,
		FPS,
		VR
	};

	public GameObject controllerSelectionUIGO;
	public GameObject controllerVRButtonUIGO;

	[System.Serializable]
	public struct ControllerData
	{
		public ControlTypes type;
		public GameObject[] controls;
	}
	public ControllerData[] controls;

	private GameObject currentControlGO;
	private GameObject currentControlUI;
	[HideInInspector]
	public ControlTypes currentControllerType;
	private bool isMainSceneLoaded = false;
	[HideInInspector]
	public GameObject mainSceneGO;
	private GameObject loadingSceneGO;

	private Scene mainScene;
	private Scene loadingScene;

	private AsyncOperation asyncLoaderMainScene;
	private string vrDevice;

	private void Start()
	{
		controllerSelectionUIGO.SetActive(false);
		if (XRDevice.isPresent)
		{
			vrDevice = XRDevice.model;
			Debug.Log(vrDevice);
			controllerVRButtonUIGO.SetActive(true);
		}
		else
		{
			controllerVRButtonUIGO.SetActive(false);
		}

		loadingSceneGO = GameObject.Find("LoadingSceneGO");
		loadingScene = SceneManager.GetSceneByName("LoadingScene");
		isMainSceneLoaded = false;
		StartCoroutine(LoadAsyncScene());
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	IEnumerator LoadAsyncScene()
	{
		asyncLoaderMainScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		asyncLoaderMainScene.allowSceneActivation = false;

		while (asyncLoaderMainScene.progress < 0.9f)
			yield return null;

		isMainSceneLoaded = true;
		controllerSelectionUIGO.SetActive(true);
	}

	public void SelectControllerTypeOnClick(int index)
	{
		currentControllerType = (ControlTypes)index;
		StartCoroutine(WaitUntilSceneLoaded((ControlTypes)index));
	}

	IEnumerator WaitUntilSceneLoaded(ControlTypes type)
	{
		while (!isMainSceneLoaded)
			yield return null;

		asyncLoaderMainScene.allowSceneActivation = true;

		Debug.Log("activation true");

		while (!asyncLoaderMainScene.isDone)
			yield return null;

		Debug.Log("isDone");

		mainScene = SceneManager.GetSceneByName("MainScene");

		Debug.Log("get scene");

		while (!mainScene.IsValid())
			yield return null;

		Debug.Log("isValid");

		switch (type)
		{
			case ControlTypes.None:
				if (XRSettings.isDeviceActive)
				{
					XRSettings.LoadDeviceByName("");
					yield return new WaitForEndOfFrame();
					XRSettings.enabled = false;
					yield return null;
				}
				currentControlGO = Instantiate(controls[(int)ControlTypes.None].controls[0]);
				break;
			case ControlTypes.FPS:
				if (XRSettings.isDeviceActive)
				{
					XRSettings.LoadDeviceByName("");
					yield return new WaitForEndOfFrame();
					XRSettings.enabled = false;
					yield return null;
				}
				currentControlGO = Instantiate(controls[(int)ControlTypes.FPS].controls[0]);
				currentControlUI = Instantiate(controls[(int)ControlTypes.FPS].controls[1]);
				break;
			case ControlTypes.VR:
				// TODO not init VR after disable WIP
				if (!XRSettings.isDeviceActive)
				{
					XRSettings.LoadDeviceByName(vrDevice);
					yield return new WaitForEndOfFrame();
					XRSettings.enabled = true;
					yield return null;
				}
				currentControlGO = Instantiate(controls[(int)ControlTypes.VR].controls[0]);
				break;
			default:
				break;
		}

		ToggleLoadingScene(false);
		ToggleMainScene(true);
	}

	public void ToggleLoadingScene(bool state)
	{
		if (state)
		{
			ClearControllerGO();
			SceneManager.SetActiveScene(loadingScene);
		}
		loadingSceneGO.SetActive(state);
	}

	public void ToggleMainScene(bool state)
	{
		if (state)
			SceneManager.SetActiveScene(mainScene);
		if (mainSceneGO == null)
			mainSceneGO = GameObject.Find("MainSceneGO");
		if (mainSceneGO != null)
			mainSceneGO.SetActive(state);
	}

	private void ClearControllerGO()
	{
		if (currentControlGO != null)
			Destroy(currentControlGO);
		if (currentControlUI != null)
			Destroy(currentControlUI);
	}
}
