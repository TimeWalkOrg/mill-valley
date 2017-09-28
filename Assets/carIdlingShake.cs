using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See documentation at http://www.pixelplacement.com/itween/documentation.php
// use any of the parameters for the ShakePosition method below
public class carIdlingShake : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
        iTween.ShakePosition(target, iTween.Hash("y", .010, "easeType", "spring", "loopType", "loop"));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
