using UnityEngine;
using System.Collections;

public class mainCam : MonoBehaviour {

	public Transform targetPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		//transform.LookAt(targetPos.position);

		transform.LookAt(new Vector3(targetPos.position.x,0f,targetPos.position.z));
	}
}
