	public var objectOne : GameObject;
	public var objectTwo : GameObject;
	var clipNotPlayed : boolean = true; // Clip not played yet?
	var myClip: AudioClip;

	function Start() {
		GetComponent.<AudioSource>().Pause();
		objectOne.GetComponent.<Renderer>().enabled = false;
//		objectTwo.GetComponent.<Renderer>().enabled = false;
		objectTwo.SetActive(false);
}
function OnTriggerEnter (myTrigger : Collider) {
	if(myTrigger.gameObject.tag == "Player" && clipNotPlayed){
		GetComponent.<AudioSource>().Play();
		clipNotPlayed = false;
		objectOne.GetComponent.<Renderer>().enabled = true;
//		objectTwo.GetComponent.<Renderer>().enabled = true;
		objectTwo.SetActive(true);
		}
	}

function OnTriggerExit (myTrigger : Collider) {

	if(myTrigger.gameObject.tag == "Player"){
		GetComponent.<AudioSource>().Pause();
		clipNotPlayed = true;
		objectOne.GetComponent.<Renderer>().enabled = false;
//		objectTwo.GetComponent.<Renderer>().enabled = false;
		objectTwo.SetActive(false);
		}
	}

