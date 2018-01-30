using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct YearData
{
	public int year;
	public string yearLabel;
	public AudioClip yearAudioClip;
};

public class YearDataMissive : Missive
{
	public YearData data;
}

public class ControlManager : MonoBehaviour
{
	#region Singleton
	private static ControlManager _instance = null;
	public static ControlManager instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<ControlManager>();
			return _instance;
		}
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			_instance = this;
		}
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnApplicationQuit()
	{
		_instance = null;
		DestroyImmediate(gameObject);
	}
	#endregion

	public Color uiDefaultColor = Color.white;
	public Color uiHighlightColor = Color.yellow;

	public YearData[] yearData;
	public int currentYear { get; private set; }
	public int currentYearIndex { get; private set; }

	private void Start()
	{
		//
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Y))
			ToggleYear();

		if (Input.GetKeyUp(KeyCode.N))
			ToggleNight();

		if (Input.GetKeyUp(KeyCode.H))
			ToggleHelp();

		if (Input.GetKeyUp(KeyCode.R))
			ToggleMenu();

		if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Escape))
			ToggleQuit();
	}

	public void ToggleYear(int year = -1)
	{
		// ++year
		if (year == -1)
		{
			currentYearIndex = (currentYearIndex >= yearData.Length-1) ? 0 : currentYearIndex + 1;
			currentYear = yearData[currentYearIndex].year;
		}
		else
		{
			// set year
			for (int i = 0; i < yearData.Length; i++)
			{
				if (year == yearData[i].year)
				{
					currentYear = year;
					currentYearIndex = i;
					SendYearDataMissive(yearData[currentYearIndex]);
					return;
				}
			}
			// default if not correct year
			currentYear = yearData[0].year;
			currentYearIndex = 0;
		}

		SendYearDataMissive(yearData[currentYearIndex]);

		// slider text and approx -done
		// ui text
		// audio
	}

	private void ToggleNight()
	{
		// night
	}

	private void ToggleHelp()
	{
		// help
	}

	private void ToggleMenu()
	{
		// menu
	}

	private void ToggleQuit()
	{
		// quit
	}

	private void SendYearDataMissive(YearData data)
	{
		YearDataMissive missive = new YearDataMissive();
		missive.data = data;
		Missive.Send(missive);
	}
}