var hideTargetTime : float; // when does the object disappear?
function Start()
	{
//		WaitForSeconds(hideTargetTime);
        Destroy(gameObject,hideTargetTime);
	}