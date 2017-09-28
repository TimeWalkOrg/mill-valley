using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToVisible : MonoBehaviour {
    public float targetDistance;
    public Transform other;
    public GameObject theCamera;
    void Start () {
	}
	
	void Update () {
        var dist = Vector3.Distance(other.position, transform.position);
        targetDistance = dist;

//        scriptName SN = theCamera.GetComponent<"Post Processing Controller">();
//        SN.var = .5;
    }
}