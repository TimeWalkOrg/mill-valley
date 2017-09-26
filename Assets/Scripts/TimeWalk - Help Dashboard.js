	// Toggle between Diffuse and Transparent/Diffuse shaders
	// when "C" key is pressed
	var shaderNormal : Shader;
	var shaderCommented : Shader;
	var colorNormal : Color = Color.white;
	var colorCommented : Color = Color.green;
	function Start() {
			GetComponent.<Renderer>().material.shader = shaderNormal;
			}
	function Update() {
		if(Input.GetKeyDown(KeyCode.C)) { // pressed the "C" comment key
			if( GetComponent.<Renderer>().material.shader == shaderNormal ){
				GetComponent.<Renderer>().material.shader = shaderCommented;
//				renderer.material.color = colorCommented;
				transform.GetComponent.<Renderer>().material.color = colorCommented;
				}
			else{
				GetComponent.<Renderer>().material.shader = shaderNormal;
				transform.GetComponent.<Renderer>().material.color = colorNormal;
				}
		}
	}