using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewTriggerComponent : MonoBehaviour
{
	private string thisURL;

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
	}
}
