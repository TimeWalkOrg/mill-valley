
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Mega Wire/Hanger")]
public class MegaWireHanger : MonoBehaviour
{
	public MegaWire	wire;
	public float	alpha		= 0.0f;
	public int		strand		= 0;
	public float	offset		= 0.0f;
	public float	weight		= 1.0f;
	//public bool		snaptomass	= false;
	public bool		align		= false;
	public Vector3	rotate		= Vector3.zero;
	Quaternion		locrot		= Quaternion.identity;
	Matrix4x4		wtm;
	public Transform	parent;

	void LateUpdate()
	{
		if ( wire == null )
		{
			if ( parent != null )
				wire = parent.GetComponentInChildren<MegaWire>();
		}

		if ( wire )
		{
			Vector3 lpos = Vector3.zero;

			if ( alpha > 0.0f || alpha < 1.0f )
			{
				Vector3 rpos = wire.SetWeight(alpha, strand, weight, false);	//snaptomass);

				lpos.x = rpos.x;
				lpos.y = rpos.y + offset;
				lpos.z = rpos.z;

				Vector3 p = wire.transform.localToWorldMatrix.MultiplyPoint(lpos);
				transform.position = p;

				if ( align )
				{
					Vector3 p1 = wire.transform.localToWorldMatrix.MultiplyPoint(wire.SetWeight(alpha + 0.001f, strand, weight, false));
					Vector3 relativePos = p1 - p;

					Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
					locrot = rotation;

					wtm.SetTRS(p, rotation, Vector3.one);

					Quaternion r1 = Quaternion.Euler(rotate);
					transform.rotation = locrot * r1;
				}
			}
		}
	}
}