
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Mega Wire/Attach")]
public class MegaWireAttach : MonoBehaviour
{
	public MegaWire	wire;
	public int		span		= 0;
	public int		connection	= 0;
	public float	alpha		= 0.0f;
	public Vector3	offset		= Vector3.zero;
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
			float alpha1 = Mathf.Clamp(alpha, 0.0f, 0.9999f);
			float fa = alpha1 * (float)wire.spans.Count;
			int sindex = (int)fa;
			float aa = fa - (float)sindex;

			MegaWireSpan span = wire.spans[sindex];
			MegaWireConnection con = span.connections[connection];

			Vector3 p = con.Interp(aa);

			transform.position = p + offset;

			if ( align )
			{
				Vector3 p1 = con.Interp(aa + 0.001f);
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