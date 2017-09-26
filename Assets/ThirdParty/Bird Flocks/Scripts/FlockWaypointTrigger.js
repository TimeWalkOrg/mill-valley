#pragma strict
var _timer:float =1;
var _flockChild:FlockChild;

function Start () {
	if(!_flockChild)
	_flockChild = transform.parent.GetComponent(FlockChild);
	var timer = Random.Range(_timer, _timer*3);
	InvokeRepeating("Trigger", timer, timer);	
}

function Trigger () {
	_flockChild.Wander(0);
}