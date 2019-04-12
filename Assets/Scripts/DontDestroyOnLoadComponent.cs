using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}
