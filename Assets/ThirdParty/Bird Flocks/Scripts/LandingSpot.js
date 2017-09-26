/**************************************									
	Copyright Unluck Software	
 	www.chemicalbliss.com						
***************************************/
#pragma strict
@HideInInspector
var landingChild:FlockChild;
@HideInInspector
var landing:boolean;
private var lerpCounter:int;
@HideInInspector
var _controller:LandingSpotController;
private var _idle:boolean;
var _thisT:Transform;					//Reference to transform component


function Start() {
	if(!_thisT)		_thisT = transform;
    if (!_controller)
        _controller = _thisT.parent.GetComponent(LandingSpotController);
    if (_controller._autoCatchDelay.x > 0)
        GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y);   
	RandomRotate();
}

function OnDrawGizmos() {
	if(!_thisT)		_thisT = transform;
	if (!_controller)
        _controller = _thisT.parent.GetComponent(LandingSpotController);
    
    Gizmos.color = Color.yellow;
    // Draw a yellow cube at the transforms position
    if (landingChild && landing)
        Gizmos.DrawLine(_thisT.position, landingChild._thisT.position);
    if (_thisT.rotation.eulerAngles.x != 0 || _thisT.rotation.eulerAngles.z != 0)
        _thisT.eulerAngles = new Vector3(0, _thisT.eulerAngles.y, 0);
    Gizmos.DrawWireCube(Vector3(_thisT.position.x, _thisT.position.y, _thisT.position.z), Vector3(.2, .2, .2));
    Gizmos.DrawWireCube(_thisT.position + (_thisT.forward * .2), Vector3(.1, .1, .1));
    Gizmos.color = Color(1, 1, 0, .05);
    Gizmos.DrawWireSphere(_thisT.position, _controller._maxBirdDistance);
}

function LateUpdate() {
    if (_controller._flock.gameObject.activeInHierarchy && landing && landingChild) {
    	if(!landingChild.gameObject.activeInHierarchy){ 
    		ReleaseFlockChild(0,0);
    	}
    	//Check distance to flock child
        var distance:float = Vector3.Distance(landingChild._thisT.position, _thisT.position);
        //Start landing if distance is close enough
        if (distance < 5 && distance > .5) {
            if(_controller._soarLand){
            	landingChild._model.GetComponent.<Animation>().CrossFade(landingChild._spawner._soarAnimation, .5);
            	if (distance < 2)
           	 		landingChild._model.GetComponent.<Animation>().CrossFade(landingChild._spawner._flapAnimation, .5);
            }
            landingChild._targetSpeed = landingChild._spawner._maxSpeed*.5;
          	landingChild._wayPoint = _thisT.position;      	
            landingChild._damping = _controller._landingTurnSpeedModifier;
            landingChild._avoid = false;
        } else if (distance <= .5) {
        	
            landingChild._wayPoint = _thisT.position;
           
            if (distance < .1 && !_idle) {
                _idle = true;
                landingChild._model.GetComponent.<Animation>().CrossFade(landingChild._spawner._idleAnimation, .55); 
            }
            
            if (distance > .01){       	
            	landingChild._targetSpeed = landingChild._spawner._minSpeed*this._controller._landingSpeedModifier;
          	    landingChild._thisT.position += (_thisT.position - landingChild._thisT.position) * Time.deltaTime *landingChild._speed*_controller._landingSpeedModifier;     	
          	}
            
            landingChild._move = false;
            lerpCounter++;
     		
     		var rot:Quaternion = landingChild._thisT.rotation;
     		var rotE:Vector3 = rot.eulerAngles;     		
     		rotE.y = Mathf.LerpAngle(landingChild._thisT.rotation.eulerAngles.y, _thisT.rotation.eulerAngles.y, lerpCounter * Time.deltaTime * .005);  		
            rot.eulerAngles = rotE;
            landingChild._thisT.rotation = rot;

            landingChild._damping =  _controller._landingTurnSpeedModifier;
        } else {
        	//Move towards landing spot
            landingChild._wayPoint = _thisT.position;
            landingChild._damping = 1;
        }

    } 
}

function GetFlockChild(minDelay:float, maxDelay:float):IEnumerator {
    yield WaitForSeconds(Random.Range(minDelay, maxDelay));
    if (_controller._flock.gameObject.activeInHierarchy && !landingChild) {
		RandomRotate();
    
        var fChild:FlockChild;

        for (var i:int = 0; i < _controller._flock._roamers.Count; i++) {
            var child:FlockChild = _controller._flock._roamers[i] as FlockChild;
            if (!child._landing && !child._dived) {         
            	if(!_controller._onlyBirdsAbove){     	
	                if (!fChild && _controller._maxBirdDistance > Vector3.Distance(child._thisT.position, _thisT.position) && _controller._minBirdDistance < Vector3.Distance(child._thisT.position, _thisT.position)) {
	                    fChild = child;
	                    if (!_controller._takeClosest) break;
	                } else if (fChild && Vector3.Distance(fChild._thisT.position, _thisT.position) > Vector3.Distance(child._thisT.position, _thisT.position)) {
	                    fChild = child;
	                }
                }else{
                	if (!fChild && child._thisT.position.y > _thisT.position.y && _controller._maxBirdDistance > Vector3.Distance(child._thisT.position, _thisT.position) && _controller._minBirdDistance < Vector3.Distance(child._thisT.position, _thisT.position)) {
	                    fChild = child;
	                    if (!_controller._takeClosest) break;
	                } else if (fChild && child._thisT.position.y > _thisT.position.y && Vector3.Distance(fChild._thisT.position, _thisT.position) > Vector3.Distance(child._thisT.position, _thisT.position)) {
						fChild = child;
	                }
                }
            }
        }
        if (fChild) {
            landingChild = fChild;
            landing = true;
           	landingChild._landing = true;
           	ReleaseFlockChild(_controller._autoDismountDelay.x, _controller._autoDismountDelay.y);
        } else if (_controller._autoCatchDelay.x > 0) {
            GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y);
        }
    }
}

function RandomRotate(){	
	if (_controller._randomRotate){
		var rot:Quaternion = _thisT.rotation;
     	var rotE:Vector3 = rot.eulerAngles;     		
     	rotE.y = Random.Range(0, 360);
        rot.eulerAngles = rotE;
		_thisT.rotation = rot;
		}
}

function InstantLand() {
    if (_controller._flock.gameObject.activeInHierarchy && !landingChild) {
        var fChild:FlockChild;
      
        for (var i:int = 0; i < _controller._flock._roamers.Count; i++) {
            var child:FlockChild = _controller._flock._roamers[i] as FlockChild;
            if (!child._landing && !child._dived) {
                     fChild = child;           
            }
        }
        if (fChild) {
            landingChild = fChild;
            landing = true;

            landingChild._landing = true;
            landingChild._thisT.position = _thisT.position;
            landingChild._model.GetComponent.<Animation>().Play(landingChild._spawner._idleAnimation);
            ReleaseFlockChild(_controller._autoDismountDelay.x, _controller._autoDismountDelay.y);
        } else if (_controller._autoCatchDelay.x > 0) {
            GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y);
        }
    }
}

function ReleaseFlockChild(minDelay:float, maxDelay:float) {
    yield WaitForSeconds(Random.Range(minDelay, maxDelay));
    if (_controller._flock.gameObject.activeInHierarchy && landingChild) {
        lerpCounter = 0;
        if (_controller._featherPS){
			_controller._featherPS.position = landingChild._thisT.position;
			_controller._featherPS.GetComponent.<ParticleSystem>().Emit(Random.Range(0,3));
        }           
		landing = false;
        _idle = false;
        landingChild._avoid = true;
        //Reset flock child to flight mode
        landingChild._damping = landingChild._spawner._maxDamping;
        landingChild._model.GetComponent.<Animation>().CrossFade(landingChild._spawner._flapAnimation, .2);
		landingChild._dived = true;
        landingChild._speed = 0;       
        landingChild._move = true;
        landingChild._landing = false;
        landingChild.Flap();     	
        landingChild._wayPoint = Vector3(landingChild._wayPoint.x, _thisT.position.y+10, landingChild._wayPoint.z);             
        yield WaitForSeconds(.1);
         if (_controller._autoCatchDelay.x > 0) {
            GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y);
        }
        landingChild = null;
    }
}