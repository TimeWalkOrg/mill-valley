
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MegaWireConstraint
{
	public bool active;
	public float	reactivate = 0.0f;
	public float	rtime = 0.0f;
	public virtual void Apply(MegaWireConnection soft)
	{
	}
}

[System.Serializable]
public class MegaWireLengthConstraint : MegaWireConstraint
{
	public int		p1;
	public int		p2;
	public float	length;
	Vector3 moveVector = Vector3.zero;

	public MegaWireLengthConstraint(int _p1, int _p2, float _len)
	{
		p1 = _p1;
		p2 = _p2;
		length = _len;
		active = true;
	}

	public override void Apply(MegaWireConnection soft)
	{
		if ( active )
		{
			moveVector.x = soft.masses[p2].pos.x - soft.masses[p1].pos.x;
			moveVector.y = soft.masses[p2].pos.y - soft.masses[p1].pos.y;
			moveVector.z = soft.masses[p2].pos.z - soft.masses[p1].pos.z;

			if ( moveVector.x != 0.0f || moveVector.y != 0.0f || moveVector.z != 0.0f )
			{
				float currentLength = moveVector.magnitude;

				float do1 = 1.0f / currentLength;

				float l = 0.5f * (currentLength - length) * do1;
				moveVector.x *= l;
				moveVector.y *= l;
				moveVector.z *= l;

				soft.masses[p1].pos.x += moveVector.x;
				soft.masses[p1].pos.y += moveVector.y;
				soft.masses[p1].pos.z += moveVector.z;

				soft.masses[p2].pos.x -= moveVector.x;
				soft.masses[p2].pos.y -= moveVector.y;
				soft.masses[p2].pos.z -= moveVector.z;
			}
		}
	}
}

[System.Serializable]
public class MegaWirePointConstraint : MegaWireConstraint
{
	public int			p1;
	public Vector3		offset;
	public Transform	obj;
	public Vector3		ps;
	public Vector3		tp;

	public MegaWirePointConstraint(int _p1, Transform trans, Vector3 off)
	{
		offset = off;
		p1 = _p1;
		obj = trans;
		active = true;
		reactivate = 0.0f;
		rtime = 0.0f;
	}

	Vector3 easeInOutSine(Vector3 start, Vector3 end, float value)
	{
		end -= start;
		return -end / 2.0f * (Mathf.Cos(Mathf.PI * value / 1.0f) - 1.0f) + start;
	}

	public void ReActivate(MegaWireConnection soft, float t)
	{
		tp = obj.TransformPoint(offset);

		if ( !active )
		{
			if ( reactivate > 0.0f )
			{
				reactivate -= 0.01f;
				//Vector3 delta = tp - soft.masses[p1].pos;

				soft.masses[p1].pos	= easeInOutSine(tp, ps, reactivate / rtime);

				if ( reactivate < 0.0f )
				{
					reactivate = 0.0f;
					active = true;
				}
			}
		}
	}

	public override void Apply(MegaWireConnection soft)
	{
		if ( active )
			soft.masses[p1].pos = tp;
	}
}

[System.Serializable]
public class MegaWireSolver
{
	public virtual void doIntegration1(MegaWireConnection rope, MegaWire wire, float dt) { }
	public virtual void Solve() { }
}

[System.Serializable]
public class MegaWireSolverVertlet : MegaWireSolver
{
	void doCalculateForces(MegaWireConnection rope, MegaWire wire)
	{
		Vector3 frc = wire.gravity;

		frc.x += rope.windFrc.x;
		frc.y += rope.windFrc.y;
		frc.z += rope.windFrc.z;

		for ( int i = 0; i < rope.masses.Count; i++ )
		{
			rope.masses[i].force.x = (rope.masses[i].mass * frc.x) + rope.masses[i].forcec.x;
			rope.masses[i].force.y = (rope.masses[i].mass * frc.y) + rope.masses[i].forcec.y;
			rope.masses[i].force.z = (rope.masses[i].mass * frc.z) + rope.masses[i].forcec.z;
		}

		for ( int i = 0; i < rope.springs.Count; i++ )
			rope.springs[i].doCalculateSpringForce1(rope);
	}

	void DoConstraints(MegaWireConnection rope, MegaWire wire)
	{
		for ( int c = 0; c < rope.constraints.Count; c++ )
		{
			rope.constraints[c].ReActivate(rope, wire.timeStep);
		}

		for ( int i = 0; i < wire.iters; i++ )
		{
			for ( int c = 0; c < rope.lenconstraints.Count; c++ )
				rope.lenconstraints[c].Apply(rope);
			for ( int c = 0; c < rope.constraints.Count; c++ )
				rope.constraints[c].Apply(rope);

		}
	}

	public override void doIntegration1(MegaWireConnection rope, MegaWire wire, float dt)
	{
		Vector3 delta;
		Vector3 frc;

		doCalculateForces(rope, wire);	// Calculate forces, only changes _f

		float t2 = dt * dt;

		float oodt = 1.0f / dt;

		/*	Then do correction step by integration with central average (Heun) */
		for ( int i = 0; i < rope.masses.Count; i++ )
		{
			Vector3 last = rope.masses[i].pos;
			//rope.masses[i].pos += wire.airdrag * (rope.masses[i].pos - rope.masses[i].last) + rope.masses[i].force * rope.masses[i].oneovermass * t2;	// * t;

			//rope.masses[i].vel = (rope.masses[i].pos - last) / dt;
			delta.x = rope.masses[i].pos.x - rope.masses[i].last.x;
			delta.y = rope.masses[i].pos.y - rope.masses[i].last.y;
			delta.z = rope.masses[i].pos.z - rope.masses[i].last.z;

			float m2 = rope.masses[i].oneovermass * t2;
			frc.x = rope.masses[i].force.x * m2;
			frc.y = rope.masses[i].force.y * m2;
			frc.z = rope.masses[i].force.z * m2;

			//wire.masses[i].pos += wire.airdrag * (delta) + frc;	//wire.masses[i].force * wire.masses[i].oneovermass * t2;	// * t;

			rope.masses[i].pos.x += wire.airdrag * delta.x + frc.x;
			rope.masses[i].pos.y += wire.airdrag * delta.y + frc.y;
			rope.masses[i].pos.z += wire.airdrag * delta.z + frc.z;

			rope.masses[i].vel.x = (rope.masses[i].pos.x - last.x) * oodt;
			rope.masses[i].vel.y = (rope.masses[i].pos.y - last.y) * oodt;
			rope.masses[i].vel.z = (rope.masses[i].pos.z - last.z) * oodt;

			rope.masses[i].last = last;
		}

		DoConstraints(rope, wire);

		if ( wire.doCollisions )
			DoCollisions(rope, wire);
	}

	void DoCollisions(MegaWireConnection rope, MegaWire wire)
	{
		if ( wire.useraycast )
		{
			int mask = wire.collisionmask.value;

			RaycastHit hit;

			float len = wire.collisiondist + wire.collisionoff;

			Vector3 upd = Vector3.up * wire.collisiondist;
			for ( int i = 0; i < rope.masses.Count; i++ )
			{
				Vector3 p = rope.masses[i].pos + upd;

				bool result = Physics.Raycast(p, -Vector3.up, out hit, len, mask);
				//bool result = Physics.SphereCast(p, 0.1f, -Vector3.up, out hit, len, mask);
				//Vector3 delta = rope.masses[i].pos - rope.masses[i].last;
				//bool result = Physics.Raycast(rope.masses[i].last, delta.normalized, out hit, delta.magnitude, mask);

				if ( result )
				{
					//Debug.Log("p " + rope.masses[i].pos + " l " + rope.masses[i].last + " h " + hit.point);
					//Vector3 hp = hit.point + (hit.normal * wire.collisionoff);
					//Debug.Log("hp " + hit.point + " mhp " + hp);

					rope.masses[i].pos.x = hit.point.x;
					rope.masses[i].pos.y = hit.point.y + wire.collisionoff;
					rope.masses[i].pos.z = hit.point.z;
					//rope.masses[i].pos = hp;

					float VdotN = Vector3.Dot(Vector3.up, rope.masses[i].vel);
					Vector3 Vn = Vector3.up * VdotN;
					// CALCULATE Vt
					//Vector3 Vt = (rope.masses[i].vel - Vn) * rope.floorfriction;
					// SCALE Vn BY COEFFICIENT OF RESTITUTION
					Vn *= 0.9f;	//rope.bounce;
					// SET THE VELOCITY TO BE THE NEW IMPULSE
					rope.masses[i].vel = Vn;	//Vt - Vn;

					rope.masses[i].last = rope.masses[i].pos;
				}
			}
		}
		else
		{
			for ( int i = 0; i < rope.masses.Count; i++ )
			{
				if ( rope.masses[i].pos.y < wire.floor )
				{
					rope.masses[i].pos.y = wire.floor;

					float VdotN = Vector3.Dot(Vector3.up, rope.masses[i].vel);
					Vector3 Vn = Vector3.up * VdotN;
					// CALCULATE Vt
					//Vector3 Vt = (rope.masses[i].vel - Vn) * rope.floorfriction;
					// SCALE Vn BY COEFFICIENT OF RESTITUTION
					Vn *= 0.9f;	//rope.bounce;
					// SET THE VELOCITY TO BE THE NEW IMPULSE
					rope.masses[i].vel = Vn;	//Vt - Vn;

					rope.masses[i].last = rope.masses[i].pos;
				}
			}
		}
	}
}

[System.Serializable]
public class MegaWireMass
{
	public Vector3		pos;
	public Vector3		last;
	public Vector3		force;
	public Vector3		vel;
	public Vector3		posc;
	public Vector3		velc;
	public Vector3		forcec;
	public float		mass;	// 1.0f normally, set to zero to lock in place
	public float		oneovermass;
	public bool			collide;

	public MegaWireMass(float m, Vector3 p)
	{
		mass = m;
		oneovermass = 1.0f / mass;
		pos = p;
		last = p;
		force = Vector3.zero;
		vel = Vector3.zero;
	}
}

[System.Serializable]
public class MegaWireSpring
{
	public int		p1;
	public int		p2;
	public float	restlen;
	public float	initlen;
	public float	ks;
	public float	kd;
	public float	len;

	public MegaWireSpring(int _p1, int _p2, float _ks, float _kd, MegaWireConnection con)
	{
		p1 = _p1;
		p2 = _p2;
		ks = _ks;
		kd = _kd;
		restlen = (con.masses[p1].pos - con.masses[p2].pos).magnitude;	// * stretch;
		initlen = restlen;
	}

	public MegaWireSpring(int _p1, int _p2, float _ks, float _kd, MegaWireConnection con, float stretch)
	{
		p1 = _p1;
		p2 = _p2;
		ks = _ks;
		kd = _kd;
		restlen = (con.masses[p1].pos - con.masses[p2].pos).magnitude * stretch;
		initlen = restlen;
	}

	public void doCalculateSpringForce(MegaWireConnection hose)
	{
		Vector3 deltaP = hose.masses[p1].pos - hose.masses[p2].pos;

		float dist = deltaP.magnitude;

		float Hterm = (dist - restlen) * ks;

		Vector3	deltaV = hose.masses[p1].vel - hose.masses[p2].vel;
		float Dterm = (Vector3.Dot(deltaV, deltaP) * kd) / dist;

		Vector3 springForce = deltaP * (1.0f / dist);
		springForce *= -(Hterm + Dterm);

		hose.masses[p1].force.x += springForce.x;
		hose.masses[p1].force.y += springForce.y;
		hose.masses[p1].force.z += springForce.z;

		hose.masses[p2].force.x -= springForce.x;
		hose.masses[p2].force.y -= springForce.y;
		hose.masses[p2].force.z -= springForce.z;
	}

	public void doCalculateSpringForce1(MegaWireConnection mod)
	{
		Vector3 direction;	// = mod.masses[p1].pos - mod.masses[p2].pos;

		direction.x = mod.masses[p1].pos.x - mod.masses[p2].pos.x;
		direction.y = mod.masses[p1].pos.y - mod.masses[p2].pos.y;
		direction.z = mod.masses[p1].pos.z - mod.masses[p2].pos.z;

		//if ( direction != Vector3.zero )
		if ( direction.x != 0.0f || direction.y != 0.0f || direction.z != 0.0f )
		{
			//float currLength = direction.magnitude;
			float currLength = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y) + (direction.z * direction.z));

			//direction = direction.normalized;
			float ool = 1.0f / currLength;
			direction.x *= ool;
			direction.y *= ool;
			direction.z *= ool;

			Vector3 force;	// = -ks * ((currLength - restlen) * direction);
			float dl = -ks * (currLength - restlen);
			force.x = dl * direction.x;
			force.y = dl * direction.y;
			force.z = dl * direction.z;

			mod.masses[p1].force.x += force.x;
			mod.masses[p1].force.y += force.y;
			mod.masses[p1].force.z += force.z;

			mod.masses[p2].force.x -= force.x;
			mod.masses[p2].force.y -= force.y;
			mod.masses[p2].force.z -= force.z;

			len = currLength;
		}
	}
}