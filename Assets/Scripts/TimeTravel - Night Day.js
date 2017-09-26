var nowIsDay : boolean = true;
var dayMaterial:Material;
var nightMaterial:Material;
//var dayColor:Color;
//var nightColor:Color;

function Update () {
	if(Input.GetKeyDown(KeyCode.N)){
		if(nowIsDay==true){
			// Change the ambient lighting
//			RenderSettings.ambientLight = nightColor;
			RenderSettings.skybox=nightMaterial;
			nowIsDay = false;
		}
		else {
			RenderSettings.skybox=dayMaterial;
			// Change the ambient lighting
//			RenderSettings.ambientLight = dayColor;
			nowIsDay = true;
			}
	}
}