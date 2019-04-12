using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeActiveComponent : MonoBehaviour
{
	public int yearBuilt;
	public int yearReplaced;

	private void OnEnable()
	{
		// enable audio/animations/etc here for time period
	}

	private void OnDisable()
	{
		// disable audio/animations/etc here for time period
	}
}
