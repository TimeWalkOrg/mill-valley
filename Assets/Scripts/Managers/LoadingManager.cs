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

	#region mono
	private void Start()
	{
		// game started in loading scene and refs set
		if (loadingUIGO != null)
		{
			loadingUIGO.SetActive(true);
			controllerSelectionUIGO.SetActive(false);
			StartCoroutine(LoadingImages());

			controllerVRButtonUIGO.SetActive(XRDevice.isPresent);

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

		if (XRDevice.isPresent)
		{
			ControlManager.instance.EnableVR();
		}
		else
		{
			loadingUIGO.SetActive(false);
			controllerSelectionUIGO.SetActive(true);
		}
		
		isFirstMainSceneLoaded = true;
	}

	public void LoadSecondaryScene(string sceneName, GameObject playerGO, Transform portalSpawn)
	{
		if (!isSecondaryLoading)
		{
			if (playerGO == null)
			{
				// need root and boundries to move rig
				Transform tempBounds = VRTK.VRTK_SDK_Bridge.GetPlayArea();
				if (tempBounds != null)
					currentPlayerGO = tempBounds.gameObject;
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
