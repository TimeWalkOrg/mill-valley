using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewTriggerAnchorGOComponent : MonoBehaviour
{
	public string buildingName;
	public GameObject webViewTriggerPrefab;
	private string url;
	private Renderer thisRenderer;

	private void Awake()
	{
		thisRenderer = GetComponent<Renderer>();
		if (thisRenderer != null)
			thisRenderer.enabled = false;
	}

	private void Start()
	{
		if (buildingName == null || buildingName == "")
		{
			Debug.LogError("Please set buildingName! using default website");
			url = "https://www.timewalk.org/";
		}
		else
		{
			url = "https://pages.timewalk.org/building-mill-valley-ca-" + buildingName + "/";
		}

		SpawnWebviewTrigger();
	}

	private void SpawnWebviewTrigger()
	{
		GameObject go = Instantiate(webViewTriggerPrefab, this.transform.position, Quaternion.identity);
		go.GetComponent<WebViewTriggerComponent>().SetURL(url);
	}
}
