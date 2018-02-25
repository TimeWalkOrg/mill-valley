using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderComponent : MonoBehaviour
{
	public GameObject mainSceneGO;
	public timeWalkDayNightToggle dayNightRef;

	public GameObject loadingManagerGO;
	public GameObject controlManagerGO;

	private void Awake()
	{
		mainSceneGO.SetActive(false);
	}

	private void Start()
	{
		if (LoadingManager.instance == null)
			Instantiate(loadingManagerGO);
		LoadingManager.instance.mainSceneGO = mainSceneGO;

		if (ControlManager.instance == null)
			Instantiate(controlManagerGO);
		ControlManager.instance.dayNightRef = dayNightRef;
	}
}
