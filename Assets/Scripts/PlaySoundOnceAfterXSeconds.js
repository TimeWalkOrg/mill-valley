//var clipPlayed : boolean = false; // Clip not played yet?
var myClip: AudioClip;
var NotPlayedYet: boolean = true;
var playStartTime : float; // when does the object disappear?
function Update()
	{
		if(NotPlayedYet){
            if (Time.timeSinceLevelLoad > playStartTime) {
                GetComponent.<AudioSource>().PlayOneShot(myClip);
			//AudioSource>().PlayOneShot(myClip);
			NotPlayedYet = false;
//		playStartTime = playStartTime + 10 + 60*Random.value; //don't play again for at least 10 seconds + 0-20 more seconds
	}
	}
}