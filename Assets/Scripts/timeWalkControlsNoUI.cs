using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timeWalkControlsNoUI : MonoBehaviour
{
    public static int yearNowValue;     //global value for the current year being shown
    public static float levelStartTime;     //global value for the time the level started
    public static bool isGamePaused = false;        //global value for whether the game is paused (e.g. by the "H" key)

    public int yearLastValue;
    private int[] yearArray;
    private int yearIndexValue = 0; //pointer into the list of years
    private int yearArraySize = 6;
    public AudioClip timeSound;
    private AudioSource source;
    public int testYearNowValue = 0;

    // Use this for initialization
    void Awake()
    {
        //AudioListener.pause = true;
        //source = GetComponent<AudioSource>();
    }
    void Start()
    {
        //levelStartTime = Time.timeSinceLevelLoad;
        //// Since we can't resize builtin arrays
        //// we have to recreate the array to resize it
        //yearArray = new int[6];

        //yearArray[0] = 1920;
        //yearArray[1] = 1950;
        //yearArray[2] = 1973;
        //yearArray[3] = 2015;
        //yearArray[4] = 1800;
        //yearArray[5] = 1850;

        //yearNowValue = yearArray[yearIndexValue];
        //yearLastValue = yearNowValue;
    }

    void Update()
    {
        //		timeText.text = System.DateTime.Now.ToString ("h:mm:ss tt");  //May cause garbage collection problem
  //      AudioListener.pause = false;

  //      // Press "Y" key to change year
  //      if (Input.GetKeyDown(KeyCode.Y))
  //      {
  //          ++yearIndexValue;
  //          if (yearIndexValue == yearArraySize)
  //              yearIndexValue = 0;
  //          yearNowValue = yearArray[yearIndexValue];
  //          testYearNowValue = yearNowValue;
  //          source.PlayOneShot(timeSound, 0.5f);
  //      }

  //      // Press "R" key to Restart the level
  //      if (Input.GetKeyDown(KeyCode.R))
  //      {
		//	LoadingManager.instance.ToggleLoadingScene(true);
		//	LoadingManager.instance.ToggleMainScene(false);
		//}

  //      // Press "Q" key to Restart the level
  //      if (Input.GetKeyDown(KeyCode.Q))
  //      { // pressed the "Q" Quit game key
  //          Application.Quit();
  //      }
    }
}