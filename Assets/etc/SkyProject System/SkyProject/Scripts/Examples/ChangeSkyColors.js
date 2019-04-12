#pragma strict

static var instanceChangeColors : ChangeSkyColors;
instanceChangeColors = FindObjectOfType(ChangeSkyColors);
if(instanceChangeColors == null)
Debug.Log("Couldn't locate Change Colors in the scene.");

var blendTime : float = 1;
var colorCheck = false;
var topColor: Color;
var bottomColor : Color;
var ambientColor : Color;
function FixedUpdate(){

	
if(colorCheck){
SkyProject.instance.stopColors=true;
SkyProject.instance.stopSunColor=true;
SkyProject.instance.stopAmbient=true;
//That's the code for stop the top color of the sky and change it to another.
SkyProject.instance.topColor = Color.Lerp(SkyProject.instance.topColor, topColor, blendTime);
//That's the code for stop the bottom color of the sky and change it to another.
SkyProject.instance.bottomColor = Color.Lerp(SkyProject.instance.bottomColor, bottomColor, blendTime);
RenderSettings.ambientLight = ambientColor;
}

if(!colorCheck){
//That's the code for return to the Sky Colors.
SkyProject.instance.stopColors=false;
SkyProject.instance.stopSunColor=false;
SkyProject.instance.stopAmbient=false;
}
}