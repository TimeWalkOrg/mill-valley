var nowIsColor : boolean = true;
//
//
// Note that I put this toggle function directly into CC_Vintage.cs 
// since I couldn't get this Javascript to talk to that .cs script
// Yes, this is lame.
//
function Update () {
	if(Input.GetKeyDown(KeyCode.B)){
		if(nowIsColor==true){
		// http://docs.unity3d.com/412/Documentation/ScriptReference/index.Accessing_Other_Game_Objects.html
//			CC_Vintage.filter = Inkwell;
			nowIsColor = false;
//			filter = GameObject.Find("CC_Vintage").GetComponent("CC_Vintage.cs").filter;
//			GameObject.Find("CC_Vintage").GetComponent("CC_Vintage.cs").filter = filter.Inkwell;


		}
		else {
			nowIsColor = true;
//			filter = GameObject.Find("CC_Vintage").GetComponent("CC_Vintage.cs").filter;
//			GameObject.Find("CC_Vintage").GetComponent("CC_Vintage.cs").filter = filter.None;

			}
	}
}