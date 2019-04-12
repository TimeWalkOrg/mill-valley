
using UnityEngine;
using System.Collections.Generic;

// If you have MegaShapes you can set false to true to use splines to define pole paths
#if false

// TODO: random rotates, scale, spacing - DONE
// TODO: drop poles onto whatever is below them - DONE
// TODO: plant in line between game objects list
// TODO: option to wire up poles - DONE
// TODO: reverse poles
// TODO: editor script
// TODO: version for gameobject way points
// TODO: close up wires if length 1 and closed
// TODO: offset from spline
// TODO: Plant list should not use objects but position handles
// TODO: option to cubic spline the list

[ExecuteInEditMode]
[AddComponentMenu("Mega Wire/Plant Poles")]
public class MegaWirePlantPoles : MonoBehaviour
{
	public MegaShape		path;
	public int				curve = 0;
	public float			start = 0.0f;
	public float			length = 1.0f;
	public float			spacing = 10.0f;
	public bool				update = false;
	public GameObject		pole;
	public Vector3			rotate = Vector3.zero;
	public float			offset = 0.0f;
	public bool				conform = true;
	public float			upright = 0.0f;
	public bool				addwires = false;
	public List<GameObject>	poles = new List<GameObject>();
	public MegaWire			copyfrom = null;
	public Material			material;
	public Vector3			positionVariation = Vector3.zero;
	public Vector3			rotateVariation = Vector3.zero;
	public Vector3			scaleVariation = Vector3.zero;
	public float			spacingVariation = 0.0f;
	public bool				reverseWire = false;
	public int				seed = 0;
	public float			wireSizeMult = 1.0f;
	public float			stretch = 1.0f;

	public void Rebuild()
	{
		if ( path && pole )
		{
			update = false;

			MegaWire oldwire = transform.GetComponentInChildren<MegaWire>();

			// Destroy all children
			while ( transform.childCount > 0 )
			{
				GameObject go = transform.GetChild(0).gameObject;

				if ( Application.isEditor )
					DestroyImmediate(go);
				else
					Destroy(go);
			}

			int polecount = (int)((path.splines[curve].length * length) / spacing);

			float alpha = start;
			float da = length / polecount;

			if ( path.splines[curve].closed )
			{
				polecount--;
			}

			poles.Clear();
#if UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
			Random.InitState(seed);
#else
			Random.seed = seed;
#endif

			float spacealpha = ((spacing / (path.splines[curve].length * length)) / 1.0f) * spacingVariation;

			// mmm need upright in here
			for ( int i = 0; i <= polecount; i++ )
			{
				float a = alpha;

				if ( i != 0 && i != polecount - 1 )
					a = alpha + (Random.Range(-1.0f, 1.0f) * spacealpha);

				Vector3 p = path.transform.TransformPoint(path.InterpCurve3D(curve, a, true));
				Vector3 p1 = path.transform.TransformPoint(path.InterpCurve3D(curve, a + 0.001f, true));

				Vector3 poff = Vector3.zero;
				poff.y = Random.Range(0.0f, 1.0f) * positionVariation.y;
				poff.z = Random.Range(-1.0f, 1.0f) * positionVariation.z;

				Vector3 dir = (p1 - p).normalized;

				Vector3 outline = Vector3.Cross(dir, Vector3.up) * (offset + poff.z);

				p += outline;
				p1 += outline;

				//Vector3 up = Vector3.up;
				//Vector3 norm = up;

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

				relativePos.y = 0.0f;	//*= upright;

				Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);

				Vector3 rrot = Vector3.zero;
				rrot.x = Random.Range(-1.0f, 1.0f) * rotateVariation.x;
				rrot.y = Random.Range(-1.0f, 1.0f) * rotateVariation.y;
				rrot.z = Random.Range(-1.0f, 1.0f) * rotateVariation.z;

				Quaternion erot = Quaternion.Euler(rotate + rrot);

				p.y -= poff.y;

				GameObject go = (GameObject)GameObject.Instantiate(pole, p, hitrot * rot * erot);	//rot * hitrot * erot);
				go.name = "Pole " + i;

				go.transform.parent = transform;

				alpha += da;
				poles.Add(go);
			}

			if ( length >= 0.99999f && path.splines[curve].closed )
				poles.Add(poles[0]);

			if ( addwires )
			{
				if ( reverseWire )
					poles.Reverse();

				MegaWire wire = MegaWire.Create(oldwire, poles, material, "Wires", copyfrom, wireSizeMult, stretch);

				if ( wire )
					wire.transform.parent = transform;
			}
		}
	}

	void Update()
	{
	}
}
#endif