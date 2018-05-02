using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSelectComponent : MonoBehaviour
{
	public ControlType thisControlType;

	public void ControlSelectOnClick()
	{
		ControlSelectMissive missive = new ControlSelectMissive();
		missive.controlType = thisControlType;
		Missive.Send(missive);
	}
}
