using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeWalkTriggeredSound : MonoBehaviour {
    public AudioClip soundToPlay;
    public GameObject objectToDisplay;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = soundToPlay;
        objectToDisplay.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            objectToDisplay.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().Stop();
            objectToDisplay.SetActive(false);
        }
    }
}
