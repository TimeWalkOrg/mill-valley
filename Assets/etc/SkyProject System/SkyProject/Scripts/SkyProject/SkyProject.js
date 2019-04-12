/////////////////////////////////////////////////////////////////////////
//				Made by KirbyRawr from Overflowing Studios.			   //
//						Overflowing Studios©						   //
//					     Thanks for buy it 	:)						   //
//			If you pirated it consider buying it in Asset Store. :D    //
/////////////////////////////////////////////////////////////////////////

#pragma strict
static var instance : SkyProject;
instance = FindObjectOfType(SkyProject);
if(instance == null)
Debug.Log("Couldn't locate SkyProject in the scene.");
//Private etc...
enum modeOfTheFog {Linear,Exponential,Exp2};
enum phaseOfTheDay {Sunrise,Day,Sunset,Night};
enum seasonsTime {Spring,Summer,Auttumn,Winter};
static var topColor : Color;
static var bottomColor : Color;
static var fogColor : Color;
private var timeLerp : float = 0;
private var timeLerpFog : float = 0;
private var timeLerpIntensity : float = 0;
private var timeLerpSunColors : float = 0;
private var timeLerpCloudsColors : float = 0;
private var timeLerpAmbientLight : float = 0;
private var timeLerpLightRain : float = 0;
private var timeLerpHeavyRain : float = 0;
private var timeLerpRain : float = 0;
private var timeLerpSun : float = 0;
private var timeLerpSunHorizon : float = 0;
private var sunColor : Color = Color(0.2, 0.3, 0.4, 0.5);
private var sunHorizonColor : Color = Color(0.2, 0.3, 0.4, 0.5);
private var sunColor2 : Color = Color(0.2, 0.3, 0.4, 0.5);
private var sunHorizonColor2 : Color = Color(0.2, 0.3, 0.4, 0.5);
private var sunColor3 : Color = Color(0.2, 0.3, 0.4, 0.5);
private var sunHorizonColor3 : Color = Color(0.2, 0.3, 0.4, 0.5);


private var cameraOfSky : GameObject;
private var alpha : float = 0.0;
private var Lerping = true;
private var frac : float;
private var frac2 : float;
private var rainFade : float;
private var divTime : float = 33000;
private var sunTex : GameObject;
private var sunHorizonTex : GameObject;

//Stoppers
private public var stopColors = false;
private public var stopSunColor = false;
private public var stopSunIntensity = false;
private public var stopAmbient = false;
private public var stopFog = false;
private public var stopClouds = false;


//One Time Vars!
private var oneTimeFunction = false;
private var oneTimeFunction2 = false;
private var oneTimeFunction3 = false;
private var oneTimeFunction4 = false;
private var oneTimeFunction5 = false;

//One Time Phases
private var oneTimeSunRise = false;
private var oneTimeDay = false;
private var oneTimeSunSet = false;
private var oneTimeNight = false;

//One Time Seasons
private var SpringTimeCheck = false;
private var SummerTimeCheck  = false;
private var AuttumTimeCheck  = false;
private var WinterTimeCheck  = false;

//One Time Weather
private var oneTimeRain = false;

//Checkers for Phases.
private var sunRise = false;
private var day = false;
private var sunSet = false;
private var night = false;

//Private GameTime
private var yearInMonths = 13;
private var monthInDays = 31;
private var dayInHours = 24;
private var hourInMinutes = 60;
private var minuteInSeconds = 60;

//Private Variables Day Phases!
private  var sunRiseHour = 7;
private  var sunRiseMinutes = 45;
 	
private  var dayHour = 8;
private  var dayMinutes = 30;
  
private  var sunSetHour =20;
private  var sunSetMinutes = 45;
  
private  var nightHour =21;
private  var nightMinutes = 30;

//Private Weather
static var rainlight = false;
static var rainheavy = false;
//ANOTHER VARIABLES
static var billboardCamera : Camera;



var PhaseOfTheDay : phaseOfTheDay = phaseOfTheDay.Sunrise;
var SeasonsTime : seasonsTime = seasonsTime.Spring;



class generalSettings{
var mainCamera : Camera;
var skyProjectLayer : int = 8;
var terrainvar : Terrain[];
var Helpers : helpers;
class helpers{
 var sunrise : Transform;
 var sunset : Transform;
 }
}

//The Time of The Game
 class timeGame{
var year : int = 0;
var month = 0;
var days = 0;
var hour  = 0;
var minutes = 0;
var seconds : float = 0;
var timeSpeed : float = 1;
}

//Colors of the Sky depends of the phase.
class colorsDay{
var blendTimeColor : float = 1;
var SunRiseColors : sunRiseColors;
var DayColors : dayColors;
var SunSetColors : sunSetColors;
var NightColors : nightColors;


class sunRiseColors{
var sunRiseColorTop : Color = Color.white;
var sunRiseColorBottom : Color = Color.white;
var sunRiseColorGlobal : Color = Color.white;
}

class dayColors{
var dayColorTop : Color = Color.white;
var dayColorBottom : Color = Color.white;
var dayColorGlobal : Color = Color.white;
}

class sunSetColors{
var sunSetColorTop : Color = Color.white;
var sunSetColorBottom : Color = Color.white;
var sunSetColorGlobal : Color = Color.white;
}


class nightColors{
var nightColorTop : Color = Color.black;
var nightColorBottom : Color = Color.blue;
var nightColorGlobal : Color = Color.white;
}
}

//Cloud Settings
class cloudSettings{
var cloudsInGame = true;
var clouds : GameObject;
var blendTimeClouds : float = 1;
var CloudsColors : cloudsColors;

class cloudsColors{
var SunRiseCloudsColor : Color;
var DayCloudsColor : Color;
var SunSetCloudsColor : Color;
var NightCloudsColor : Color;
}
}

//Fog Settings
class fogSettings{

var dinamicFog = false;
var ModeOfTheFog : modeOfTheFog = modeOfTheFog.Exp2;
var startDistance : float = 0;
var endDistance : float = 200;
var density : float = 0.01;
var ColorsFog : fogSettings.colorsFog;

class colorsFog{
var blendFog : float = 1;
var SunRiseFog : Color;
var DayFog : Color;
var SunSetFog : Color;
var NightFog : Color;
}
}

//Seasons
class seasons{

var seasonsInGame = false;

var Spring : spring;
var Summer : summer;
var Auttum : auttum;
var Winter : winter;
var SpringTexturesSeason;

class spring{
var springMonth = 3;
var springDay = 21;
var grassColorDrySpring : Color;
var grassColorHealthySpring : Color;
var springTexturesSeason : SpringTexturesSeason;
var springTrees : SpringTrees;

class SpringTexturesSeason{
var texturesSpring : Texture2D[];
}
class SpringTrees{
var treesSpring : GameObject[];
}
}

class summer{
var summerMonth = 6;
var summerDay = 21;
var grassColorDrySummer : Color;
var grassColorHealthySummer : Color;
var summerTexturesSeason : SummerTexturesSeason;
var summerTrees : SummerTrees;

class SummerTexturesSeason{
var texturesSummer : Texture2D[];
}

class SummerTrees{
var treesSummer : GameObject[];
}
}

class auttum{
var auttumMonth = 9;
var auttumDay = 23;
var grassColorDryAuttum : Color;
var grassColorHealthyAuttum : Color;
var auttumTexturesSeason : AuttumTexturesSeason;
var auttumTrees : AuttumTrees;

class AuttumTexturesSeason{
var texturesAuttum : Texture2D[];
}

class AuttumTrees{
var treesAuttum : GameObject[];
}
}

class winter{
var winterMonth = 12;
var winterDay = 23;
var grassColorDryWinter : Color;
var grassColorHealthyWinter : Color;
var winterTexturesSeason : WinterTexturesSeason;
var winterTrees : WinterTrees;

class WinterTexturesSeason{
var texturesWinter : Texture2D[];
}

class WinterTrees{
var treesWinter : GameObject[];
}

}
}

//Temperature
class temperature{
//Some vars -^w^-
var actualTemperature : float;
var SpringTemperature : temperature.springTemperature;
var SummerTemperature : temperature.summerTemperature;
var AuttumTemperature : temperature.auttumTemperature;
var WinterTemperature : temperature.winterTemperature;

//SpringTemperature
class springTemperature{
var SunRiseTemperatureSpring : sunRiseTemperatureSpring;
var DayTemperatureSpring : dayTemperatureSpring;
var SunSetTemperatureSpring : sunSetTemperatureSpring;
var NightTemperatureSpring : nightTemperatureSpring;

class sunRiseTemperatureSpring{
var minimumSpring : float = 15;
var maximumSpring : float = 30;
}

class dayTemperatureSpring{
var minimumSpring : float = 15;
var maximumSpring : float = 30;
}

class sunSetTemperatureSpring{
var minimumSpring : float = 15;
var maximumSpring : float = 30;
}

class nightTemperatureSpring{
var minimumSpring : float = 15;
var maximumSpring : float = 30;
}
}

//SummerTemperature
class summerTemperature{

var SunRiseTemperatureSummer : sunRiseTemperatureSummer;
var DayTemperatureSummer : dayTemperatureSummer;
var SunSetTemperatureSummer : sunSetTemperatureSummer;
var NightTemperatureSummer : nightTemperatureSummer;

class sunRiseTemperatureSummer{
var minimumSummer : float = 15;
var maximumSummer : float = 30;
}

class dayTemperatureSummer{
var minimumSummer : float = 15;
var maximumSummer : float = 30;
}

class sunSetTemperatureSummer{
var minimumSummer : float = 15;
var maximumSummer : float = 30;
}

class nightTemperatureSummer{
var minimumSummer : float = 15;
var maximumSummer : float = 30;
}
}

//AuttumTemperature
class auttumTemperature{

var SunRiseTemperatureAuttum : sunRiseTemperatureAuttum;
var DayTemperatureAuttum : dayTemperatureAuttum;
var SunSetTemperatureAuttum : sunSetTemperatureAuttum;
var NightTemperatureAuttum : nightTemperatureAuttum;

class sunRiseTemperatureAuttum{
var minimumAuttum : float = 15;
var maximumAuttum : float = 30;
}

class dayTemperatureAuttum{
var minimumAuttum : float = 15;
var maximumAuttum : float = 30;
}

class sunSetTemperatureAuttum{
var minimumAuttum : float = 15;
var maximumAuttum : float = 30;
}

class nightTemperatureAuttum{
var minimumAuttum : float = 15;
var maximumAuttum : float = 30;
}
}

//WinterTemperature
class winterTemperature{

var SunRiseTemperatureWinter : sunRiseTemperatureWinter;
var DayTemperatureWinter : dayTemperatureWinter;
var SunSetTemperatureWinter : sunSetTemperatureWinter;
var NightTemperatureWinter : nightTemperatureWinter;

class sunRiseTemperatureWinter{
var minimumWinter : float = 15;
var maximumWinter : float = 30;
}

class dayTemperatureWinter{
var minimumWinter : float = 15;
var maximumWinter : float = 30;
}

class sunSetTemperatureWinter{
var minimumWinter : float = 15;
var maximumWinter : float = 30;
}

class nightTemperatureWinter{
var minimumWinter : float = 15;
var maximumWinter : float = 30;
}
}
}

//Light Settings
class lightSettings{

var dinamicLight = false;
var dinamicAmbientLight = false;
var moonInGame = false;

var Sun : GameObject;
var Moon : GameObject;
var LightColors : lightSettings.lightColors;
var Intensities : lightSettings.intensities;
var LightAmbientColors : lightAmbientColors;


class lightColors{
var blendTimeColorLight : float = 1;
var SunRiseLight : Color;
var DayLight : Color;
var SunSetLight : Color;
var NightLight : Color;
}

class lightAmbientColors{
var blendTimeAmbientColor : float = 1;
var SunRiseAmbientColor : Color;
var DayAmbientColor : Color;
var SunSetAmbientColor : Color;
var NightAmbientColor : Color;


}
class intensities{
var blendTimeIntensity : float = 1;
var sunRiseIntensity : float = 0.5;
var dayIntensity : float = 0.5;
var sunSetIntensity : float = 0.5;
var nightIntensity : float = 0.5;
}
}

//Splash Screens
class splashScreen{
private var alpha : float = 1.0;
var SplashScreensInGame = false;
var fadeInTime : float = 1;
var fadeOutTime : float = 1;
var waitTime : float = 1.0;
var SplashTextures : splashTextures;

class splashTextures{
var springSplash : GUITexture;
var summerSplash : GUITexture;
var auttumSplash : GUITexture;
var winterSplash : GUITexture;
}
}

class weather{
var weatherInGame = false; 
var RainSettings : rainSettings;

class rainSettings{
var rainInGame = false;
var rain : GameObject;
var blendRain : float = 0.005;
var blendTimeLightRain : float = 1;
var blendTimeHeavyRain : float = 1;

var SpringRain : springRain;
var SummerRain : summerRain;
var AuttumRain : auttumRain;
var WinterRain : winterRain;

static var randomNumber = 0;
public var randomType = 0;

//Spring
class springRain{
var rainPercentage = 50;
var rainTypePercentage = 50;

var LightRain : lightRain;
var HeavyRain : heavyRain;

class lightRain {
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;

}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;

}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;

}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;

}
}
class heavyRain{
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
}
}

//Summer
class summerRain{
var rainPercentage = 50;
var rainTypePercentage = 50;
var LightRain : lightRain;
var HeavyRain : heavyRain;

class lightRain {
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
}

class heavyRain{
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
}

}

//Auttum
class auttumRain{
var rainPercentage = 50;
var rainTypePercentage = 50;
var LightRain : lightRain;
var HeavyRain : heavyRain;

class lightRain {
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;

}
}
class heavyRain{
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
}
}

//Winter
class winterRain{
var rainPercentage = 50;
var rainTypePercentage = 50;
var LightRain : lightRain;
var HeavyRain : heavyRain;

class lightRain {
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
}
class heavyRain{
var maximumTemperature : float = 25;
var minimumTemperature : float = 5;
var rainFade : float = 1;

var SunRiseColorsRain : sunRiseColorsRain;
var DayColorsRain : dayColorsRain;
var SunSetColorsRain : sunSetColorsRain;
var NightColorsRain : nightColorsRain;

class sunRiseColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class dayColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class sunSetColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
class nightColorsRain{
var topColor : Color;
var bottomColor : Color;
var ambientColor : Color;
}
}
}
}
}
//Another//

///////////

//Variables of the classes.
var GeneralSettings : generalSettings;
var ColorsOfTheDay : colorsDay;
var LightSettings : lightSettings;
var FogSettings : fogSettings;
var CloudSettings : cloudSettings;
var Seasons : seasons;
var SplashScreens : splashScreen;
var Temperature : temperature;
var Weather : weather;
var ActualTimeGame : timeGame;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////F//////////U///////////N///////////C///////////T//////////////I////////////O////////////N///////////////S///////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function Awake(){
Helpers();
sunTex = GameObject.Find("/SkySphere/Sun & Moon/SunLight/Sun");	
sunHorizonTex = GameObject.Find("/SkySphere/Sun & Moon/SunLight/Sun/SunHorizon");

sunColor = Color(sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").r,sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").g
, sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").b, 0);

sunHorizonColor = Color(sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").r,sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").g
, sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").b, 0);

sunColor2 = Color(sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").r,sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").g
, sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").b, sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").a);

sunHorizonColor2 = Color(sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").r,sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").g
, sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").b, sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").a);

sunColor3 = Color(sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").r,sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").g
, sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor").b, 0.8);

sunHorizonColor3 = Color(sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").r,sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").g
, sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor").b, 0.0);

//CASES FOG
	if (FogSettings.ModeOfTheFog == modeOfTheFog.Linear && FogSettings.dinamicFog==true){
	RenderSettings.fogMode = FogMode.Linear;
	RenderSettings.fogStartDistance = FogSettings.startDistance;
	RenderSettings.fogEndDistance = FogSettings.endDistance;
	}
	
	if (FogSettings.ModeOfTheFog == modeOfTheFog.Exponential && FogSettings.dinamicFog==true){
	RenderSettings.fogMode = FogMode.Exponential;
	RenderSettings.fogDensity = FogSettings.density;
	}
	
	if (FogSettings.ModeOfTheFog == modeOfTheFog.Exp2 && FogSettings.dinamicFog==true){
	RenderSettings.fogMode = FogMode.ExponentialSquared;
	RenderSettings.fogDensity = FogSettings.density;
	}

//CASES PHASES

	//SunriseCase
	if (PhaseOfTheDay == phaseOfTheDay.Sunrise){
	ActualTimeGame.hour = sunRiseHour;
	ActualTimeGame.minutes = sunRiseMinutes;
	
	
	if(SeasonsTime==seasonsTime.Spring){
	
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunRiseTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.SunRiseTemperatureSpring.maximumSpring);
	}
	
	if(SeasonsTime==seasonsTime.Summer){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunRiseTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.SunRiseTemperatureSummer.maximumSummer);
	}
	
	if(SeasonsTime==seasonsTime.Auttumn){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.SunRiseTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.SunRiseTemperatureAuttum.maximumAuttum);
	}
	
	if(SeasonsTime==seasonsTime.Winter){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.SunRiseTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.SunRiseTemperatureWinter.maximumWinter);
	}
	
	}

	//DayCase
	if (PhaseOfTheDay == phaseOfTheDay.Day){
	ActualTimeGame.hour = dayHour;
	ActualTimeGame.minutes = dayMinutes;  
	
	frac = 0.05454542;
	if(SeasonsTime==seasonsTime.Spring){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.DayTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.DayTemperatureSpring.maximumSpring);
	}
	
	if(SeasonsTime==seasonsTime.Summer){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.DayTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.DayTemperatureSummer.maximumSummer);
	}
	
	if(SeasonsTime==seasonsTime.Auttumn){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.DayTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.DayTemperatureAuttum.maximumAuttum);
	}
	
	if(SeasonsTime==seasonsTime.Winter){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.DayTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.DayTemperatureWinter.maximumWinter);
	}
	
	}

	//SunsetCase
	if (PhaseOfTheDay == phaseOfTheDay.Sunset){
	ActualTimeGame.hour = sunSetHour;
	ActualTimeGame.minutes = sunSetMinutes;
	
	
	if(SeasonsTime==seasonsTime.Spring){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunSetTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.SunSetTemperatureSpring.maximumSpring);
	}
	
	if(SeasonsTime==seasonsTime.Summer){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunSetTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.SunSetTemperatureSummer.maximumSummer);
	}
	
	if(SeasonsTime==seasonsTime.Auttumn){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.SunSetTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.SunSetTemperatureAuttum.maximumAuttum);
	}
	
	if(SeasonsTime==seasonsTime.Winter){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.SunSetTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.SunSetTemperatureWinter.maximumWinter);
	}
	
	frac = 0.9454537;
	}

	//NightCase
	if (PhaseOfTheDay == phaseOfTheDay.Night){
	ActualTimeGame.hour = nightHour;
	ActualTimeGame.minutes = nightMinutes;  
	
	
	if(SeasonsTime==seasonsTime.Spring){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.NightTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.NightTemperatureSpring.maximumSpring);
	}
	
	if(SeasonsTime==seasonsTime.Summer){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.NightTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.NightTemperatureSummer.maximumSummer);
	}
	
	if(SeasonsTime==seasonsTime.Auttumn){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.NightTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.NightTemperatureAuttum.maximumAuttum);
	}
	
	if(SeasonsTime==seasonsTime.Winter){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.NightTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.NightTemperatureWinter.maximumWinter);
	}
	
	}
	
//CASES SEASONS

	//Spring
	if(SeasonsTime == seasonsTime.Spring){
	ActualTimeGame.days = Seasons.Spring.springDay;
	ActualTimeGame.month = Seasons.Spring.springMonth;

	if(SplashScreens.SplashScreensInGame==true){
	SplashScreens.SplashTextures.springSplash.enabled = true;
	}
	}
	
	//Summer
	if(SeasonsTime == seasonsTime.Summer){
	ActualTimeGame.days = Seasons.Summer.summerDay;
	ActualTimeGame.month = Seasons.Summer.summerMonth;
	
	if(SplashScreens.SplashScreensInGame==true){
	SplashScreens.SplashTextures.summerSplash.enabled = true;
	}
	}
	//Auttum
	if(SeasonsTime == seasonsTime.Auttumn){
	ActualTimeGame.days = Seasons.Auttum.auttumDay;
	ActualTimeGame.month = Seasons.Auttum.auttumMonth;

	if(SplashScreens.SplashScreensInGame==true){
	SplashScreens.SplashTextures.auttumSplash.enabled = true;
	}
	}
	
	//Winter
	if(SeasonsTime == seasonsTime.Winter){
	ActualTimeGame.days = Seasons.Winter.winterDay;
	ActualTimeGame.month = Seasons.Winter.winterMonth;
	
	if(SplashScreens.SplashScreensInGame==true){
	SplashScreens.SplashTextures.winterSplash.enabled = true;
	}
	}
	
//CAMERA CODE
	GeneralSettings.mainCamera.clearFlags = CameraClearFlags.Depth;
	GeneralSettings.mainCamera.cullingMask = GeneralSettings.mainCamera.cullingMask & ~(1 << GeneralSettings.skyProjectLayer);
	cameraOfSky = new GameObject("CameraOfSky");
	cameraOfSky.AddComponent (Camera);
	cameraOfSky.GetComponent.<Camera>().clearFlags = CameraClearFlags.Color;
	cameraOfSky.GetComponent.<Camera>().depth = -10;
	cameraOfSky.GetComponent.<Camera>().cullingMask = 1 << GeneralSettings.skyProjectLayer;
	cameraOfSky.GetComponent.<Camera>().transform.position = this.transform.position;
	cameraOfSky.GetComponent.<Camera>().fieldOfView = GeneralSettings.mainCamera.GetComponent.<Camera>().fieldOfView;
	cameraOfSky.GetComponent.<Camera>().nearClipPlane = 0.01;
	cameraOfSky.GetComponent.<Camera>().backgroundColor = Color.black;
	billboardCamera = GeneralSettings.mainCamera;
}
////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////


function FixedUpdate(){
Fades();
SplashUpdate();
Calendar();
SunTrajectory();
MoonTrajectory();
SkyPhases();
MeshColors();
SeasonsGame();
if(ActualTimeGame.timeSpeed > 2000){
ActualTimeGame.timeSpeed = 2000;
}
}


////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////


function LateUpdate(){



    //Rotation CameraOfSky
    cameraOfSky.transform.rotation = GeneralSettings.mainCamera.transform.rotation;
      
	//  slider*(SecondsDay*speed/speedDefault);
	
	

}


////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

function Calendar (){


//Seconds

ActualTimeGame.seconds += Time.deltaTime*ActualTimeGame.timeSpeed;

if(ActualTimeGame.timeSpeed != 1){
ActualTimeGame.seconds += Time.deltaTime*ActualTimeGame.timeSpeed;
}



//Minutes
if (ActualTimeGame.seconds >= minuteInSeconds){
ActualTimeGame.seconds = 0;
ActualTimeGame.minutes += 1;
}

//Hours
if (ActualTimeGame.minutes >= hourInMinutes){
ActualTimeGame.minutes = 0;
ActualTimeGame.hour += 1;
}

//Days
if (ActualTimeGame.hour >= dayInHours){
ActualTimeGame.hour = 0;
ActualTimeGame.days +=1;
}


//Month
if (ActualTimeGame.days >= monthInDays){
ActualTimeGame.days = 0;
ActualTimeGame.month += 1;
}

//Year
if (ActualTimeGame.month >= yearInMonths){
ActualTimeGame.month = 0;
ActualTimeGame.year +=1;
}
}



/////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////

function SkyPhases(){

//Blend
	 

if(ActualTimeGame.timeSpeed < 1000){
 divTime = 25000;
 }
if(ActualTimeGame.timeSpeed < 500){
 divTime = 52000;
 }


if(CloudSettings.cloudsInGame == false){
CloudSettings.clouds.SetActive(false);

}














/////////////////////////////////////////////
/////////////////SUNRISE////////////////////
///////////////////////////////////////////
if (ActualTimeGame.hour == sunRiseHour && ActualTimeGame.minutes >= sunRiseMinutes || sunRise==true){
sunRise = true;
day = false;
sunSet = false;
night = false;



/////////////////////Blends////////////////////////////		

	//Blend Color Sky
	if (rainFade < 1 || rainFade==0){
	timeLerp += Time.deltaTime/ColorsOfTheDay.blendTimeColor;
	}
	
	//Blend Light Color
	if (timeLerpSunColors < 1 || timeLerpSunColors==0){
	timeLerpSunColors += Time.deltaTime/LightSettings.LightColors.blendTimeColorLight;
	}
	
	//Blend Fog
	if (timeLerpFog < 1 || timeLerpFog==0){
	timeLerpFog += Time.deltaTime/FogSettings.ColorsFog.blendFog;
	}
	
	//Blend Intensity
	if (timeLerpIntensity < 1 || timeLerpIntensity==0){
	timeLerpIntensity += Time.deltaTime/LightSettings.Intensities.blendTimeIntensity;
	}
	
	//Blend Ambient Light
	if (timeLerpAmbientLight < 1 || timeLerpAmbientLight==0){
	timeLerpAmbientLight += Time.deltaTime/LightSettings.LightAmbientColors.blendTimeAmbientColor;
	}
	
	//Blend Clouds
	if (timeLerpCloudsColors < 1 || timeLerpCloudsColors==0){
 	timeLerpCloudsColors += Time.deltaTime/CloudSettings.blendTimeClouds;
 	}
 	
 		//Blend Light Rain
 	if (timeLerpLightRain < 1 || timeLerpLightRain==0){
 	timeLerpLightRain += Time.deltaTime/Weather.RainSettings.blendTimeLightRain;
 	}
 	
 	//Blend Light Rain
 	if (timeLerpHeavyRain < 1 || timeLerpHeavyRain==0){
 	timeLerpHeavyRain += Time.deltaTime/Weather.RainSettings.blendTimeHeavyRain;
 	}
 	
 	if (timeLerpRain < 1 || timeLerpRain==0){
 	timeLerpRain += Time.deltaTime * Weather.RainSettings.blendRain;
 	}
	
	//Sun
	if (timeLerpSun < 1 || timeLerpSun==0){
	timeLerpSun += Time.deltaTime/1;
	}
/////////////////////Blends End////////////////////////////		
	if(stopColors==false){
	//Colors 
	topColor = Color.Lerp(topColor, ColorsOfTheDay.SunRiseColors.sunRiseColorTop, timeLerp);
	bottomColor = Color.Lerp(bottomColor, ColorsOfTheDay.SunRiseColors.sunRiseColorBottom, timeLerp);
	}
	
	
	//Fog
	if(FogSettings.dinamicFog==true){
	if(stopFog==false){
	fogColor = RenderSettings.fogColor=Color.Lerp(fogColor, FogSettings.ColorsFog.SunRiseFog, timeLerpFog);
	}
	}
	
	//SunColor
	if(LightSettings.Sun !=null && LightSettings.dinamicLight==true){
	if(stopSunColor==false){
	LightSettings.Sun.GetComponent.<Light>().color = Color.Lerp(LightSettings.Sun.GetComponent.<Light>().color,LightSettings.LightColors.SunRiseLight,timeLerpSunColors);
	}
	}	
	
	//Sun.light Intensity
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunIntensity==false){
	LightSettings.Sun.GetComponent.<Light>().intensity = Mathf.Lerp(LightSettings.Sun.GetComponent.<Light>().intensity, LightSettings.Intensities.sunRiseIntensity, timeLerpIntensity);
	}
	}
	
	//Ambient Color
	if(LightSettings.dinamicAmbientLight==true){
	if(stopAmbient==false){
	RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight,LightSettings.LightAmbientColors.SunRiseAmbientColor,timeLerpAmbientLight);
	}
	}
	
	//Clouds Color
	if(CloudSettings.clouds!=null && CloudSettings.cloudsInGame==true){
	if(stopClouds==false){
	CloudSettings.clouds.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(CloudSettings.clouds.GetComponent.<Renderer>().material.GetColor("_TintColor"),
	CloudSettings.CloudsColors.SunRiseCloudsColor,timeLerpCloudsColors));
	}
	}
	
	
oneTimeSunSet = false;
oneTimeDay = false;
oneTimeNight = false;
	
	//Temperatures
	if(!oneTimeSunRise){
	
	Weather.RainSettings.randomNumber = Random.Range(0,100);
	Weather.RainSettings.randomType = Random.Range(0,100);
	
	if(SpringTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunRiseTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.SunRiseTemperatureSpring.maximumSpring);
	}
	if(SummerTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunRiseTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.SunRiseTemperatureSummer.maximumSummer);
	}
	
	if(AuttumTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.SunRiseTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.SunRiseTemperatureAuttum.maximumAuttum);
	}
	
	if(WinterTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.SunRiseTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.SunRiseTemperatureWinter.maximumWinter);
	}
	oneTimeSunRise = true;
	}
	
	//Checkers!
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck==true){
		if(Weather.RainSettings.rain != null){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck==true){
	if(Weather.RainSettings.rain != null){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 }
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck==true){
	if(Weather.RainSettings.rain != null){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 }
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck==true){
	if(Weather.RainSettings.rain != null){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 }
		 }
	
 	

	

	//WEATHER
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	//SPRING WEATHER SUNRISE
	if(Weather.RainSettings.rain != null){
		if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck == true){
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SpringRain.rainTypePercentage
		   && Weather.RainSettings.SpringRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SpringRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.LightRain.SunRiseColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.LightRain.SunRiseColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.LightRain.SunRiseColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SpringRain.rainTypePercentage
	 && Weather.RainSettings.SpringRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SpringRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.HeavyRain.SunRiseColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.HeavyRain.SunRiseColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.HeavyRain.SunRiseColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	
	//SUMMER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SummerRain.rainTypePercentage
		   && Weather.RainSettings.SummerRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SummerRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.LightRain.SunRiseColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.LightRain.SunRiseColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.LightRain.SunRiseColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SummerRain.rainTypePercentage
	 && Weather.RainSettings.SummerRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SummerRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.HeavyRain.SunRiseColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.HeavyRain.SunRiseColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.HeavyRain.SunRiseColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//AUTTUM WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck == true){
		 
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.AuttumRain.rainTypePercentage
		   && Weather.RainSettings.AuttumRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.LightRain.SunRiseColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.LightRain.SunRiseColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.LightRain.SunRiseColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.AuttumRain.rainTypePercentage
	 && Weather.RainSettings.AuttumRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.HeavyRain.SunRiseColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.HeavyRain.SunRiseColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.HeavyRain.SunRiseColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//WINTER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck == true){
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.WinterRain.rainTypePercentage
		   && Weather.RainSettings.WinterRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.WinterRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.LightRain.SunRiseColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.LightRain.SunRiseColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.LightRain.SunRiseColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.WinterRain.rainTypePercentage
	 && Weather.RainSettings.WinterRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.WinterRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.HeavyRain.SunRiseColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.HeavyRain.SunRiseColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.HeavyRain.SunRiseColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	}
	}
	
	
timeLerp=0;
timeLerpFog=0;
timeLerpIntensity = 0;
timeLerpSunColors  = 0;
timeLerpCloudsColors  = 0;
timeLerpAmbientLight = 0;
timeLerpLightRain  = 0;
timeLerpHeavyRain = 0;
timeLerpRain = 0;
}

/////////////////////////////////////////////
/////////////////////DAY////////////////////
///////////////////////////////////////////
if (ActualTimeGame.hour == dayHour && ActualTimeGame.minutes >= dayMinutes || day == true){

sunRise = false;
day = true;
sunSet = false;
night = false;
/////////////////////Blends////////////////////////////	

	//Blend
	 if (timeLerp < 1 || timeLerp==0){
 	timeLerp += Time.deltaTime/ColorsOfTheDay.blendTimeColor;
 	}
 	
 	//Blend Light Color
	if (timeLerpSunColors < 1 || timeLerpSunColors==0){
	timeLerpSunColors += Time.deltaTime/LightSettings.LightColors.blendTimeColorLight;
	}
	
	//Blend Fog
	if (timeLerpFog < 1 || timeLerpFog==0){
	timeLerpFog += Time.deltaTime/FogSettings.ColorsFog.blendFog;
	}
	
	//Blend Intensity
	if (timeLerpIntensity < 1 || timeLerpIntensity==0){
	timeLerpIntensity += Time.deltaTime/LightSettings.Intensities.blendTimeIntensity;
	}
	
	//Blend Ambient Light
	if (timeLerpAmbientLight < 1 || timeLerpAmbientLight==0){
	timeLerpAmbientLight += Time.deltaTime/LightSettings.LightAmbientColors.blendTimeAmbientColor;
	}
	
	//Blend Clouds
	if (timeLerpCloudsColors < 1 || timeLerpCloudsColors==0){
 	timeLerpCloudsColors += Time.deltaTime/CloudSettings.blendTimeClouds;
 	}
 	
 	//Blend Light Rain
 	if (timeLerpLightRain < 1 || timeLerpLightRain==0){
 	timeLerpLightRain += Time.deltaTime/Weather.RainSettings.blendTimeLightRain;
 	}
 	
 	//Blend Heavy Rain
 	if (timeLerpHeavyRain < 1 || timeLerpHeavyRain==0){
 	timeLerpHeavyRain += Time.deltaTime/Weather.RainSettings.blendTimeHeavyRain;
 	}
 	
/////////////////////Blends End////////////////////////////

	if(stopColors==false){
	//Colors 
	topColor = Color.Lerp(topColor, ColorsOfTheDay.DayColors.dayColorTop, timeLerp);
	bottomColor = Color.Lerp(bottomColor, ColorsOfTheDay.DayColors.dayColorBottom, timeLerp);
	}
	
	//Fog
	if(FogSettings.dinamicFog==true){
	if(stopFog==false){
	fogColor = RenderSettings.fogColor=Color.Lerp(fogColor, FogSettings.ColorsFog.DayFog, timeLerpFog);
	}
	}
	
	//SunColor
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunColor==false){
	LightSettings.Sun.GetComponent.<Light>().color = Color.Lerp(LightSettings.Sun.GetComponent.<Light>().color,LightSettings.LightColors.DayLight,timeLerpSunColors);
	}
	}
	
	//Sun.light Intensity
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunIntensity==false){
	LightSettings.Sun.GetComponent.<Light>().intensity = Mathf.Lerp(LightSettings.Sun.GetComponent.<Light>().intensity, LightSettings.Intensities.dayIntensity, timeLerpIntensity);
	}
	}
	
	//Ambient Color
	if(LightSettings.dinamicAmbientLight==true){
	if(stopAmbient==false){
	RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight,LightSettings.LightAmbientColors.DayAmbientColor,timeLerpAmbientLight);
	}
	}
	//Clouds Color
	if(!CloudSettings.clouds==null && CloudSettings.cloudsInGame==true){
	if(stopClouds==false){
	CloudSettings.clouds.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(CloudSettings.clouds.GetComponent.<Renderer>().material.GetColor("_TintColor"),
	CloudSettings.CloudsColors.DayCloudsColor,timeLerpCloudsColors));
	}
	}
	
oneTimeSunRise = false;
oneTimeSunSet = false;
oneTimeNight = false;

	if(!oneTimeDay){
	
    Weather.RainSettings.randomNumber = Random.Range(0,100);
	Weather.RainSettings.randomType = Random.Range(0,100);
	
	//Temperatures
	if(SpringTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.DayTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.DayTemperatureSpring.maximumSpring);
	}
	
	if(SummerTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.DayTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.DayTemperatureSummer.maximumSummer);
	}
	
	if(AuttumTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.DayTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.DayTemperatureAuttum.maximumAuttum);
	}
	
	if(WinterTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.DayTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.DayTemperatureWinter.maximumWinter);
	}
	oneTimeDay = true;
	}
	
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	//Checkers!
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 }
	
 	

	

	//WEATHER
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	//SPRING WEATHER SUNRISE
	if(Weather.RainSettings.rain != null){
		if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SpringRain.rainTypePercentage
		   && Weather.RainSettings.SpringRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SpringRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.LightRain.DayColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.LightRain.DayColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.LightRain.DayColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SpringRain.rainTypePercentage
	 && Weather.RainSettings.SpringRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SpringRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.HeavyRain.DayColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.HeavyRain.DayColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.HeavyRain.DayColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	
	//SUMMER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SummerRain.rainTypePercentage
		   && Weather.RainSettings.SummerRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SummerRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.LightRain.DayColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.LightRain.DayColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.LightRain.DayColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SummerRain.rainTypePercentage
	 && Weather.RainSettings.SummerRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SummerRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.HeavyRain.DayColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.HeavyRain.DayColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.HeavyRain.DayColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//AUTTUM WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck == true){
		 
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.AuttumRain.rainTypePercentage
		   && Weather.RainSettings.AuttumRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.LightRain.DayColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.LightRain.DayColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.LightRain.DayColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.AuttumRain.rainTypePercentage
	 && Weather.RainSettings.AuttumRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.HeavyRain.DayColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.HeavyRain.DayColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.HeavyRain.DayColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//WINTER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck == true){
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.WinterRain.rainTypePercentage
		   && Weather.RainSettings.WinterRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.WinterRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.LightRain.DayColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.LightRain.DayColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.LightRain.DayColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.WinterRain.rainTypePercentage
	 && Weather.RainSettings.WinterRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.WinterRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.HeavyRain.DayColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.HeavyRain.DayColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.HeavyRain.DayColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	}
	}
	
	
	
	
timeLerp=0;
timeLerpFog=0;
timeLerpIntensity = 0;
timeLerpSunColors  = 0;
timeLerpCloudsColors  = 0;
timeLerpAmbientLight = 0;
timeLerpLightRain  = 0;
timeLerpHeavyRain = 0;
timeLerpRain  = 0;
}

/////////////////////////////////////////////
//////////////////SUNSET////////////////////
///////////////////////////////////////////
if (ActualTimeGame.hour == sunSetHour && ActualTimeGame.minutes >= sunSetMinutes || sunSet==true){
sunRise = false;
day = false;
sunSet = true;
night = false;
/////////////////////Blends////////////////////////////	

	//Blend
	if (timeLerp < 1 || timeLerp==0){
 	timeLerp += Time.deltaTime/ColorsOfTheDay.blendTimeColor;
 	}
 	
 	//Blend Light Color
	if (timeLerpSunColors < 1 || timeLerpSunColors==0){
	timeLerpSunColors += Time.deltaTime/LightSettings.LightColors.blendTimeColorLight;
	}
	
	//Blend Fog
	if (timeLerpFog < 1 || timeLerpFog==0){
	timeLerpFog += Time.deltaTime/FogSettings.ColorsFog.blendFog;
	}
	
	//Blend Intensity
	if (timeLerpIntensity < 1 || timeLerpIntensity==0){
	timeLerpIntensity += Time.deltaTime/LightSettings.Intensities.blendTimeIntensity;
	}
	
	//Blend Ambient Light
	if (timeLerpAmbientLight < 1 || timeLerpAmbientLight==0){
	timeLerpAmbientLight += Time.deltaTime/LightSettings.LightAmbientColors.blendTimeAmbientColor;
	}
	
	//Blend Clouds
	if (timeLerpCloudsColors < 1 || timeLerpCloudsColors==0){
 	timeLerpCloudsColors += Time.deltaTime/CloudSettings.blendTimeClouds;
 	}
 	
 	//Blend Light Rain
 	if (timeLerpLightRain < 1 || timeLerpLightRain==0){
 	timeLerpLightRain += Time.deltaTime/Weather.RainSettings.blendTimeLightRain;
 	}
 	
 	//Blend Heavy Rain
 	if (timeLerpHeavyRain < 1 || timeLerpHeavyRain==0){
 	timeLerpHeavyRain += Time.deltaTime/Weather.RainSettings.blendTimeHeavyRain;
 	}
	
/////////////////////Blends End////////////////////////////		
	
 	if(stopColors==false){
	//Colors
	topColor = Color.Lerp(topColor, ColorsOfTheDay.SunSetColors.sunSetColorTop, timeLerp);
	bottomColor = Color.Lerp(bottomColor, ColorsOfTheDay.SunSetColors.sunSetColorBottom, timeLerp);
	}
	
	//Fog
	if(FogSettings.dinamicFog==true){
	if(stopFog==false){
	fogColor = RenderSettings.fogColor=Color.Lerp(fogColor, FogSettings.ColorsFog.SunSetFog, timeLerpFog);
	}
	}
	//SunColor
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunColor==false){
	LightSettings.Sun.GetComponent.<Light>().color = Color.Lerp(LightSettings.Sun.GetComponent.<Light>().color,LightSettings.LightColors.SunSetLight,timeLerpSunColors);
	}
	}
	//Sun.light Intensity
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunIntensity==false){
	LightSettings.Sun.GetComponent.<Light>().intensity = Mathf.Lerp(LightSettings.Sun.GetComponent.<Light>().intensity, LightSettings.Intensities.sunSetIntensity, timeLerpIntensity);
	}
	}
	
	//Ambient Color
	if(LightSettings.dinamicAmbientLight==true){
	if(stopAmbient==false){
	RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight,LightSettings.LightAmbientColors.SunSetAmbientColor,timeLerpAmbientLight);
	}
	}
	
	//Clouds Color
	if(CloudSettings.clouds!=null && CloudSettings.cloudsInGame==true){
	if(stopClouds==false){
	CloudSettings.clouds.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(CloudSettings.clouds.GetComponent.<Renderer>().material.GetColor("_TintColor"),
	CloudSettings.CloudsColors.SunSetCloudsColor,timeLerpCloudsColors));
	}
	}
	
oneTimeSunRise = false;
oneTimeDay = false;
oneTimeNight = false;

	//Temperatures
	if(!oneTimeSunSet){
	
    Weather.RainSettings.randomNumber = Random.Range(0,100);
	Weather.RainSettings.randomType = Random.Range(0,100);
	
	if(SpringTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunSetTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.SunSetTemperatureSpring.maximumSpring);
	}
	
	if(SummerTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.SunSetTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.SunSetTemperatureSummer.maximumSummer);
	}
	
	if(AuttumTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.SunSetTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.SunSetTemperatureAuttum.maximumAuttum);
	}
	
	if(WinterTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.SunSetTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.SunSetTemperatureWinter.maximumWinter);
	}
	oneTimeSunSet = true;
	}
	
//Checkers!
if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 }
	if (timeLerpRain < 1 || timeLerpRain==0){
 	timeLerpRain += Time.deltaTime / Weather.RainSettings.SpringRain.HeavyRain.rainFade;
 	}
 	

	

	//WEATHER
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	//SPRING WEATHER SUNRISE
	if(Weather.RainSettings.rain != null){
		if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SpringRain.rainTypePercentage
		   && Weather.RainSettings.SpringRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SpringRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.LightRain.SunSetColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.LightRain.SunSetColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.LightRain.SunSetColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SpringRain.rainTypePercentage
	 && Weather.RainSettings.SpringRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SpringRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.HeavyRain.SunSetColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.HeavyRain.SunSetColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.HeavyRain.SunSetColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	
	//SUMMER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SummerRain.rainTypePercentage
		   && Weather.RainSettings.SummerRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SummerRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.LightRain.SunSetColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.LightRain.SunSetColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.LightRain.SunSetColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SummerRain.rainTypePercentage
	 && Weather.RainSettings.SummerRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SummerRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.HeavyRain.SunSetColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.HeavyRain.SunSetColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.HeavyRain.SunSetColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//AUTTUM WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck == true){
		 
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.AuttumRain.rainTypePercentage
		   && Weather.RainSettings.AuttumRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.LightRain.SunSetColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.LightRain.SunSetColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.LightRain.SunSetColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.AuttumRain.rainTypePercentage
	 && Weather.RainSettings.AuttumRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.HeavyRain.SunSetColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.HeavyRain.SunSetColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.HeavyRain.SunSetColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//WINTER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck == true){
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.WinterRain.rainTypePercentage
		   && Weather.RainSettings.WinterRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.WinterRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.LightRain.SunSetColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.LightRain.SunSetColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.LightRain.SunSetColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.WinterRain.rainTypePercentage
	 && Weather.RainSettings.WinterRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.WinterRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.HeavyRain.SunSetColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.HeavyRain.SunSetColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.HeavyRain.SunSetColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	}
	}
	
	
timeLerp=0;
timeLerpFog=0;
timeLerpIntensity = 0;
timeLerpSunColors  = 0;
timeLerpCloudsColors  = 0;
timeLerpAmbientLight = 0;
timeLerpLightRain  = 0;
timeLerpHeavyRain = 0;
timeLerpRain  = 0;
}
/////////////////////////////////////////////
///////////////////NIGHT////////////////////
///////////////////////////////////////////

if (ActualTimeGame.hour == nightHour && ActualTimeGame.minutes >= nightMinutes || night==true){

sunRise = false;
day = false;
sunSet = false;
night=true;
/////////////////////Blends////////////////////////////	

	//Blend
	if (timeLerp < 1 || timeLerp==0){
	 timeLerp += Time.deltaTime/ColorsOfTheDay.blendTimeColor;
	}
	
	
	//Blend Light Color
	if (timeLerpSunColors < 1 || timeLerpSunColors==0){
	timeLerpSunColors += Time.deltaTime/LightSettings.LightColors.blendTimeColorLight;
	}
	
	//Blend Fog
	if (timeLerpFog < 1 || timeLerpFog==0){
	timeLerpFog += Time.deltaTime/FogSettings.ColorsFog.blendFog;
	}
	
	//Blend Intensity
	if (timeLerpIntensity < 1 || timeLerpIntensity==0){
	timeLerpIntensity += Time.deltaTime/LightSettings.Intensities.blendTimeIntensity;
	}
	
	//Blend Ambient Light
	if (timeLerpAmbientLight < 1 || timeLerpAmbientLight==0){
	timeLerpAmbientLight += Time.deltaTime/LightSettings.LightAmbientColors.blendTimeAmbientColor;
	}
	
	//Blend Clouds
	if (timeLerpCloudsColors < 1 || timeLerpCloudsColors==0){
 	timeLerpCloudsColors += Time.deltaTime/CloudSettings.blendTimeClouds;
 	}
 	
 	//Blend Light Rain
 	if (timeLerpLightRain < 1 || timeLerpLightRain==0){
 	timeLerpLightRain += Time.deltaTime/Weather.RainSettings.blendTimeLightRain;
 	}
 	
 	//Blend Heavy Rain
 	if (timeLerpHeavyRain < 1 || timeLerpHeavyRain==0){
 	timeLerpHeavyRain += Time.deltaTime/Weather.RainSettings.blendTimeHeavyRain;
 	}
	
/////////////////////Blends End////////////////////////////		
	if(stopColors==false){
	//Colors
	topColor = Color.Lerp(topColor, ColorsOfTheDay.NightColors.nightColorTop, timeLerp);
	bottomColor = Color.Lerp(bottomColor, ColorsOfTheDay.NightColors.nightColorBottom, timeLerp);
	}
	
	//Fog
	if(FogSettings.dinamicFog==true){
	if(stopFog==false){
	fogColor = RenderSettings.fogColor=Color.Lerp(fogColor, FogSettings.ColorsFog.NightFog, timeLerpFog);
	}
	}
	
	//SunColor
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunColor==false){
	LightSettings.Sun.GetComponent.<Light>().color = Color.Lerp(LightSettings.Sun.GetComponent.<Light>().color,LightSettings.LightColors.NightLight,timeLerpSunColors);
	}
	}
	
	//Sun.light Intensity
	if( LightSettings.Sun!=null && LightSettings.dinamicLight==true){
	if(stopSunIntensity==false){
	LightSettings.Sun.GetComponent.<Light>().intensity = Mathf.Lerp(LightSettings.Sun.GetComponent.<Light>().intensity, LightSettings.Intensities.nightIntensity, timeLerpIntensity);
	}
	}
	
	//Ambient Color
	if(LightSettings.dinamicAmbientLight==true){
	if(stopAmbient==false){
	RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight,LightSettings.LightAmbientColors.NightAmbientColor,timeLerpAmbientLight);
	}
	}
	
	//Clouds Color
	if(CloudSettings.clouds!=null && CloudSettings.cloudsInGame==true){
	if(stopClouds==false){
	CloudSettings.clouds.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(CloudSettings.clouds.GetComponent.<Renderer>().material.GetColor("_TintColor"),
	CloudSettings.CloudsColors.NightCloudsColor,timeLerpCloudsColors));
	}
	}
oneTimeSunRise = false;
oneTimeDay = false;
oneTimeSunSet = false;

	
	if(!oneTimeNight){
    
    Weather.RainSettings.randomNumber = Random.Range(0,100);
	Weather.RainSettings.randomType = Random.Range(0,100);
    
	//Temperatures
	if(SpringTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.NightTemperatureSpring.minimumSpring,
	Temperature.SpringTemperature.NightTemperatureSpring.maximumSpring);
	}
	
	if(SummerTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.SpringTemperature.NightTemperatureSpring.minimumSpring,
	Temperature.SummerTemperature.NightTemperatureSummer.maximumSummer);
	}
	
	if(AuttumTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.AuttumTemperature.NightTemperatureAuttum.minimumAuttum,
	Temperature.AuttumTemperature.NightTemperatureAuttum.maximumAuttum);
	}
	
	if(WinterTimeCheck == true){
	Temperature.actualTemperature = Random.Range(Temperature.WinterTemperature.NightTemperatureWinter.minimumWinter,
	Temperature.WinterTemperature.NightTemperatureWinter.maximumWinter);
	}
	oneTimeNight = true;
	}
	
	//Checkers!
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	if(Weather.RainSettings.rain != null){
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
		 
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber > Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck==true){
		 Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate,0,Time.time);
		 stopColors = false;
		 stopSunColor = false;
		 stopAmbient = false;
		 }
	}
	}
	if (timeLerpRain < 1 || timeLerpRain==0){
 	timeLerpRain += Time.deltaTime / Weather.RainSettings.SpringRain.HeavyRain.rainFade;
 	}
 	

	

	//WEATHER
	if(Weather.weatherInGame == true && Weather.RainSettings.rainInGame==true){
	if(Weather.RainSettings.rain != null){
	//SPRING WEATHER SUNRISE
		if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SpringRain.rainPercentage && SpringTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SpringRain.rainTypePercentage
		   && Weather.RainSettings.SpringRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SpringRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.LightRain.NightColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.LightRain.NightColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.LightRain.NightColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SpringRain.rainTypePercentage
	 && Weather.RainSettings.SpringRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SpringRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SpringRain.HeavyRain.NightColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SpringRain.HeavyRain.NightColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SpringRain.HeavyRain.NightColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	
	//SUMMER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.SummerRain.rainPercentage && SummerTimeCheck == true){
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.SummerRain.rainTypePercentage
		   && Weather.RainSettings.SummerRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.SummerRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		    stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.LightRain.NightColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.LightRain.NightColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.LightRain.NightColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.SummerRain.rainTypePercentage
	 && Weather.RainSettings.SummerRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.SummerRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.SummerRain.HeavyRain.NightColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.SummerRain.HeavyRain.NightColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.SummerRain.HeavyRain.NightColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//AUTTUM WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.AuttumRain.rainPercentage && AuttumTimeCheck == true){
		 
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.AuttumRain.rainTypePercentage
		   && Weather.RainSettings.AuttumRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.LightRain.NightColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.LightRain.NightColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.LightRain.NightColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.AuttumRain.rainTypePercentage
	 && Weather.RainSettings.AuttumRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.AuttumRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.AuttumRain.HeavyRain.NightColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.AuttumRain.HeavyRain.NightColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.AuttumRain.HeavyRain.NightColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	
	//WINTER WEATHER SUNRISE
	if(0 <= Weather.RainSettings.randomNumber && Weather.RainSettings.randomNumber <= Weather.RainSettings.WinterRain.rainPercentage && WinterTimeCheck == true){
		 stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
		 if(Weather.RainSettings.randomType > Weather.RainSettings.WinterRain.rainTypePercentage
		   && Weather.RainSettings.WinterRain.LightRain.minimumTemperature <= Temperature.actualTemperature
		   && Temperature.actualTemperature< Weather.RainSettings.WinterRain.LightRain.maximumTemperature 
		   || rainlight==true){
		   
				rainheavy=false;
				rainlight = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,800,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.LightRain.NightColorsRain.topColor, timeLerpLightRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.LightRain.NightColorsRain.bottomColor, timeLerpLightRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.LightRain.NightColorsRain.ambientColor,timeLerpLightRain);
		
	
	}
	
	
	if(Weather.RainSettings.randomType < Weather.RainSettings.WinterRain.rainTypePercentage
	 && Weather.RainSettings.WinterRain.HeavyRain.minimumTemperature <= Temperature.actualTemperature 
	 && Temperature.actualTemperature< Weather.RainSettings.WinterRain.HeavyRain.maximumTemperature 
	 || rainheavy==true){
		 
		  stopColors = true;
		 stopSunColor = true;
		 stopAmbient = true;
		 LightSettings.Sun.SetActive(false);
		 
				rainlight = false;
			    rainheavy = true;
				Weather.RainSettings.rain.GetComponent(ParticleSystem).emissionRate = Mathf.Lerp(0,2000,Time.time);
				topColor = Color.Lerp(topColor, Weather.RainSettings.WinterRain.HeavyRain.NightColorsRain.topColor, timeLerpHeavyRain);
		 		bottomColor = Color.Lerp(bottomColor, Weather.RainSettings.WinterRain.HeavyRain.NightColorsRain.bottomColor, timeLerpHeavyRain);
		 		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Weather.RainSettings.WinterRain.HeavyRain.NightColorsRain.ambientColor,timeLerpHeavyRain);
				
	}
	}
	}
	}
	
	
timeLerp=0;
timeLerpFog=0;
timeLerpIntensity = 0;
timeLerpSunColors  = 0;
timeLerpCloudsColors  = 0;
timeLerpAmbientLight = 0;
timeLerpLightRain  = 0;
timeLerpHeavyRain = 0;
timeLerpRain  = 0;
}
}


/////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////


function MeshColors(){
//We Get The mesh and set the colors
    var mesh : Mesh = GetComponent(MeshFilter).sharedMesh;
    var uv : Vector2[] = mesh.uv;
    var colors : Color[] = new Color[uv.Length];
 
    // Instead if vertex.y we use uv.x
    for (var i = 0; i < uv.Length;i++)
        colors[i] = Color.Lerp(topColor, bottomColor, uv[i].y-0.1);
       
    mesh.colors = colors;
}




/////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////

function SeasonsGame(){

if(Terrain.activeTerrain != null){
var grass : DetailPrototype[] = Terrain.activeTerrain.terrainData.detailPrototypes;
var terraintexture : SplatPrototype[] = Terrain.activeTerrain.terrainData.splatPrototypes;
var terraintree : TreePrototype[] = Terrain.activeTerrain.terrainData.treePrototypes;
}
if(Seasons.seasonsInGame == true){
//Spring

if(ActualTimeGame.month == Seasons.Spring.springMonth && ActualTimeGame.days == Seasons.Spring.springDay){

oneTimeFunction2 = false;
oneTimeFunction3 = false;
oneTimeFunction4 = false;




if(!oneTimeFunction){
oneTimeFunction = true;

SpringTimeCheck  = true;
SummerTimeCheck  = false;
AuttumTimeCheck  = false;
WinterTimeCheck  = false;

if(SplashScreens.SplashScreensInGame==true){

SplashScreens.SplashTextures.springSplash.GetComponent.<GUITexture>().enabled=true;

}
if(Terrain.activeTerrain != null){
if(grass.Length>0){
grass[0].healthyColor = Seasons.Spring.grassColorHealthySpring;
grass[0].dryColor = Seasons.Spring.grassColorDrySpring;
}

if(grass.length>1){
grass[1].healthyColor = Seasons.Spring.grassColorHealthySpring;
grass[1].dryColor = Seasons.Spring.grassColorDrySpring;
}

if(grass.length>2){
grass[2].healthyColor = Seasons.Spring.grassColorHealthySpring;
grass[2].dryColor = Seasons.Spring.grassColorDrySpring;
}

if(grass.length>3){
grass[3].healthyColor = Seasons.Spring.grassColorHealthySpring;
grass[3].dryColor = Seasons.Spring.grassColorDrySpring;
}

if(grass.length>4){
grass[4].healthyColor = Seasons.Spring.grassColorHealthySpring;
grass[4].dryColor = Seasons.Spring.grassColorDrySpring;
}

for(var s = 0;s<Seasons.Spring.springTexturesSeason.texturesSpring.Length;s++){
if(Seasons.Spring.springTexturesSeason.texturesSpring[s]!= null){
  terraintexture[s].texture=Seasons.Spring.springTexturesSeason.texturesSpring[s];
}
}


for(var st = 0;st<Seasons.Spring.springTrees.treesSpring.Length;st++){
if(Seasons.Spring.springTrees.treesSpring[st]!= null){
  terraintree[st].prefab=Seasons.Spring.springTrees.treesSpring[st];
}
}

Terrain.activeTerrain.terrainData.detailPrototypes = grass;
Terrain.activeTerrain.terrainData.splatPrototypes = terraintexture;
Terrain.activeTerrain.terrainData.treePrototypes = terraintree;
}
}
}
//Summer
if(ActualTimeGame.month == Seasons.Summer.summerMonth && ActualTimeGame.days == Seasons.Summer.summerDay){

oneTimeFunction = false;
oneTimeFunction3 = false;
oneTimeFunction4 = false;




if(!oneTimeFunction2){
oneTimeFunction2 = true;

SpringTimeCheck  = false;
SummerTimeCheck  = true;
AuttumTimeCheck  = false;
WinterTimeCheck  = false;

if(SplashScreens.SplashScreensInGame==true){

SplashScreens.SplashTextures.summerSplash.GetComponent.<GUITexture>().enabled=true;

}
if(GeneralSettings.terrainvar != null){
if(grass.Length>0){
grass[0].healthyColor = Seasons.Summer.grassColorHealthySummer;
grass[0].dryColor = Seasons.Summer.grassColorHealthySummer;
}

if(grass.length>1){
grass[1].healthyColor = Seasons.Summer.grassColorHealthySummer;
grass[1].dryColor = Seasons.Summer.grassColorHealthySummer;
}

if(grass.length>2){
grass[2].healthyColor = Seasons.Spring.grassColorHealthySpring;
grass[2].dryColor = Seasons.Spring.grassColorDrySpring;
}

if(grass.length>3){
grass[3].healthyColor = Seasons.Summer.grassColorHealthySummer;
grass[3].dryColor = Seasons.Summer.grassColorHealthySummer;
}

if(grass.length>4){
grass[4].healthyColor = Seasons.Summer.grassColorHealthySummer;
grass[4].dryColor = Seasons.Summer.grassColorHealthySummer;
}

for(var m = 0;m<Seasons.Summer.summerTexturesSeason.texturesSummer.Length;m++){
if(Seasons.Summer.summerTexturesSeason.texturesSummer[m]!= null){
  terraintexture[m].texture=Seasons.Summer.summerTexturesSeason.texturesSummer[m];
}
}

for(var mt = 0;mt<Seasons.Summer.summerTrees.treesSummer.Length;mt++){
if(Seasons.Summer.summerTrees.treesSummer[mt]!= null){
  terraintree[mt].prefab=Seasons.Summer.summerTrees.treesSummer[mt];
}
}

Terrain.activeTerrain.terrainData.detailPrototypes = grass;
Terrain.activeTerrain.terrainData.splatPrototypes = terraintexture;
Terrain.activeTerrain.terrainData.treePrototypes = terraintree;
}
}
}
//Auttum
if(ActualTimeGame.month == Seasons.Auttum.auttumMonth && ActualTimeGame.days == Seasons.Auttum.auttumDay){

oneTimeFunction = false;
oneTimeFunction2 = false;
oneTimeFunction4 = false;

if(!oneTimeFunction3){
oneTimeFunction3 = true;

SpringTimeCheck  = false;
SummerTimeCheck  = false;
AuttumTimeCheck  = true;
WinterTimeCheck  = false;

if(SplashScreens.SplashScreensInGame==true){

SplashScreens.SplashTextures.auttumSplash.GetComponent.<GUITexture>().enabled=true;

}
if(GeneralSettings.terrainvar != null){
if(grass.Length>0){
grass[0].healthyColor = Seasons.Auttum.grassColorHealthyAuttum;
grass[0].dryColor = Seasons.Auttum.grassColorHealthyAuttum;
}

if(grass.length>1){
grass[1].healthyColor = Seasons.Auttum.grassColorHealthyAuttum;
grass[1].dryColor = Seasons.Auttum.grassColorHealthyAuttum;
}

if(grass.length>2){
grass[2].healthyColor = Seasons.Auttum.grassColorHealthyAuttum;
grass[2].dryColor = Seasons.Auttum.grassColorDryAuttum;
}

if(grass.length>3){
grass[3].healthyColor = Seasons.Auttum.grassColorHealthyAuttum;
grass[3].dryColor = Seasons.Auttum.grassColorHealthyAuttum;
}

if(grass.length>4){
grass[4].healthyColor = Seasons.Auttum.grassColorHealthyAuttum;
grass[4].dryColor = Seasons.Auttum.grassColorHealthyAuttum;
}

//Textures
for(var a = 0;a<Seasons.Auttum.auttumTexturesSeason.texturesAuttum.Length;a++){
if(Seasons.Auttum.auttumTexturesSeason.texturesAuttum[a]!= null){
  terraintexture[a].texture=Seasons.Auttum.auttumTexturesSeason.texturesAuttum[a];
}
}

for(var at = 0;at<Seasons.Auttum.auttumTrees.treesAuttum.Length;at++){
if(Seasons.Auttum.auttumTrees.treesAuttum[at]!= null){
  terraintree[at].prefab=Seasons.Auttum.auttumTrees.treesAuttum[at];
}
}


Terrain.activeTerrain.terrainData.detailPrototypes = grass;
Terrain.activeTerrain.terrainData.splatPrototypes = terraintexture;
Terrain.activeTerrain.terrainData.treePrototypes = terraintree;
}
}
}
//Winter
if(ActualTimeGame.month == Seasons.Winter.winterMonth && ActualTimeGame.days == Seasons.Winter.winterDay){

oneTimeFunction = false;
oneTimeFunction2 = false;
oneTimeFunction3 = false;

if(!oneTimeFunction4){
oneTimeFunction4 = true;

SpringTimeCheck  = false;
SummerTimeCheck  = false;
AuttumTimeCheck  = false;
WinterTimeCheck  = true;

if(SplashScreens.SplashScreensInGame==true){

SplashScreens.SplashTextures.winterSplash.GetComponent.<GUITexture>().enabled=true;

}
if(Terrain.activeTerrain != null){
if(grass.Length>0){
grass[0].healthyColor = Seasons.Winter.grassColorHealthyWinter;
grass[0].dryColor = Seasons.Winter.grassColorHealthyWinter;
}

if(grass.length>1){
grass[1].healthyColor = Seasons.Winter.grassColorHealthyWinter;
grass[1].dryColor = Seasons.Winter.grassColorHealthyWinter;
}

if(grass.length>2){
grass[2].healthyColor = Seasons.Winter.grassColorHealthyWinter;
grass[2].dryColor = Seasons.Winter.grassColorDryWinter;
}

if(grass.length>3){
grass[3].healthyColor = Seasons.Winter.grassColorHealthyWinter;
grass[3].dryColor = Seasons.Winter.grassColorHealthyWinter;
}

if(grass.length>4){
grass[4].healthyColor = Seasons.Winter.grassColorHealthyWinter;
grass[4].dryColor = Seasons.Winter.grassColorHealthyWinter;
}


for(var w = 0;w<Seasons.Winter.winterTexturesSeason.texturesWinter.Length;w++){
if(Seasons.Winter.winterTexturesSeason.texturesWinter[w]!= null){
  terraintexture[w].texture=Seasons.Winter.winterTexturesSeason.texturesWinter[w];
}
}

for(var wt = 0;wt<Seasons.Winter.winterTrees.treesWinter.Length;wt++){
if(Seasons.Winter.winterTrees.treesWinter[wt]!= null){
  terraintree[wt].prefab=Seasons.Winter.winterTrees.treesWinter[wt];
}
}


Terrain.activeTerrain.terrainData.detailPrototypes = grass;
Terrain.activeTerrain.terrainData.splatPrototypes = terraintexture;
Terrain.activeTerrain.terrainData.treePrototypes = terraintree;
}
}
}

}
}

/////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////









/////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////

function SplashUpdate(){

if(SplashScreens.SplashScreensInGame==true){

if(SplashScreens.SplashTextures.springSplash.GetComponent.<GUITexture>().enabled==true){

if(Lerping==true){
alpha = Mathf.Lerp(alpha, 1, SplashScreens.fadeInTime * Time.deltaTime);
SplashScreens.SplashTextures.springSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha >= 0.5){
yield WaitForSeconds(SplashScreens.waitTime);
Lerping=false;
}
if(Lerping==false){
alpha = Mathf.Lerp(alpha, -0.5, SplashScreens.fadeOutTime * Time.deltaTime);
SplashScreens.SplashTextures.springSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha <= 0 && Lerping == false){
SplashScreens.SplashTextures.springSplash.GetComponent.<GUITexture>().enabled=false;
alpha=0;
Lerping=true;
}
}

if(SplashScreens.SplashTextures.summerSplash.GetComponent.<GUITexture>().enabled==true){

if(Lerping==true){
alpha = Mathf.Lerp(alpha, 1, SplashScreens.fadeInTime * Time.deltaTime);
SplashScreens.SplashTextures.summerSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha >= 0.5){
yield WaitForSeconds(SplashScreens.waitTime);
Lerping=false;
}
if(Lerping==false){
alpha = Mathf.Lerp(alpha, -0.5, SplashScreens.fadeOutTime * Time.deltaTime);
SplashScreens.SplashTextures.summerSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha <= 0 && Lerping == false){
SplashScreens.SplashTextures.summerSplash.GetComponent.<GUITexture>().enabled=false;
alpha=0;
Lerping=true;
}
}

if(SplashScreens.SplashTextures.auttumSplash.GetComponent.<GUITexture>().enabled==true){

if(Lerping==true){
alpha = Mathf.Lerp(alpha, 1, SplashScreens.fadeInTime * Time.deltaTime);
SplashScreens.SplashTextures.auttumSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha >= 0.5){
yield WaitForSeconds(SplashScreens.waitTime);
Lerping=false;
}
if(Lerping==false){
alpha = Mathf.Lerp(alpha, -0.5, SplashScreens.fadeOutTime * Time.deltaTime);
SplashScreens.SplashTextures.auttumSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha <= 0 && Lerping == false){
SplashScreens.SplashTextures.auttumSplash.GetComponent.<GUITexture>().enabled=false;
alpha=0;
Lerping=true;
}
}

if(SplashScreens.SplashTextures.winterSplash.GetComponent.<GUITexture>().enabled==true){

if(Lerping==true){
alpha = Mathf.Lerp(alpha, 1, SplashScreens.fadeInTime * Time.deltaTime);
SplashScreens.SplashTextures.winterSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha >= 0.5){
yield WaitForSeconds(SplashScreens.waitTime);
Lerping=false;
}
if(Lerping==false){
alpha = Mathf.Lerp(alpha, -0.5, SplashScreens.fadeOutTime * Time.deltaTime);
SplashScreens.SplashTextures.winterSplash.GetComponent.<GUITexture>().color = new Color(0.4,0.4,0.4, alpha);
}
if(alpha <= 0 && Lerping == false){
SplashScreens.SplashTextures.winterSplash.GetComponent.<GUITexture>().enabled=false;
alpha=0;
Lerping=true;
}
}
}
}



function SunTrajectory(){

if(sunRise || day || sunSet ){

frac2=0;
if(LightSettings.Sun!=null && LightSettings.dinamicLight==true){
LightSettings.Moon.SetActive(false);



 // The center of the arc
 frac  += Time.deltaTime*(ActualTimeGame.timeSpeed/divTime);

        var center = (GeneralSettings.Helpers.sunrise.position + GeneralSettings.Helpers.sunset.position) * 0.5;
        // move the center a bit downwards to make the arc vertical
        center -= Vector3(0,0.1,0);
    
        // Interpolate over the arc relative to center
        var riseRelCenter = GeneralSettings.Helpers.sunrise.position - center;
        var setRelCenter = GeneralSettings.Helpers.sunset.position - center;
        
        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        LightSettings.Sun.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter,frac);
        LightSettings.Sun.transform.position += center;
        }
        }
 
}

function MoonTrajectory(){

if(night==true){
frac = 0;
if(LightSettings.Moon!=null && LightSettings.moonInGame==true){
LightSettings.Moon.SetActive(true);
frac2 += Time.deltaTime*(ActualTimeGame.timeSpeed/24000);
 // The center of the arc
        var center = (GeneralSettings.Helpers.sunrise.position + GeneralSettings.Helpers.sunset.position) * 0.5;
        // move the center a bit downwards to make the arc vertical
        center -= Vector3(0,0.1,0);
    
        // Interpolate over the arc relative to center
        var riseRelCenter = GeneralSettings.Helpers.sunrise.position - center;
        var setRelCenter = GeneralSettings.Helpers.sunset.position - center;
        
        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        LightSettings.Moon.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, frac2);
        LightSettings.Moon.transform.position += center;
        }
        
        }

}


function Fades (){
if (timeLerpSun < 1 || timeLerpSun==0){
	timeLerpSun += Time.deltaTime/1;
	}
if(sunRise || sunSet){
sunTex.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor"),
sunColor2,timeLerpSun));

sunHorizonTex.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor"),
sunHorizonColor2,timeLerpSun));
}

if(day){
sunTex.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor"),
sunColor3,timeLerpSun));

sunHorizonTex.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor"),
sunHorizonColor3,timeLerpSun));
}

if(night){
sunTex.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(sunTex.GetComponent.<Renderer>().material.GetColor("_TintColor"),
sunColor,timeLerpSun));

sunHorizonTex.GetComponent.<Renderer>().material.SetColor("_TintColor", Color.Lerp(sunHorizonTex.GetComponent.<Renderer>().material.GetColor("_TintColor"),
sunHorizonColor,timeLerpSun));
}

timeLerpSun  = 0;
}

function Helpers(){
if(LightSettings.Moon==null && LightSettings.moonInGame==true){
Debug.Log("You need to setup a [Moon] in SkySphere Manager!");
}
if(LightSettings.dinamicLight==true && LightSettings.Sun==null){
Debug.Log("You need to setup a [Sun] in SkySphere Manager!");
}
if(GeneralSettings.Helpers.sunrise==null){
Debug.Log("Critic!: You need to set up a [sunrise helper], if you don't know, put the prefab of SkySphere again");
}
if(GeneralSettings.Helpers.sunset==null){
Debug.Log("Critic!: You need to set up a [sunset helper], if you don't know, put the prefab of SkySphere again");
}
if(RenderSettings.fog==false && FogSettings.dinamicFog==true){
Debug.Log("You checked dinamic fog, but you didn't check fog in Render Settings!");
}
if(CloudSettings.clouds == null && CloudSettings.cloudsInGame==true){
Debug.Log("You need to setup [Clouds]");
}
}

function OnApplicationQuit(){
instance=null;
}