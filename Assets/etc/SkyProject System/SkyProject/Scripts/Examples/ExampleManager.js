#pragma strict

var calendar : CalendarGUI;
var changeColors : ChangeSkyColors;
var rain : ActiveRain;
var red : float = 0;
var green : float = 0;
var blue : float = 0;
var red2 : float = 0;
var green2 : float = 0;
var blue2 : float = 0;

private var mark1 = "x";
private var mark2 = " ";
private var mark3 = " ";
private var check1 = false;
private var check2 = false;
function OnGUI(){

//SEASONS
if(GUI.Button(Rect(Screen.width-700,20,64,32),"Spring")){
SkyProject.instance.ActualTimeGame.days=21;
SkyProject.instance.ActualTimeGame.month=3;
}

if(GUI.Button(Rect(Screen.width-700,60,64,32),"Summer")){
SkyProject.instance.ActualTimeGame.days=21;
SkyProject.instance.ActualTimeGame.month=6;
}

if(GUI.Button(Rect(Screen.width-630,20,64,32),"Auttumn")){
SkyProject.instance.ActualTimeGame.days=23;
SkyProject.instance.ActualTimeGame.month=9;
}

if(GUI.Button(Rect(Screen.width-630,60,64,32),"Winter")){
SkyProject.instance.ActualTimeGame.days=23;
SkyProject.instance.ActualTimeGame.month=12;
}

//CALENDAR
if(GUI.Button(Rect(Screen.width-500,20,128,64),"Calendar"+"["+mark1+"]")){
calendar.enabled=!calendar.enabled;
if(calendar.enabled==true){
mark1 = "x";
}
if(calendar.enabled==false){
mark1 = " ";
}
}

//RAIN
if(GUI.Button(Rect(Screen.width-350,20,128,64),"Rain"+"["+mark2+"]")){
rain.instanceActiveRain.rainCheck=!rain.instanceActiveRain.rainCheck;
if(rain.instanceActiveRain.rainCheck==true){
mark2 = "x";
}
if(rain.instanceActiveRain.rainCheck==false){
mark2 = " ";
}
}

//CHANGECOLORS
if(GUI.Button(Rect(Screen.width-200,20,128,64),"Change Colors"+"["+mark3+"]")){
changeColors.instanceChangeColors.colorCheck=!changeColors.instanceChangeColors.colorCheck;
check1=!check1;
if(changeColors.instanceChangeColors.colorCheck==true){
mark3 = "x";
}
if(changeColors.instanceChangeColors.colorCheck==false){
mark3 = " ";
}
}
if(check1 == true){
if(GUI.Button(Rect(Screen.width-200,100,128,64),"Custom Colors?")){
check2=!check2;
}

if(check2==true){
GUI.Label(Rect (Screen.width-485, 100, 64, 20), "Red");
GUI.Label(Rect (Screen.width-385, 100, 64, 20), "Green");
GUI.Label(Rect (Screen.width-285, 100, 64, 20), "Blue");
GUI.Label(Rect (Screen.width-560, 118, 64, 20), "Top");
GUI.Label(Rect (Screen.width-560, 148, 64, 20), "Bottom");
red = GUI.HorizontalSlider (Rect (Screen.width-500, 120, 64, 20), red,0.0,1.0);
green = GUI.HorizontalSlider (Rect (Screen.width-400, 120, 64, 20), green,0.0,1.0);
blue = GUI.HorizontalSlider (Rect (Screen.width-300, 120, 64, 20), blue,0.0,1.0);
red2 = GUI.HorizontalSlider (Rect (Screen.width-500, 150, 64, 20), red2,0.0,1.0);
green2 = GUI.HorizontalSlider (Rect (Screen.width-400, 150, 64, 20), green2,0.0,1.0);
blue2 = GUI.HorizontalSlider (Rect (Screen.width-300, 150, 64, 20), blue2,0.0,1.0);
changeColors.instanceChangeColors.topColor = Color(red,green,blue);
changeColors.instanceChangeColors.bottomColor = Color(red2,green2,blue2);
}
}
}