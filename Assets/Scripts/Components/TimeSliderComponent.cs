using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimeSliderComponent : MonoBehaviour
{
	public Text sliderText;
	public Slider yearSlider;
	private int currentYear;

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
		sliderText.text = missive.data.yearLabel;
		yearSlider.value = missive.data.year;
		currentYear = missive.data.year;
	}

	public void OnEndDrag(BaseEventData eventData)
	{
		int currentValue = Mathf.RoundToInt(yearSlider.value);
		YearData nearest = ControlManager.instance.yearData.OrderBy(x => Mathf.Abs((long)x.year - currentValue)).First();
		if (nearest.year != yearSlider.value)
		{
			SendYearDataMissive(nearest);
		}
		else
		{
			sliderText.text = nearest.yearLabel;
			yearSlider.value = nearest.year;
			currentYear = nearest.year;
		}
	}

	private void SendYearDataMissive(YearData data)
	{
		YearDataMissive missive = new YearDataMissive();
		missive.data = data;
		Missive.Send(missive);
	}
}
