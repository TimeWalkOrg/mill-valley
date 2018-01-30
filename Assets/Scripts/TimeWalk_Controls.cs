using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeWalk_Controls : MonoBehaviour
{
	public Text locationText;
	public Text timeText;
	public Text yearText;
	public Text eraText;
	public Text helpHintText;
	public Text helpFullText;
	public Text sliderText;
	public Slider yearSlider;
	public Color uiDefaultColor = Color.white;
	public Color uiDefaultHighlightColor = Color.yellow;
	public static int yearNowValue;		//global value for the current year being shown
	public static float levelStartTime;		//global value for the time the level started
	public static bool isGamePaused = false;        //global value for whether the game is paused (e.g. by the "H" key)

	public int yearTextFontSize = 17;
	public AudioClip timeSound;
	public int yearLastValue;

	public float effectTimeLength = 3.0f; // duration of font effect
	public float helpHintStartDelay = 4.0f;
	public float helpHintDuration = 5.0f;

	private int[] yearArray;
	private int yearIndexValue = 0; //pointer into the list of years
	private int yearArraySize = 6;
	private AudioSource source;
	private Canvas canvas;
	private float effectTimeEnd = 0.0f;
	private float effectTimeStart = 0.0f;
	private float helpHintStartTime;
	private float helpHintEndTime;

	void Awake ()
	{
		AudioListener.pause = true;
		source = GetComponent<AudioSource>();
		
		//canvas = GetComponent<Canvas>();
		//canvas.enabled = false;
	}

	void Start()
	{
		levelStartTime = Time.timeSinceLevelLoad;
		// Since we can't resize builtin arrays
		// we have to recreate the array to resize it
		yearArray = new int[6];

		yearArray[0] = 1920;
		yearArray[1] = 1950;
		yearArray[2] = 1973;
		yearArray[3] = 2015;
		yearArray[4] = 1800;
		yearArray[5] = 1850;

		yearNowValue = yearArray [yearIndexValue];
        yearSlider.value = yearArray[yearIndexValue]; // Sets to 1920 position
        yearLastValue = yearNowValue;
		helpHintStartTime = levelStartTime + helpHintStartDelay;
		helpHintEndTime = helpHintStartTime + helpHintDuration;
		yearText.color = uiDefaultColor;
		timeText.color = uiDefaultColor;
		helpHintText.color = uiDefaultColor;
		helpFullText.color = uiDefaultColor;
		locationText.color = uiDefaultColor;
		sliderText.color = uiDefaultHighlightColor;
		
		// hide all of the UI elements at start
		helpHintText.enabled = false;
		helpFullText.enabled = false;
	}

	void Update ()
	{
		//timeText.text = System.DateTime.Now.ToString ("h:mm:ss tt");  //May cause garbage collection problem
		//AudioListener.pause = false;

		//// Press "Y" key to change year
 	//	if (Input.GetKeyDown (KeyCode.Y))
		//{
		//	++yearIndexValue;
		//	if (yearIndexValue == yearArraySize)
		//		yearIndexValue = 0; 
		//	yearNowValue = yearArray [yearIndexValue];

		//	// temporary hack to change titles.  was having trouble making text array work...
		//	// eraText.text = eraTitleArray [yearIndexValue];
					
		//	if (yearIndexValue == 0) sliderText.text = "1920: Prohibition";
		//	if (yearIndexValue == 1) sliderText.text = "1950: Mid-Century";
		//	if (yearIndexValue == 2) sliderText.text = "1973: Sweetwater";
		//	if (yearIndexValue == 3) sliderText.text = "2015: the present";
		//	if (yearIndexValue == 4) sliderText.text = "1800: Chief Marin";
		//	if (yearIndexValue == 5) sliderText.text = "1850: John Reed's Mill";

		//	//eraText.enabled = true;
		//	yearSlider.value = yearNowValue;

		//	source.PlayOneShot(timeSound,0.5f);
		//	yearText.text = yearNowValue.ToString ();
		//	effectTimeStart = Time.timeSinceLevelLoad;
		//	effectTimeEnd = effectTimeStart + effectTimeLength;
		//}

		//if (Time.timeSinceLevelLoad < effectTimeEnd)
		//{
		//	//
		//}
		//else
		//{
		//	yearText.color = uiDefaultColor;
		//	sliderText.text = yearText.text;
		//}
		
		//// Show location/time/year after delay
		////if (Time.timeSinceLevelLoad > helpHintStartTime)
		////{
		////	canvas.enabled = true;
		////}

		//// Show Help hint briefly after launch
		//if ((Time.timeSinceLevelLoad > helpHintStartTime) && (Time.timeSinceLevelLoad < helpHintEndTime))
		//{
		//	helpHintText.enabled = true;
		//}
		//else
		//{
		//	helpHintText.enabled = false;
		//}

		//// Press "H" key to toggle Help text
		//if (Input.GetKeyDown (KeyCode.H))
		//{
		//	if(!helpFullText.enabled)
		//	{
		//		helpFullText.enabled = true;
		//		isGamePaused = true;
		//	}
		//	else
		//	{
		//		helpFullText.enabled = false;
		//		isGamePaused = false;
		//	}
		//}

		//// Press "R" key to Restart the level
		//if(Input.GetKeyDown(KeyCode.R))
		//{
		//	LoadingManager.instance.ToggleLoadingScene(true);
		//	LoadingManager.instance.ToggleMainScene(false);
		//}

		//// Press "Q" key to Restart the level
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	Application.Quit ();
		//}

		// snap slider to closest year if not in mousedown mode
		//if ((Input.GetMouseButtonUp (0)) && (yearLastValue != yearSlider.value))
		//{
		//	int closestYear = 0;
		//	int closestGap = 9999;
		//	int closestIndex = 0;
		//	// cycle through array of years and find the closest value to snap to
		//	for(int i = 0; i < yearArraySize; i++)
		//	{
		//		int gapCheck = Mathf.Abs(yearArray[i] - Mathf.RoundToInt (yearSlider.value)); //;
		//		if (gapCheck < closestGap)
		//		{
		//			closestGap = gapCheck;
		//			closestYear = yearArray[i];
		//			closestIndex = i;
		//		}
		//	}

		//	yearSlider.value = closestYear;
		//	if (yearNowValue != Mathf.RoundToInt (yearSlider.value))
		//	{
		//		yearNowValue = Mathf.RoundToInt (yearSlider.value); // change yearSliderValue
		//		// temporary hack to change titles.  was having trouble making text array work...
		//		// eraText.text = eraTitleArray [yearIndexValue];
				
		//		if (closestIndex == 0) sliderText.text = "1920: Prohibition";
		//		if (closestIndex == 1) sliderText.text = "1950: Mid-Century";
		//		if (closestIndex == 2) sliderText.text = "1973: Sweetwater";
		//		if (closestIndex == 3) sliderText.text = "2015: the present";
		//		if (closestIndex == 4) sliderText.text = "1800: Chief Marin";
		//		if (closestIndex == 5) sliderText.text = "1850: John Reed's Mill";
				
		//		source.PlayOneShot(timeSound,0.5f);
		//		yearText.text = yearNowValue.ToString ();
		//		effectTimeStart = Time.timeSinceLevelLoad;
		//		effectTimeEnd = effectTimeStart + effectTimeLength;	
		//		isGamePaused = false;	
		//	}
		//}
	}
}