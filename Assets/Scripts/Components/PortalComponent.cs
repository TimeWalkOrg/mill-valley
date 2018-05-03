using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PortalComponent : MonoBehaviour
{
	// scene name to load bc you can't use scenes in inspector and builds, leave blank if going back to main scene 
	public string sceneToLoadName;
	public string portalLabel;
	public Transform portalSpawn;
	public Text forwardText;
	public Text backwardText;

	private void Start()
	{
		if (portalLabel == null || portalLabel == "")
		{
			forwardText.text = "Back";
			backwardText.text = "Back";
		}
		else
		{
			forwardText.text = "To " + portalLabel;
			backwardText.text = "To " + portalLabel;
		}
	}

	private void OnEnable()
	{
		// if secondary scene on enable spawn at local
		if (sceneToLoadName == null || sceneToLoadName == "")
			LoadingManager.instance.MovePlayerToPortal(portalSpawn);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Entered Portal");
		if (other.tag == "Player" || other.tag == "VRPlayer")
		{
			if (sceneToLoadName == null || sceneToLoadName == "")
			{
				LoadingManager.instance.ExitSecondaryScene();
			}
			else
			{
				if (other.tag == "VRPlayer")
					LoadingManager.instance.LoadSecondaryScene(sceneToLoadName, null, portalSpawn);
				else
					LoadingManager.instance.LoadSecondaryScene(sceneToLoadName, other.gameObject, portalSpawn);
			}
				
		}
	}
}
