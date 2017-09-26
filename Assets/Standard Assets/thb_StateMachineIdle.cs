using UnityEngine;
using System.Collections;

public class thb_StateMachineIdle : StateMachineBehaviour {
	private int value;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//		animator.SetInteger(“IdleIndex”, Random.Range(0, 5)); //randomly chooses from 5 options
		value = Random.Range (0, 5);
//		value = 2;
//		Debug.Log (value);
		animator.SetInteger("IdleIndex", value); //randomly chooses from 5 options
	}
}
