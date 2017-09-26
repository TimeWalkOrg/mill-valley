
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MegaWireConnectionDef
{
	public Vector3	inOffset	= Vector3.zero;
	public Vector3	outOffset	= Vector3.zero;
	public float	radius		= 0.01f;

	public MegaWireConnectionDef() { }
	public MegaWireConnectionDef(MegaWireConnectionDef src)
	{
		inOffset = src.inOffset;
		outOffset = src.outOffset;
		radius = src.radius;
	}
}

[System.Serializable]
public class MegaWireConnection
{
	public Vector3		inOffset	= Vector3.zero;
	public Vector3		outOffset	= Vector3.zero;
	public float		radius		= 0.01f;
	public Vector3		windFrc		= Vector3.zero;
	public Transform	start;
	public Transform	end;
	public float		WireLength	= 0.0f;
	Matrix4x4			wtm;
	public List<MegaWireMass>			masses		= new List<MegaWireMass>();
	public List<MegaWireSpring>			springs		= new List<MegaWireSpring>();
	public List<MegaWirePointConstraint>	constraints	= new List<MegaWirePointConstraint>();
	public List<MegaWireLengthConstraint>	lenconstraints	= new List<MegaWireLengthConstraint>();
	public Vector3[]					masspos;

	// Should set the sim wake time so it will update correctly
	public void SetEndConstraintActive(int index, bool active, float time)
	{
		if ( active )
		{
			constraints[index].reactivate = time;
			constraints[index].rtime = time;
			constraints[index].ps = masses[constraints[index].p1].pos;
		}
		else
			constraints[index].active = active;
	}

	public void Update(MegaWire wire, float timeStep)
	{
		wire.verletsolver.doIntegration1(this, wire, timeStep);
	}

	public void Init(MegaWire wire)
	{
		if ( start == null || end == null )
			return;

		Vector3 p1 = start.TransformPoint(outOffset);
		Vector3 p2 = end.TransformPoint(inOffset);

		WireLength = (p1 - p2).magnitude;

		if ( masses == null )
			masses = new List<MegaWireMass>();

		masses.Clear();
		float ms = wire.Mass / (float)(wire.points + 1);

		for ( int i = 0; i <= wire.points; i++ )
		{
			float alpha = (float)i / (float)wire.points;

			float rn = (Random.value - 0.5f) * 2.0f * (wire.massRand * ms);
			float m = rn + ms;
			MegaWireMass rm = new MegaWireMass(m, Vector3.Lerp(p1, p2, alpha));
			masses.Add(rm);
		}

		if ( springs == null )
			springs = new List<MegaWireSpring>();

		springs.Clear();

		if ( constraints == null )
			constraints = new List<MegaWirePointConstraint>();

		if ( lenconstraints == null )
			lenconstraints = new List<MegaWireLengthConstraint>();

		constraints.Clear();
		lenconstraints.Clear();

		for ( int i = 0; i < masses.Count - 1; i++ )
		{
			MegaWireSpring spr = new MegaWireSpring(i, i + 1, wire.spring, wire.damp, this, wire.stretch);
			springs.Add(spr);

			if ( wire.lengthConstraints )
			{
				MegaWireLengthConstraint lcon = new MegaWireLengthConstraint(i, i + 1, spr.restlen);
				lenconstraints.Add(lcon);
			}
		}

		if ( wire.stiffnessSprings )
		{
			int gap = 2;
			for ( int i = 0; i < masses.Count - gap; i++ )
			{
				MegaWireSpring spr = new MegaWireSpring(i, i + 2, wire.stiffrate, wire.stiffdamp, this, wire.stretch);
				springs.Add(spr);
				//float alpha = (float)i / (float)masses.Count;
				//MegaWireSpring spr = new MegaWireSpring(i, i + gap, stiffspring * stiffnessCrv.Evaluate(alpha), stiffdamp * stiffnessCrv.Evaluate(alpha), this);	//, stretch);
				//springs.Add(spr);

				//WireLengthConstraint lcon = new WireLengthConstraint(i, i + gap, spr.restlen);
				//constraints.Add(lcon);
			}
		}

		// Apply fixed end constraints
		MegaWirePointConstraint pcon = new MegaWirePointConstraint(0, start.transform, outOffset);
		constraints.Add(pcon);

		pcon = new MegaWirePointConstraint(masses.Count - 1, end.transform, inOffset);
		constraints.Add(pcon);

		masspos = new Vector3[masses.Count + 2];

		for ( int i = 0; i < masses.Count; i++ )
			masspos[i + 1] = masses[i].pos;

		masspos[0] = masses[0].pos - (masses[1].pos - masses[0].pos);
		masspos[masspos.Length - 1] = masses[masses.Count - 1].pos + (masses[masses.Count - 1].pos - masses[masses.Count - 2].pos);
	}

	public void MoveMasses(MegaWire wire)
	{
		for ( int i = 0; i < masses.Count; i++ )
		{
			masspos[i + 1] = masses[i].pos;
			masses[i].forcec = Vector3.zero;
		}

		masspos[0] = masses[0].pos - (masses[1].pos - masses[0].pos);
		masspos[masspos.Length - 1] = masses[masses.Count - 1].pos + (masses[masses.Count - 1].pos - masses[masses.Count - 2].pos);
	}

	public Vector3 Interp(float t)
	{
		int numSections = masspos.Length - 3;
		int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
		float u = t * (float)numSections - (float)currPt;

		Vector3 a = masspos[currPt];
		Vector3 b = masspos[currPt + 1];
		Vector3 c = masspos[currPt + 2];
		Vector3 d = masspos[currPt + 3];

		return 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
	}

	public Vector3 Velocity(float t)
	{
		int numSections = masspos.Length - 3;
		int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
		float u = t * (float)numSections - (float)currPt;

		Vector3 a = masspos[currPt];
		Vector3 b = masspos[currPt + 1];
		Vector3 c = masspos[currPt + 2];
		Vector3 d = masspos[currPt + 3];

		return 1.5f * (-a + 3f * b - 3f * c + d) * (u * u) + (2f * a - 5f * b + 4f * c - d) * u + .5f * c - .5f * a;
	}

	public Vector3 LinInterp(float t)
	{
		int numSections = masspos.Length - 3;
		int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
		float u = t * (float)numSections - (float)currPt;

		Vector3 b = masspos[currPt + 1];
		Vector3 c = masspos[currPt + 2];

		return Vector3.Lerp(b, c, u);
	}

	public Matrix4x4 GetDeformMat(float alpha)
	{
		Vector3 ps	= Interp(alpha);
		Vector3 ps1	= Velocity(alpha);

		Quaternion rotation = Quaternion.LookRotation(ps1, Vector3.up);

		float xx = rotation.x * rotation.x;
		float xy = rotation.x * rotation.y;
		float xz = rotation.x * rotation.z;
		float xw = rotation.x * rotation.w;

		float yy = rotation.y * rotation.y;
		float yz = rotation.y * rotation.z;
		float yw = rotation.y * rotation.w;

		float zz = rotation.z * rotation.z;
		float zw = rotation.z * rotation.w;

		wtm.m00 = 1.0f - 2.0f * (yy + zz);
		wtm.m01 = 2.0f * (xy - zw);
		wtm.m02 = 2.0f * (xz + yw);

		wtm.m10 = 2.0f * (xy + zw);
		wtm.m11 = 1.0f - 2.0f * (xx + zz);
		wtm.m12 = 2.0f * (yz - xw);

		wtm.m20 = 2.0f * (xz - yw);
		wtm.m21 = 2.0f * (yz + xw);
		wtm.m23 = 1.0f - 2.0f * (xx + yy);

		wtm.m03 = ps.x;
		wtm.m13 = ps.y;
		wtm.m23 = ps.z;

		return wtm;
	}

	public Matrix4x4 GetDeformMatLin(float alpha)
	{
		Vector3 ps1;
		Vector3 ps	= LinInterp(alpha);

		alpha += 0.001f;
		if ( alpha > 1.0f )
		{
			alpha = 0.999f;
			ps1 = ps - LinInterp(alpha);
		}
		else
			ps1	= LinInterp(alpha) - ps;

		Quaternion rotation = Quaternion.LookRotation(ps1, Vector3.up);

		float xx = rotation.x * rotation.x;
		float xy = rotation.x * rotation.y;
		float xz = rotation.x * rotation.z;
		float xw = rotation.x * rotation.w;

		float yy = rotation.y * rotation.y;
		float yz = rotation.y * rotation.z;
		float yw = rotation.y * rotation.w;

		float zz = rotation.z * rotation.z;
		float zw = rotation.z * rotation.w;

		wtm.m00 = 1.0f - 2.0f * (yy + zz);
		wtm.m01 = 2.0f * (xy - zw);
		wtm.m02 = 2.0f * (xz + yw);

		wtm.m10 = 2.0f * (xy + zw);
		wtm.m11 = 1.0f - 2.0f * (xx + zz);
		wtm.m12 = 2.0f * (yz - xw);

		wtm.m20 = 2.0f * (xz - yw);
		wtm.m21 = 2.0f * (yz + xw);
		wtm.m23 = 1.0f - 2.0f * (xx + yy);

		wtm.m03 = ps.x;
		wtm.m13 = ps.y;
		wtm.m23 = ps.z;

		return wtm;
	}
}
