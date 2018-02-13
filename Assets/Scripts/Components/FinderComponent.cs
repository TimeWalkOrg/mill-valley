using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderComponent : MonoBehaviour
{
	public GameObject mainSceneGO;
	public timeWalkDayNightToggle dayNightRef;

	private void Awake()
	{
		mainSceneGO.SetActive(false);
	}

	private void Start()
	{
		LoadingManager.instance.mainSceneGO = mainSceneGO;
		ControlManager.instance.dayNightRef = dayNightRef;
	}
}
