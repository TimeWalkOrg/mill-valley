using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderComponent : MonoBehaviour
{
	public GameObject mainSceneGO;

	private void Awake()
	{
		mainSceneGO.SetActive(false);
	}

	private void Start()
	{
		LoadingManager.instance.mainSceneGO = mainSceneGO;
	}
}
