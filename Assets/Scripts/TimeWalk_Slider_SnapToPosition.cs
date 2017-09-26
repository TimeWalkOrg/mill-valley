using UnityEngine;
using System.Collections;
//using UnityEngine.UI;


public class TimeWalk_Slider_SnapToPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		GameObject yearSlider = GameObject.FindGameObjectWithTag("Slider");
//		yearSlider.GetComponent<yearSlider>().value();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) print ("got Input.GetMouseButtonUp()");
	}
	void OnMouseUp(){
		// snap slider to closest year if not in mousedown mode
		print ("got to OnMouseUp()");
//			if (yearLastValue != yearSlider.value) {
//				yearSlider.value = 1800;
//				yearNowValue = Mathf.RoundToInt (yearSlider.value);
//			}
	}
}
