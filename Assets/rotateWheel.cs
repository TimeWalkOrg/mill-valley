using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateWheel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(-(40 * (Vector3.right * Time.deltaTime)));
    }
}
