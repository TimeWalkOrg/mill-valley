
using UnityEngine;
using System.Collections;

public class BearAnimationScript : MonoBehaviour {
	public Animation anim;
	void Start() {
		anim = GetComponent<Animation>();
		foreach (AnimationState state in anim) {
			state.speed = 0.5F;
		}
	}
}