	var yearBuilt : int;
	var yearReplaced : int;
	private var lastYearDisplayed : int = 0;
	private var currentYearNowValue: int;

function Update ()
{
	// Hack until replaced with c#
	currentYearNowValue = 1920;
	//if (TimeWalk_Controls.yearNowValue == 0)
	//{
	//	currentYearNowValue = 1920;
	//}
	//else
	//{
	//	currentYearNowValue = TimeWalk_Controls.yearNowValue;
	//}
/*
	if(timeWalkControlsNoUI.yearNowValue == 0){
		currentYearNowValue = 1920;
	} else {
		currentYearNowValue = timeWalkControlsNoUI.yearNowValue;
	}
*/
//    Debug.Log(TimeWalk_Controls.yearNowValue.ToString ()); //TimeWalk_Controls.yearNowValue
	if (lastYearDisplayed != currentYearNowValue) // if year has changed, then...
	{
		if((currentYearNowValue >= yearBuilt) && (currentYearNowValue < yearReplaced)) // Object is VISIBLE
		{
			// Child objects are VISIBLE (i.e. SetActive)

 			for (var child : Transform in transform)
 			{
				child.gameObject.SetActive(true);
			}
		}
		else
		{
			// Child objects are NOT visible
			for (var child : Transform in transform)
 			{
				child.gameObject.SetActive(false);
			}
		}
		lastYearDisplayed = currentYearNowValue; // update lastYearDisplayed to the current TimeWalk date
	}
}