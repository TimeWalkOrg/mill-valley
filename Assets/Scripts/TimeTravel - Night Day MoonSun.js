	var nowIsDay : boolean;
	var dayMaterial:Material;
	var nightMaterial:Material;
	var colorMoon : Color;
	var colorSun : Color;
	var colorSkyColorDay : Color;
	var colorSkyColorNight : Color;
	var dayIntensity : float = 0.4f;
	var nightIntensity : float = 0.1f;
	var moonlightOnObject: GameObject;
function Update () {
	if(Input.GetKeyDown(KeyCode.N)){
		if(nowIsDay==true){
			RenderSettings.skybox=nightMaterial;
			RenderSettings.ambientIntensity=nightIntensity;
			RenderSettings.ambientLight = colorSkyColorNight;
			GetComponent.<Light>().color = colorMoon;
			nowIsDay = false;
		}
		else {
			RenderSettings.skybox=dayMaterial;
			RenderSettings.ambientLight = colorSkyColorDay;
			GetComponent.<Light>().color = colorSun;
			RenderSettings.ambientIntensity=dayIntensity;
			nowIsDay = true;
			}
	}

}