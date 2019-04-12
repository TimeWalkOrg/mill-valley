#pragma strict

private var mainCamera2 : Camera;

function Start(){
var SkyProjectScript : SkyProject = GetComponent(SkyProject);
mainCamera2 = SkyProjectScript.billboardCamera;
}

function FixedUpdate () {

transform.LookAt(transform.position + mainCamera2.transform.rotation * Vector3.down,
mainCamera2.transform.rotation * Vector3.back);

}