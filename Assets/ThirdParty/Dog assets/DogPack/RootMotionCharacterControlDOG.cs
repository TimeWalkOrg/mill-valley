using UnityEngine;
using System.Collections;


//[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacterControlDOG: MonoBehaviour
{
	public float turningSpeed = 90f;
	public RootMotionComputer computer;
	public CharacterController character;
	public bool dogScratching = false;
	public bool dogIdle = true;
	public AudioClip myClip;
	public float randomEvent;
	
	
	void Start()
	{
		// validate component references
		if (computer == null) computer = GetComponent(typeof(RootMotionComputer)) as RootMotionComputer;
		if (character == null) character = GetComponent(typeof(CharacterController)) as CharacterController;
		
		// tell the computer to just output values but not apply motion
		computer.applyMotion = false;
		// tell the computer that this script will manage its execution
		computer.isManagedExternally = true;
		// since we are using a character controller, we only want the z translation output
		computer.computationMode = RootMotionComputationMode.ZTranslation;
		// initialize the computer
		computer.Initialize();
		
		// set up properties for the animations
		GetComponent<Animation>()["idle"].layer = 0; GetComponent<Animation>()["idle"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["sitidle"].layer = 1; GetComponent<Animation>()["sitidle"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["walk"].layer = 1; GetComponent<Animation>()["walk"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["trot"].layer = 1; GetComponent<Animation>()["trot"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["scratching"].layer = 1; GetComponent<Animation>()["scratching"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["beg"].layer = 1; GetComponent<Animation>()["beg"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["barking"].layer = 3; GetComponent<Animation>()["barking"].wrapMode = WrapMode.Once;
		GetComponent<Animation>()["howl"].layer = 3; GetComponent<Animation>()["howl"].wrapMode = WrapMode.Once;
		
		GetComponent<Animation>().Play("sitidle");
		dogIdle = true;
		
	}
	
	void Update()
	{
//		float targetMovementWeight = 0f;
//		float throttle = 0f;

			
		// toggle scratching/idle status every once in a while (2% chance per test)
		randomEvent = Random.value;
		if (randomEvent < 0.01f) {
				if (dogScratching) {
					GetComponent<Animation> ().CrossFade ("sitidle", 0.5f);
					dogScratching = false;
				} else {
					GetComponent<Animation> ().CrossFade ("scratching", 0.5f);
					dogScratching = true;
				}
			}
		}
	void OnTriggerEnter(Collider other) {
		if ((other.gameObject.tag == "Player") && dogIdle == true) {
			GetComponent<Animation>().CrossFade ("beg", 0.2f);
			GetComponent<AudioSource>().mute = false;
			GetComponent<AudioSource>().PlayOneShot(myClip, 0.7f);
			dogIdle = false;
		}
	}
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			GetComponent<Animation>().CrossFade ("sitidle", 0.2f);
			GetComponent<AudioSource>().mute = true;
			dogIdle = true;
		}
	}
	
	void LateUpdate()
	{
		computer.ComputeRootMotion();
		
		// move the character using the computer's output
		//		character.SimpleMove(transform.TransformDirection(computer.deltaPosition)/Time.deltaTime);
	}
}