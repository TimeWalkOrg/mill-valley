/****************************************
	Copyright Unluck Software	
 	www.chemicalbliss.com																															
*****************************************/
@CustomEditor (FlockChild)

class FlockChildEditor extends Editor {
    function OnInspectorGUI () {
    	DrawDefaultInspector();
    	if(!target._thisT || !target._model || !target._modelT){
    		EditorGUILayout.LabelField("Find and fill empty variables", EditorStyles.boldLabel); 
			if(GUILayout.Button("Click Me! ")) {
			 	target.FindRequiredComponents();
			}
		}
		if (GUI.changed)	EditorUtility.SetDirty (target);
    }
}