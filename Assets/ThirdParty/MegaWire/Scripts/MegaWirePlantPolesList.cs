
using UnityEngine;
using System.Collections.Generic;

// TODO: random rotates, scale, spacing - DONE
// TODO: drop poles onto whatever is below them - DONE
// TODO: plant in line between game objects list
// TODO: option to wire up poles - DONE
// TODO: reverse poles - DONE
// TODO: editor script - DONE
// TODO: version for gameobject way points
// TODO: close up wires if length 1 and closed - DONE
// TODO: offset from spline - DONE

public enum MegaWireGizmoType
{
	Waypoint,
	Pole,
	Both,
}

public enum MegaWireUnits
{
	Meters,
	Centimeters,
	Feet,
	Inches,
	Yards,
}

[ExecuteInEditMode]
[AddComponentMenu("Mega Wire/Plant Poles List")]
public class MegaWirePlantPolesList : MonoBehaviour
{
	public List<Vector3>		waypoints = new List<Vector3>();
	public bool					closed = false;
	public int					curve = 0;
	public float				start = 0.0f;
	public float				length = 1.0f;
	public float				spacing = 10.0f;
	public bool					update = false;
	public GameObject			pole;
	public Vector3				rotate = Vector3.zero;
	public float				offset = 0.0f;
	public bool					conform = true;
	public float				upright = 0.0f;
	public bool					addwires = false;
	public List<GameObject>		poles = new List<GameObject>();
	public MegaWire				copyfrom = null;
	public Material				material;
	public Vector3				positionVariation = Vector3.zero;
	public Vector3				rotateVariation = Vector3.zero;
	public Vector3				scaleVariation = Vector3.zero;
	public float				spacingVariation = 0.0f;
	public bool					reverseWire = false;
	public int					seed = 0;
	public float				wireSizeMult = 1.0f;
	public float				stretch = 1.0f;
	public bool					showgizmo = true;
	public bool					showgizmoparams = false;
	public MegaWireGizmoType	gizmoType = MegaWireGizmoType.Waypoint;
	public float				arrowwidth = 0.2f;
	public float				arrowlength = 1.1f;
	public float				vertStart = 0.2f;
	public float				vertLength = 1.5f;
	public float				arrowoff = 0.8f;
	public float				dashdist = 2.0f;

	public Color				arrowCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color				lineCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color				otherCol = new Color(0.75f, 0.75f, 0.75f, 1.0f);
	public Color				dashCol = new Color(0.5f, 0.5f, 0.5f, 1.0f);

	public MegaWireUnits		units = MegaWireUnits.Meters;
	public float				unitsScale = 1.0f;

	[System.Serializable]
	public class MegaWireSection
	{
		public Vector3	p;
		public Vector3	p1;
		public float	length;
		public float	seglength;
	}

	List<MegaWireSection>	knots = new List<MegaWireSection>();
	float pathlength = 0.0f;

	public Vector3 Interpolate(float alpha)
	{
		int	seg = 0;

		float dist = alpha * pathlength;

		for ( seg = 0; seg < knots.Count; seg++ )
		{
			if ( dist <= knots[seg].length )
				break;
		}

		alpha = 1.0f - ((knots[seg].length - dist) / knots[seg].seglength);

		if ( seg < knots.Count )
			return Vector3.Lerp(knots[seg].p, knots[seg].p1, alpha);
		else
			return Vector3.Lerp(knots[seg].p, knots[seg].p1, alpha);
	}

	public Vector3 InterpCurve3D(float alpha)
	{
		if ( alpha < 0.0f )
		{
			if ( closed )
				alpha = Mathf.Repeat(alpha, 1.0f);
			else
			{
				Vector3 ps = Interpolate(0.0f);
				Vector3 ps1 = Interpolate(0.01f);

				Vector3	delta = ps1 - ps;
				delta.Normalize();
				return ps + ((pathlength * alpha) * delta);
			}
		}
		else
		{
			if ( alpha >= 1.0f )
			{
				if ( closed )
					alpha = alpha % 1.0f;
				else
				{
					Vector3 ps = Interpolate(1.0f);
					Vector3 ps1 = Interpolate(0.99f);

					Vector3	delta = ps1 - ps;
					delta.Normalize();
					return ps + ((pathlength * (1.0f - alpha)) * delta);
				}
			}
		}
		return Interpolate(alpha);
	}

	float CalcLength()
	{
		float length = 0.0f;

		knots.Clear();

		if ( waypoints.Count > 1 )
		{
			for ( int i = 0; i < waypoints.Count - 1; i++ )
			{
				Vector3 p = transform.TransformPoint(waypoints[i]);
				Vector3 np = transform.TransformPoint(waypoints[i + 1]);

				MegaWireSection k = new MegaWireSection();

				k.seglength = Vector3.Distance(np, p);

				k.p = p;
				k.p1 = np;
				length += k.seglength;
				k.length = length;

				knots.Add(k);
			}

			if ( closed )
			{
				Vector3 p = transform.TransformPoint(waypoints[waypoints.Count - 1]);
				Vector3 np = transform.TransformPoint(waypoints[0]);

				MegaWireSection k = new MegaWireSection();

				k.seglength = Vector3.Distance(np, p);

				k.p = p;
				k.p1 = np;
				length += k.seglength;
				k.length = length;

				knots.Add(k);
			}
		}

		pathlength = length;
		return length;
	}

	public void Rebuild()
	{
		CalcLength();
		if ( waypoints.Count > 1 && pole )
		{
			update = false;

			//GameObject oldwire = null;
			MegaWire oldwire = transform.GetComponentInChildren<MegaWire>();
			// Destroy all children
			//MegaWireConnectionHelper[] spans = transform.GetComponentsInChildren<MegaWireConnectionHelper>();

			//if ( true )	//spans.Length == 0 )
			{
				Transform[] spans2 = transform.GetComponentsInChildren<Transform>();

				for ( int i = 0; i < spans2.Length; i++ )
				{
					if ( spans2[i] != null && spans2[i] != transform && spans2[i].name.Contains(gameObject.name) )
					{
						if ( Application.isEditor )
							DestroyImmediate(spans2[i].gameObject);
						else
							Destroy(spans2[i].gameObject);
					}
				}
			}
#if false
			else
			{
				for ( int i = 0; i < spans.Length; i++ )
				{
					if ( Application.isEditor )
						DestroyImmediate(spans[i].gameObject);
					else
						Destroy(spans[i].gameObject);
				}

				MegaWireSpan[] spans1 = transform.GetComponentsInChildren<MegaWireSpan>();

				for ( int i = 0; i < spans1.Length; i++ )
				{
					if ( Application.isEditor )
						DestroyImmediate(spans1[i].gameObject);
					else
						Destroy(spans1[i].gameObject);
				}
			}
#endif
#if false
			while ( transform.childCount > 0 )
			{
				GameObject go = transform.GetChild(0).gameObject;

				// Dont destroy wire object
				MegaWire wire = go.GetComponent<MegaWire>();

				if ( !wire )
				{
					if ( Application.isEditor )
						DestroyImmediate(go);
					else
						Destroy(go);
				}
				else
					oldwire = go;
			}
#endif

			int polecount = (int)((pathlength * length) / spacing);

			float alpha = start;
			float da = length / polecount;

			if ( closed )
				polecount--;

			poles.Clear();

#if UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
			Random.InitState(seed);
#else
			Random.seed = seed;
#endif

			float spacealpha = ((spacing / (pathlength * length)) / 2.0f) * spacingVariation;

			for ( int i = 0; i <= polecount; i++ )
			{
				float a = alpha;

				if ( i != 0 && i != polecount )
					a = alpha + (Random.Range(-1.0f, 1.0f) * spacealpha);

				Vector3 p = InterpCurve3D(a);
				Vector3 p1 = InterpCurve3D(a + 0.001f);

				Vector3 poff = Vector3.zero;
				poff.y = Random.Range(0.0f, 1.0f) * positionVariation.y;
				poff.z = Random.Range(-1.0f, 1.0f) * positionVariation.z;

				Vector3 dir = (p1 - p).normalized;

				Vector3 outline = Vector3.Cross(dir, Vector3.up) * (offset + poff.z);

				p += outline;
				p1 += outline;

				Quaternion hitrot = Quaternion.identity;

				if ( conform )
				{
					Ray ray = new Ray();
					Vector3 origin = p;
					origin.y = 1000.0f;

					ray.origin = origin;
					ray.direction = Vector3.down;

					RaycastHit[] hits = Physics.RaycastAll(ray);

					if ( hits.Length > 0 )
					{
						int hindex = 0;
						p = hits[hindex].point;

						for ( int j = 1; j < hits.Length; j++ )
						{
							if ( hits[j].point.y > p.y )
							{
								hindex = j;
								p = hits[hindex].point;
							}
						}

						Vector3 norm1 = Vector3.Lerp(hits[hindex].normal, Vector3.up, upright).normalized;
						hitrot = Quaternion.FromToRotation(Vector3.up, norm1);
					}
				}
				else
				{
				}

				Vector3 relativePos = p1 - p;

				relativePos.y = 0.0f;

				Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);

				Vector3 rrot = Vector3.zero;
				rrot.x = Random.Range(-1.0f, 1.0f) * rotateVariation.x;
				rrot.y = Random.Range(-1.0f, 1.0f) * rotateVariation.y;
				rrot.z = Random.Range(-1.0f, 1.0f) * rotateVariation.z;

				Quaternion erot = Quaternion.Euler(rotate + rrot);

				p.y -= poff.y;

				GameObject go = (GameObject)GameObject.Instantiate(pole, p, hitrot * rot * erot);	//rot * hitrot * erot);
				go.name = name + " Pole " + i;

				go.transform.parent = transform;

				alpha += da;
				poles.Add(go);
			}

			if ( length >= 0.99999f && closed )
				poles.Add(poles[0]);

			if ( addwires )
			{
				if ( reverseWire )
					poles.Reverse();

				MegaWire wire = MegaWire.Create(oldwire, poles, material, name + " Wires", copyfrom, wireSizeMult, stretch);
				if ( wire )
					wire.transform.parent = transform;
			}
			else
			{
				if ( oldwire )
				{
					if ( Application.isPlaying )
					{
						Destroy(oldwire);	
					}
					else
						DestroyImmediate(oldwire);
					oldwire = null;
				}
			}
		}
	}
}
