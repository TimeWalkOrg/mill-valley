var fadeTargetLevel : float; // How bright is default fade level?
var fadeWaitForSeconds : float; // How long to hold on photo before fading?
var fadeTargetTime : float; // How fast does scene fade?
private var fadeStarted : boolean = false; // Has the fade already happened?
var fadeStartLevel : float;


function Start()
	{
//		WaitForSeconds(fadeWaitForSeconds);
//		Fade(fadeTargetTime,0.0);
// print (TimeWalk_Controls.levelStartTime);
	GetComponent.<Renderer>().material.color.a = fadeStartLevel;
	}
function Update()
      {
          if(Time.timeSinceLevelLoad > fadeWaitForSeconds+TimeWalk_Controls.levelStartTime) // after fadeTargetTime, then fade...
          {
               Fade(fadeTargetTime,fadeTargetLevel);
               fadeStarted = true;
          }
//          if(Input.GetKeyDown(KeyCode.F))
//          {
//               Fade(fadeTargetTime,0.0);
//          }
//          if(Input.GetKeyDown(KeyCode.G))
//          {
//               Fade(2,1.0);
//          }
//           if(Input.GetKeyDown(KeyCode.H)) // make transparent
//          {
//               Fade(0,fadeTargetLevel);
//          }
      }

      function Fade(time: float, targetAlpha: float)
      {
           var t = 0.0;
           var currentAlpha = GetComponent.<Renderer>().material.color.a;
           while(t <= 1)
           {
               GetComponent.<Renderer>().material.color.a = Mathf.Lerp(currentAlpha, targetAlpha, t);
               t += Time.deltaTime/time;
               yield;
 
           }
           GetComponent.<Renderer>().material.color.a = targetAlpha;
      }