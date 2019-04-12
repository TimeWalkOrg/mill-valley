using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightOnEnableComponent : MonoBehaviour
{
	private void OnEnable()
	{
		ControlManager.instance.dayNightRef.SetNight();
	}

	private void OnDisable()
	{
		ControlManager.instance.dayNightRef.SetDay();
	}
}
