#pragma strict
//	var clipNotPlayed : boolean = true; // Clip not played yet?
//	var moviePlayingStatus : boolean; // Movie play status?
//	var movieLength : float; // Movie length in seconds
//	var movieShowings : int = 0;
	var MovTex:MovieTexture;
	var myClip: AudioClip;
function Start () {
		GetComponent.<Renderer>().material.mainTexture = MovTex;
		MovTex.loop = true;
		MovTex.Play();
//		movieShowings = movieShowings + 1; 
		GetComponent.<AudioSource>().PlayOneShot(myClip);
//		clipNotPlayed = false;
//		moviePlayingStatus = MovTex.isPlaying;
//		movieLength = MovTex.duration;
}

function Update () {
//	moviePlayingStatus = MovTex.isPlaying;
//	if (!MovTex.isPlaying){
//		renderer.material.mainTexture = MovTex;
//		MovTex.Play(); 
//		movieShowings = movieShowings + 1; 
//		}
}