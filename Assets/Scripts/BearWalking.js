//var anim: Animation;
var startTime:float = 0.0f;
var intervalSec:float = 1.5f;

function Awake() {
	startTime = Time.timeSinceLevelLoad;
    GetComponent.<Animation>().CrossFade ("4LegsRoar");
}

// Plays an animation only if we are not playing it already.
function Update() {
	if(Time.timeSinceLevelLoad > startTime + intervalSec)
	{
		GetComponent.<Animation>().CrossFade ("idle4Legs");
	} 
}