	// Toggle between Diffuse and Transparent/Diffuse shaders
	// when COMMENT key is pressed
	function Start() {
		GetComponent.<Renderer>().enabled = false; // start scene hidden
}
	function Update() {
		if(Input.GetKeyDown(KeyCode.C)) { // pressed the "C" comment key
			if( GetComponent.<Renderer>().enabled ){
				GetComponent.<Renderer>().enabled = false;
				}
			else{
				GetComponent.<Renderer>().enabled = true;
				}
		}
	}