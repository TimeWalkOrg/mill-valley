/**************************************									
	FlockChild v2.3
	Copyright Unluck Software	
 	www.chemicalbliss.com								
***************************************/
#pragma strict
#pragma downcast
@HideInInspector 
var _spawner:FlockController;			//Reference to the flock controller that spawned this bird
@HideInInspector 
var _wayPoint : Vector3;				//Waypoint used to steer towards
var _speed:float;						//Current speed of bird
@HideInInspector 		
var _dived:boolean =true;				//Indicates if this bird has recently performed a dive movement
@HideInInspector 
var _stuckCounter:float;				//prevents looping around a waypoint by increasing minimum distance to waypoint
@HideInInspector 
var _damping:float;						//Damping used for steering (steer speed)
@HideInInspector 
var _soar:boolean = true;				// Indicates if this is soaring
@HideInInspector 
var _landing:boolean;					// Indicates if bird is landing or sitting idle
private var _lerpCounter:int;			// Used for smoothing motions like speed and leveling rotation
@HideInInspector 
var _targetSpeed:float;					// Max bird speed
@HideInInspector 
var _move:boolean = true;				// Indicates if bird can fly
var _model:GameObject;					// Reference to bird model
var _modelT:Transform;					// Reference to bird model transform (caching tranform to avoid any extra getComponent calls)
@HideInInspector 
var _avoidValue:float;					//Random value used to check for obstacles. Randomized to lessen uniformed behaviour when avoiding
@HideInInspector 
var _avoidDistance:float;				//How far from an obstacle this can be before starting to avoid it
private var _soarTimer:float;	
private var _instantiated:boolean;
private static var _updateNextSeed:int = 0;		
private var _updateSeed:int = -1;
@HideInInspector 
var _avoid:boolean = true;
var _thisT:Transform;

function Start(){
	FindRequiredComponents();			//Check if references to transform and model are set (These should be set in the prefab to avoid doind this once a bird is spawned, click "Fill" button in prefab)
	Wander(0);
	SetRandomScale();
	_thisT.position = findWaypoint();	
	RandomizeStartAnimationFrame();	
	InitAvoidanceValues();
	_speed = _spawner._minSpeed;
	_spawner._activeChildren++;
	_instantiated = true;
	if(_spawner._updateDivisor > 1){
		var _updateSeedCap:int = _spawner._updateDivisor -1;
		_updateNextSeed++;
	    this._updateSeed = _updateNextSeed;
	    _updateNextSeed = _updateNextSeed % _updateSeedCap;
	}
}

function Update() {
	//Skip frames
	if (_spawner._updateDivisor <=1 || _spawner._updateCounter == _updateSeed){
		SoarTimeLimit();
		CheckForDistanceToWaypoint();
		RotationBasedOnWaypointOrAvoidance();
	    LimitRotationOfModel();
	}
}

function OnDisable() {
	CancelInvoke();
	_spawner._activeChildren--;
}

function OnEnable() {
	if(_instantiated){
		_spawner._activeChildren++;
		if(_landing){
			_model.GetComponent.<Animation>().Play(_spawner._idleAnimation);
		}else{
			_model.GetComponent.<Animation>().Play(_spawner._flapAnimation);
		}		
	}
}

function FindRequiredComponents(){
	if(!_thisT)		_thisT = transform;	
	if(!_model)		_model = _thisT.Find("Model").gameObject;	
	if(!_modelT)	_modelT = _model.transform;
}

function RandomizeStartAnimationFrame(){
	for (var state : AnimationState in _model.GetComponent.<Animation>()) {
	 	state.time = Random.value * state.length;
	}
}

function InitAvoidanceValues(){
	_avoidValue = Random.Range(.3, .1);	
	if(_spawner._birdAvoidDistanceMax != _spawner._birdAvoidDistanceMin){
		_avoidDistance = Random.Range(_spawner._birdAvoidDistanceMax , _spawner._birdAvoidDistanceMin);
		return;
	}
	_avoidDistance = _spawner._birdAvoidDistanceMin;
}

function SetRandomScale(){
	var sc = Random.Range(_spawner._minScale, _spawner._maxScale);
	_thisT.localScale=Vector3(sc,sc,sc);
}

//Soar Timeout - Limits how long a bird can soar
function SoarTimeLimit(){	
	if(this._soar && _spawner._soarMaxTime > 0){ 		
   		if(_soarTimer > _spawner._soarMaxTime){
   			this.Flap();
   			_soarTimer = 0;
   		}else {
   			_soarTimer+=_spawner._newDelta;
   		}
   	}
}

function CheckForDistanceToWaypoint(){
	if(!_landing && (_thisT.position - _wayPoint).magnitude < _spawner._waypointDistance+_stuckCounter){
        Wander(0);
        _stuckCounter=0;
    }else if(!_landing){
    	_stuckCounter+=_spawner._newDelta;
    }else{
    	_stuckCounter=0;
    }
}

function RotationBasedOnWaypointOrAvoidance(){
	var lookit:Vector3 = _wayPoint - _thisT.position;
    if(_targetSpeed > -1 && lookit != Vector3.zero){
    var rotation = Quaternion.LookRotation(lookit);
	
	_thisT.rotation = Quaternion.Slerp(_thisT.rotation, rotation, _spawner._newDelta * _damping);
	}
	
	if(_spawner._childTriggerPos){
		if((_thisT.position - _spawner._posBuffer).magnitude < 1){
			_spawner.SetFlockRandomPosition();
		}
	}
	_speed = Mathf.Lerp(_speed, _targetSpeed, _lerpCounter * _spawner._newDelta *.05);
	_lerpCounter++;
	//Position forward based on object rotation
	if(_move){
		_thisT.position += _thisT.forward*_speed*_spawner._newDelta;
		if(_avoid && _spawner._birdAvoid) 
		Avoidance();
	}
}

function Avoidance():boolean {
	var hit : RaycastHit;
	var fwd:Vector3 = _modelT.forward;
	var r:boolean;
	var rot:Quaternion;
	var rotE:Vector3;
	var pos:Vector3;
	pos = _thisT.position;
	rot = _thisT.rotation;
	rotE = _thisT.rotation.eulerAngles;
	if (Physics.Raycast(_thisT.position, fwd+(_modelT.right*_avoidValue), hit, _avoidDistance, _spawner._avoidanceMask)){	
		rotE.y -= _spawner._birdAvoidHorizontalForce*_spawner._newDelta*_damping;
		rot.eulerAngles = rotE;
		_thisT.rotation = rot;
		r= true;
	}else if (Physics.Raycast(_thisT.position,fwd+(_modelT.right*-_avoidValue), hit, _avoidDistance, _spawner._avoidanceMask)){
		rotE.y += _spawner._birdAvoidHorizontalForce*_spawner._newDelta*_damping;
		rot.eulerAngles = rotE;
		_thisT.rotation = rot;
		r= true;		
	}
	if (_spawner._birdAvoidDown && !this._landing && Physics.Raycast(_thisT.position, -Vector3.up, hit, _avoidDistance, _spawner._avoidanceMask)){			
		rotE.x -= _spawner._birdAvoidVerticalForce*_spawner._newDelta*_damping;
		rot.eulerAngles = rotE;
		_thisT.rotation = rot;				
		pos.y += _spawner._birdAvoidVerticalForce*_spawner._newDelta*.01;
		_thisT.position = pos;
		r= true;			
	}else if (_spawner._birdAvoidUp && !this._landing && Physics.Raycast(_thisT.position, Vector3.up, hit, _avoidDistance, _spawner._avoidanceMask)){			
		rotE.x += _spawner._birdAvoidVerticalForce*_spawner._newDelta*_damping;
		rot.eulerAngles = rotE;
		_thisT.rotation = rot;
		pos.y -= _spawner._birdAvoidVerticalForce*_spawner._newDelta*.01;
		_thisT.position = pos;
		r= true;			
	}
	return r;
}

function LimitRotationOfModel(){
	var rot:Quaternion;
	var rotE:Vector3;
	rot = _modelT.localRotation;
	rotE = rot.eulerAngles;	
	if((_soar && _spawner._flatSoar|| _spawner._flatFly && !_soar)&& _wayPoint.y > _thisT.position.y||_landing){	
		rotE.x = Mathf.LerpAngle(_modelT.localEulerAngles.x, -_thisT.localEulerAngles.x, _lerpCounter * _spawner._newDelta * .75);
		rot.eulerAngles = rotE;
		_modelT.localRotation = rot;
	}else{	
		rotE.x = Mathf.LerpAngle(_modelT.localEulerAngles.x, 0, _lerpCounter * _spawner._newDelta * .75);
		rot.eulerAngles = rotE;
		_modelT.localRotation = rot;
	}
}

function Wander(delay:float){
	if(!_landing){
		_damping = Random.Range(_spawner._minDamping, _spawner._maxDamping);       
	    _targetSpeed = Random.Range(_spawner._minSpeed, _spawner._maxSpeed);
	    _lerpCounter = 0;	    
	    Invoke("SetRandomMode", delay);
	}
}

function SetRandomMode(){
	CancelInvoke("SetRandomMode");
	if(!_dived && Random.value < _spawner._soarFrequency){
	   	 	Soar();
		}else if(!_dived && Random.value < _spawner._diveFrequency){	
			Dive();
		}else{	
			Flap();
		}
}

function Flap(){
	if(_move){
	 	if(this._model) _model.GetComponent.<Animation>().CrossFade(_spawner._flapAnimation, .5);
		_soar=false;
		animationSpeed();
		_wayPoint = findWaypoint();
		_dived = false;
	}
}

function findWaypoint():Vector3{
	var t:Vector3;
	t.x = Random.Range(-_spawner._spawnSphere, _spawner._spawnSphere) + _spawner._posBuffer.x;
	t.z = Random.Range(-_spawner._spawnSphereDepth, _spawner._spawnSphereDepth) + _spawner._posBuffer.z;
	t.y = Random.Range(-_spawner._spawnSphereHeight, _spawner._spawnSphereHeight) + _spawner._posBuffer.y;
	return t;
}

function Soar(){
	if(_move){
		 _model.GetComponent.<Animation>().CrossFade(_spawner._soarAnimation, 1.5);
	   	_wayPoint= findWaypoint();
	    _soar = true;
    }
}

function Dive(){
	if(_spawner._soarAnimation!=null){
		_model.GetComponent.<Animation>().CrossFade(_spawner._soarAnimation, 1.5);
	}else{
		for (var state : AnimationState in _model.GetComponent.<Animation>()) {
   	 		if(_thisT.position.y < _wayPoint.y +25){
   	 			state.speed = 0.1;
   	 		}
   	 	}
 	}
 	_wayPoint= findWaypoint();
	_wayPoint.y -= _spawner._diveValue;
	_dived = true;
}

function animationSpeed(){
	for (var state : AnimationState in _model.GetComponent.<Animation>()) {
		if(!_dived && !_landing){
			state.speed = Random.Range(_spawner._minAnimationSpeed, _spawner._maxAnimationSpeed);
		}else{
			state.speed = _spawner._maxAnimationSpeed;
		}   
	}
}