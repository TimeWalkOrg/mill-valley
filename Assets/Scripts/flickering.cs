using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickering : MonoBehaviour {
    public float timeOn;
    public float timeOff;
    public float changeTime;
    public GameObject light;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Time.time > changeTime)
        {
            light.SetActive(!light.activeSelf);
            if (light.activeSelf)
            {
                changeTime = Time.time + timeOn;
            }
            else
            {
                changeTime = Time.time + timeOff;
            }
        }
    }
}
