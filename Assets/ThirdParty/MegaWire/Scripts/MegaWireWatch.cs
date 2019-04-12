
using UnityEngine;

[AddComponentMenu("Mega Wire/Watch")]
public class MegaWireWatch : MonoBehaviour
{
	public bool	realtime = false;
	public GameObject	watch;
	public Matrix4x4	mat;

	void Start()
	{
		if ( watch )
		{
			mat = watch.transform.localToWorldMatrix;
		}
	}

	void Update()
	{
		if ( realtime && Application.isPlaying && watch )
		{
			if ( mat != watch.transform.localToWorldMatrix )
			{
				mat = watch.transform.localToWorldMatrix;
				MegaWirePlantPolesList plant = GetComponent<MegaWirePlantPolesList>();
				
				if ( plant )
					plant.Rebuild();
			}
		}
	}

}