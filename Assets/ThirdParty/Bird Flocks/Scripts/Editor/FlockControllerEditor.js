/****************************************
	FlockController Editor	
	Copyright Unluck Software	
 	www.chemicalbliss.com																															
*****************************************/
@CustomEditor (FlockController)
@CanEditMultipleObjects

class FlockControllerEditor extends Editor {
	var avoidanceMask: SerializedProperty;	
	
	
	function OnEnable(){
		avoidanceMask= serializedObject.FindProperty("_avoidanceMask");
		
		//Fix upgrading older version
		if(target._positionSphereDepth == -1){
			target._positionSphereDepth = target._positionSphere;
		}
		if(target._spawnSphereDepth == -1){
			target._spawnSphereDepth = target._spawnSphere;
		}
	}
	
    function OnInspectorGUI () {	
    	var warningColor: Color = Color32(255, 174, 0, 255);
		var warningColor2: Color = Color.yellow;
		var dColor: Color = Color32(175, 175, 175, 255);
		var aColor: Color = Color.white;
		var warningStyle = new GUIStyle(GUI.skin.label);
		warningStyle.normal.textColor = warningColor;
		warningStyle.fontStyle = FontStyle.Bold;
		var warningStyle2 = new GUIStyle(GUI.skin.label);
		warningStyle2.normal.textColor = warningColor2;
		warningStyle2.fontStyle = FontStyle.Bold;

    	GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		if(UnityEditor.EditorApplication.isPlaying)
		{
			GUI.enabled = false;
		}
		target._updateDivisor = EditorGUILayout.Slider("Frame Skipping", target._updateDivisor, 1, 10);
		GUI.enabled = true;
		if(target._updateDivisor > 4)
		{
			EditorGUILayout.LabelField("Will cause choppy movement", warningStyle);
		}
		else if(target._updateDivisor > 2)
		{
			EditorGUILayout.LabelField("Can cause choppy movement	", warningStyle2);
		}
		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
    	target._childPrefab = EditorGUILayout.ObjectField("Bird Prefab", target._childPrefab, typeof(FlockChild),false) as FlockChild;
    	EditorGUILayout.LabelField("Drag & Drop bird prefab from project folder", EditorStyles.miniLabel); 
    	
    	
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
    	
    	EditorGUILayout.LabelField("Roaming Area", EditorStyles.boldLabel);

    	target._positionSphere = EditorGUILayout.IntField("Roaming Area Width" , target._positionSphere);
    	if(target._positionSphere < 0)
    	target._positionSphere = 0;
    	target._positionSphereDepth = EditorGUILayout.IntField("Roaming Area Depth" , target._positionSphereDepth);
    	if(target._positionSphereDepth < 0)
    	target._positionSphereDepth = 0;
    	target._positionSphereHeight = EditorGUILayout.IntField("Roaming Area Height" , target._positionSphereHeight);
    	if(target._positionSphereHeight < 0)
    	target._positionSphereHeight = 0;
///GROUPING
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
    	
    	EditorGUILayout.LabelField("Grouping", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Move birds into a parent transform", EditorStyles.miniLabel);
		target._groupChildToFlock = EditorGUILayout.Toggle("Group to Flock", target._groupChildToFlock);
		if(target._groupChildToFlock)
		{
			GUI.enabled = false;
		}
		target._groupChildToNewTransform = EditorGUILayout.Toggle("Group to New GameObject", target._groupChildToNewTransform);
		target._groupName = EditorGUILayout.TextField("Group Name", target._groupName);
		GUI.enabled = true;
    	
    	
		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
///FLOCK
    	EditorGUILayout.LabelField("Size of the flock", EditorStyles.boldLabel);
    	
    	target._childAmount = EditorGUILayout.Slider("Bird Amount", target._childAmount, 0,999);
    	target._spawnSphere = EditorGUILayout.FloatField("Flock Width" , target._spawnSphere);
    	if(target._spawnSphere < 1)
    	target._spawnSphere = 1;
    	target._spawnSphereDepth = EditorGUILayout.FloatField("Flock Depth" , target._spawnSphereDepth);
    	if(target._spawnSphereDepth < 1)
    	target._spawnSphereDepth = 1;
    	target._spawnSphereHeight = EditorGUILayout.FloatField("Flock Height" , target._spawnSphereHeight);
    	if(target._spawnSphereHeight < 1)
    	target._spawnSphereHeight = 1;
    	target._startPosOffset = EditorGUILayout.Vector3Field("Start Position Offset", target._startPosOffset);
    	target._slowSpawn = EditorGUILayout.Toggle("Slowly Spawn Birds" , target._slowSpawn);
    	
    	
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
///BEHAVIOR		
    	EditorGUILayout.LabelField("Behaviors and Appearance", EditorStyles.boldLabel); 
    	EditorGUILayout.LabelField("Change how the birds move and behave", EditorStyles.miniLabel);
    	target._minSpeed = EditorGUILayout.FloatField("Birds Min Speed" , target._minSpeed);
    	target._maxSpeed = EditorGUILayout.FloatField("Birds Max Speed" , target._maxSpeed);
    	target._diveValue = EditorGUILayout.FloatField("Birds Dive Depth" , target._diveValue);  	
    	target._diveFrequency = EditorGUILayout.Slider("Birds Dive Chance" , target._diveFrequency, 0.0, .7);
    	target._soarFrequency = EditorGUILayout.Slider("Birds Soar Chance" , target._soarFrequency, 0.0, 1.0);
    	target._soarMaxTime = EditorGUILayout.FloatField("Soar Time (0 = Always)" , target._soarMaxTime);
    	
    	

		
		
    	target._minDamping = EditorGUILayout.FloatField("Min Damping Turns" , target._minDamping); 	
    	target._maxDamping = EditorGUILayout.FloatField("Max Damping Turns" , target._maxDamping);
    	EditorGUILayout.LabelField("Bigger number = faster turns", EditorStyles.miniLabel);  
    	
    	

    	
    	
    	target._minScale = EditorGUILayout.FloatField("Birds Min Scale" , target._minScale);
    	target._maxScale = EditorGUILayout.FloatField("Birds Max Scale" , target._maxScale);
    	EditorGUILayout.LabelField("Randomize size of birds when added", EditorStyles.miniLabel);
    	
    	
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
		
    	EditorGUILayout.LabelField("Disable Pitch Rotation", EditorStyles.boldLabel);
    	EditorGUILayout.LabelField("Flattens out rotation when flying or soaring upwards", EditorStyles.miniLabel);   	
    	target._flatSoar = EditorGUILayout.Toggle("Flat Soar" , target._flatSoar);
		target._flatFly = EditorGUILayout.Toggle("Flat Fly" , target._flatFly);
 		
 		
 		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
		
    	EditorGUILayout.LabelField("Animations", EditorStyles.boldLabel);
    	target._soarAnimation = EditorGUILayout.TextField("Soar Animation", target._soarAnimation);
    	target._flapAnimation = EditorGUILayout.TextField("Flap Animation", target._flapAnimation);
    	target._idleAnimation = EditorGUILayout.TextField("Idle Animation", target._idleAnimation);
    	target._minAnimationSpeed = EditorGUILayout.FloatField("Min Anim Speed" , target._minAnimationSpeed);
    	target._maxAnimationSpeed = EditorGUILayout.FloatField("Max Anim Speed" , target._maxAnimationSpeed);  	
		
		
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
		
    	EditorGUILayout.LabelField("Bird Trigger Flock Waypoint", EditorStyles.boldLabel);
    	EditorGUILayout.LabelField("Birds own waypoit triggers a new flock waypoint", EditorStyles.miniLabel);
    	target._childTriggerPos = EditorGUILayout.Toggle("Bird Trigger Waypoint" , target._childTriggerPos);
    	target._waypointDistance = EditorGUILayout.FloatField("Distance To Waypoint" , target._waypointDistance);
    	
    	
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
		
		EditorGUILayout.LabelField("Automatic Flock Waypoint", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Automaticly change the flock waypoint (0 = never)", EditorStyles.miniLabel);
		target._randomPositionTimer = EditorGUILayout.FloatField("Auto Waypoint Delay" , target._randomPositionTimer);
		if(target._randomPositionTimer < 0){
			target._randomPositionTimer = 0;
		}
		
		
    	EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
		
    	EditorGUILayout.LabelField("Force Bird Waypoints", EditorStyles.boldLabel);
    	EditorGUILayout.LabelField("Force all birds to change waypoints when flock changes waypoint", EditorStyles.miniLabel);
		target._forceChildWaypoints = EditorGUILayout.Toggle("Force Bird Waypoints" , target._forceChildWaypoints);
		target._forcedRandomDelay = EditorGUILayout.IntField("Bird Waypoint Delay" , target._forcedRandomDelay);
		
		
		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = Color.white;
		
		
		EditorGUILayout.LabelField("Avoidance", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Birds will steer away from colliders (Ray)", EditorStyles.miniLabel);
		target._birdAvoid = EditorGUILayout.Toggle("Bird Avoid" , target._birdAvoid);
		if(target._birdAvoid){
			EditorGUILayout.PropertyField(avoidanceMask, new GUIContent("Collider Mask"));
			
			target._birdAvoidHorizontalForce = EditorGUILayout.FloatField("Avoid Horizontal Force" , target._birdAvoidHorizontalForce);
						
			var minVal : float = target._birdAvoidDistanceMin;
			var minLimit : float = .5;
			var maxVal : float = target._birdAvoidDistanceMax;
			var maxLimit : float = 8;
			
			EditorGUILayout.LabelField("Min Avoid Distance:", minVal.ToString());
			EditorGUILayout.LabelField("Max Avoid Distance:", maxVal.ToString());
			
			EditorGUILayout.MinMaxSlider(minVal, maxVal, minLimit, maxLimit);
			target._birdAvoidDistanceMin = minVal;
			target._birdAvoidDistanceMax = maxVal;
			
			target._birdAvoidDown = EditorGUILayout.Toggle("Bird Avoid Up" , target._birdAvoidDown);
			target._birdAvoidUp = EditorGUILayout.Toggle("Bird Avoid Down" , target._birdAvoidUp);
			if(target._birdAvoidDown || target._birdAvoidUp)
				target._birdAvoidVerticalForce = EditorGUILayout.FloatField("Avoid Vertical Force" , target._birdAvoidVerticalForce);
		}
		EditorGUILayout.EndVertical();

		if(target._forcedRandomDelay < 0){
			target._forcedRandomDelay = 0;
		}	
        if (GUI.changed)
            EditorUtility.SetDirty (target);
    }
}