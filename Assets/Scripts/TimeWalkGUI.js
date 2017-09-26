var whenString : String; // when are we (e.g. "December 4, 1915")
var whereString : String; // where are we (e.g. "Mill Valley, California")
var songString : String; // name of currently playing song
var help1String : String; // Keyboard shortcuts
var help2String : String; // Keyboard shortcuts
//private var lastInterval : double; // Last interval end time
var secondsBeforeGUI : float;
var secondsbeforeSongTitle : float;
var secondsofSongTitle : float;
	var fontForGUI : Font;

//function GUIKeyDown(key : KeyCode) : boolean
//{
//    if (Event.current.type == EventType.KeyDown)
//        return (Event.current.keyCode == key);
//    return false;
//}
function OnGUI() {
	if(Time.timeSinceLevelLoad > secondsBeforeGUI){
		GUI.skin.font = fontForGUI;
		GUI.contentColor = Color.white;
	   var timeString = System.DateTime.Now.ToString("h:mm:ss tt");
//	   var dateString = System.DateTime.Now.ToString("MM/dd/yyyy");
	   var dateString = whenString;

	   var timeRect = new Rect(20,20,400,100);
	   var dateRect = new Rect(20,40,400,100);
	   var whereRect =  new Rect(20,60,400,100);
	   var songRect =  new Rect(20,660,600,100);
	   var help1Rect =  new Rect(20,600,800,100);
	   var help2Rect =  new Rect(20,620,800,100);


	   GUI.Label(timeRect, timeString);
	   GUI.Label(dateRect, dateString);
//	   		if (GUIKeyDown(KeyCode.H)) {// the "W" key
//	   					GUI.Label(helpRect, helpString);
//	   					}
	   GUI.Label(whereRect, whereString);
//	   	if(Input.GetKeyDown(KeyCode.H)){ // User wants help
//			GUI.Label(helpRect, helpString);
//			}
	   	if((Time.timeSinceLevelLoad > secondsbeforeSongTitle) && (Time.timeSinceLevelLoad < secondsbeforeSongTitle + secondsofSongTitle)){ 	
				GUI.Label(songRect, songString);	
				GUI.Label(help1Rect, help1String);	
				GUI.Label(help2Rect, help2String);
				}
		}
   }