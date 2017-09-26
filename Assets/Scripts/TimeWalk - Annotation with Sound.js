		var myClip: AudioClip;
	// when COMMENT key is pressed
	function Start() {
		GetComponent.<Renderer>().enabled = false; // start scene hidden

}
	function Update() {
		if(Input.GetKeyDown(KeyCode.C)) { // pressed the "C" comment key
			if( GetComponent.<Renderer>().enabled ){
				GetComponent.<Renderer>().enabled = false;
				GetComponent.<AudioSource>().Stop();
				}
			else{
				GetComponent.<Renderer>().enabled = true;
				GetComponent.<AudioSource>().PlayOneShot(myClip);
				}
		}
	}