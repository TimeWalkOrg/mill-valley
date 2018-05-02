using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
	public List<GameObject> portalList;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			portalList.Add(child.gameObject);
		}
	}

	private void OnEnable()
	{
		bool state = (ControlManager.instance.currentControlType != ControlType.None);
		TogglePortals(state);
	}

	private void TogglePortals(bool state)
	{
		if (portalList == null) return;
		for (int i = 0; i < portalList.Count; i++)
		{
			portalList[i].SetActive(state);
		}
	}
}
