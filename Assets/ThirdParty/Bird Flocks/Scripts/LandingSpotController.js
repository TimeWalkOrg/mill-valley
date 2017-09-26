/**************************************									
	LandingSpotController
	Copyright Unluck Software	
 	www.chemicalbliss.com					
***************************************/
#pragma strict
var _randomRotate:boolean = true;					// Random rotation when a bird lands
var _autoCatchDelay:Vector2 = Vector2(10, 20);		// Random Min/Max time for landing spot to make a bird land
var _autoDismountDelay:Vector2 = Vector2(10, 20);	// Random Min/Max time for birds to automaticly fly away from landing spot
var _maxBirdDistance:float = 20;					// The maximum distance to a bird for it to land
var _minBirdDistance:float = 5;						// The minimum distance to a bird for it to land
var _takeClosest:boolean;							// Toggle this to make landingspots make the closest bird to it land
var _flock:FlockController;							// Assign the FlockController to pick birds from
var _landOnStart:boolean;							// Put birds on the landing spots at start
var _soarLand:boolean = true;						// Birds will soar while aproaching landing spot
var _onlyBirdsAbove:boolean;						// Only birds above landing spot will land
var _landingSpeedModifier:float = .5;				// Adjust bird movement speed while clost to the landing spot
var _landingTurnSpeedModifier:float = 5;
var _featherPS:Transform;							// Update: Changed from GameObject to Transform 
var _thisT:Transform;								// Transform reference

function Start () {
	if(!_thisT) _thisT = transform;
	if(!_flock){
	 _flock = GameObject.FindObjectOfType(FlockController);
	 Debug.Log(this + " has no assigned FlockController, a random FlockController has been assigned");
	 }
	 
	#if UNITY_EDITOR
	if(_autoCatchDelay.x >0 &&(_autoCatchDelay.x < 5||_autoCatchDelay.y < 5)){
		Debug.Log(this.name + ": autoCatchDelay values set low, this might result in strange behaviours");
	}
	#endif
	
	if(_landOnStart){
		InstantLandOnStart(.1);
	}
}

function ScareAll () {
	for (var i:int=0;  i< _thisT.childCount; i++){
		if(_thisT.GetChild(i).GetComponent(LandingSpot)){
		var spot:LandingSpot = _thisT.GetChild(i).GetComponent(LandingSpot);
		spot.ReleaseFlockChild(0,1);
		}
	}
}

function ScareAll (minDelay:float, maxDelay:float) {
	for (var i:int=0;  i< _thisT.childCount; i++){
		if(_thisT.GetChild(i).GetComponent(LandingSpot)){
		var spot:LandingSpot = _thisT.GetChild(i).GetComponent(LandingSpot);
		spot.ReleaseFlockChild(minDelay,maxDelay);
		}
	}
}
function LandAll () {
	for (var i:int=0;  i< _thisT.childCount; i++){	
		if(_thisT.GetChild(i).GetComponent(LandingSpot)){		
		var spot:LandingSpot = _thisT.GetChild(i).GetComponent(LandingSpot);
		spot.GetFlockChild(0,2);
		}
	}
}
//This function was added to fix a error with having a button calling InstantLand
function InstantLandOnStart (delay:float) {
	yield WaitForSeconds(delay);
	for (var i:int=0;  i< _thisT.childCount; i++){			
		if(_thisT.GetChild(i).GetComponent(LandingSpot)){
		var spot:LandingSpot = _thisT.GetChild(i).GetComponent(LandingSpot);
		spot.InstantLand();
		}
	}
}

function InstantLand (delay:float) {
	yield WaitForSeconds(delay);
	for (var i:int=0;  i< _thisT.childCount; i++){	
		if(_thisT.GetChild(i).GetComponent(LandingSpot)){		
		var spot:LandingSpot = _thisT.GetChild(i).GetComponent(LandingSpot);
		spot.InstantLand();
		}
	}
}