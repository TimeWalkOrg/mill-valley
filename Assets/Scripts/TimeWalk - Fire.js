	var isVisible : boolean;
//	public var fireHappening: boolean = false;

	function Start() {
		if (!isVisible){
		for (var child : Transform in transform)
 				{
				child.gameObject.SetActive(false);
				}
			}
}
	function Update() {
	if(Input.GetKeyDown(KeyCode.F)) {
				
		if(isVisible){ // Object is already VISIBLE
			// Hide child objects (i.e. SetActive)

 			for (var child : Transform in transform)
 				{
				child.gameObject.SetActive(false);
				isVisible = false;
//				fireHappening = false;
				}
		}
		else {
			// Show child objects (i.e. SetActive)

				for (var child : Transform in transform)
 				{
				child.gameObject.SetActive(true);
				isVisible = true;	
//				fireHappening = true;
				}
			}	
		}	

	}