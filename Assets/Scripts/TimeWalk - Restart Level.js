	#pragma strict
	import UnityEngine.SceneManagement;
	function Update() {
		if(Input.GetKeyDown(KeyCode.R)) { // pressed the "R" restart level key
			// OLD Application.LoadLevel (0);
			SceneManager.LoadScene(0);
		}
	}