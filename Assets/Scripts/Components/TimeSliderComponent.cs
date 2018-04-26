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
	private YearData currentYearData;
	private float effectTimeLength = 3.0f;

	private void Start()
	{
		currentYearData = new YearData();
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
		if (missive == null) return;

		sliderText.text = missive.data.yearLabel;
		yearSlider.value = missive.data.year;
		currentYearData.year = missive.data.year;
		sliderText.color = Color.yellow;
		CancelInvoke();
		Invoke("TimedSliderTextChange", effectTimeLength);
	}

	public void OnEndDrag(BaseEventData eventData)
	{
		int currentValue = Mathf.RoundToInt(yearSlider.value);
		YearData nearest = ControlManager.instance.yearData.OrderBy(x => Mathf.Abs((long)x.year - currentValue)).First();
		if (nearest.year != yearSlider.value)
		{
			SendYearDataMissive(nearest);
			ControlManager.instance.SetCurrentTime(nearest);
		}
		else
		{
			sliderText.text = nearest.yearLabel;
			yearSlider.value = nearest.year;
			currentYearData.year = nearest.year;
		}
	}

	private void TimedSliderTextChange()
	{
		sliderText.text = yearSlider.value.ToString();
		sliderText.color = Color.white;
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}

	private void SendYearDataMissive(YearData data)
	{
		YearDataMissive missive = new YearDataMissive();
		missive.data = data;
		Missive.Send(missive);
	}
}
