using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewTriggerComponent : MonoBehaviour
{
	//public GameObject exclaimationGO;
	public GameObject vRWebViewGO;
	// TODO needs #define
	public ZenFulcrum.EmbeddedBrowser.Browser browser;
	private string thisURL;

	private bool isWebViewActive = false;
	private bool isVRPlayerNear = false;

	private void Start()
	{
		DisableWebView();
	}

	public void SetURL(string url)
	{
		thisURL = url;
	}

	private void OnTriggerEnter(Collider other)
	{
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
		// if you wanted players to be able to move away to disable in FPS uncomment this
		//if (other.tag == "Player")
		//{
		//	DisableWebView();
		//}

		if (other.tag == "VRPlayer")
		{
			DisableWebView();
		}
	}

	public void DisableWebView()
	{
		if (vRWebViewGO != null)
			vRWebViewGO.SetActive(false);
		isWebViewActive = false;
	}

	private void EnableWebView()
	{
		if (vRWebViewGO != null)
			vRWebViewGO.SetActive(true);
		isWebViewActive = true;
	}
}
