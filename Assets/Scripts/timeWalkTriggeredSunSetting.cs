using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeWalkTriggeredSunSetting : MonoBehaviour {
    public GameObject sunDirection;
//    public int sunNewARotation;
//    public int sunAngleIncrement;
    public int sunOriginalXRotation = 164;
    public int sunOriginalYRotation = 85;
    public int sunOriginalZRotation = 88;
    public int sunNewXRotation = 32;
    public int sunNewYRotation = 85;
    public int sunNewZRotation = 88;
    private bool isChanging = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sunDirection.transform.Rotate(sunNewXRotation, sunNewYRotation, sunNewZRotation);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sunDirection.transform.Rotate(sunOriginalXRotation, sunOriginalYRotation, sunOriginalZRotation);
        }
    }
}
