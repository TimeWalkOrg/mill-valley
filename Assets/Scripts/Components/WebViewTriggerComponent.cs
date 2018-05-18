using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WebViewTriggerComponent : MonoBehaviour
{
	public GameObject webviewHolder;
	public GameObject vRWebViewGO;
	// TODO needs #define
	public ZenFulcrum.EmbeddedBrowser.Browser browser;
	private string thisURL;

	private bool isWebViewActive = false;

	private void Start()
	{
		DisableWebView();
		ToggleWebViewHolder(false);
	}

	private void OnEnable()
	{
		Missive.AddListener<CreditsMissive>(OnToggleCredits);
		SceneManager.activeSceneChanged += ChangedActiveScene;
	}

	private void OnDisable()
	{
		Missive.RemoveListener<CreditsMissive>(OnToggleCredits);
		SceneManager.activeSceneChanged -= ChangedActiveScene;
	}

	private void ChangedActiveScene(Scene current, Scene next)
	{
		ToggleWebViewHolder(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!webviewHolder.activeInHierarchy) return;

		if (other.tag == "Player")
		{
			ControlManager.instance.SendWebViewMissive(thisURL);
		}

		if (other.tag == "VRPlayer")
		{
			EnableWebView();
			this.transform.LookAt(other.transform.position);
			this.transform.rotation = Quaternion.Euler(new Vector3(0, this.transform.rotation.eulerAngles.y, 0));
			if (browser != null)
				browser.Url = thisURL;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "VRPlayer")
		{
			DisableWebView();
		}
	}

	public void SetURL(string url)
	{
		thisURL = url;
	}

	public void DisableWebView()
	{
		if (vRWebViewGO == null) return;

		vRWebViewGO.SetActive(false);
		isWebViewActive = false;
	}

	private void EnableWebView()
	{
		if (vRWebViewGO == null) return;

		vRWebViewGO.SetActive(true);
		isWebViewActive = true;
	}

	private void ToggleWebViewHolder(bool state)
	{
		if (webviewHolder == null) return;

		webviewHolder.SetActive(state);
	}

	private void OnToggleCredits(CreditsMissive missive)
	{
		webviewHolder.SetActive(!webviewHolder.activeInHierarchy);
	}
}
