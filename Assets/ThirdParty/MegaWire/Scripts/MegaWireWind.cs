 
using UnityEngine;

public enum MegaWindType
{
	Planar,
	Spherical,
}

// TODO: add noise to strength and general direction - DONE
[ExecuteInEditMode]
[AddComponentMenu("Mega Wire/Wind")]
public class MegaWireWind : MonoBehaviour
{
	public float		decay			= 0.0f;
	public float		turb			= 0.0f;
	public float		strength		= 4.0f;

	public MegaWindType type			= MegaWindType.Planar;
	public float		freq			= 0.0f;
	public float		scale			= 1.0f;
	public Vector3		force			= Vector3.zero;

	MegaWirePerlin		iperlin			= MegaWirePerlin.Instance;

	static float		forceScaleFactor = (1200.0f * 1200.0f) / (30.0f * 30.0f);

	public Vector3		gizmoSize		= new Vector2(10.0f, 10.0f);
	//public float		gizmosize		= 10.0f;
	public int			divs			= 10;
	public bool			displayGizmo	= true;
	public float		gizscale		= 1.0f;
	public Vector3		gizmopos		= Vector3.zero;
	public Color		gizmocol		= Color.white;

	// Direction and strength noise
	public bool			dirnoise		= false;
	public float		dirfreq			= 1.0f;
	public float		dirphase		= 0.0f;
	public Vector3		dirscale		= Vector3.zero;

	public bool			strengthnoise	= false;
	public float		strengthfreq	= 1.0f;
	public float		strengthphase	= 0.0f;
	public float		strengthscale	= 0.0f;

	public Vector3		dir				= Vector3.zero;
	public float		t				= 0.0f;
	Vector3				rotseed			= new Vector3(2.0f, 4.0f, 6.0f);
	float				strengthval		= 0.0f;
	Vector3				dirval			= Vector3.zero;

	float RTurbulence(Vector3 p, float freq)
	{
		Vector3 v = p * freq;
		return iperlin.Noise(v.x, v.y, v.z);
	}

	public virtual Vector3 Force(Vector3 pos)
	{
		if ( decay < 0.0f )
			decay = 0.0f;

		float cstr = strength + strengthval;

		Vector3 rot = dir + dirval;

		float elevation = Mathf.Deg2Rad * rot.x;
		float heading = Mathf.Deg2Rad * rot.y;

		Vector3 forward = new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));

		if ( type == MegaWindType.Planar )
		{
			force = forward;
			if ( decay != 0.0f )
			{
				float dist = Mathf.Abs(Vector3.Dot(force, pos - transform.position));
				cstr *= Mathf.Exp(-decay * dist);
			}
			force *= cstr * 0.0001f * forceScaleFactor;
		}
		else
		{
			float dist;
			force = pos - transform.position;
			dist  = Vector3.Magnitude(force);

			if ( dist != 0.0f )
				force /= dist;

			if ( decay != 0.0f )
				cstr *= Mathf.Exp(-decay * dist);

			force *= cstr * 0.0001f * forceScaleFactor;
		}

		float cturb = turb;
		float cfreq = freq;

		if ( cturb != 0.0f )
		{
			Vector3 tf = Vector3.zero;
			Vector3 pt = pos - transform.position;
			cfreq *= 0.01f;
			cturb *= 0.0001f * forceScaleFactor;

			Vector3 p = pt;
			p.x  = cfreq * t;
			tf.x = RTurbulence(p, scale);
			p    = pt;
			p.y  = cfreq * t;
			tf.y = RTurbulence(p, scale);
			p    = pt;
			p.z  = cfreq * t;
			tf.z = RTurbulence(p, scale);

			return force + (cturb * tf);
		}
		else
			return force;
	}

	void Update()
	{
		t += Time.deltaTime;

		if ( strengthnoise )
		{
			strengthphase += Time.deltaTime * strengthfreq;
			strengthval = iperlin.Noise(2.0f, 3.0f, strengthphase) * strengthscale;
		}

		if ( dirnoise )
		{
			dirphase += Time.deltaTime * dirfreq;

			Vector3 d = Vector3.zero;
			Vector3 sp = rotseed;
			d.x = iperlin.Noise(sp.y, sp.z, dirphase);
			d.y = iperlin.Noise(sp.x, sp.z, dirphase);
			d.z = iperlin.Noise(sp.x, sp.y, dirphase);

			dirval = Vector3.Scale(d, dirscale);
		}
	}
}