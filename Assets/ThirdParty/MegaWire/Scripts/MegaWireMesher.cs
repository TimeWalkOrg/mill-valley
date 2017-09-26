
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MegaWireMesher
{
	public bool			show = false;
	static Vector3[]	tan1;
	static Vector3[]	tan2;

	public virtual void BuildMesh(MegaWire rope, MegaWireSpan span) { }

	static public void BuildTangents(Mesh mesh, Vector3[] verts, Vector2[] uvs, Vector3[] norms, int[] tris)
	{
		int triangleCount = mesh.triangles.Length;
		int vertexCount = mesh.vertices.Length;

		if ( tan1 == null || tan1.Length < vertexCount )
		{
			tan1 = new Vector3[vertexCount];
			tan2 = new Vector3[vertexCount];
		}

		Vector4[]	tangents = new Vector4[vertexCount];

		for ( int a = 0; a < triangleCount; a += 3 )
		{
			long i1 = tris[a];
			long i2 = tris[a + 1];
			long i3 = tris[a + 2];

			Vector3 v1 = verts[i1];
			Vector3 v2 = verts[i2];
			Vector3 v3 = verts[i3];

			Vector2 w1 = uvs[i1];
			Vector2 w2 = uvs[i2];
			Vector2 w3 = uvs[i3];

			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;

			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float r = 1.0f / (s1 * t2 - s2 * t1);

			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}

		for ( int a = 0; a < vertexCount; a++ )
		{
			Vector3 n = norms[a];
			Vector3 t = tan1[a];

			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}

		mesh.tangents = tangents;
	}
}

[System.Serializable]
public class MegaWireStrandedMesher : MegaWireMesher
{
	Matrix4x4		wtm;
	public int		sides			= 4;
	public int		segments		= 20;
	public float	uvtwist			= 0.0f;
	public float	uvtilex			= 1.0f;
	public float	uvtiley			= 1.0f;
	public int		strands			= 1;
	public float	offset			= 0.0f;
	public float	Twist			= 0.0f;
	public bool		cap				= true;
	public float	strandRadius	= 0.0f;
	public float	SegsPerUnit		= 1.0f;
	public float	TwistPerUnit	= 0.0f;
	public bool		calcBounds		= true;
	public bool		calcTangents	= false;
	public bool		genuv			= true;
	public bool		linInterp		= false;
	Vector3			ropeup;
	Vector3[]		cross;
	Vector3[]		cnorms;

	public void Copy(MegaWireStrandedMesher src)
	{
		sides			= src.sides;
		segments		= src.segments;
		uvtwist			= src.uvtwist;
		uvtilex			= src.uvtilex;
		uvtiley			= src.uvtiley;
		strands			= src.strands;
		offset			= src.offset;
		Twist			= src.Twist;
		cap				= src.cap;
		strandRadius	= src.strandRadius;
		SegsPerUnit		= src.SegsPerUnit;
		TwistPerUnit	= src.TwistPerUnit;
		calcBounds		= src.calcBounds;
		linInterp		= src.linInterp;
		genuv			= src.genuv;
	}

	public override void BuildMesh(MegaWire rope, MegaWireSpan span)
	{
		int wires = span.connections.Count;

		float lengthuvtile = uvtiley * span.WireLength;

		Twist = TwistPerUnit * span.WireLength;
		segments = (int)(span.WireLength * SegsPerUnit);
		if ( segments < 1 )
			segments = 1;

		int vcount = ((segments + 1) * (sides + 1)) * strands * wires;
		int tcount = ((sides * 2) * segments) * strands * wires;

		if ( span.verts == null || span.verts.Length != vcount )
			span.verts = new Vector3[vcount];

		bool buildtris = false;

		if ( (span.uvs == null || span.uvs.Length != vcount) && genuv )
		{
			span.uvs = new Vector2[vcount];
			rope.builduvs = true;
		}

		if ( span.tris == null || span.tris.Length != tcount * 3 )
		{
			span.tris = new int[tcount * 3];
			buildtris = true;
		}

		if ( span.norms == null || span.norms.Length != vcount )
			span.norms = new Vector3[vcount];

		int vi = 0;
		int ti = 0;

		BuildCrossSection(1.0f);

		for ( int c = 0; c < span.connections.Count; c++ )
		{
			MegaWireConnection con = span.connections[c];

			// TODO: inspector should update radius from def, then user can control per span
			float off = (rope.connections[c].radius * 0.5f) + offset;

			float sradius = 0.0f;

			if ( strands == 1 )
			{
				off = offset;
				sradius = rope.connections[c].radius;
			}
			else
				sradius = (rope.connections[c].radius * 0.5f) + strandRadius;

			Vector2 uv = Vector2.zero;
			Vector3 soff = Vector3.zero;

			int vo;

			for ( int s = 0; s < strands; s++ )
			{
				if ( strands == 1 )
				{
					vo = vi;

					if ( linInterp )
					{
						for ( int i = 0; i <= segments; i++ )
						{
							float alpha = ((float)i / (float)segments);
							float uvt = alpha * uvtwist;

							wtm = con.GetDeformMatLin(alpha);

							for ( int v = 0; v <= cross.Length; v++ )
							{
								Vector3 p = cross[v % cross.Length];
								span.verts[vi] = wtm.MultiplyPoint3x4(p * sradius);
								span.norms[vi] = wtm.MultiplyVector(p);

								if ( genuv && rope.builduvs )
								{
									uv.y = alpha * lengthuvtile;
									uv.x = (((float)v / (float)cross.Length) * uvtilex) + uvt;

									span.uvs[vi] = uv;
								}

								vi++;
							}
						}
					}
					else
					{
						for ( int i = 0; i <= segments; i++ )
						{
							float alpha = ((float)i / (float)segments);
							float uvt = alpha * uvtwist;

							wtm = con.GetDeformMat(alpha);

							for ( int v = 0; v <= cross.Length; v++ )
							{
								Vector3 p = cross[v % cross.Length];
								span.verts[vi] = wtm.MultiplyPoint3x4(p * sradius);

								span.norms[vi] = wtm.MultiplyVector(p);

								if ( genuv && rope.builduvs )
								{
									uv.y = alpha * lengthuvtile;
									uv.x = (((float)v / (float)cross.Length) * uvtilex) + uvt;

									span.uvs[vi] = uv;
								}
								vi++;
							}
						}
					}
				}
				else
				{
					float ang = ((float)s / (float)strands) * Mathf.PI * 2.0f;

					soff.x = Mathf.Sin(ang) * off;
					soff.z = Mathf.Cos(ang) * off;

					vo = vi;

					if ( linInterp )
					{
						for ( int i = 0; i <= segments; i++ )
						{
							float alpha = ((float)i / (float)segments);

							float uvt = alpha * uvtwist;

							float tst = (alpha * Twist * Mathf.PI * 2.0f);
							soff.x = Mathf.Sin(ang + tst) * off;
							soff.z = Mathf.Cos(ang + tst) * off;
							wtm = con.GetDeformMatLin(alpha);
							
							for ( int v = 0; v <= cross.Length; v++ )
							{
								Vector3 p = cross[v % cross.Length];
								span.verts[vi] = wtm.MultiplyPoint3x4((p * sradius) + soff);
								span.norms[vi] = wtm.MultiplyVector(p);

								if ( genuv && rope.builduvs )
								{
									uv.y = alpha * lengthuvtile;
									uv.x = (((float)v / (float)cross.Length) * uvtilex) + uvt;

									span.uvs[vi] = uv;
								}

								vi++;
							}
						}
					}
					else
					{
						for ( int i = 0; i <= segments; i++ )
						{
							float alpha = ((float)i / (float)segments);

							float uvt = alpha * uvtwist;

							float tst = (alpha * Twist * Mathf.PI * 2.0f);
							soff.x = Mathf.Sin(ang + tst) * off;
							soff.z = Mathf.Cos(ang + tst) * off;
							wtm = con.GetDeformMat(alpha);

							for ( int v = 0; v <= cross.Length; v++ )
							{
								Vector3 p = cross[v % cross.Length];
								span.verts[vi] = wtm.MultiplyPoint3x4((p * sradius) + soff);
								span.norms[vi] = wtm.MultiplyVector(p);

								if ( genuv && rope.builduvs )
								{
									uv.y = alpha * lengthuvtile;
									uv.x = (((float)v / (float)cross.Length) * uvtilex) + uvt;

									span.uvs[vi] = uv;
								}
								vi++;
							}
						}
					}
				}

				if ( buildtris )
				{
					int sc = sides + 1;
					for ( int i = 0; i < segments; i++ )
					{
						for ( int v = 0; v < cross.Length; v++ )
						{
							int v1 = ((i + 1) * sc) + v + vo;
							int v2 = ((i + 1) * sc) + ((v + 1) % sc) + vo;
							int v3 = (i * sc) + v + vo;
							int v4 = (i * sc) + ((v + 1) % sc) + vo;

							span.tris[ti++] = v1;	//((i + 1) * sc) + v + vo;
							span.tris[ti++] = v2;	//((i + 1) * sc) + ((v + 1) % sc) + vo;
							span.tris[ti++] = v3;	//(i * sc) + v + vo;

							span.tris[ti++] = v2;	//((i + 1) * sc) + ((v + 1) % sc) + vo;
							span.tris[ti++] = v4;	//(i * sc) + ((v + 1) % sc) + vo;
							span.tris[ti++] = v3;	//(i * sc) + v + vo; 
						}
					}
				}
			}
		}

		if ( (!genuv && rope.builduvs) || buildtris )
		{
#if UNITY_3_5
			span.mesh.Clear();
#else
			span.mesh.Clear(false);
#endif
		}

		span.mesh.vertices = span.verts;

		if ( genuv && rope.builduvs )
			span.mesh.uv = span.uvs;

		if ( (!genuv && rope.builduvs) || buildtris )
			span.mesh.triangles = span.tris;

		if ( calcBounds )
			span.mesh.RecalculateBounds();

		span.mesh.normals = span.norms;

		if ( calcTangents && genuv )
			BuildTangents(span.mesh, span.verts, span.uvs, span.mesh.normals, span.tris);

		span.vcount = vcount;
	}

	void BuildCrossSection(float rad)
	{
		if ( cross == null || cross.Length != sides )
			cross = new Vector3[sides];

		if ( cnorms == null || cnorms.Length != sides )
			cnorms = new Vector3[sides];

		Vector3 p = Vector3.zero;
		for ( int i = 0; i < sides; i++ )
		{
			float ang = ((float)i / (float)sides) * Mathf.PI * 2.0f;

			p.x = Mathf.Sin(ang);
			p.y = Mathf.Cos(ang);
			cross[i] = p;
		}
	}
}
