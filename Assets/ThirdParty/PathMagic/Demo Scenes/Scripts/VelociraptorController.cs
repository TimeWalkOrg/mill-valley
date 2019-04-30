using UnityEngine;
using System.Collections;

public class VelociraptorController : MonoBehaviour
{
	private Transform thisTransform;
	private Animator anim;
	private Vector3 lastPos;

	// Use this for initialization
	void Start ()
	{
		thisTransform = transform;
		anim = GetComponent<Animator> ();
		lastPos = thisTransform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float vel = (1f / Time.deltaTime) * (lastPos - thisTransform.position).magnitude;
		anim.SetFloat ("Velocity", vel);
		Debug.Log (vel);

		lastPos = thisTransform.position;
	}
}
