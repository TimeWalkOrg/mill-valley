using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WebViewTriggerData
{
	public string buildingName;
	public string url;
	public Transform webViewTriggerAnchorGO;
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
	}

	void OnApplicationQuit()
	{
		_instance = null;
		DestroyImmediate(gameObject);
	}
	#endregion

	public GameObject webViewTriggerPrefab;
	public WebViewTriggerData[] webViewTriggerData;

	private List<GameObject> activeWebViewTriggerList = new List<GameObject>();

	private void Start()
	{
		InitActiveResearchTeamObjects();
	}

	private void OnEnable()
	{
		Missive.AddListener<CreditsMissive>(OnToggleCredits);
	}

	private void OnDisable()
	{
		Missive.RemoveListener<CreditsMissive>(OnToggleCredits);
	}

	private void InitActiveResearchTeamObjects()
	{
		for (int i = 0; i < webViewTriggerData.Length; i++)
		{
			if (webViewTriggerData[i].webViewTriggerAnchorGO != null)
			{
				GameObject go = Instantiate(webViewTriggerPrefab, webViewTriggerData[i].webViewTriggerAnchorGO.position, Quaternion.Euler(new Vector3(90, 0, 0)));
				go.GetComponent<WebViewTriggerComponent>().SetURL(webViewTriggerData[i].url);
				activeWebViewTriggerList.Add(go);
				go.SetActive(false);
			}
		}
	}

	private void OnToggleCredits(CreditsMissive missive)
	{
		for (int i = 0; i < activeWebViewTriggerList.Count; i++)
		{
			activeWebViewTriggerList[i].SetActive(!activeWebViewTriggerList[i].activeInHierarchy);
		}
	}
}
