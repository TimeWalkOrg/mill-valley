using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;

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
		Application.backgroundLoadingPriority = ThreadPriority.Low;
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
	public GameObject loadingUIGO;
	public Slider loadingSlider;
	public Image loadingImage;
	public Sprite[] loadingSprites;

	[System.Serializable]
	public struct ControllerData
	{
		public ControlTypes type;
		public GameObject[] controls;
	}
	public ControllerData[] controls;

	[HideInInspector]
	public GameObject currentControlGO;
	[HideInInspector]
	public GameObject currentControlUI;
	[HideInInspector]
	public ControlTypes currentControllerType;
	public bool IsVR { get { return (currentControllerType == ControlTypes.VR); } }

	private bool isMainSceneLoaded = false;
	[HideInInspector]
	public GameObject mainSceneGO;
	[HideInInspector]
	public bool isFirstMainSceneLoaded = false;
	[HideInInspector]
	public Transform currentPortalSpawn;
	public GameObject currentPlayerGO;
    public float imagePauseTime = 5.0f;

	private GameObject loadingSceneGO;
	private GameObject secondarySceneGO;

	private Scene mainScene;
	private Scene loadingScene;
	private Scene secondaryScene;

	private AsyncOperation asyncLoaderMainScene;
	private AsyncOperation asyncLoaderSecondaryScene;
	private bool isSecondaryLoading = false;
	private string vrDevice;

	#region mono
	private void Start()
	{
		// game started in loading scene and refs set
		if (loadingUIGO != null)
		{
			loadingUIGO.SetActive(true);
			controllerSelectionUIGO.SetActive(false);
			StartCoroutine(LoadingImages());

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
		else // spawned in main scene for testing
		{
			isMainSceneLoaded = true;
			if (XRSettings.isDeviceActive)
				SelectControllerTypeOnClick((int)ControlTypes.VR);
			else
				SelectControllerTypeOnClick((int)ControlTypes.FPS);
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
	#endregion

	IEnumerator LoadingImages()
	{
		while (loadingUIGO.activeInHierarchy)
		{
			int index = (int)Random.Range(0, loadingSprites.Length);
			loadingImage.sprite = loadingSprites[index];
			yield return new WaitForSecondsRealtime(imagePauseTime);
		}
	}

	IEnumerator LoadAsyncScene()
	{
		loadingSlider.value = 0f;
		asyncLoaderMainScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		asyncLoaderMainScene.allowSceneActivation = false;

		while (asyncLoaderMainScene.progress < 0.9f)
		{
			if (loadingSlider.value < 1f)
				loadingSlider.value += 0.005f;
			yield return null;
		}
		asyncLoaderMainScene.allowSceneActivation = true;
		yield return new WaitForEndOfFrame();
		yield return null;

		while (!asyncLoaderMainScene.isDone)
		{
			yield return null;
		}
		Debug.Log("Finished loading Build");

		isMainSceneLoaded = true;
		loadingUIGO.SetActive(false);
		controllerSelectionUIGO.SetActive(true);
	}

	public void LoadSecondaryScene(string sceneName, GameObject playerGO, Transform portalSpawn)
	{
		if (!isSecondaryLoading)
		{
			currentPlayerGO = playerGO;
			currentPortalSpawn = portalSpawn;
			StartCoroutine(LoadAsyncSecondaryScene(sceneName));
		}
	}

	IEnumerator LoadAsyncSecondaryScene(string sceneName)
	{
		isSecondaryLoading = true;
		
		asyncLoaderSecondaryScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		while (!asyncLoaderMainScene.isDone)
			yield return null;

		secondaryScene = SceneManager.GetSceneByName(sceneName);
		while (!secondaryScene.IsValid())
			yield return null;

		while (!secondaryScene.isLoaded)
			yield return null;

		SceneManager.SetActiveScene(secondaryScene);

		mainSceneGO.SetActive(false);
		isSecondaryLoading = false;
	}

	public void ExitSecondaryScene()
	{
		ToggleMainScene(true);
		MovePlayerToPortal();
		SceneManager.UnloadSceneAsync(secondaryScene);
	}

	public void MovePlayerToPortal(Transform portalSpawn = null)
	{
		if (isFirstMainSceneLoaded)
		{
			if (portalSpawn == null)
			{
				currentPlayerGO.transform.position = currentPortalSpawn.position;
				currentPlayerGO.transform.rotation = currentPortalSpawn.rotation;
			}
			else
			{
				currentPlayerGO.transform.position = portalSpawn.position;
				currentPlayerGO.transform.rotation = portalSpawn.rotation;
			}
		}
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
		yield return new WaitForEndOfFrame();
		ControlManager.instance.ToggleYear(1920);
		isFirstMainSceneLoaded = true;
	}

	public void ToggleLoadingScene(bool state)
	{
		if (loadingScene == null) return; // started in main scene so no loading scene return
		if (loadingSceneGO == null) return;

		if (state)
		{
			ClearControllerGO();
			SceneManager.SetActiveScene(loadingScene);
			if (secondaryScene.IsValid())
				SceneManager.UnloadSceneAsync(secondaryScene);
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

	public bool IsMainSceneActive()
	{
		return (mainSceneGO != null && mainSceneGO.activeInHierarchy);
	}

	private void ClearControllerGO()
	{
		if (currentControlGO != null)
			Destroy(currentControlGO);
		if (currentControlUI != null)
			Destroy(currentControlUI);
	}
}
