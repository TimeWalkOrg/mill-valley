using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeActiveComponent : MonoBehaviour
{
	public int yearBuilt;
	public int yearReplaced;
	private int lastYearDisplayed = 0;
	private int currentYearNowValue;

	private void Update()
	{
		if (TimeWalk_Controls.yearNowValue == 0)
		{
			currentYearNowValue = 1920;
		}
		else
		{
			currentYearNowValue = TimeWalk_Controls.yearNowValue;
		}

		if (lastYearDisplayed != currentYearNowValue) // if year has changed, then...
		{
			bool state = (currentYearNowValue >= yearBuilt) && (currentYearNowValue < yearReplaced);
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(state);
			}
			lastYearDisplayed = currentYearNowValue; // update lastYearDisplayed to the current TimeWalk date
		}
	}
}
