
#if false
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Mega Wire/Flow Wind")]
public class MegaWireFlowWind : MegaWireWind
{
	public MegaFlow	source;
	public int		frame;
	public float	flowscale = 1.0f;

	public override Vector3 Force(Vector3 pos)
	{
		Vector3 frc = base.Force(pos);

		if ( source )
		{
			source.SetMatrix();
			bool inbounds = false;
			frc += source.frames[frame].GetGridVelWorld(pos, ref inbounds) * flowscale * source.Scale;
		}

		return frc;
	}
}
#endif