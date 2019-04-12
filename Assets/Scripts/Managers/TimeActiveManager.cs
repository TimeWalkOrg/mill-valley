using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeActiveManager : MonoBehaviour
{
	public List<TimeActiveComponent> timeActiveRefs = new List<TimeActiveComponent>();

	private void Awake()
	{
		for (int i = 0; i < timeActiveRefs.Count; i++)
		{
			timeActiveRefs[i].gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		Missive.AddListener<YearDataMissive>(OnYearData);
	}

	private void OnDisable()
	{
		Missive.RemoveListener<YearDataMissive>(OnYearData);
	}

	private void OnYearData(YearDataMissive missive)
	{
		for (int i = 0; i < timeActiveRefs.Count; i++)
		{
			bool state = (missive.data.year >= timeActiveRefs[i].yearBuilt) && (missive.data.year < timeActiveRefs[i].yearReplaced);
			timeActiveRefs[i].gameObject.SetActive(state);
		}
	}
}
