var nowInPast : boolean = true;
function Update () {
	if(Input.GetKeyDown(KeyCode.T)){
		if(nowInPast==true){
		// Move the object back into view
			transform.localPosition = Vector3(0,0,0);  // move back into view
			nowInPast = false;
		}
		else {
				// make the object invisible
				// Move the object to (0, 0, 0)
			transform.localPosition = Vector3(0, -100, 0);
			nowInPast = true;
			}
	}
}