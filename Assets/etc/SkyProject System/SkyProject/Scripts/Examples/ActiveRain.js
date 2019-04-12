#pragma strict

static var instanceActiveRain : ActiveRain;
instanceActiveRain = FindObjectOfType(ActiveRain);
if(instanceActiveRain == null)
Debug.Log("Couldn't locate SkyProject in the scene.");
var blendTime : float = 1;
var rainCheck = false;




function FixedUpdate(){
	
if(rainCheck){
SkyProject.instance.rainlight = true;
SkyProject.instance.Weather.RainSettings.randomNumber = 0;
}
if(!rainCheck){
SkyProject.instance.rainlight = false;
SkyProject.instance.Weather.RainSettings.randomNumber = 100;
}
}
