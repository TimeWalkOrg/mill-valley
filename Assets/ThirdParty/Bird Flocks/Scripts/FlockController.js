/****************************************
	FlockController v2.3
	Copyright 2015 Unluck Software	
 	www.chemicalbliss.com
 	
 	v1.01
 	Flock can now be moved freely on the stage
 	
 	v1.02
 	Script is now pragma strict
 	
 	v1.03
 	Fixed issue with decreasing bird amount in runtime
 	
 	v1.04
 	Added Soar
 	
 	v1.05
 	Added Flat Soar and Flat Fly
 	
 	v2.0
 	Landing birds waypoint system
 	
 	v2.01
 	Added Slow Spawn
 	
 	v2.1 - 22.09.2014
 	Added Avoid
 	Changed to boxed area
 	
 	v2.11 - 23.09.2014
 	Added Avoid Up and Down
 	Added Soar Timeout
 	Added "Only Birds Above" to landing spots
 	
 	v2.2 - 05.11.2014
 	Various improvements to landing
 	
 	v2,3 - 16.01.2015
 	Changed _roamers array to list (Use _roamers.Count instead of _roamers.Length)
 	Added scripts in C#
 	Roaming area size can now be rectangular
 	Flock starting position offset
 	Swapped _posBuffer and transform.position (flock can now be easily moved)
 	Added grouping/parenting
 	Cached transforms
 	Added avoidance layer mask
 	 																																																																							
*****************************************/
#pragma strict
import System.Collections.Generic;
var _childPrefab:FlockChild;			// Assign prefab with FlockChild script attached
var _childAmount:int = 250;				// Number of objects
var _slowSpawn:boolean;					// Birds will not be instantiated all at once at start
var _spawnSphere:float = 3;				// Range around the spawner waypoints will created
var _spawnSphereHeight:float = 3;		// Height of the spawn sphere
var _spawnSphereDepth:float = -1;
var _minSpeed:float = 6;				// minimum random speed
var _maxSpeed:float = 10;				// maximum random speed
var _minScale:float = .7;				// minimum random size
var _maxScale:float = 1;				// maximum random size
var _soarFrequency:float = 0;			// How often soar is initiated 1 = always 0 = never
var _soarAnimation:String="Soar";		// Animation -required- for soar functionality
var _flapAnimation:String="Flap";		// Animation used for flapping
var _idleAnimation:String="Idle";		// Animation -required- for sitting idle functionality
var _diveValue:float = 7;				// Dive depth
var _diveFrequency:float = 0.5;			// How often dive 1 = always 0 = never
var _minDamping:float = 1;				// Rotation tween damping, lower number = smooth/slow rotation (if this get stuck in a loop, increase this value)
var _maxDamping:float = 2;
var _waypointDistance:float = 1;		// How close this can get to waypoint before creating a new waypoint (also fixes stuck in a loop)
var _minAnimationSpeed:float = 2;		// Minimum animation speed
var _maxAnimationSpeed:float = 4;		// Maximum animation speed
var _randomPositionTimer:float = 10;	// *** 
var _positionSphere:float = 25;			// If _randomPositionTimer is bigger than zero the controller will be moved to a random position within this sphere
var _positionSphereHeight:float = 25;	// Overides height of sphere for more controll
var _positionSphereDepth:float = -1;
var _childTriggerPos:boolean;			// Runs the random position function when a child reaches the controller
var _forceChildWaypoints:boolean;		// Forces all children to change waypoints when this changes position
var _forcedRandomDelay:float = 1.5;		// Random delay added before forcing new waypoint
var _flatFly:boolean;					// Birds will not rotate upwards as much when flapping
var _flatSoar:boolean;					// Birds will not rotate upwards as much when soaring
var _birdAvoid:boolean;					// Avoid colliders left and right
var _birdAvoidHorizontalForce:int = 1000; // How much a bird will react to avoid collision left and right
var _birdAvoidDown:boolean;				// Avoid colliders below
var _birdAvoidUp:boolean;				// Avoid colliders above bird
var _birdAvoidVerticalForce:int = 300;	// How much a bird will react to avoid collision down and up
var _birdAvoidDistanceMax:float = 4.5;	// Maximum distance to check for collision to avoid
var _birdAvoidDistanceMin:float = 5;	// Minimum distance to check for collision to avoid
var _soarMaxTime:float;					// Stops soaring after x seconds, use to avoid birds soaring for too long
var _avoidanceMask:LayerMask = -1;		// Avoidance collider mask
var _roamers:List.<FlockChild>;
var _posBuffer:Vector3;
var _updateDivisor:int = 1;				//Skip update every N frames (Higher numbers might give choppy results, 3 - 4 on 60fps , 2 - 3 on 30 fps)
var _newDelta:float;
var _updateCounter:int;
var _activeChildren:float;
var _groupChildToNewTransform:boolean;	// Parents fish transform to school transform
var _groupTransform:Transform;			//
var _groupName:String = "";				//
var _groupChildToFlock:boolean;			// Parents fish transform to school transform
var _startPosOffset:Vector3;
var _thisT:Transform;

function Start () {
	_thisT = transform;
	///FIX FOR UPDATING FROM OLDER VERSION
	if(_positionSphereDepth == -1){
		_positionSphereDepth = _positionSphere;
	}	
	if(_spawnSphereDepth == -1){
		_spawnSphereDepth = _spawnSphere;
	}
	///FIX	
	_posBuffer = _thisT.position+_startPosOffset;
	if(!_slowSpawn){
		AddChild(_childAmount);
	}
	InvokeRepeating("SetFlockRandomPosition", _randomPositionTimer, _randomPositionTimer);
}

function AddChild(amount:int){
	if(_groupChildToNewTransform)InstantiateGroup();
	for(var i:int=0;i<amount;i++){
		var obj : FlockChild = Instantiate(_childPrefab);	
	    obj._spawner = this;
	    _roamers.Add(obj);
	   AddChildToParent(obj.transform);
	}	
}

function AddChildToParent(obj:Transform){	
    if(_groupChildToFlock){
		obj.parent = transform;
		return;
	}
	if(_groupChildToNewTransform){
		obj.parent = _groupTransform;
		return;
	}
}

function RemoveChild(amount:int){
	for(var i:int=0;i<amount;i++){
		var dObj:FlockChild = _roamers[_roamers.Count-1];
		_roamers.RemoveAt(_roamers.Count-1);
		Destroy(dObj.gameObject);
	}
}

function Update () {
	if(_activeChildren > 0){
		if(_updateDivisor > 1){
			_updateCounter++;
		    _updateCounter = _updateCounter % _updateDivisor;	
			_newDelta = Time.deltaTime*_updateDivisor;	
		}else{
			_newDelta = Time.deltaTime;
		}	
	}
	UpdateChildAmount();
}

function InstantiateGroup(){
	if(_groupTransform) return;
	var g:GameObject = new GameObject();

	_groupTransform = g.transform;
	_groupTransform.position = _thisT.position;
	
	if(_groupName != ""){
		g.name = _groupName;
		return;
	}	
	g.name = _thisT.name + " Fish Container";
}

function UpdateChildAmount(){	
	if(_childAmount>= 0 && _childAmount < _roamers.Count){
		RemoveChild(1);
		return;
	}
	if (_childAmount > _roamers.Count){	
		AddChild(1);
	}
}

function OnDrawGizmos () {
	if(!_thisT) _thisT = transform;
		if(!Application.isPlaying && _posBuffer != _thisT.position+_startPosOffset){
			_posBuffer = _thisT.position+_startPosOffset;
       		
       	}
       	if(_positionSphereDepth == -1){
				_positionSphereDepth = _positionSphere;
			}	
			if(_spawnSphereDepth == -1){
				_spawnSphereDepth = _spawnSphere;
			}
       	Gizmos.color = Color.blue;
       	Gizmos.DrawWireCube (_posBuffer, Vector3(_spawnSphere*2, _spawnSphereHeight*2 ,_spawnSphereDepth*2));
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube (_thisT.position, Vector3((_positionSphere*2)+_spawnSphere*2, (_positionSphereHeight*2)+_spawnSphereHeight*2 ,(_positionSphereDepth*2)+_spawnSphereDepth*2));
    }

//Set waypoint randomly inside box
function SetFlockRandomPosition () {
	var t:Vector3;
	t.x = Random.Range(-_positionSphere, _positionSphere) + _thisT.position.x;
	t.z = Random.Range(-_positionSphereDepth, _positionSphereDepth) + _thisT.position.z;
	t.y = Random.Range(-_positionSphereHeight, _positionSphereHeight) + _thisT.position.y;
//	var hit : RaycastHit;
//	if (Physics.Raycast(_posBuffer, t, hit, Vector3.Distance(_posBuffer, t))){
//			_posBuffer.LookAt(hit.point);
//			t = hit.point - (_thisT.forward*-3);
//	}
	_posBuffer = t;	
	if(_forceChildWaypoints){
		for (var i:int = 0; i < _roamers.Count; i++) {
  		 	(_roamers[i] as FlockChild).Wander(Random.value*_forcedRandomDelay);
		}	
	}
}

//Instantly destroys all birds
function destroyBirds () {
		for (var i:int = 0; i < _roamers.Count; i++) {
			Destroy((_roamers[i] as FlockChild).gameObject);	
		}
		_childAmount = 0;
		_roamers.Clear();
}