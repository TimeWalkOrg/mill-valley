using UnityEngine;
using System.Collections;

public class sun : MonoBehaviour {
    public float timeSpeed;
    public float timeOfDay;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
        timeOfDay = timeSpeed * Time.deltaTime;
        transform.RotateAround(Vector3.zero,Vector3.right, timeOfDay);
		transform.LookAt(Vector3.zero);
	}
}
