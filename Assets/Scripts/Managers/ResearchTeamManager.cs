using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResearchTeamData
{
	public string buildingName;
	public string street;
	public string[] team;
	public string date;
	public Transform researchTeamAnchorGO;
}

public class ResearchTeamManager : MonoBehaviour
{
	#region Singleton
	private static ResearchTeamManager _instance = null;
	public static ResearchTeamManager instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<ResearchTeamManager>();
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

	public GameObject researchTeamGOPrefab;
	public ResearchTeamData[] researchTeamData;

	private List<GameObject> activeResearchTeamList = new List<GameObject>();

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
		for (int i = 0; i < researchTeamData.Length; i++)
		{
			if (researchTeamData[i].researchTeamAnchorGO != null)
			{
				GameObject go = Instantiate(researchTeamGOPrefab, researchTeamData[i].researchTeamAnchorGO.position, researchTeamData[i].researchTeamAnchorGO.rotation);
				go.GetComponent<ResearchTeamComponent>().SetResearchTeamData(researchTeamData[i]);
				activeResearchTeamList.Add(go);
				go.SetActive(false);
			}
		}
	}

	private void OnToggleCredits(CreditsMissive missive)
	{
		for (int i = 0; i < activeResearchTeamList.Count; i++)
		{
			activeResearchTeamList[i].SetActive(!activeResearchTeamList[i].activeInHierarchy);
		}
	}
}
