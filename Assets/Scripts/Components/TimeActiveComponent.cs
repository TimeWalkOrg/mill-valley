using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeActiveComponent : MonoBehaviour
{
	public int yearBuilt;
	public int yearReplaced;
	private int lastYearDisplayed = 0;

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
		if (lastYearDisplayed != missive.data.year)
		{
			bool state = (missive.data.year >= yearBuilt) && (missive.data.year < yearReplaced);
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(state);
			}
			lastYearDisplayed = missive.data.year; // update lastYearDisplayed to the current TimeWalk date
		}
	}
}
