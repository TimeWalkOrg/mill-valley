var nowIsDay : boolean;

function Start() {
		for (var child : Transform in transform)
 			{
				child.gameObject.SetActive(!nowIsDay);
			}
		}

function Update () {
	if(Input.GetKeyDown(KeyCode.N)){
		if(nowIsDay==true){
			// Child objects are VISIBLE (i.e. SetActive)

 			for (var child : Transform in transform)
 				{
				child.gameObject.SetActive(true);
				}
			nowIsDay = false;
		}
		else {
			// Child objects are VISIBLE (i.e. SetActive)

 			for (var child : Transform in transform)
 				{
				child.gameObject.SetActive(false);
				}
			nowIsDay = true;
			}
	}
}