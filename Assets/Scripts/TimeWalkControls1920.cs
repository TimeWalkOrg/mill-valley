using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeWalkControls1920 : MonoBehaviour
{

    public static int yearNowValue;     //global value for the current year being shown
    public static float levelStartTime;     //global value for the time the level started

    public int yearLastValue;
    private int[] yearArray;
    private int yearIndexValue = 0; //pointer into the list of years
    private int yearArraySize = 6;

    // Use this for initialization
    void Awake()
    {
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

        yearNowValue = yearArray[yearIndexValue];
        yearLastValue = yearNowValue;
    }

    void Update()
    {

        // Press "R" key to Restart the level
        if (Input.GetKeyDown(KeyCode.R))
        { // pressed the "R" restart level key
            Application.LoadLevel(0);
        }
        // Press "Q" key to Restart the level
        if (Input.GetKeyDown(KeyCode.Q))
        { // pressed the "Q" Quit game key
            Application.Quit();
        }

    }



}