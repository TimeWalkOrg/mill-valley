using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateWheel : MonoBehaviour {
    public float rotationSpeed;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(-(rotationSpeed * (Vector3.right * Time.deltaTime)));
    }
}
