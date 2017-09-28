using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carIdlingShake : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
        iTween.ShakePosition(target, iTween.Hash("y", .01, "easeType", "spring", "loopType", "loop", "time", 1.0));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
