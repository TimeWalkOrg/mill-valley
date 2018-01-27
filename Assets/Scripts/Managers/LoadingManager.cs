using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
	#region Singleton
	private static LoadingManager _instance = null;
	public static LoadingManager instance
	{
		get
		{
			if (_instance == null)
				GameObject.FindObjectOfType<LoadingManager>();
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

	[System.Serializable]
	public struct ControllerData
	{
		public ControlTypes type;
		public GameObject[] controls;
	}
	public ControllerData[] controls;

	private GameObject currentControlGO;
	private GameObject currentControlUI;
	private ControlTypes currentControllerType;
	private bool isMainSceneLoaded = false;

	AsyncOperation asyncLoaderMainScene;

	private void Start()
	{
		isMainSceneLoaded = false;
		StartCoroutine(LoadAsyncScene());
	}

	private void OnEnable()
	{
		// TODO reload main scene if restarted from main scene into loading scene
	}

	IEnumerator LoadAsyncScene()
	{
		asyncLoaderMainScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		asyncLoaderMainScene.allowSceneActivation = false;

		while (asyncLoaderMainScene.progress < 0.9f)
			yield return null;

		yield return new WaitForEndOfFrame();

		isMainSceneLoaded = true;
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

		while (!asyncLoaderMainScene.isDone)
			yield return null;

		ClearControllerGO();
		switch (type)
		{
			case ControlTypes.None:
				currentControlGO = Instantiate(controls[(int)ControlTypes.None].controls[0]);
				break;
			case ControlTypes.FPS:
				currentControlGO = Instantiate(controls[(int)ControlTypes.FPS].controls[0]);
				currentControlUI = Instantiate(controls[(int)ControlTypes.FPS].controls[1]);
				break;
			case ControlTypes.VR:
				currentControlGO = Instantiate(controls[(int)ControlTypes.VR].controls[0]);
				break;
			default:
				break;
		}

		Scene mainScene = SceneManager.GetSceneByName("MainScene");
		while (!mainScene.IsValid())
			yield return null;

		SceneManager.SetActiveScene(mainScene);
		SceneManager.UnloadScene(0);
	}

	public void ClearControllerGO()
	{
		if (currentControlGO != null)
			Destroy(currentControlGO);
		if (currentControlUI != null)
			Destroy(currentControlUI);
	}
}
