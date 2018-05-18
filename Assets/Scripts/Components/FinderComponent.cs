using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderComponent : MonoBehaviour
{
	public GameObject mainSceneGO;
	public timeWalkDayNightToggle dayNightRef;

	public GameObject loadingManagerGO;
	public GameObject controlManagerGO;

	public GameObject lightingRefGO;
	public GameObject environmentRefGO;
	public GameObject terrianRefGO;

	IEnumerator Start()
	{
		// lessen async load
		if (terrianRefGO != null)
			terrianRefGO.SetActive(true);
		yield return new WaitForEndOfFrame();

		if (lightingRefGO != null)
			lightingRefGO.SetActive(true);
		yield return new WaitForEndOfFrame();

		if (environmentRefGO != null)
			environmentRefGO.SetActive(true);
		yield return new WaitForEndOfFrame();

		// hacks for starting in MainScene instead of loading for edits
		if (LoadingManager.instance == null)
			Instantiate(loadingManagerGO);
		LoadingManager.instance.mainSceneGO = mainSceneGO;

		if (ControlManager.instance == null)
			Instantiate(controlManagerGO);
		ControlManager.instance.dayNightRef = dayNightRef;
	}
}
