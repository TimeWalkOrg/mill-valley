using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalComponent : MonoBehaviour
{
	// scene name to load bc you can't use scenes in inspector and builds, leave blank if going back to main scene 
	public string sceneToLoadName;
	public Transform portalSpawn;

	private void OnEnable()
	{
		// if secondary scene on enable spawn at local
		if (sceneToLoadName == null || sceneToLoadName == "")
			LoadingManager.instance.MovePlayerToPortal(portalSpawn);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Entered Portal");
		if (other.tag == "Player")
		{
			if (sceneToLoadName == null || sceneToLoadName == "")
				LoadingManager.instance.ExitSecondaryScene();
			else
				LoadingManager.instance.LoadSecondaryScene(sceneToLoadName, other.gameObject, portalSpawn);
		}
	}
}
