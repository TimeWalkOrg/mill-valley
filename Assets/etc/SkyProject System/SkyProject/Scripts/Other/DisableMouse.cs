using UnityEngine;
using System.Collections;


public class DisableMouse : MonoBehaviour {
	public MouseLook mouse;
	public MouseLook fps;
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.P)){
	mouse.enabled=!mouse.enabled;
	fps.enabled=!fps.enabled;
}
}
}
