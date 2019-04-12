var clipNotPlayed : boolean = true; // Clip not played yet?
//var MovTex:MovieTexture;
var myClip: AudioClip;

function OnTriggerEnter (myTrigger : Collider) {
	if(myTrigger.gameObject.tag == "Player" && clipNotPlayed){
//		renderer.material.mainTexture = MovTex;
//		MovTex.Play(); 
		GetComponent.<AudioSource>().PlayOneShot(myClip);
		clipNotPlayed = false;
	}
}