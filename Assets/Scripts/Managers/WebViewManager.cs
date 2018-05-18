using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WebViewTriggerData
{
	public string buildingName;
	public string url;
}

public class WebViewManager : MonoBehaviour
{
	#region Singleton
	private static WebViewManager _instance = null;
	public static WebViewManager instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<WebViewManager>();
			return _instance;
		}
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			_instance = this;
		}
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnApplicationQuit()
	{
		_instance = null;
		DestroyImmediate(gameObject);
	}
	#endregion

	public GameObject webViewTriggerPrefab;
	//public WebViewTriggerData[] webViewTriggerData;

	/*
	 * dowds
	 * https://pages.timewalk.org/building-mill-valley-ca-dowds-moving/
	 * 
	 * landwater
	 * https://pages.timewalk.org/building-mill-valley-ca-land-and-water-co/
	 * 
	 * leal
	 * https://pages.timewalk.org/building-mill-valley-ca-leal-building/
	 * 
	 * oshaugnhessy
	 * https://pages.timewalk.org/building-mill-valley-ca-oshaughnessy/
	 * 
	 * vasco
	 * https://pages.timewalk.org/building-mill-valley-ca-vasco/
	 * 
	 * depot
	 * https://pages.timewalk.org/building-mill-valley-ca-train-depot/
	 * 
	 * carnegieLibrary
	 * https://pages.timewalk.org/building-mill-valley-ca-carnegie-library/
	 * 
	 * hubTheatre
	 * https://pages.timewalk.org/building-mill-valley-ca-hub-theatre/
	 * 
	 * keystoneBuilding
	 * https://pages.timewalk.org/building-mill-valley-ca-keystone-building/
	 * 
	 * bank-of-mill-valley
	 * https://pages.timewalk.org/building-mill-valley-ca-bank-of-mill-valley/
	 * 
	 * browns-building
	 * https://pages.timewalk.org/building-mill-valley-ca-browns-building/
	 * 
	 * lumberyard
	 * https://pages.timewalk.org/building-mill-valley-ca-lumberyard/
	 * 
	 * mill-valley-market
	 * https://pages.timewalk.org/building-mill-valley-ca-mill-valley-market/
	 * 
	 * outdoor-art-club
	 * https://pages.timewalk.org/building-mill-valley-ca-outdoor-art-club/
	 * 
	 * town-hall
	 * https://pages.timewalk.org/building-mill-valley-ca-town-hall/
	 * 
	 */
}
