#pragma strict
//Simple GUI script demo controlling bird flock and landing spots.

var _landingSpotController:LandingSpotController;
var _flockController:FlockController;
var hSliderValue : float = 250.0;

function OnGUI () {
	GUI.Label(Rect(20,20,125,18),"Landing Spots: " + _landingSpotController.transform.childCount);
	if(GUI.Button(Rect(20,40,125,18),"Scare All"))
		_landingSpotController.ScareAll();
	if(GUI.Button(Rect(20,60,125,18),"Land In Reach"))
		_landingSpotController.LandAll();
	if(GUI.Button(Rect(20,80,125,18),"Land Instant"))
		_landingSpotController.InstantLand(0.01);
	if(GUI.Button(Rect(20,100,125,18),"Destroy")){
		_flockController.destroyBirds();
		}
	GUI.Label(Rect(20,120,125,18),"Bird Amount: " + _flockController._childAmount);
	 _flockController._childAmount = GUI.HorizontalSlider(Rect (20, 140, 125, 18), _flockController._childAmount, 0.0f , 250.0f);
}