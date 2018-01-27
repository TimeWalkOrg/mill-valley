using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ControlTypes
{
	None = 0,
	FPS,
	VR
};

public class LoadingManager : MonoBehaviour
{
	#region Singleton
	private static LoadingManager _instance = null;
	public static bool hasInstance
	{
		get { _instanceCheck(); return _instance != null; }
	}
	private static LoadingManager _instanceCheck()
	{
		if (_instance == null)
			_instance = GameObject.FindObjectOfType<LoadingManager>();
		return _instance;
	}
	public static LoadingManager instance
	{
		get
		{
			_instanceCheck();
			if (_instance == null)
				Debug.LogError("<color=red>LoadingManager Not Found!</color>");
			return _instance;
		}
	}

	void OnApplicationQuit()
	{
		_instance = null;
		DestroyImmediate(gameObject);
	}
	#endregion

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

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public void SelectControllerTypeOnClick(int index)
	{
		currentControllerType = (ControlTypes)index;
		LoadMainMenu((ControlTypes)index);
	}

	private void LoadMainMenu(ControlTypes type)
	{
		if (currentControlGO != null)
			Destroy(currentControlGO);
		if (currentControlUI != null)
			Destroy(currentControlUI);

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
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
		SceneManager.UnloadSceneAsync(0);
	}
}


