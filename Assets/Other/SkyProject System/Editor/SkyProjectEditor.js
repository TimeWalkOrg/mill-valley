/////////////////////////////////////////////////////////////////////////
//				Made by KirbyRawr from Overflowing Studios.			   //
//						Overflowing Studios©						   //
//					     Thanks for buy it 	:)						   //
//			If you pirated it consider buying it in Asset Store. :D	   //
/////////////////////////////////////////////////////////////////////////

#pragma strict
@CustomEditor(SkyProject)
class SkyProjectEditor extends Editor {
//////////////////////////////
/////////Variables///////////
////////////////////////////

//Textures//
var lights2 = Resources.Load("lights",Texture);
var clouds2 = Resources.Load("clouds",Texture);
var seasons2 = Resources.Load("seasons",Texture);
var fog2 = Resources.Load("fog",Texture);
var temperature2 = Resources.Load("temperature",Texture);
var phases2 = Resources.Load("phases",Texture);
var weather2 = Resources.Load("weather",Texture); 
var time2 = Resources.Load("time",Texture);
var general2 = Resources.Load("general",Texture);
var splash2 = Resources.Load("splash",Texture);
///////////


//Foldouts!
var showGeneralSettings : boolean = false;
var showActualTime : boolean = false;
var showHelpers : boolean = false;
var showInGame : boolean = false;

var showColors : boolean = false;
var showColorsSunrise : boolean = false;
var showColorsDay : boolean = false;
var showColorsSunset : boolean = false;
var showColorsNight : boolean = false;

var showLightSettings : boolean = false;
var showLightColors : boolean = false;
var showAmbientLight : boolean = false;
var showIntensities : boolean = false;
var showFogSettings : boolean = false;
var showFogColors : boolean = false;
var showCloudsSettings : boolean = false;
var showCloudsColors : boolean = false;

var showSeasons : boolean = false;
var showSpring : boolean = false;
var showSummer : boolean = false;
var showAuttum : boolean = false;
var showWinter : boolean = false;

var showGUITextures : boolean = false;
var showTexturesProp : boolean = false;

var showTemperatures : boolean = false;
var showSpringTemperature : boolean = false;
var showSummerTemperature : boolean = false;
var showAuttumTemperature : boolean = false;
var showWinterTemperature : boolean = false;

var showSpringTemperatureSunRise : boolean = false;
var showSpringTemperatureDay : boolean = false;
var showSpringTemperatureSunSet : boolean = false;
var showSpringTemperatureNight : boolean = false;

var showSummerTemperatureSunRise : boolean = false;
var showSummerTemperatureDay : boolean = false;
var showSummerTemperatureSunSet : boolean = false;
var showSummerTemperatureNight : boolean = false;

var showAuttumTemperatureSunRise : boolean = false;
var showAuttumTemperatureDay : boolean = false;
var showAuttumTemperatureSunSet : boolean = false;
var showAuttumTemperatureNight : boolean = false;

var showWinterTemperatureSunRise : boolean = false;
var showWinterTemperatureDay : boolean = false;
var showWinterTemperatureSunSet : boolean = false;
var showWinterTemperatureNight : boolean = false;

//Weather//
var showWeather : boolean = false;
var showSpringWeather : boolean = false;
var showSummerWeather : boolean = false;
var showAuttumWeather : boolean = false;
var showWinterWeather : boolean = false;
var showRainSettings : boolean = false;
var showSpringRain : boolean = false;
var showSummerRain : boolean = false;
var showAuttumRain : boolean = false;
var showWinterRain : boolean = false;

var showLightRainSpring : boolean = false;
var showLightRainSpringSunRise : boolean = false;
var showLightRainSpringDay : boolean = false;
var showLightRainSpringSunSet : boolean = false;
var showLightRainSpringNight : boolean = false;

var showHeavyRainSpring : boolean = false;
var showHeavyRainSpringSunRise : boolean = false;
var showHeavyRainSpringDay : boolean = false;
var showHeavyRainSpringSunSet : boolean = false;
var showHeavyRainSpringNight : boolean = false;

var showLightRainSummer : boolean = false;
var showLightRainSummerSunRise : boolean = false;
var showLightRainSummerDay : boolean = false;
var showLightRainSummerSunSet : boolean = false;
var showLightRainSummerNight : boolean = false;

var showHeavyRainSummer : boolean = false;
var showHeavyRainSummerSunRise : boolean = false;
var showHeavyRainSummerDay : boolean = false;
var showHeavyRainSummerSunSet : boolean = false;
var showHeavyRainSummerNight : boolean = false;

var showLightRainAuttum : boolean = false;
var showLightRainAuttumSunRise : boolean = false;
var showLightRainAuttumDay : boolean = false;
var showLightRainAuttumSunSet : boolean = false;
var showLightRainAuttumNight : boolean = false;

var showHeavyRainAuttum : boolean = false;
var showHeavyRainAuttumSunRise : boolean = false;
var showHeavyRainAuttumDay : boolean = false;
var showHeavyRainAuttumSunSet : boolean = false;
var showHeavyRainAuttumNight : boolean = false;

var showLightRainWinter : boolean = false;
var showLightRainWinterSunRise : boolean = false;
var showLightRainWinterDay : boolean = false;
var showLightRainWinterSunSet : boolean = false;
var showLightRainWinterNight : boolean = false;

var showHeavyRainWinter : boolean = false;
var showHeavyRainWinterSunRise : boolean = false;
var showHeavyRainWinterDay : boolean = false;
var showHeavyRainWinterSunSet : boolean = false;
var showHeavyRainWinterNight : boolean = false;



//SerializedProperties//
var phaseProp : SerializedProperty;
var seasonProp : SerializedProperty;
///////////////////////


//////////////////////
//General Settings//
var cameraProp : SerializedProperty;
var skyLayerProp : SerializedProperty;
var terrainProp : SerializedProperty;
var sunRiseProp : SerializedProperty;
var sunSetProp : SerializedProperty;

//IN GAME//
var cloudInGameProp : SerializedProperty;
var dinamicFogProp : SerializedProperty;
var seasonsProp : SerializedProperty;
var weatherProp : SerializedProperty;
var splashProp : SerializedProperty;
///////////

//////////////////

//Phases//
var blendColorsProp : SerializedProperty;
var sunRiseColorsPropTop : SerializedProperty;
var sunRiseColorsPropBottom : SerializedProperty;
var dayColorsPropTop : SerializedProperty;
var dayColorsPropBottom : SerializedProperty;
var sunSetColorsPropTop : SerializedProperty;
var sunSetColorsPropBottom : SerializedProperty;
var nightColorsPropTop : SerializedProperty;
var nightColorsPropBottom : SerializedProperty;
////////////////

//Time MANAGER//
var yearProp : SerializedProperty;   
var monthProp : SerializedProperty;   
static var daysProp : SerializedProperty;   
var hourProp : SerializedProperty;   
var minutesProp : SerializedProperty;   
var secondsProp : SerializedProperty;   
var timeSpeedProp : SerializedProperty;
///////////////

//Light Settings//
var sunProp : SerializedProperty;
var moonProp : SerializedProperty;
var blendLightProp : SerializedProperty;
var sunriseLightProp : SerializedProperty;
var dayLightProp : SerializedProperty;
var sunsetLightProp : SerializedProperty;
var nightLightProp : SerializedProperty;

var blendAmbientProp : SerializedProperty;	
var sunriseAmbientProp : SerializedProperty;
var dayAmbientProp : SerializedProperty;
var sunsetAmbientProp : SerializedProperty;
var nightAmbientProp : SerializedProperty;

var blendIntensitiesProp : SerializedProperty;
var sunriseIntensityProp : SerializedProperty;
var dayIntensityProp : SerializedProperty;
var sunsetIntensityProp : SerializedProperty;
var nightIntensityProp : SerializedProperty;
/////////////////

//Fog Settings//
var fogModeProp : SerializedProperty;
var startDistanceProp : SerializedProperty;
var endDistanceProp : SerializedProperty;
var densityProp : SerializedProperty;

var blendFogProp : SerializedProperty;
var sunRiseFogProp : SerializedProperty; 
var dayFogProp : SerializedProperty;
var sunSetFogProp : SerializedProperty;
var nightFogProp : SerializedProperty;
//////////

//Cloud Settings//
var cloudsProp : SerializedProperty;
var blendCloudsProp : SerializedProperty;

var sunRiseCloudProp : SerializedProperty; 
var dayCloudProp : SerializedProperty;
var sunSetCloudProp : SerializedProperty;
var nightCloudProp : SerializedProperty;
//////////

//Seasons Settings//
//Spring//
var springmonthProp : SerializedProperty;
var springdayProp : SerializedProperty;
var springgrassColorHealthyProp : SerializedProperty; 
var springgrassColorDryProp : SerializedProperty;
var springTexturesProp : SerializedProperty;
var springTreesProp : SerializedProperty;

//Summer//
var summermonthProp : SerializedProperty;
var summerdayProp : SerializedProperty;
var summergrassColorHealthyProp : SerializedProperty; 
var summergrassColorDryProp : SerializedProperty;
var summerTexturesProp : SerializedProperty;
var summerTreesProp : SerializedProperty;

//Auttum//
var auttummonthProp : SerializedProperty;
var auttumdayProp : SerializedProperty;
var auttumgrassColorHealthyProp : SerializedProperty; 
var auttumgrassColorDryProp : SerializedProperty;
var auttumTexturesProp : SerializedProperty;
var auttumTreesProp : SerializedProperty;

//Winter//
var wintermonthProp : SerializedProperty;
var winterdayProp : SerializedProperty;
var wintergrassColorHealthyProp : SerializedProperty; 
var wintergrassColorDryProp : SerializedProperty;
var winterTexturesProp : SerializedProperty;
var winterTreesProp : SerializedProperty;
//////////


//GUI Textures//
var fadeInTimeProp : SerializedProperty;
var fadeOutTimeProp : SerializedProperty;
var waitTimeProp : SerializedProperty; 
var springTextureProp : SerializedProperty;
var summerTextureProp : SerializedProperty;
var auttumTextureProp : SerializedProperty;
var winterTextureProp: SerializedProperty;
///////////////

//Temperature//
var springSunRiseMax : SerializedProperty;
var springDayMax : SerializedProperty;
var springSunSetMax : SerializedProperty;
var springNightMax : SerializedProperty;

var springSunRiseMin : SerializedProperty;
var springDayMin: SerializedProperty;
var springSunSetMin : SerializedProperty;
var springNightMin: SerializedProperty;

var summerSunRiseMax : SerializedProperty;
var summerDayMax : SerializedProperty;
var summerSunSetMax : SerializedProperty;
var summerNightMax : SerializedProperty;

var summerSunRiseMin : SerializedProperty;
var summerDayMin: SerializedProperty;
var summerSunSetMin : SerializedProperty;
var summerNightMin: SerializedProperty;

var auttumSunRiseMax : SerializedProperty;
var auttumDayMax : SerializedProperty;
var auttumSunSetMax : SerializedProperty;
var auttumNightMax : SerializedProperty;

var auttumSunRiseMin : SerializedProperty;
var auttumDayMin: SerializedProperty;
var auttumSunSetMin : SerializedProperty;
var auttumNightMin: SerializedProperty;

var winterSunRiseMax : SerializedProperty;
var winterDayMax : SerializedProperty;
var winterSunSetMax : SerializedProperty;
var winterNightMax : SerializedProperty;

var winterSunRiseMin : SerializedProperty;
var winterDayMin : SerializedProperty;
var winterSunSetMin : SerializedProperty;
var winterNightMin : SerializedProperty;
//////////////////

//Weather//
var rainProp : SerializedProperty;
var rainblendTimeLightProp : SerializedProperty;
var rainblendTimeHeavyProp : SerializedProperty;

var rainSpringPercentage : SerializedProperty;
var rainSpringPercentageType : SerializedProperty;
var rainSpringLightMaxTemp : SerializedProperty;
var rainSpringLightMinTemp : SerializedProperty;
var rainSpringLightFade : SerializedProperty;
var rainSpringLightSunRiseTop : SerializedProperty;
var rainSpringLightSunRiseBottom : SerializedProperty;
var rainSpringLightDayTop : SerializedProperty;
var rainSpringLightDayBottom : SerializedProperty;
var rainSpringLightSunSetTop : SerializedProperty;
var rainSpringLightSunSetBottom : SerializedProperty;
var rainSpringLightNightTop : SerializedProperty;
var rainSpringLightNightBottom : SerializedProperty;
var rainSpringHeavyMaxTemp : SerializedProperty;
var rainSpringHeavyMinTemp : SerializedProperty;
var rainSpringHeavyFade : SerializedProperty;
var rainSpringHeavySunRiseTop : SerializedProperty;
var rainSpringHeavySunRiseBottom : SerializedProperty;
var rainSpringHeavyDayTop : SerializedProperty;
var rainSpringHeavyDayBottom : SerializedProperty;
var rainSpringHeavySunSetTop : SerializedProperty;
var rainSpringHeavySunSetBottom : SerializedProperty;
var rainSpringHeavyNightTop : SerializedProperty;
var rainSpringHeavyNightBottom : SerializedProperty;

var rainSummerPercentage : SerializedProperty;
var rainSummerPercentageType : SerializedProperty;
var rainSummerLightMaxTemp : SerializedProperty;
var rainSummerLightMinTemp : SerializedProperty;
var rainSummerLightFade : SerializedProperty;
var rainSummerLightSunRiseTop : SerializedProperty;
var rainSummerLightSunRiseBottom : SerializedProperty;
var rainSummerLightDayTop : SerializedProperty;
var rainSummerLightDayBottom : SerializedProperty;
var rainSummerLightSunSetTop : SerializedProperty;
var rainSummerLightSunSetBottom : SerializedProperty;
var rainSummerLightNightTop : SerializedProperty;
var rainSummerLightNightBottom : SerializedProperty;
var rainSummerHeavyMaxTemp : SerializedProperty;
var rainSummerHeavyMinTemp : SerializedProperty;
var rainSummerHeavyFade : SerializedProperty;
var rainSummerHeavySunRiseTop : SerializedProperty;
var rainSummerHeavySunRiseBottom : SerializedProperty;
var rainSummerHeavyDayTop : SerializedProperty;
var rainSummerHeavyDayBottom : SerializedProperty;
var rainSummerHeavySunSetTop : SerializedProperty;
var rainSummerHeavySunSetBottom : SerializedProperty;
var rainSummerHeavyNightTop : SerializedProperty;
var rainSummerHeavyNightBottom : SerializedProperty;

var rainAuttumPercentage : SerializedProperty;
var rainAuttumPercentageType : SerializedProperty;
var rainAuttumLightMaxTemp : SerializedProperty;
var rainAuttumLightMinTemp : SerializedProperty;
var rainAuttumLightFade : SerializedProperty;
var rainAuttumLightSunRiseTop : SerializedProperty;
var rainAuttumLightSunRiseBottom : SerializedProperty;
var rainAuttumLightDayTop : SerializedProperty;
var rainAuttumLightDayBottom : SerializedProperty;
var rainAuttumLightSunSetTop : SerializedProperty;
var rainAuttumLightSunSetBottom : SerializedProperty;
var rainAuttumLightNightTop : SerializedProperty;
var rainAuttumLightNightBottom : SerializedProperty;
var rainAuttumHeavyMaxTemp : SerializedProperty;
var rainAuttumHeavyMinTemp : SerializedProperty;
var rainAuttumHeavyFade : SerializedProperty;
var rainAuttumHeavySunRiseTop : SerializedProperty;
var rainAuttumHeavySunRiseBottom : SerializedProperty;
var rainAuttumHeavyDayTop : SerializedProperty;
var rainAuttumHeavyDayBottom : SerializedProperty;
var rainAuttumHeavySunSetTop : SerializedProperty;
var rainAuttumHeavySunSetBottom : SerializedProperty;
var rainAuttumHeavyNightTop : SerializedProperty;
var rainAuttumHeavyNightBottom : SerializedProperty;

var rainWinterPercentage : SerializedProperty;
var rainWinterPercentageType : SerializedProperty;
var rainWinterLightMaxTemp : SerializedProperty;
var rainWinterLightMinTemp : SerializedProperty;
var rainWinterLightFade : SerializedProperty;
var rainWinterLightSunRiseTop : SerializedProperty;
var rainWinterLightSunRiseBottom : SerializedProperty;
var rainWinterLightDayTop : SerializedProperty;
var rainWinterLightDayBottom : SerializedProperty;
var rainWinterLightSunSetTop : SerializedProperty;
var rainWinterLightSunSetBottom : SerializedProperty;
var rainWinterLightNightTop : SerializedProperty;
var rainWinterLightNightBottom : SerializedProperty;
var rainWinterHeavyMaxTemp : SerializedProperty;
var rainWinterHeavyMinTemp : SerializedProperty;
var rainWinterHeavyFade : SerializedProperty;
var rainWinterHeavySunRiseTop : SerializedProperty;
var rainWinterHeavySunRiseBottom : SerializedProperty;
var rainWinterHeavyDayTop : SerializedProperty;
var rainWinterHeavyDayBottom : SerializedProperty;
var rainWinterHeavySunSetTop : SerializedProperty;
var rainWinterHeavySunSetBottom : SerializedProperty;
var rainWinterHeavyNightTop : SerializedProperty;
var rainWinterHeavyNightBottom : SerializedProperty;

var rainSpringLightSunRiseAmbient : SerializedProperty;
var rainSpringHeavySunRiseAmbient : SerializedProperty;

var rainSummerLightSunRiseAmbient : SerializedProperty;
var rainSummerHeavySunRiseAmbient : SerializedProperty;

var rainAuttumLightSunRiseAmbient : SerializedProperty;
var rainAuttumHeavySunRiseAmbient : SerializedProperty;

var rainWinterLightSunRiseAmbient : SerializedProperty;
var rainWinterHeavySunRiseAmbient : SerializedProperty;


var rainSpringLightDayAmbient : SerializedProperty;
var rainSpringHeavyDayAmbient : SerializedProperty;

var rainSummerLightDayAmbient : SerializedProperty;
var rainSummerHeavyDayAmbient : SerializedProperty;

var rainAuttumLightDayAmbient : SerializedProperty;
var rainAuttumHeavyDayAmbient : SerializedProperty;

var rainWinterLightDayAmbient : SerializedProperty;
var rainWinterHeavyDayAmbient : SerializedProperty;


var rainSpringLightSunSetAmbient : SerializedProperty;
var rainSpringHeavySunSetAmbient : SerializedProperty;

var rainSummerLightSunSetAmbient : SerializedProperty;
var rainSummerHeavySunSetAmbient : SerializedProperty;

var rainAuttumLightSunSetAmbient : SerializedProperty;
var rainAuttumHeavySunSetAmbient : SerializedProperty;

var rainWinterLightSunSetAmbient : SerializedProperty;
var rainWinterHeavySunSetAmbient : SerializedProperty;


var rainSpringLightNightAmbient : SerializedProperty;
var rainSpringHeavyNightAmbient : SerializedProperty;

var rainSummerLightNightAmbient : SerializedProperty;
var rainSummerHeavyNightAmbient : SerializedProperty;

var rainAuttumLightNightAmbient : SerializedProperty;
var rainAuttumHeavyNightAmbient : SerializedProperty;

var rainWinterLightNightAmbient : SerializedProperty;
var rainWinterHeavyNightAmbient : SerializedProperty;

function OnEnable () {

phaseProp = serializedObject.FindProperty("PhaseOfTheDay"); 
seasonProp = serializedObject.FindProperty("SeasonsTime"); 

//General Settings//
cameraProp = serializedObject.FindProperty("GeneralSettings.mainCamera"); 
skyLayerProp = serializedObject.FindProperty("GeneralSettings.skyProjectLayer"); 
terrainProp = serializedObject.FindProperty("GeneralSettings.terrainvar");
sunSetProp = serializedObject.FindProperty("GeneralSettings.Helpers.sunset");
sunRiseProp = serializedObject.FindProperty("GeneralSettings.Helpers.sunrise");

//Vars//
cloudInGameProp = serializedObject.FindProperty("CloudSettings.cloudsInGame"); 
dinamicFogProp = serializedObject.FindProperty("FogSettings.dinamicFog"); 
seasonsProp = serializedObject.FindProperty("Seasons.seasonsInGame"); 
weatherProp = serializedObject.FindProperty("Weather.weatherInGame"); 
splashProp = serializedObject.FindProperty("SplashScreens.SplashScreensInGame"); 
///////

///////////////////

//TIME MANAGER//
yearProp = serializedObject.FindProperty("ActualTimeGame.year");   
monthProp = serializedObject.FindProperty("ActualTimeGame.month");   
daysProp = serializedObject.FindProperty("ActualTimeGame.days");   
hourProp = serializedObject.FindProperty("ActualTimeGame.hour");   
minutesProp = serializedObject.FindProperty("ActualTimeGame.minutes");   
secondsProp = serializedObject.FindProperty("ActualTimeGame.seconds");            
timeSpeedProp = serializedObject.FindProperty("ActualTimeGame.timeSpeed");
///////////////

//Colors Phase//	
blendColorsProp = serializedObject.FindProperty("ColorsOfTheDay.blendTimeColor"); 

sunRiseColorsPropTop = serializedObject.FindProperty("ColorsOfTheDay.SunRiseColors.sunRiseColorTop"); 
sunRiseColorsPropBottom = serializedObject.FindProperty("ColorsOfTheDay.SunRiseColors.sunRiseColorBottom");

dayColorsPropTop = serializedObject.FindProperty("ColorsOfTheDay.DayColors.dayColorTop"); 
dayColorsPropBottom = serializedObject.FindProperty("ColorsOfTheDay.DayColors.dayColorBottom");  

sunSetColorsPropTop = serializedObject.FindProperty("ColorsOfTheDay.SunSetColors.sunSetColorTop"); 
sunSetColorsPropBottom = serializedObject.FindProperty("ColorsOfTheDay.SunSetColors.sunSetColorBottom");

nightColorsPropTop = serializedObject.FindProperty("ColorsOfTheDay.NightColors.nightColorTop"); 
nightColorsPropBottom = serializedObject.FindProperty("ColorsOfTheDay.NightColors.nightColorBottom"); 


//Light Settings//	
blendColorsProp = serializedObject.FindProperty("ColorsOfTheDay.blendTimeColor"); 
sunProp = serializedObject.FindProperty("LightSettings.Sun"); 
moonProp = serializedObject.FindProperty("LightSettings.Moon");
blendLightProp = serializedObject.FindProperty("LightSettings.LightColors.blendTimeColorLight");
sunriseLightProp = serializedObject.FindProperty("LightSettings.LightColors.SunRiseLight");
dayLightProp = serializedObject.FindProperty("LightSettings.LightColors.DayLight");
sunsetLightProp = serializedObject.FindProperty("LightSettings.LightColors.SunSetLight");
nightLightProp = serializedObject.FindProperty("LightSettings.LightColors.NightLight");

blendAmbientProp = serializedObject.FindProperty("LightSettings.LightAmbientColors.blendTimeAmbientColor");
sunriseAmbientProp = serializedObject.FindProperty("LightSettings.LightAmbientColors.SunRiseAmbientColor");
dayAmbientProp = serializedObject.FindProperty("LightSettings.LightAmbientColors.DayAmbientColor");
sunsetAmbientProp = serializedObject.FindProperty("LightSettings.LightAmbientColors.SunSetAmbientColor");
nightAmbientProp = serializedObject.FindProperty("LightSettings.LightAmbientColors.NightAmbientColor");

blendIntensitiesProp = serializedObject.FindProperty("LightSettings.Intensities.blendTimeIntensity");
sunriseIntensityProp = serializedObject.FindProperty("LightSettings.Intensities.sunRiseIntensity");
dayIntensityProp = serializedObject.FindProperty("LightSettings.Intensities.dayIntensity");
sunsetIntensityProp= serializedObject.FindProperty("LightSettings.Intensities.sunSetIntensity");
nightIntensityProp = serializedObject.FindProperty("LightSettings.Intensities.nightIntensity");
////////////////

//Fog Settings//	
dinamicFogProp = serializedObject.FindProperty("FogSettings.dinamicFog");
fogModeProp = serializedObject.FindProperty("FogSettings.ModeOfTheFog");
startDistanceProp = serializedObject.FindProperty("FogSettings.startDistance"); 
endDistanceProp = serializedObject.FindProperty("FogSettings.endDistance");
densityProp = serializedObject.FindProperty("FogSettings.density");

blendFogProp = serializedObject.FindProperty("FogSettings.ColorsFog.blendFog");
sunRiseFogProp = serializedObject.FindProperty("FogSettings.ColorsFog.SunRiseFog");
dayFogProp  = serializedObject.FindProperty("FogSettings.ColorsFog.DayFog");
sunSetFogProp = serializedObject.FindProperty("FogSettings.ColorsFog.SunSetFog");
nightFogProp = serializedObject.FindProperty("FogSettings.ColorsFog.NightFog");
///////////////


//Cloud Settings//	
cloudsProp = serializedObject.FindProperty("CloudSettings.clouds"); 
blendCloudsProp = serializedObject.FindProperty("CloudSettings.blendTimeClouds"); 

sunRiseCloudProp  = serializedObject.FindProperty("CloudSettings.CloudsColors.SunRiseCloudsColor");
dayCloudProp  = serializedObject.FindProperty("CloudSettings.CloudsColors.DayCloudsColor");
sunSetCloudProp  = serializedObject.FindProperty("CloudSettings.CloudsColors.SunSetCloudsColor");
nightCloudProp  = serializedObject.FindProperty("CloudSettings.CloudsColors.NightCloudsColor");
///////////////

//Seasons//
springdayProp = serializedObject.FindProperty("Seasons.Spring.springDay");
springmonthProp = serializedObject.FindProperty("Seasons.Spring.springMonth");
springgrassColorHealthyProp= serializedObject.FindProperty("Seasons.Spring.grassColorHealthySpring"); 
springgrassColorDryProp= serializedObject.FindProperty("Seasons.Spring.grassColorDrySpring");
springTexturesProp = serializedObject.FindProperty("Seasons.Spring.springTexturesSeason.texturesSpring");
springTreesProp = serializedObject.FindProperty("Seasons.Spring.springTrees.treesSpring");

summerdayProp = serializedObject.FindProperty("Seasons.Summer.summerDay");
summermonthProp = serializedObject.FindProperty("Seasons.Summer.summerMonth");
summergrassColorHealthyProp= serializedObject.FindProperty("Seasons.Summer.grassColorHealthySummer"); 
summergrassColorDryProp= serializedObject.FindProperty("Seasons.Summer.grassColorDrySummer");
summerTexturesProp = serializedObject.FindProperty("Seasons.Summer.summerTexturesSeason.texturesSummer");
summerTreesProp = serializedObject.FindProperty("Seasons.Summer.summerTrees.treesSummer");

auttumdayProp = serializedObject.FindProperty("Seasons.Auttum.auttumDay");
auttummonthProp = serializedObject.FindProperty("Seasons.Auttum.auttumMonth");
auttumgrassColorHealthyProp= serializedObject.FindProperty("Seasons.Auttum.grassColorHealthyAuttum"); 
auttumgrassColorDryProp= serializedObject.FindProperty("Seasons.Auttum.grassColorDryAuttum");
auttumTexturesProp = serializedObject.FindProperty("Seasons.Auttum.auttumTexturesSeason.texturesAuttum");
auttumTreesProp = serializedObject.FindProperty("Seasons.Auttum.auttumTrees.treesAuttum");

winterdayProp = serializedObject.FindProperty("Seasons.Winter.winterDay");
wintermonthProp = serializedObject.FindProperty("Seasons.Winter.winterMonth");
wintergrassColorHealthyProp= serializedObject.FindProperty("Seasons.Winter.grassColorHealthyWinter"); 
wintergrassColorDryProp= serializedObject.FindProperty("Seasons.Winter.grassColorDryWinter");
winterTexturesProp = serializedObject.FindProperty("Seasons.Winter.winterTexturesSeason.texturesWinter");
winterTreesProp = serializedObject.FindProperty("Seasons.Winter.winterTrees.treesWinter");
//////////////

//GUI TEXTURES//
fadeInTimeProp = serializedObject.FindProperty("SplashScreens.fadeInTime");
fadeOutTimeProp = serializedObject.FindProperty("SplashScreens.fadeOutTime");
waitTimeProp = serializedObject.FindProperty("SplashScreens.waitTime");
springTextureProp = serializedObject.FindProperty("SplashScreens.SplashTextures.springSplash");
summerTextureProp = serializedObject.FindProperty("SplashScreens.SplashTextures.summerSplash");
auttumTextureProp = serializedObject.FindProperty("SplashScreens.SplashTextures.auttumSplash");
winterTextureProp = serializedObject.FindProperty("SplashScreens.SplashTextures.winterSplash");
///////////////

//TEMPERATURE//

springSunRiseMin = serializedObject.FindProperty("Temperature.SpringTemperature.SunRiseTemperatureSpring.minimumSpring");
summerSunRiseMin = serializedObject.FindProperty("Temperature.SummerTemperature.SunRiseTemperatureSummer.minimumSummer");
auttumSunRiseMin = serializedObject.FindProperty("Temperature.AuttumTemperature.SunRiseTemperatureAuttum.minimumAuttum");
winterSunRiseMin = serializedObject.FindProperty("Temperature.WinterTemperature.SunRiseTemperatureWinter.minimumWinter");

springDayMin = serializedObject.FindProperty("Temperature.SpringTemperature.DayTemperatureSpring.minimumSpring");
summerDayMin = serializedObject.FindProperty("Temperature.SummerTemperature.DayTemperatureSummer.minimumSummer");
auttumDayMin = serializedObject.FindProperty("Temperature.AuttumTemperature.DayTemperatureAuttum.minimumAuttum");
winterDayMin = serializedObject.FindProperty("Temperature.WinterTemperature.DayTemperatureWinter.minimumWinter");

springSunSetMin = serializedObject.FindProperty("Temperature.SpringTemperature.SunSetTemperatureSpring.minimumSpring");
summerSunSetMin = serializedObject.FindProperty("Temperature.SummerTemperature.SunSetTemperatureSummer.minimumSummer");
auttumSunSetMin = serializedObject.FindProperty("Temperature.AuttumTemperature.SunSetTemperatureAuttum.minimumAuttum");
winterSunSetMin = serializedObject.FindProperty("Temperature.WinterTemperature.SunSetTemperatureWinter.minimumWinter");

springNightMin = serializedObject.FindProperty("Temperature.SpringTemperature.NightTemperatureSpring.minimumSpring");
summerNightMin = serializedObject.FindProperty("Temperature.SummerTemperature.NightTemperatureSummer.minimumSummer");
auttumNightMin = serializedObject.FindProperty("Temperature.AuttumTemperature.NightTemperatureAuttum.minimumAuttum");
winterNightMin = serializedObject.FindProperty("Temperature.WinterTemperature.NightTemperatureWinter.minimumWinter");

//MAX
springSunRiseMax = serializedObject.FindProperty("Temperature.SpringTemperature.SunRiseTemperatureSpring.maximumSpring");
summerSunRiseMax = serializedObject.FindProperty("Temperature.SummerTemperature.SunRiseTemperatureSummer.maximumSummer");
auttumSunRiseMax = serializedObject.FindProperty("Temperature.AuttumTemperature.SunRiseTemperatureAuttum.maximumAuttum");
winterSunRiseMax = serializedObject.FindProperty("Temperature.WinterTemperature.SunRiseTemperatureWinter.maximumWinter");

springDayMax = serializedObject.FindProperty("Temperature.SpringTemperature.DayTemperatureSpring.maximumSpring");
summerDayMax = serializedObject.FindProperty("Temperature.SummerTemperature.DayTemperatureSummer.maximumSummer");
auttumDayMax = serializedObject.FindProperty("Temperature.AuttumTemperature.DayTemperatureAuttum.maximumAuttum");
winterDayMax = serializedObject.FindProperty("Temperature.WinterTemperature.DayTemperatureWinter.maximumWinter");

springSunSetMax = serializedObject.FindProperty("Temperature.SpringTemperature.SunSetTemperatureSpring.maximumSpring");
summerSunSetMax = serializedObject.FindProperty("Temperature.SummerTemperature.SunSetTemperatureSummer.maximumSummer");
auttumSunSetMax = serializedObject.FindProperty("Temperature.AuttumTemperature.SunSetTemperatureAuttum.maximumAuttum");
winterSunSetMax = serializedObject.FindProperty("Temperature.WinterTemperature.SunSetTemperatureWinter.maximumWinter");

springNightMax = serializedObject.FindProperty("Temperature.SpringTemperature.NightTemperatureSpring.maximumSpring");
summerNightMax = serializedObject.FindProperty("Temperature.SummerTemperature.NightTemperatureSummer.maximumSummer");
auttumNightMax = serializedObject.FindProperty("Temperature.AuttumTemperature.NightTemperatureAuttum.maximumAuttum");
winterNightMax = serializedObject.FindProperty("Temperature.WinterTemperature.NightTemperatureWinter.maximumWinter");
//////////////

//Weather//
rainProp = serializedObject.FindProperty("Weather.RainSettings.rain");
rainblendTimeLightProp = serializedObject.FindProperty("Weather.RainSettings.blendTimeLightRain");
rainblendTimeHeavyProp = serializedObject.FindProperty("Weather.RainSettings.blendTimeHeavyRain");

//SPRING
rainSpringPercentage = serializedObject.FindProperty("Weather.RainSettings.SpringRain.rainPercentage");
rainSpringPercentageType = serializedObject.FindProperty("Weather.RainSettings.SpringRain.rainTypePercentage");

rainSpringLightMaxTemp = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.maximumTemperature");
rainSpringLightMinTemp = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.minimumTemperature");
rainSpringLightFade = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.rainFade");

rainSpringLightSunRiseTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.SunRiseColorsRain.topColor");
rainSpringLightSunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.SunRiseColorsRain.bottomColor"); 
rainSpringLightSunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.SunRiseColorsRain.ambientColor"); 

rainSpringLightDayTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.DayColorsRain.topColor");
rainSpringLightDayBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.DayColorsRain.bottomColor"); 
rainSpringLightDayAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.DayColorsRain.ambientColor"); 

rainSpringLightSunSetTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.SunSetColorsRain.topColor");
rainSpringLightSunSetBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.SunSetColorsRain.bottomColor"); 
rainSpringLightSunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.SunSetColorsRain.ambientColor"); 

rainSpringLightNightTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.NightColorsRain.topColor");
rainSpringLightNightBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.NightColorsRain.bottomColor");
rainSpringLightNightAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.LightRain.NightColorsRain.ambientColor"); 
 
rainSpringHeavyMaxTemp = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.maximumTemperature");
rainSpringHeavyMinTemp = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.minimumTemperature");
rainSpringHeavyFade = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.rainFade");

rainSpringHeavySunRiseTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.SunRiseColorsRain.topColor");
rainSpringHeavySunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.SunRiseColorsRain.bottomColor"); 
rainSpringHeavySunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.SunRiseColorsRain.ambientColor"); 

rainSpringHeavyDayTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.DayColorsRain.topColor");
rainSpringHeavyDayBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.DayColorsRain.bottomColor"); 
rainSpringHeavyDayAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.DayColorsRain.ambientColor"); 

rainSpringHeavySunSetTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.SunSetColorsRain.topColor");
rainSpringHeavySunSetBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.SunSetColorsRain.bottomColor"); 
rainSpringHeavySunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.SunSetColorsRain.ambientColor"); 

rainSpringHeavyNightTop = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.NightColorsRain.topColor");
rainSpringHeavyNightBottom = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.NightColorsRain.bottomColor");
rainSpringHeavyNightAmbient = serializedObject.FindProperty("Weather.RainSettings.SpringRain.HeavyRain.NightColorsRain.ambientColor");  

//SUMMER
rainSummerPercentage = serializedObject.FindProperty("Weather.RainSettings.SummerRain.rainPercentage");
rainSummerPercentageType = serializedObject.FindProperty("Weather.RainSettings.SummerRain.rainTypePercentage");

rainSummerLightMaxTemp = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.maximumTemperature");
rainSummerLightMinTemp = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.minimumTemperature");
rainSummerLightFade = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.rainFade");

rainSummerLightSunRiseTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.SunRiseColorsRain.topColor");
rainSummerLightSunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.SunRiseColorsRain.bottomColor"); 
rainSummerLightSunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.SunRiseColorsRain.ambientColor"); 

rainSummerLightDayTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.DayColorsRain.topColor");
rainSummerLightDayBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.DayColorsRain.bottomColor"); 
rainSummerLightDayAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.DayColorsRain.ambientColor"); 

rainSummerLightSunSetTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.SunSetColorsRain.topColor");
rainSummerLightSunSetBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.SunSetColorsRain.bottomColor"); 
rainSummerLightSunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.SunSetColorsRain.ambientColor"); 

rainSummerLightNightTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.NightColorsRain.topColor");
rainSummerLightNightBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.NightColorsRain.bottomColor");
rainSummerLightNightAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.LightRain.NightColorsRain.ambientColor"); 

rainSummerHeavyMaxTemp = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.maximumTemperature");
rainSummerHeavyMinTemp = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.minimumTemperature");
rainSummerHeavyFade = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.rainFade");

rainSummerHeavySunRiseTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.SunRiseColorsRain.topColor");
rainSummerHeavySunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.SunRiseColorsRain.bottomColor"); 
rainSummerHeavySunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.SunRiseColorsRain.ambientColor"); 

rainSummerHeavyDayTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.DayColorsRain.topColor");
rainSummerHeavyDayBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.DayColorsRain.bottomColor"); 
rainSummerHeavyDayAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.DayColorsRain.ambientColor"); 

rainSummerHeavySunSetTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.SunSetColorsRain.topColor");
rainSummerHeavySunSetBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.SunSetColorsRain.bottomColor"); 
rainSummerHeavySunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.SunSetColorsRain.ambientColor"); 

rainSummerHeavyNightTop = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.NightColorsRain.topColor");
rainSummerHeavyNightBottom = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.NightColorsRain.bottomColor"); 
rainSummerHeavyNightAmbient = serializedObject.FindProperty("Weather.RainSettings.SummerRain.HeavyRain.NightColorsRain.ambientColor"); 

//AUTTUM
rainAuttumPercentage = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.rainPercentage");
rainAuttumPercentageType = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.rainTypePercentage");

rainAuttumLightMaxTemp = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.maximumTemperature");
rainAuttumLightMinTemp = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.minimumTemperature");
rainAuttumLightFade = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.rainFade");

rainAuttumLightSunRiseTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.SunRiseColorsRain.topColor");
rainAuttumLightSunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.SunRiseColorsRain.bottomColor");
rainAuttumLightSunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.SunRiseColorsRain.ambientColor");   

rainAuttumLightDayTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.DayColorsRain.topColor");
rainAuttumLightDayBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.DayColorsRain.bottomColor"); 
rainAuttumLightDayAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.DayColorsRain.ambientColor");   

rainAuttumLightSunSetTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.SunSetColorsRain.topColor");
rainAuttumLightSunSetBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.SunSetColorsRain.bottomColor"); 
rainAuttumLightSunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.SunSetColorsRain.ambientColor");   

rainAuttumLightNightTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.NightColorsRain.topColor");
rainAuttumLightNightBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.NightColorsRain.bottomColor");
rainAuttumLightNightAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.LightRain.NightColorsRain.ambientColor");   
  
rainAuttumHeavyMaxTemp = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.maximumTemperature");
rainAuttumHeavyMinTemp = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.minimumTemperature");
rainAuttumHeavyFade = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.rainFade");

rainAuttumHeavySunRiseTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.SunRiseColorsRain.topColor");
rainAuttumHeavySunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.SunRiseColorsRain.bottomColor"); 
rainAuttumHeavySunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.SunRiseColorsRain.ambientColor");   

rainAuttumHeavyDayTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.DayColorsRain.topColor");
rainAuttumHeavyDayBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.DayColorsRain.bottomColor"); 
rainAuttumHeavyDayAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.DayColorsRain.ambientColor");   

rainAuttumHeavySunSetTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.SunSetColorsRain.topColor");
rainAuttumHeavySunSetBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.SunSetColorsRain.bottomColor"); 
rainAuttumHeavySunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.SunSetColorsRain.ambientColor");   

rainAuttumHeavyNightTop = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.NightColorsRain.topColor");
rainAuttumHeavyNightBottom = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.NightColorsRain.bottomColor"); 
rainAuttumHeavyNightAmbient = serializedObject.FindProperty("Weather.RainSettings.AuttumRain.HeavyRain.NightColorsRain.ambientColor");   

//WINTER

rainWinterPercentage = serializedObject.FindProperty("Weather.RainSettings.WinterRain.rainPercentage");
rainWinterPercentageType = serializedObject.FindProperty("Weather.RainSettings.WinterRain.rainTypePercentage");

rainWinterLightMaxTemp = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.maximumTemperature");
rainWinterLightMinTemp = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.minimumTemperature");
rainWinterLightFade = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.rainFade");

rainWinterLightSunRiseTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.SunRiseColorsRain.topColor");
rainWinterLightSunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.SunRiseColorsRain.bottomColor"); 
rainWinterLightSunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.SunRiseColorsRain.ambientColor");  

rainWinterLightDayTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.DayColorsRain.topColor");
rainWinterLightDayBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.DayColorsRain.bottomColor"); 
rainWinterLightDayAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.DayColorsRain.ambientColor");  

rainWinterLightSunSetTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.SunSetColorsRain.topColor");
rainWinterLightSunSetBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.SunSetColorsRain.bottomColor"); 
rainWinterLightSunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.SunSetColorsRain.ambientColor");  

rainWinterLightNightTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.NightColorsRain.topColor");
rainWinterLightNightBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.NightColorsRain.bottomColor");
rainWinterLightNightAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.LightRain.NightColorsRain.ambientColor");  
 
rainWinterHeavyMaxTemp = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.maximumTemperature");
rainWinterHeavyMinTemp = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.minimumTemperature");
rainWinterHeavyFade = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.rainFade");

rainWinterHeavySunRiseTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.SunRiseColorsRain.topColor");
rainWinterHeavySunRiseBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.SunRiseColorsRain.bottomColor"); 
rainWinterHeavySunRiseAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.SunRiseColorsRain.ambientColor"); 

rainWinterHeavyDayTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.DayColorsRain.topColor");
rainWinterHeavyDayBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.DayColorsRain.bottomColor"); 
rainWinterHeavyDayAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.DayColorsRain.ambientColor"); 

rainWinterHeavySunSetTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.SunSetColorsRain.topColor");
rainWinterHeavySunSetBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.SunSetColorsRain.bottomColor"); 
rainWinterHeavySunSetAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.SunSetColorsRain.ambientColor"); 

rainWinterHeavyNightTop = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.NightColorsRain.topColor");
rainWinterHeavyNightBottom = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.NightColorsRain.bottomColor"); 
rainWinterHeavyNightAmbient = serializedObject.FindProperty("Weather.RainSettings.WinterRain.HeavyRain.NightColorsRain.ambientColor"); 

}




function OnInspectorGUI(){
//Serialized!
serializedObject.Update ();
EditorGUILayout.Space();
EditorGUILayout.PropertyField(phaseProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(seasonProp);

//General Settings//
EditorGUILayout.Space();
EditorGUILayout.LabelField(GUIContent(general2),GUILayout.Width(120),GUILayout.Height(40));
showGeneralSettings = EditorGUILayout.Foldout(showGeneralSettings,"General Settings");

if(showGeneralSettings){
EditorGUI.indentLevel++;
EditorGUILayout.PropertyField(cameraProp);
EditorGUILayout.PropertyField(skyLayerProp, GUILayout.ExpandWidth(false));
//EditorGUILayout.PropertyField(terrainProp,true);
EditorGUILayout.Space();
showInGame = EditorGUILayout.Foldout(showInGame, "Enable/Disable Functions");
if(showInGame){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(cloudInGameProp); 
EditorGUILayout.PropertyField(dinamicFogProp);
EditorGUILayout.PropertyField(seasonsProp);
EditorGUILayout.PropertyField(weatherProp);
EditorGUILayout.PropertyField(splashProp);
}
EditorGUILayout.Space();
showHelpers = EditorGUILayout.Foldout(showHelpers,"Helpers");
if(showHelpers){
EditorGUILayout.PropertyField(sunSetProp);
EditorGUILayout.PropertyField(sunRiseProp);
}
EditorGUI.indentLevel--;
}


EditorGUILayout.Space();


//Foldout General Settings
EditorGUILayout.LabelField(GUIContent(phases2),GUILayout.Width(150),GUILayout.Height(40));
showColors = EditorGUILayout.Foldout(showColors,"Phases Colors");
if(showColors){
EditorGUI.indentLevel++;

	EditorGUILayout.PropertyField(blendColorsProp, GUILayout.ExpandWidth(false));
	EditorGUILayout.Space();
	showColorsSunrise = EditorGUILayout.Foldout(showColorsSunrise,"SunRise");
	if(showColorsSunrise){
	EditorGUILayout.PropertyField(sunRiseColorsPropTop);
	EditorGUILayout.PropertyField(sunRiseColorsPropBottom);
	}
	EditorGUILayout.Space();
	showColorsDay = EditorGUILayout.Foldout(showColorsDay,"Day");
	if(showColorsDay){
	EditorGUILayout.PropertyField(dayColorsPropTop);
	EditorGUILayout.PropertyField(dayColorsPropBottom);
	}
	EditorGUILayout.Space();
	showColorsSunset = EditorGUILayout.Foldout(showColorsSunset,"SunSet");
	if(showColorsSunset){
	EditorGUILayout.PropertyField(sunSetColorsPropTop);
	EditorGUILayout.PropertyField(sunSetColorsPropBottom);
	}
	EditorGUILayout.Space();
	showColorsNight = EditorGUILayout.Foldout(showColorsNight,"Night");
	if(showColorsNight){
	EditorGUILayout.PropertyField(nightColorsPropTop);
	EditorGUILayout.PropertyField(nightColorsPropBottom);
	}
	EditorGUI.indentLevel--;
}


EditorGUILayout.Space();


//TIME MANAGER//
EditorGUILayout.LabelField(GUIContent(time2),GUILayout.Width(150),GUILayout.Height(40));
showActualTime = EditorGUILayout.Foldout(showActualTime,"Time Manager");
if(showActualTime){
EditorGUI.indentLevel++;
EditorGUILayout.PropertyField(yearProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(monthProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(daysProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(hourProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(minutesProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(secondsProp);
EditorGUILayout.Space();
EditorGUILayout.Slider(timeSpeedProp, 0f, 2000f, new GUIContent ("TimeSpeed"));
EditorGUI.indentLevel--;
}
EditorGUILayout.Space();


//Light Settings//
EditorGUILayout.LabelField(GUIContent(lights2),GUILayout.Width(120),GUILayout.Height(40));
showLightSettings = EditorGUILayout.Foldout(showLightSettings,"Light Settings");
if(showLightSettings){

EditorGUILayout.Space();
EditorGUI.indentLevel++;
EditorGUILayout.PropertyField(sunProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(moonProp);
EditorGUILayout.Space();
showLightColors = EditorGUILayout.Foldout(showLightColors,"Light Colors");
if(showLightColors){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(blendLightProp, GUILayout.ExpandWidth(false));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(sunriseLightProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(dayLightProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(sunsetLightProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(nightLightProp);
}
//Ambient Light//
EditorGUILayout.Space();
showAmbientLight = EditorGUILayout.Foldout(showAmbientLight,"Ambient Colors");
if(showAmbientLight){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(blendAmbientProp, GUILayout.ExpandWidth(false));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(sunriseAmbientProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(dayAmbientProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(sunsetAmbientProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(nightAmbientProp);
}

//Light Intensity//
EditorGUILayout.Space();
showIntensities = EditorGUILayout.Foldout(showIntensities,"Light Intensities");
if(showIntensities){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(blendIntensitiesProp, GUILayout.ExpandWidth(false));
EditorGUILayout.Space();
EditorGUILayout.Slider(sunriseIntensityProp, 0f, 8f);
EditorGUILayout.Space();
EditorGUILayout.Slider(dayIntensityProp, 0f, 8f);
EditorGUILayout.Space();
EditorGUILayout.Slider(sunsetIntensityProp, 0f, 8f);
EditorGUILayout.Space();
EditorGUILayout.Slider(nightIntensityProp, 0f, 8f);
}
EditorGUI.indentLevel--;
}

EditorGUILayout.Space();
EditorGUILayout.LabelField(GUIContent(fog2),GUILayout.Width(120),GUILayout.Height(40));
showFogSettings = EditorGUILayout.Foldout(showFogSettings,"Fog Settings");
if(showFogSettings){
EditorGUI.indentLevel++;
EditorGUILayout.Space();
EditorGUILayout.PropertyField(blendFogProp, GUILayout.ExpandWidth(false));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(fogModeProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(startDistanceProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(endDistanceProp);
EditorGUILayout.Space();
EditorGUILayout.Slider(densityProp, 0f, 1f, new GUIContent ("Density"));
EditorGUILayout.Space();
showFogColors = EditorGUILayout.Foldout(showFogColors,"Fog Colors");
EditorGUILayout.Space();
if(showFogColors){
EditorGUILayout.PropertyField(sunRiseFogProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(dayFogProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(sunSetFogProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(nightFogProp);
}
EditorGUI.indentLevel--;
}

EditorGUILayout.Space();
EditorGUILayout.LabelField(GUIContent(clouds2),GUILayout.Width(120),GUILayout.Height(40));
showCloudsSettings = EditorGUILayout.Foldout(showCloudsSettings,"Cloud Settings");
if(showCloudsSettings){
EditorGUI.indentLevel++;
EditorGUILayout.Space();
EditorGUILayout.PropertyField(cloudsProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(blendCloudsProp, GUILayout.ExpandWidth(false));
EditorGUILayout.Space();
showCloudsColors = EditorGUILayout.Foldout(showCloudsColors,"Cloud Colors");
EditorGUILayout.Space();
if(showCloudsColors){
EditorGUILayout.PropertyField(sunRiseCloudProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(dayCloudProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(sunSetCloudProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(nightCloudProp);
}
EditorGUI.indentLevel--;
}

EditorGUILayout.Space();
EditorGUILayout.LabelField(GUIContent(seasons2),GUILayout.Width(120),GUILayout.Height(40f));
showSeasons = EditorGUILayout.Foldout(showSeasons,"Seasons");
if(showSeasons){
EditorGUI.indentLevel++;
EditorGUILayout.Space();

showSpring = EditorGUILayout.Foldout(showSpring,"Spring");
if(showSpring){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(springdayProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(springmonthProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(springgrassColorHealthyProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(springgrassColorDryProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(springTexturesProp,true);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(springTreesProp,true);
}
EditorGUILayout.Space();
EditorGUILayout.Space();
EditorGUILayout.Space();
showSummer = EditorGUILayout.Foldout(showSummer,"Summer");
if(showSummer){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(summerdayProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(summermonthProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(summergrassColorHealthyProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(summergrassColorDryProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(summerTexturesProp,true);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(summerTreesProp,true);
}

EditorGUILayout.Space();
EditorGUILayout.Space();
EditorGUILayout.Space();
showAuttum = EditorGUILayout.Foldout(showAuttum,"Auttumn");
if(showAuttum){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(auttumdayProp,new GUIContent ("Auttumn Day"));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(auttummonthProp,new GUIContent ("Auttumn Month"));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(auttumgrassColorHealthyProp,new GUIContent ("Grass Color Healthy Auttumn"));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(auttumgrassColorDryProp,new GUIContent ("Grass Color Dry Auttumn"));
EditorGUILayout.Space();
EditorGUILayout.PropertyField(auttumTexturesProp,new GUIContent ("Auttumn Textures"),true);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(auttumTreesProp,new GUIContent ("Auttumn Trees"),true);
}

EditorGUILayout.Space();
EditorGUILayout.Space();
EditorGUILayout.Space();
showWinter = EditorGUILayout.Foldout(showWinter,"Winter");
if(showWinter){
EditorGUILayout.Space();
EditorGUILayout.PropertyField(winterdayProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(wintermonthProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(wintergrassColorHealthyProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(wintergrassColorDryProp);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(winterTexturesProp,true);
EditorGUILayout.Space();
EditorGUILayout.PropertyField(winterTreesProp,true);
}
EditorGUI.indentLevel--;
}

EditorGUILayout.Space();
EditorGUILayout.LabelField(GUIContent(splash2),GUILayout.Width(150),GUILayout.Height(40));
showGUITextures = EditorGUILayout.Foldout(showGUITextures,"Splash Textures");
if(showGUITextures){
EditorGUI.indentLevel++;
	EditorGUILayout.Space();
	EditorGUILayout.PropertyField(fadeInTimeProp);
	EditorGUILayout.Space();
	EditorGUILayout.PropertyField(fadeOutTimeProp);
	EditorGUILayout.Space();
	EditorGUILayout.PropertyField(waitTimeProp);
	EditorGUILayout.Space();
	showTexturesProp = EditorGUILayout.Foldout(showTexturesProp,"Textures");
	if(showTexturesProp){
	EditorGUILayout.PropertyField(springTextureProp);
	EditorGUILayout.PropertyField(summerTextureProp);
	EditorGUILayout.PropertyField(auttumTextureProp,new GUIContent ("Auttumn Splash"));
	EditorGUILayout.PropertyField(winterTextureProp);
	}
	EditorGUI.indentLevel--;
}

EditorGUILayout.Space();
EditorGUILayout.LabelField(GUIContent(temperature2),GUILayout.Width(150),GUILayout.Height(40));
showTemperatures = EditorGUILayout.Foldout(showTemperatures,"Temperature");
if(showTemperatures){
EditorGUI.indentLevel++;

	EditorGUILayout.Space();
	
	showSpringTemperature = EditorGUILayout.Foldout(showSpringTemperature,"Spring Temperature");
	if(showSpringTemperature){
	EditorGUI.indentLevel++;
	
		EditorGUILayout.Space();
		showSpringTemperatureSunRise = EditorGUILayout.Foldout(showSpringTemperatureSunRise,"SunRise Temperature");
		if(showSpringTemperatureSunRise){
			EditorGUILayout.PropertyField(springSunRiseMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(springSunRiseMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showSpringTemperatureDay = EditorGUILayout.Foldout(showSpringTemperatureDay,"Day Temperature");
		if(showSpringTemperatureDay){
			EditorGUILayout.PropertyField(springDayMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(springDayMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showSpringTemperatureSunSet = EditorGUILayout.Foldout(showSpringTemperatureSunSet,"SunSet Temperature");
		if(showSpringTemperatureSunSet){
			EditorGUILayout.PropertyField(springSunSetMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(springSunSetMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showSpringTemperatureNight = EditorGUILayout.Foldout(showSpringTemperatureNight,"Night Temperature");
		if(showSpringTemperatureNight){	
			EditorGUILayout.PropertyField(springNightMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(springNightMax, GUILayout.ExpandWidth(false));
		}
		EditorGUI.indentLevel--;
	}
	
	EditorGUILayout.Space();
	
	showSummerTemperature = EditorGUILayout.Foldout(showSummerTemperature,"Summer Temperature");
	if(showSummerTemperature){
	EditorGUI.indentLevel++;
	
		EditorGUILayout.Space();
		showWinterTemperatureSunRise = EditorGUILayout.Foldout(showWinterTemperatureSunRise,"SunRise Temperature");
		if(showWinterTemperatureSunRise){
			EditorGUILayout.PropertyField(summerSunRiseMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(summerSunRiseMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showSummerTemperatureDay = EditorGUILayout.Foldout(showSummerTemperatureDay,"Day Temperature");
		if(showSummerTemperatureDay){
			EditorGUILayout.PropertyField(summerDayMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(summerDayMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showSummerTemperatureSunSet = EditorGUILayout.Foldout(showSummerTemperatureSunSet,"SunSet Temperature");
		if(showSummerTemperatureSunSet){
			EditorGUILayout.PropertyField(summerSunSetMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(summerSunSetMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showSummerTemperatureNight = EditorGUILayout.Foldout(showSummerTemperatureNight,"Night Temperature");
		if(showSummerTemperatureNight){
			EditorGUILayout.PropertyField(summerNightMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(summerNightMax, GUILayout.ExpandWidth(false));
		}
		EditorGUI.indentLevel--;
	}
	
	EditorGUILayout.Space();
	
	showAuttumTemperature = EditorGUILayout.Foldout(showAuttumTemperature,"Auttumn Temperature");
	if(showAuttumTemperature){
	EditorGUI.indentLevel++;
	
		EditorGUILayout.Space();
		showAuttumTemperatureSunRise = EditorGUILayout.Foldout(showAuttumTemperatureSunRise,"SunRise Temperature");
		if(showAuttumTemperatureSunRise){
			EditorGUILayout.PropertyField(auttumSunRiseMin,new GUIContent ("Auttumn Min"), GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(auttumSunRiseMax,new GUIContent ("Auttumn Max"), GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showAuttumTemperatureDay = EditorGUILayout.Foldout(showAuttumTemperatureDay,"Day Temperature");
		if(showAuttumTemperatureDay){
			EditorGUILayout.PropertyField(auttumDayMin,new GUIContent ("Auttumn Min"), GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(auttumDayMax,new GUIContent ("Auttumn Max"), GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showAuttumTemperatureSunSet = EditorGUILayout.Foldout(showAuttumTemperatureSunSet,"SunSet Temperature");
		if(showAuttumTemperatureSunSet){
			EditorGUILayout.PropertyField(auttumSunSetMin,new GUIContent ("Auttumn Min"), GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(auttumSunSetMax,new GUIContent ("Auttumn Max"), GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showAuttumTemperatureNight = EditorGUILayout.Foldout(showAuttumTemperatureNight,"Night Temperature");
		if(showAuttumTemperatureNight){
			EditorGUILayout.PropertyField(auttumNightMin,new GUIContent ("Auttumn Min"), GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(auttumNightMax,new GUIContent ("Auttumn Max"), GUILayout.ExpandWidth(false));
		}
		EditorGUI.indentLevel--;
	}
	
	EditorGUILayout.Space();
	
	showWinterTemperature = EditorGUILayout.Foldout(showWinterTemperature,"Winter Temperature");
	if(showWinterTemperature){
		EditorGUI.indentLevel++;
		EditorGUILayout.Space();
		showWinterTemperatureSunRise = EditorGUILayout.Foldout(showWinterTemperatureSunRise,"SunRise Temperature");
		if(showWinterTemperatureSunRise){
			EditorGUILayout.PropertyField(winterSunRiseMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(winterSunRiseMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showWinterTemperatureDay = EditorGUILayout.Foldout(showWinterTemperatureDay,"Day Temperature");
		if(showWinterTemperatureDay){
			EditorGUILayout.PropertyField(winterDayMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(winterDayMax, GUILayout.ExpandWidth(false));
		}
		EditorGUILayout.Space();
		showWinterTemperatureSunSet = EditorGUILayout.Foldout(showWinterTemperatureSunSet,"SunSet Temperature");
		if(showWinterTemperatureSunSet){
			EditorGUILayout.PropertyField(winterSunSetMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(winterSunSetMax, GUILayout.ExpandWidth(false));
		}
		
		EditorGUILayout.Space();
		showWinterTemperatureNight = EditorGUILayout.Foldout(showWinterTemperatureNight,"Night Temperature");
		if(showWinterTemperatureNight){
			EditorGUILayout.PropertyField(winterNightMin, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(winterNightMax, GUILayout.ExpandWidth(false));
		}
		EditorGUI.indentLevel--;
	}
	EditorGUI.indentLevel--;
	}
	
	EditorGUILayout.Space();
			
		EditorGUILayout.LabelField(GUIContent(weather2),GUILayout.Width(150),GUILayout.Height(40));
		showWeather = EditorGUILayout.Foldout(showWeather,"Weather");
		if(showWeather){
		EditorGUI.indentLevel++;
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(rainProp);
		EditorGUILayout.PropertyField(rainblendTimeLightProp);
		EditorGUILayout.PropertyField(rainblendTimeHeavyProp);
		EditorGUI.indentLevel--;
		
		//Spring
		EditorGUI.indentLevel++;
		EditorGUILayout.Space();
		showSpringWeather = EditorGUILayout.Foldout(showSpringWeather,"Spring Weather");
		EditorGUILayout.Space();
		if(showSpringWeather){
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		EditorGUILayout.IntSlider(rainSpringPercentage, 0, 100);
		EditorGUILayout.IntSlider(rainSpringPercentageType, 0, 100);
			
		
			showSpringRain = EditorGUILayout.Foldout(showSpringRain,"Rain");
			if(showSpringRain){
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
	showLightRainSpring = EditorGUILayout.Foldout(showLightRainSpring,"Light Rain");
	
	 if(showLightRainSpring){
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(rainSpringLightMaxTemp);
			EditorGUILayout.PropertyField(rainSpringLightMinTemp);
			EditorGUILayout.PropertyField(rainSpringLightFade);
			
			EditorGUI.indentLevel++;
				 showLightRainSpringSunRise = EditorGUILayout.Foldout(showLightRainSpringSunRise,"SunRise");
					if(showLightRainSpringSunRise){
					EditorGUILayout.PropertyField(rainSpringLightSunRiseTop);
					EditorGUILayout.PropertyField(rainSpringLightSunRiseBottom);
					EditorGUILayout.PropertyField(rainSpringLightSunRiseAmbient);
					
		}
		EditorGUILayout.Space();
				 showLightRainSpringDay = EditorGUILayout.Foldout(showLightRainSpringDay,"Day");
					if(showLightRainSpringDay){
					EditorGUILayout.PropertyField(rainSpringLightDayTop);
					EditorGUILayout.PropertyField(rainSpringLightDayBottom);
					EditorGUILayout.PropertyField(rainSpringLightDayAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainSpringSunSet = EditorGUILayout.Foldout(showLightRainSpringSunSet,"SunSet");
					if(showLightRainSpringSunSet){
					
					EditorGUILayout.PropertyField(rainSpringLightSunSetTop);
					EditorGUILayout.PropertyField(rainSpringLightSunSetBottom);
					EditorGUILayout.PropertyField(rainSpringLightSunSetAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainSpringNight = EditorGUILayout.Foldout(showLightRainSpringNight,"Night");
					if(showLightRainSpringNight){
					
					EditorGUILayout.PropertyField(rainSpringLightNightTop);
					EditorGUILayout.PropertyField(rainSpringLightNightBottom);
					EditorGUILayout.PropertyField(rainSpringLightNightAmbient);
		}
		EditorGUI.indentLevel--;
		}
	EditorGUILayout.Space();
	showHeavyRainSpring = EditorGUILayout.Foldout(showHeavyRainSpring,"Heavy Rain");
	EditorGUILayout.Space();
	 if(showHeavyRainSpring){
	 
			 EditorGUILayout.PropertyField(rainSpringHeavyMaxTemp);
			EditorGUILayout.PropertyField(rainSpringHeavyMinTemp);
			EditorGUILayout.PropertyField(rainSpringHeavyFade);
	 
			EditorGUI.indentLevel++;
				 showHeavyRainSpringSunRise = EditorGUILayout.Foldout(showHeavyRainSpringSunRise,"SunRise");
					if(showHeavyRainSpringSunRise){
					
					EditorGUILayout.PropertyField(rainSpringHeavySunRiseTop);
					EditorGUILayout.PropertyField(rainSpringHeavySunRiseBottom);
					EditorGUILayout.PropertyField(rainSpringHeavySunRiseAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainSpringDay = EditorGUILayout.Foldout(showHeavyRainSpringDay,"Day");
					if(showHeavyRainSpringDay){
					
					EditorGUILayout.PropertyField(rainSpringHeavyDayTop);
					EditorGUILayout.PropertyField(rainSpringHeavyDayBottom);
					EditorGUILayout.PropertyField(rainSpringHeavyDayAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainSpringSunSet = EditorGUILayout.Foldout(showHeavyRainSpringSunSet,"SunSet");
					if(showHeavyRainSpringSunSet){
					
					EditorGUILayout.PropertyField(rainSpringHeavySunSetTop);
					EditorGUILayout.PropertyField(rainSpringHeavySunSetBottom);
					EditorGUILayout.PropertyField(rainSpringHeavySunSetAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainSpringNight = EditorGUILayout.Foldout(showHeavyRainSpringNight,"Night");
					if(showHeavyRainSpringNight){
					
					EditorGUILayout.PropertyField(rainSpringHeavyNightTop);
					EditorGUILayout.PropertyField(rainSpringHeavyNightBottom);
					EditorGUILayout.PropertyField(rainSpringHeavyNightAmbient);
					
		}
		EditorGUI.indentLevel--;
		}
		
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		}		
		
	//Summer
		
		EditorGUILayout.Space();
		showSummerWeather = EditorGUILayout.Foldout(showSummerWeather,"Summer Weather");
		EditorGUILayout.Space();
		if(showSummerWeather){
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			EditorGUILayout.IntSlider(rainSummerPercentage, 0, 100);
			EditorGUILayout.IntSlider(rainSummerPercentageType, 0, 100);
		showSummerRain = EditorGUILayout.Foldout(showSummerRain,"Rain");
		if(showSummerRain){
		EditorGUI.indentLevel++;
		EditorGUILayout.Space();
			showLightRainSummer = EditorGUILayout.Foldout(showLightRainSummer,"Light Rain");
			if(showLightRainSummer){
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(rainSummerLightMaxTemp);
			EditorGUILayout.PropertyField(rainSummerLightMinTemp);
			EditorGUILayout.PropertyField(rainSummerLightFade);
			
				EditorGUI.indentLevel++;
					 showLightRainSummerSunRise = EditorGUILayout.Foldout(showLightRainSummerSunRise,"SunRise");
					if(showLightRainSummerSunRise){
					
					EditorGUILayout.PropertyField(rainSummerLightSunRiseTop);
					EditorGUILayout.PropertyField(rainSummerLightSunRiseBottom);
					EditorGUILayout.PropertyField(rainSummerLightSunRiseAmbient);
		}
		
		EditorGUILayout.Space();
				 showLightRainSummerDay = EditorGUILayout.Foldout(showLightRainSummerDay,"Day");
					if(showLightRainSummerDay){
					
					EditorGUILayout.PropertyField(rainSummerLightDayTop);
					EditorGUILayout.PropertyField(rainSummerLightDayBottom);
					EditorGUILayout.PropertyField(rainSummerLightDayAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainSummerSunSet = EditorGUILayout.Foldout(showLightRainSummerSunSet,"SunSet");
					if(showLightRainSummerSunSet){
					
					EditorGUILayout.PropertyField(rainSummerLightSunSetTop);
					EditorGUILayout.PropertyField(rainSummerLightSunSetBottom);
					EditorGUILayout.PropertyField(rainSummerLightSunSetAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainSummerNight = EditorGUILayout.Foldout(showLightRainSummerNight,"Night");
					if(showLightRainSummerNight){
					
					EditorGUILayout.PropertyField(rainSummerLightNightTop);
					EditorGUILayout.PropertyField(rainSummerLightNightBottom);
					EditorGUILayout.PropertyField(rainSummerLightNightAmbient);
		}
		EditorGUI.indentLevel--;
		}
		
		
		EditorGUILayout.Space();
		showHeavyRainSummer = EditorGUILayout.Foldout(showHeavyRainSummer,"Heavy Rain");
			if(showHeavyRainSummer){
			
			EditorGUILayout.PropertyField(rainSummerHeavyMaxTemp);
			EditorGUILayout.PropertyField(rainSummerHeavyMinTemp);
			EditorGUILayout.PropertyField(rainSummerHeavyFade);
			
				EditorGUI.indentLevel++;
					 showHeavyRainSummerSunRise = EditorGUILayout.Foldout(showHeavyRainSummerSunRise,"SunRise");
					if(showHeavyRainSummerSunRise){
					
					EditorGUILayout.PropertyField(rainSummerHeavySunRiseTop);
					EditorGUILayout.PropertyField(rainSummerHeavySunRiseBottom);
					
		}
		
		EditorGUILayout.Space();
				 showHeavyRainSummerDay = EditorGUILayout.Foldout(showHeavyRainSummerDay,"Day");
					if(showHeavyRainSummerDay){
					
					EditorGUILayout.PropertyField(rainSummerHeavyDayTop);
					EditorGUILayout.PropertyField(rainSummerHeavyDayBottom);
					
		}
		EditorGUILayout.Space();
				 showHeavyRainSummerSunSet = EditorGUILayout.Foldout(showHeavyRainSummerSunSet,"SunSet");
					if(showHeavyRainSummerSunSet){
					
					EditorGUILayout.PropertyField(rainSummerHeavySunSetTop);
					EditorGUILayout.PropertyField(rainSummerHeavySunSetBottom);
					
		}
		EditorGUILayout.Space();
				 showHeavyRainSummerNight = EditorGUILayout.Foldout(showHeavyRainSummerNight,"Night");
					if(showHeavyRainSummerNight){
					
					EditorGUILayout.PropertyField(rainSummerHeavyNightTop);
					EditorGUILayout.PropertyField(rainSummerHeavyNightBottom);
					
		}
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		}
	//Auttum
	
		EditorGUILayout.Space();
		showAuttumWeather = EditorGUILayout.Foldout(showAuttumWeather,"Auttumn Weather");
		EditorGUILayout.Space();
		if(showAuttumWeather){
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			EditorGUILayout.IntSlider(rainAuttumPercentage, 0, 100);
			EditorGUILayout.IntSlider(rainAuttumPercentageType, 0, 100);
			
			showAuttumRain = EditorGUILayout.Foldout(showAuttumRain,"Rain");
			if(showAuttumRain){
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			showLightRainAuttum = EditorGUILayout.Foldout(showLightRainAuttum,"Light Rain");
			
			
		
			if(showLightRainAuttum){
			
			EditorGUILayout.PropertyField(rainAuttumLightMaxTemp);
			EditorGUILayout.PropertyField(rainAuttumLightMinTemp);
			EditorGUILayout.PropertyField(rainAuttumLightFade);
			
				EditorGUI.indentLevel++;
				 showLightRainAuttumSunRise = EditorGUILayout.Foldout(showLightRainAuttumSunRise,"SunRise");
					if(showLightRainAuttumSunRise){
					
					EditorGUILayout.PropertyField(rainAuttumLightSunRiseTop);
					EditorGUILayout.PropertyField(rainAuttumLightSunRiseBottom);
					EditorGUILayout.PropertyField(rainAuttumLightSunRiseAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainAuttumDay = EditorGUILayout.Foldout(showLightRainAuttumDay,"Day");
					if(showLightRainAuttumDay){
					
					EditorGUILayout.PropertyField(rainAuttumLightDayTop);
					EditorGUILayout.PropertyField(rainAuttumLightDayBottom);
					EditorGUILayout.PropertyField(rainAuttumLightDayAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainAuttumSunSet = EditorGUILayout.Foldout(showLightRainAuttumSunSet,"SunSet");
					if(showLightRainAuttumSunSet){
					
					EditorGUILayout.PropertyField(rainAuttumLightSunSetTop);
					EditorGUILayout.PropertyField(rainAuttumLightSunSetBottom);
					EditorGUILayout.PropertyField(rainAuttumLightSunSetAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainAuttumNight = EditorGUILayout.Foldout(showLightRainAuttumNight,"Night");
					if(showLightRainAuttumNight){
					
					EditorGUILayout.PropertyField(rainAuttumLightNightTop);
					EditorGUILayout.PropertyField(rainAuttumLightNightBottom);
					EditorGUILayout.PropertyField(rainAuttumLightNightAmbient);
					
		}
		EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space();
		showHeavyRainAuttum = EditorGUILayout.Foldout(showHeavyRainAuttum,"Heavy Rain");
			
		
			if(showHeavyRainAuttum){
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(rainAuttumHeavyMaxTemp);
			EditorGUILayout.PropertyField(rainAuttumHeavyMinTemp);
			EditorGUILayout.PropertyField(rainAuttumHeavyFade);
			
			EditorGUI.indentLevel++;
				 showHeavyRainAuttumSunRise = EditorGUILayout.Foldout(showHeavyRainAuttumSunRise,"SunRise");
					if(showHeavyRainAuttumSunRise){
					
					EditorGUILayout.PropertyField(rainAuttumHeavySunRiseTop);
					EditorGUILayout.PropertyField(rainAuttumHeavySunRiseBottom);
					EditorGUILayout.PropertyField(rainAuttumHeavySunRiseAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainAuttumDay = EditorGUILayout.Foldout(showHeavyRainAuttumDay,"Day");
 					if(showHeavyRainAuttumDay){
 	
 					EditorGUILayout.PropertyField(rainAuttumHeavyDayTop);
					EditorGUILayout.PropertyField(rainAuttumHeavyDayBottom);
					EditorGUILayout.PropertyField(rainAuttumHeavyDayAmbient);
	
		}
		EditorGUILayout.Space();
				 showHeavyRainAuttumSunSet = EditorGUILayout.Foldout(showHeavyRainAuttumSunSet,"SunSet");
					if(showHeavyRainAuttumSunSet){
					
					EditorGUILayout.PropertyField(rainAuttumHeavySunSetTop);
					EditorGUILayout.PropertyField(rainAuttumHeavySunSetBottom);
					EditorGUILayout.PropertyField(rainAuttumHeavySunSetAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainAuttumNight = EditorGUILayout.Foldout(showHeavyRainAuttumNight,"Night");
					if(showHeavyRainAuttumNight){
					
					EditorGUILayout.PropertyField(rainAuttumHeavyNightTop);
					EditorGUILayout.PropertyField(rainAuttumHeavyNightBottom);
					EditorGUILayout.PropertyField(rainAuttumHeavyNightAmbient);
		}
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		}
		
	//Winter
		EditorGUILayout.Space();
		showWinterWeather = EditorGUILayout.Foldout(showWinterWeather,"Winter Weather");
		EditorGUILayout.Space();
		if(showWinterWeather){
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			EditorGUILayout.IntSlider(rainWinterPercentage, 0, 100);
			EditorGUILayout.IntSlider(rainWinterPercentageType, 0, 100);
			showWinterRain = EditorGUILayout.Foldout(showWinterRain,"Rain");
			
			if(showWinterRain){
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
		showLightRainWinter = EditorGUILayout.Foldout(showLightRainWinter,"Light Rain");
			if(showLightRainWinter){
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(rainWinterLightMaxTemp);
			EditorGUILayout.PropertyField(rainWinterLightMinTemp);
			EditorGUILayout.PropertyField(rainWinterLightFade);
			
			EditorGUI.indentLevel++;
				 showLightRainWinterSunRise = EditorGUILayout.Foldout(showLightRainWinterSunRise,"SunRise");
					if(showLightRainWinterSunRise){
					
					EditorGUILayout.PropertyField(rainWinterLightSunRiseTop);
					EditorGUILayout.PropertyField(rainWinterLightSunRiseBottom);
					EditorGUILayout.PropertyField(rainWinterLightSunRiseAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainWinterDay = EditorGUILayout.Foldout(showLightRainWinterDay,"Day");
					if(showLightRainWinterDay){
					
					EditorGUILayout.PropertyField(rainWinterLightDayTop);
					EditorGUILayout.PropertyField(rainWinterLightDayBottom);
					EditorGUILayout.PropertyField(rainWinterLightDayAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainWinterSunSet = EditorGUILayout.Foldout(showLightRainWinterSunSet,"SunSet");
					if(showLightRainWinterSunSet){
					
					EditorGUILayout.PropertyField(rainWinterLightSunSetTop);
					EditorGUILayout.PropertyField(rainWinterLightSunSetBottom);
					EditorGUILayout.PropertyField(rainWinterLightSunSetAmbient);
		}
		EditorGUILayout.Space();
				 showLightRainWinterNight = EditorGUILayout.Foldout(showLightRainWinterNight,"Night");
					if(showLightRainWinterNight){
					
					EditorGUILayout.PropertyField(rainWinterLightNightTop);
					EditorGUILayout.PropertyField(rainWinterLightNightBottom);
					EditorGUILayout.PropertyField(rainWinterLightNightAmbient);
		}
		EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space();
		showHeavyRainWinter = EditorGUILayout.Foldout(showHeavyRainWinter,"Heavy Rain");
			if(showHeavyRainWinter){
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(rainWinterHeavyMaxTemp);
			EditorGUILayout.PropertyField(rainWinterHeavyMinTemp);
			EditorGUILayout.PropertyField(rainWinterHeavyFade);
			
			EditorGUI.indentLevel++;
				 showHeavyRainWinterSunRise = EditorGUILayout.Foldout(showHeavyRainWinterSunRise,"SunRise");
					if(showHeavyRainWinterSunRise){
					
					EditorGUILayout.PropertyField(rainWinterHeavySunRiseTop);
					EditorGUILayout.PropertyField(rainWinterHeavySunRiseBottom);
					EditorGUILayout.PropertyField(rainWinterHeavySunRiseAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainWinterDay = EditorGUILayout.Foldout(showHeavyRainWinterDay,"Day");
					if(showHeavyRainWinterDay){
					
					EditorGUILayout.PropertyField(rainWinterHeavyDayTop);
					EditorGUILayout.PropertyField(rainWinterHeavyDayBottom);
					EditorGUILayout.PropertyField(rainWinterHeavyDayAmbient);	
		}
		EditorGUILayout.Space();
				 showHeavyRainWinterSunSet = EditorGUILayout.Foldout(showHeavyRainWinterSunSet,"SunSet");
					if(showHeavyRainWinterSunSet){
					
					EditorGUILayout.PropertyField(rainWinterHeavySunSetTop);
					EditorGUILayout.PropertyField(rainWinterHeavySunSetBottom);
					EditorGUILayout.PropertyField(rainWinterHeavySunSetAmbient);
		}
		EditorGUILayout.Space();
				 showHeavyRainWinterNight = EditorGUILayout.Foldout(showHeavyRainWinterNight,"Night");
					if(showHeavyRainWinterNight){
					
					EditorGUILayout.PropertyField(rainWinterHeavyNightTop);
					EditorGUILayout.PropertyField(rainWinterHeavyNightBottom);
					EditorGUILayout.PropertyField(rainWinterHeavyNightAmbient);
		}
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;	
		}
		EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
	}
	

serializedObject.ApplyModifiedProperties();
}
///////END///////

function OnInspectorUpdate() {
this.Repaint();
}
}
