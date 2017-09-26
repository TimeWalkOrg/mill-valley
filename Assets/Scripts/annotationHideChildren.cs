using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script will toggle the active state of all of the children of the parent object (e.g. annotation photos).  Make sure the children start out inactive.

public class annotationHideChildren : MonoBehaviour {
    public Component[] childrenObjects;
	// Use this for initialization
	void Start () {
        childrenObjects = GetComponentsInChildren< Transform > (true);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        { // pressed the "C" comment key
            for (int a = 1; a < childrenObjects.Length; a++) // skips the first entry, which is the parent object!
            {
                childrenObjects[a].gameObject.SetActive(!childrenObjects[a].gameObject.activeSelf);
            }

        }
    }
}