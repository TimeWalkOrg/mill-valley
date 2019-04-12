using UnityEngine;
using System.Collections;

public class PC2TestMenu : MonoBehaviour {

	protected PC2Parser[] parsers;

	// Use this for initialization
	void Start () {
		parsers = FindObjectsOfType<PC2Parser>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected float sliderVal = 24;
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(0,0,100,30), "Stormy"))
		{
			foreach(PC2Parser parser in parsers)
				parser.Blend(0, 1.0f);
		}

		if (GUI.Button(new Rect(120,0,100,30), "Calm"))
		{
			foreach(PC2Parser parser in parsers)
				parser.Blend(1, 1.0f);
		}

//		if (GUI.Button(new Rect(Screen.width - 340,0,100,30), "Single"))
//		{
//			Application.LoadLevel("example1");
//		}
//
//		if (GUI.Button(new Rect(Screen.width - 220,0,100,30), "Synced"))
//		{
//			Application.LoadLevel("example2");
//		}
//
//		if (GUI.Button(new Rect(Screen.width - 100,0,100,30), "Unsynced"))
//		{
//			Application.LoadLevel("example3");
//		}

		float newSliderVal = GUI.HorizontalSlider(new Rect(250, 8, 100, 30), sliderVal, 5, 60);

		if (newSliderVal != sliderVal)
		{
			sliderVal = newSliderVal;

			foreach(PC2Parser parser in parsers)
				parser.framerate = (int) sliderVal;
		}
	}
}
