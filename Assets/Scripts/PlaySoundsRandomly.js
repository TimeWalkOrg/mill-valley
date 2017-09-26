//var clipPlayed : boolean = false; // Clip not played yet?
var myClip: AudioClip;
var playStartTime : float; // when does the object disappear?
var playNextPause : float = 10.0f; // how long delay before playing sound again?
var playNextInterval : float = 60.0f; // how many seconds after pause before next sound plays (times Random.value)?
function Update()
	{
		if(Time.timeSinceLevelLoad > playStartTime){
		GetComponent.<AudioSource>().PlayOneShot(myClip);
		playStartTime = playStartTime + playNextPause + playNextInterval*Random.value; //don't play again for at least playNextPause seconds + 0-playNextInterval more seconds
	}
}