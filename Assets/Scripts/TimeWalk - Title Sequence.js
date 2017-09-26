	var hideTargetTime : float; // when does the object disappear?
		function Update() {
		if(Time.timeSinceLevelLoad > hideTargetTime){
		Application.LoadLevel (1);
		}
		}