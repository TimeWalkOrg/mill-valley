#pragma strict
//////////////////////////////////
//CALENDAR FOR SKYPROJECT SYSTEM//
//////////////////////////////////


function OnGUI(){

GUI.Box(Rect(20,20,64,24),"Hour : "+SkyProject.instance.ActualTimeGame.hour);
GUI.Box(Rect(100,20,80,24),"Minutes : "+SkyProject.instance.ActualTimeGame.minutes);
GUI.Box(Rect(200,20,128,24),"Seconds : "+SkyProject.instance.ActualTimeGame.seconds);

GUI.Box(Rect(20,60,64,24),"Year : "+SkyProject.instance.ActualTimeGame.year);
GUI.Box(Rect(100,60,80,24),"Month : "+SkyProject.instance.ActualTimeGame.month);
GUI.Box(Rect(200,60,128,24),"Day : "+SkyProject.instance.ActualTimeGame.days);

GUI.Box(Rect(20,100,170,24),"Temperature : "+SkyProject.instance.Temperature.actualTemperature+" °C");
}

