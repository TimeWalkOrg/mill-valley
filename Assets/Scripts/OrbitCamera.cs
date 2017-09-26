using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

	public Transform target;
	public float rotateSpeed = 3.0f;

	float distance;
	float _x;
	float _y;
	float timer;
	float scrollWheel;

	void Start () {
		distance = 10;
		_y = 5;
		_x = 330;

		RotateCamera();
		transform.LookAt(target);
	}
	
	void Update() {
		scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		
		timer += Time.deltaTime;
		
		if (scrollWheel > 0 && timer > 0.01f) {
			timer = 0;
			distance -= 0.5f;

			if (distance <= 1) {
				distance = 1;
			}

			RotateCamera();
		}
		
		if (scrollWheel < 0 && timer > 0.01f) {
			timer = 0;
			distance += 0.5f;

			if (distance >= 20) {
				distance = 20;
			}

			RotateCamera();
		}

		if (Input.GetMouseButton(0)) {
			_x += Input.GetAxis("Mouse X") * rotateSpeed;
			_y -= Input.GetAxis("Mouse Y") * rotateSpeed;
			
			RotateCamera();
		}
	}

	void RotateCamera() {
		Quaternion rotation = Quaternion.Euler(_y, _x, 0);
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
		transform.rotation = rotation;
		transform.position = position;
	}
}
