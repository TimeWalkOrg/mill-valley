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

	public GameObject controllerSelectionUIGO;
	public GameObject controllerVRButtonUIGO;
	public GameObject loadingUIGO;
	public Slider loadingSlider;
	public Image loadingImage;
	public Sprite[] loadingSprites;

	private bool isMainSceneLoaded = false;
	[HideInInspector]
	public GameObject mainSceneGO;
	[HideInInspector]
	public bool isFirstMainSceneLoaded = false;
	[HideInInspector]
	public Transform currentPortalSpawn;
	[HideInInspector]
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

	// Guard so the one-time VR canvas conversion is not repeated.
	private bool isVRSelectionMenuSetup = false;

	#region mono
	private void Start()
	{
		// game started in loading scene and refs set
		if (loadingUIGO != null)
		{
			loadingUIGO.SetActive(true);
			controllerSelectionUIGO.SetActive(false);
			StartCoroutine(LoadingImages());

			controllerVRButtonUIGO.SetActive(XRDeviceUtil.isPresent());

			loadingSceneGO = GameObject.Find("LoadingSceneGO");
			loadingScene = SceneManager.GetSceneByName("LoadingScene");
			isMainSceneLoaded = false;
			StartCoroutine(LoadAsyncScene());
		}
		else // spawned in main scene for testing
		{
			isMainSceneLoaded = true;
			isFirstMainSceneLoaded = true;
			mainScene = SceneManager.GetSceneByName("MainScene");
			ToggleMainScene(true);
			ControlManager.instance.EnableTestingControlType();
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

		isMainSceneLoaded = true;
		mainScene = SceneManager.GetSceneByName("MainScene");

		// XR now initializes manually (see XRBootstrap) instead of auto-init-on-startup,
		// to avoid the pre-splash main-thread deadlock. Wait for it to finish before
		// checking isPresent() so we don't wrongly fall into desktop mode. This is cheap:
		// the async scene load above already took several seconds, so XR is normally ready.
		while (!XRBootstrap.IsXRReady)
			yield return null;

		Debug.Log("[MV] LoadAsyncScene done. isPresent=" + XRDeviceUtil.isPresent() +
				  " xrActive=" + XRBootstrap.IsXRActive);
		loadingUIGO.SetActive(false);

		if (XRDeviceUtil.isPresent())
		{
			// In a headset, the 3-option desktop menu (Fly-through / Walk-through / VR) is
			// confusing and only "VR" is actually usable (the other two are desktop control
			// schemes). So skip the menu and go straight into walkable VR. EnableVR() sets up
			// head tracking and attaches VRLocomotion (left stick = walk, right stick = turn).
			Debug.Log("[MV] Headset present -> auto-starting VR mode");
			ControlManager.instance.EnableVR();
		}
		else
		{
			// Desktop: show the mouse-clickable selection menu.
			controllerSelectionUIGO.SetActive(true);
		}

		isFirstMainSceneLoaded = true;
	}

	// BUG 2 FIX (VR): The control-selection menu uses a Screen-Space Overlay canvas,
	// which is invisible in a headset and has no mouse pointer. Convert it to World
	// Space, parent it to the active VR camera so it follows the head, and place it a
	// readable distance in front of the user. Then attach VRMenuInput so the user can
	// select a mode with the controller buttons. Runs only once (idempotent).
	private void SetupVRSelectionMenu()
	{
		if (isVRSelectionMenuSetup)
			return;

		if (controllerSelectionUIGO == null)
			return;

		// Activate the VR rig now so there is a live, head-tracked center-eye camera to
		// attach the menu to. Without this the rig (and its only HMD camera) is inactive
		// until a mode is chosen, so the menu would parent to the wrong camera (e.g. the
		// desktop MainCamera) and would not be visible/head-tracked in the headset.
		Camera cam = ControlManager.instance.ActivateVRRigForMenu();
		if (cam == null)
			cam = ControlManager.instance.GetVRCamera();
		Debug.Log("[MV] SetupVRSelectionMenu cam=" + (cam != null ? cam.gameObject.name : "NULL"));

		// Convert the menu canvas to World Space so it renders in the HMD. The Canvas is a
		// PARENT of controllerSelectionUIGO (the selection UI is a child panel), so look up
		// the hierarchy first; fall back to a child search just in case.
		Canvas canvas = controllerSelectionUIGO.GetComponentInParent<Canvas>();
		if (canvas == null)
			canvas = controllerSelectionUIGO.GetComponentInChildren<Canvas>(true);
		Debug.Log("[MV] SetupVRSelectionMenu canvas=" + (canvas != null ? canvas.gameObject.name : "NULL"));
		if (canvas != null)
		{
			canvas.renderMode = RenderMode.WorldSpace;

			Transform canvasTransform = canvas.transform;
			if (cam != null)
				canvasTransform.SetParent(cam.transform, false);

			// 2m in front of the head, facing forward, scaled down so a large pixel
			// canvas (e.g. 1920x1080) reads as a reasonably sized world-space panel.
			canvasTransform.localPosition = new Vector3(0f, 0f, 2f);
			canvasTransform.localRotation = Quaternion.identity;
			canvasTransform.localScale = Vector3.one * 0.0025f;
		}

		// Attach controller-based selection input (idempotent).
		if (controllerSelectionUIGO.GetComponent<VRMenuInput>() == null)
			controllerSelectionUIGO.AddComponent<VRMenuInput>();

		isVRSelectionMenuSetup = true;
	}

	public void LoadSecondaryScene(string sceneName, GameObject playerGO, Transform portalSpawn)
	{
		if (!isSecondaryLoading)
		{
			if (playerGO == null)
			{
				currentPlayerGO = GameObject.FindGameObjectWithTag("VRPlayer");
				// need root and boundries to move rig
				//Transform tempBounds = VRTK.VRTK_SDK_Bridge.GetPlayArea();
				//if (tempBounds != null)
				//	currentPlayerGO = tempBounds.gameObject;
			}
			else
				currentPlayerGO = playerGO;

			currentPortalSpawn = portalSpawn;
			StartCoroutine(LoadAsyncSecondaryScene(sceneName));
		}
	}

	IEnumerator LoadAsyncSecondaryScene(string sceneName)
	{
		isSecondaryLoading = true;

		asyncLoaderSecondaryScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

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
				// return to main scene so move player to saved position and rotation
				currentPlayerGO.transform.position = currentPortalSpawn.position;
				currentPlayerGO.transform.rotation = currentPortalSpawn.rotation;
			}
			else
			{
				// onenable call from portal component so use passed position and rotation
				currentPlayerGO.transform.position = portalSpawn.position;
				currentPlayerGO.transform.rotation = portalSpawn.rotation;
			}
		}
	}

	public void ToggleLoadingScene(bool state)
	{
		if (loadingScene == null) return; // started in main scene so no loading scene return
		if (loadingSceneGO == null) return;

		if (state)
		{
			ControlManager.instance.DisableAllControlTypes();
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
		{
			FinderComponent tempRef = FindObjectOfType<FinderComponent>();
			if (tempRef != null)
			{
				mainSceneGO = tempRef.mainSceneGO;
			}
		}
		if (mainSceneGO != null)
			mainSceneGO.SetActive(state);
	}

	public bool IsMainSceneActive()
	{
		return (mainSceneGO != null && mainSceneGO.activeInHierarchy);
	}
}
