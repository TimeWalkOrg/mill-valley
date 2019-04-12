using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeItNight : MonoBehaviour
{
    public timeWalkDayNightToggle sunLightScript;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject g = GameObject.FindGameObjectWithTag("SunLightObject");
            sunLightScript = g.GetComponent<timeWalkDayNightToggle>();

            // accesses the bool named "nowIsDay" and changed its value.
            sunLightScript.nowIsDay = false;
        }
    }
}
