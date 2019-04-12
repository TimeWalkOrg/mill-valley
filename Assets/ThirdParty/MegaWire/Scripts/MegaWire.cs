
using UnityEngine;
using System.Collections.Generic;

// TODO: Multithread - set of wires per thread should be simple, cant do setmesh though
// TODO: Increase wire size on distance
// TODO: better wind gizmo
// TODO: add global turbelence and variation to wind
// TODO: Any attach objects should work when a wire is replaced
// TODO: Check selected objects for any connection info in megawires window
// TODO: Noise option for thin wires
// TODO: barbs on wire
// TODO: option in wire window to select all children with helpers
// TODO: option to disable connections so no wire made
// TODO: first and last pole different connection points
// TODO: Add ribbon mesher
// TODO: New component to connect masses on separate wires with another rope
// TODO: option maybe per strand for lin, lod thing as well
// TODO: option to skin mesh
// TODO: option for physics based wires
// TODO: break a wire
// TODO: self collision
// TODO: lod system
// TODO: sleep physics
// TODO: add remove and reorder poles
// TODO: End objects on wires
// TODO: option for final lod to just turn the span mesh off

// TODO: super constraint (or separate constraint type lists) - DONE
// TODO: Check changes from 4.2 work in 3.5 - DONE
// TODO: test on 4.3 - DONE
// TODO: Rebuild should not delete the wires object, just update it, that wuld be best - DONE
// TODO: Picking should show arrows - DONE
// TOOD: Pole plant should copy the wire settings from any existing wire so dont lose settigns - DONE
// TODO: Store images - DONE
// TODO: web pages - DONE
// TODO: submit to store - DONE
// TODO: only show replace window if list exists - DONE
// TODO: Add help context to pages - DONE
// TODO: Do simple demo scene - DONE
// TODO: Do video - DONE
// TODO: Add Undo - DONE
// TODO: Final code tidy - DONE
// TODO: put all scripts into MegaWire component section - DONE
// TODO: option to reverse pole list for rotated poles - DONE
// TODO: Add Unity poles as prefabs with connections defined - DONE
// TODO: connection def script added to poles for auto wire building (saved in prefab) (can mix poles with same number of connectiosn then) - DONE
// TODO: Option to close loop of wires - DONE
// TODO: Plant poles script - DONE
// TODO: See about calculating normals instead of using Unity method - DONE
// TODO: on plat list, add point should add extrapolated away - DONE
// TODO: Option to disable end constraints - DONE
// TODO: time option to reconnect end constraints - DONE
// TODO: start physics run time before distance stop kicks in - DONE
// TODO: Put advanced physics stuff in own panel - DONE
// TODO: Show strain in springs - DONE
// TODO: Do some optimizing - DONE
// TOOD: Disable all option - DONE
// TODO: time option for run physics - DONE
// TODO: dont build uvs and tris if they havent changed - DONE
// TODO: Option to turn of gen uvs - DONE
// TODO: Option for linear or cubic interp - DONE
// TODO: option for length constraint - DONE
// TODO: Add stiffness springs - DONE
// TODO: frame count for update with index - DONE
// TODO: Wind effect slider, can control how much wind force effects - DONE
// TODO: Calc tangents option - DONE
// TODO: Calc bounds - DONE
// TODO: Show vertex count for wire in inspector - DONE
// TODO: Inspector for WireSpan - DONE
// TODO: bool to turn on/off all spans - DONE
// TODO: bool to disable wire updates - DONE
// TODO: Disable on invisible - DONE
// TODO: wake up physics if pole moved - DONE
// TODO: simple collision - DONE
// TODO: prewarm - DONE
// TODO: wind - DONE
// TODO: disable on distance (part of lod) - DONE
// TODO: Split code up - DONE
// TODO: external force (like walk rope) so can hang objects - DONE
// TODO: Tidy code - DONE
// TOOD: more options in wire window, ie connections, or option to pick an existing wire setup - DONE
// TODO: If disabled on distance need to wake up if pole moves for a set time - DONE
// TODO: gizmos to show lod levels - DONE
// TODO: distance should be from middle mass - DONE
// TODO: Get stretch working so can tighten up wire - DONE

[System.Serializable]
public class MegaWireLodLevel
{
	public float distance = 1.0f;
	public int sides = 2;
	public float segsperunit = 1.0f;
}

[AddComponentMenu("Procedural/Mega Wire")]
[ExecuteInEditMode]
public class MegaWire : MonoBehaviour
{
	public bool							Rebuild				= false;
	public float						fudge				= 2.0f;
	public MegaWireStrandedMesher		strandedMesher		= new MegaWireStrandedMesher();

	// Physics/Solver params
	public float						spring				= 10.0f;
	public float						damp				= 1.0f;
	public float						timeStep			= 0.01f;
	public float						Mass				= 1.0f;
	public Vector3						gravity				= new Vector3(0.0f, -9.81f, 0.0f);
	public float						airdrag				= 0.99f;
	public float						massRand			= 0.0f;
	public bool							doCollisions		= false;
	public bool							useraycast			= false;
	public LayerMask					collisionmask;
	public float						collisionoff		= 0.0f;
	public float						collisiondist		= 100.0f;
	public float						floor				= 0.0f;
	public int							points				= 10;
	public int							iters				= 4;

	public int							frameWait			= 0;
	public int							frameNum			= 0;

	public bool							stiffnessSprings	= false;
	public float						stiffrate			= 10.0f;
	public float						stiffdamp			= 1.0f;
	public bool							lengthConstraints	= false;

	Matrix4x4							wtm;
	public MegaWireSolver				verletsolver		= new MegaWireSolverVertlet();
	public bool							showphysics			= false;
	public bool							showconnections		= false;
	public bool							showmeshparams		= false;
	public List<MegaWireConnectionDef>	connections			= new List<MegaWireConnectionDef>();
	public List<MegaWireSpan>			spans				= new List<MegaWireSpan>();
	static public Vector3				windDir				= Vector3.forward;
	static public float					windFrc				= 1.0f;
	public float						windEffect			= 1.0f;
	public MegaWireWind					wind;
	public Material						material;

	// Lod values
	public bool							uselod				= false;
	public float						disableDist			= 10.0f;
	public bool							disableOnNotVisible	= false;
	public float						lodreducesides		= 0.5f;
	public float						lodreducesegs		= 0.5f;

	// Will need to copy these objects into the scene
	public GameObject					startObj;
	public float						startAlpha			= 0.0f;
	public Vector3						startRot			= Vector3.zero;
	public Vector3						startOffset			= Vector3.zero;

	public GameObject					endObj;
	public float						endAlpha			= 0.0f;
	public Vector3						endRot				= Vector3.zero;
	public Vector3						endOffset			= Vector3.zero;

	// Lod mesh levels, dist from camera with segs and sides values
	public List<MegaWireLodLevel>		lods				= new List<MegaWireLodLevel>();
	public bool							hidespans			= true;
	public bool							disableOnDistance	= false;
	public float						distfromcamera		= 0.0f;
	public float						rbodyforce			= 10.0f;
	public bool							Enabled				= true;
	public bool							ShowWire			= true;
	public float						awakeTime			= 2.0f;
	public bool							displayGizmo		= true;
	public Color						gizmoColor			= new Color(0.2f, 0.2f, 0.2f, 0.2f);
	public List<Transform>				poles				= new List<Transform>();
	public float						stretch				= 1.0f;
	public float						warmPhysicsTime		= 5.0f;
	public bool							builduvs			= true;

	static public bool					DisableAll			= false;
	public float						startTime			= 4.0f;

	public bool							showWindParams		= false;
	public bool							showPhysicsAdv		= false;
	public bool							showAttach			= false;

	public void Copy(MegaWire from, MegaWireConnectionHelper helper)
	{
		fudge				= from.fudge;
		spring				= from.spring;
		damp				= from.damp;
		timeStep			= from.timeStep;
		Mass				= from.Mass;
		gravity				= from.gravity;
		airdrag				= from.airdrag;
		massRand			= from.massRand;
		points				= from.points;
		iters				= from.iters;
		wind				= from.wind;
		material			= from.material;
		disableOnNotVisible	= from.disableOnNotVisible;
		disableDist			= from.disableDist;
		disableOnDistance	= from.disableOnDistance;
		stretch				= from.stretch;
		awakeTime			= from.awakeTime;
		gizmoColor			= from.gizmoColor;
		displayGizmo		= from.displayGizmo;
		disableOnDistance	= from.disableOnDistance;
		distfromcamera		= from.distfromcamera;
		frameWait			= from.frameWait;
		frameNum			= from.frameNum;
		stiffnessSprings	= from.stiffnessSprings;
		stiffrate			= from.stiffrate;
		stiffdamp			= from.stiffdamp;
		lengthConstraints	= from.lengthConstraints;

		connections.Clear();
		if ( helper )
		{
			for ( int i = 0; i < helper.connections.Count; i++ )
				connections.Add(new MegaWireConnectionDef(helper.connections[i]));
		}
		else
		{
			for ( int i = 0; i < from.connections.Count; i++ )
				connections.Add(new MegaWireConnectionDef(from.connections[i]));
		}

		strandedMesher.Copy(from.strandedMesher);
	}

	public void Create()
	{
		List<GameObject> objs = new List<GameObject>();
		for ( int i = 0; i < poles.Count; i++ )
		{
			objs.Add(poles[i].gameObject);
		}

		Create(this, objs, material, name, null, 1.0f, 1.0f);
	}

	// each wire length should have a very simple script attached that can do onvisible etc
	// to turn off the updates, other than that all updates should be done from MegaWire
	static public MegaWire Create(MegaWire wire, List<GameObject> objs, Material mat, string name, MegaWire copyfrom, float wiresize, float str)
	{
		//MegaWire wire = null;

		if ( objs != null && objs.Count > 1 )
		{
			GameObject newwire = null;

			if ( wire == null )
			{
				newwire = new GameObject();
				newwire.name = name;

				wire = newwire.AddComponent<MegaWire>();

				wire.material = mat;
				wire.stretch = str;
			}
			else
				newwire = wire.gameObject;

			wire.poles.Clear();
			wire.spans.Clear();
			wire.connections.Clear();
			wire.poles.Add(objs[0].transform);
			wire.stretch = str;

			bool hide = true;
			if ( copyfrom )
			{
				hide = copyfrom.hidespans;
			}

			// Make the connections, each connection is a new gameobject child of the wire object
			for ( int i = 0; i < objs.Count - 1; i++ )
			{
				GameObject pole = new GameObject();

				if ( hide )
					pole.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
				pole.name = name + " Span Mesh " + i;

				pole.transform.parent = newwire.transform;

				MegaWireSpan span = pole.AddComponent<MegaWireSpan>();

				span.start = objs[i].transform;
				span.end = objs[i + 1].transform;

				MeshFilter mf = pole.GetComponent<MeshFilter>();

				mf.sharedMesh = new Mesh();
				MeshRenderer mr = pole.GetComponent<MeshRenderer>();

				Material[] mats = new Material[1];
				mats[0] = wire.material;
				mr.sharedMaterials = mats;

				span.mesh = mf.sharedMesh;
				span.mesh.name = name + " Wire Mesh " + i;
				span.Init(wire);
				wire.spans.Add(span);

				wire.poles.Add(objs[i + 1].transform);
			}

			MegaWireConnectionHelper helper = objs[0].GetComponent<MegaWireConnectionHelper>();

			if ( copyfrom )
				wire.Copy(copyfrom, helper);
			else
			{
				// Check if any pole has a helper on it, if so use that
				if ( helper )
				{
					wire.Copy(wire, helper);
				}
				else
				{
					// Add the first connection
					MegaWireConnectionDef con = new MegaWireConnectionDef();
					wire.connections.Add(con);
				}
			}

			if ( wiresize != 1.0f )
			{
				for ( int i = 0; i < wire.connections.Count; i++ )
				{
					wire.connections[i].radius *= wiresize;
				}
			}

			wire.Init();
		}

		return wire;
	}

	public void ChangeStretch(float stretch)
	{
		for ( int i = 0; i < spans.Count; i++ )
		{
			MegaWireSpan span = spans[i];

			for ( int c = 0; c < span.connections.Count; c++ )
			{
				MegaWireConnection con = span.connections[c];
				for ( int s = 0; s < con.springs.Count; s++ )
				{
					con.springs[s].restlen = con.springs[s].initlen * stretch;
				}
			}
		}
	}

	public int GetVertexCount()
	{
		int count = 0;

		for ( int i = 0; i < spans.Count; i++ )
		{
			count += spans[i].vcount;
		}
		return count;
	}

	public void SetHidden(bool hide)
	{
		for ( int i = 0; i < spans.Count; i++ )
		{
			if ( hide )
				spans[i].gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
			else
				spans[i].gameObject.hideFlags = 0;
		}
	}

	public void SetSelection(List<GameObject> objs, bool add)
	{
	}

	public void UpdateOffsets()
	{
		for ( int s = 0; s < spans.Count; s++ )
		{
			MegaWireSpan span = spans[s];

			for ( int c = 0; c < span.connections.Count; c++ )
			{
				MegaWireConnection con = span.connections[c];

				con.inOffset = connections[c].inOffset;
				con.outOffset = connections[c].outOffset;

				for ( int i = 0; i < con.constraints.Count; i++ )
				{
					MegaWireConstraint cn = con.constraints[i];

					if ( cn.GetType() == typeof(MegaWirePointConstraint) )
					{
						MegaWirePointConstraint wpc = (MegaWirePointConstraint)cn;

						if ( wpc.p1 == 0 )
							wpc.offset = con.inOffset;
						else
							wpc.offset = con.outOffset;
					}
				}
			}
		}
	}

	public void Init()
	{
		for ( int s = 0; s < spans.Count; s++ )
		{
			MegaWireSpan span = spans[s];

			span.connections.Clear();

			for ( int c = 0; c < connections.Count; c++ )
			{
				MegaWireConnection con = new MegaWireConnection();

				con.start = span.start;
				con.end = span.end;
				con.inOffset = connections[c].inOffset;
				con.outOffset = connections[c].outOffset;
				con.radius = connections[c].radius;

				span.connections.Add(con);

				con.Init(this);
			}
		}
	}

	void WireUpdate(float t)
	{
		float time = 0.0f;

		if ( !Application.isPlaying )
			time = 0.0f;
		else
			time = Time.deltaTime * fudge;

		if ( time > 0.05f )
			time = 0.05f;

		Vector3 campos = Vector3.zero;
		
		if ( Camera.main != null ) 
			campos = Camera.main.transform.position;

		float dsqr = disableDist * disableDist;

		// Frame wait
		bool	allow = true;

		if ( Application.isPlaying )
		{
			if ( frameWait > 0 )
			{
				frameNum++;
				if ( frameNum < frameWait )
					allow = false;
				else
					frameNum = 0;
			}
		}

		bool startmode = false;
		if ( startTime > 0.0f )
		{
			startTime -= Time.deltaTime;
			startmode = true;
		}

		// Update all the wind forces and lod values
		for ( int s = 0; s < spans.Count; s++ )
		{
			Vector3 pos = ((spans[s].start.position + spans[s].end.position) * 0.5f);

			if ( disableOnDistance )
			{
				Vector3 mp = (spans[s].connections[0].masses[0].pos + spans[s].connections[0].masses[spans[s].connections[0].masses.Count - 1].pos) * 0.5f;
				distfromcamera = Vector3.SqrMagnitude(mp - campos);
				if ( distfromcamera < dsqr )
					spans[s].AllowUpdates = true;
				else
					spans[s].AllowUpdates = false;
			}
			else
				spans[s].AllowUpdates = true;

			spans[s].buildmesh = false;
			if ( spans[s].AllowUpdates )
			{
				if ( disableOnNotVisible )
				{
					if ( spans[s].visible )
						spans[s].buildmesh = true;
				}
				else
					spans[s].buildmesh = true;
			}

			// Turn off of framedelay says no
			if ( !allow )
				spans[s].buildmesh = false;

			if ( startmode )
				spans[s].buildmesh = true;

			if ( spans[s].time > 0.0f )
			{
				spans[s].buildmesh = true;
				spans[s].time -= Time.deltaTime;
			}

			if ( wind )
			{
				for ( int c = 0; c < spans[s].connections.Count; c++ )
				{
					Vector3 wpos = pos + spans[s].connections[c].inOffset;
					Vector3 force = wind.Force(wpos);	//spans[s].transform.position);

					spans[s].connections[c].windFrc = force * windEffect;
				}
			}
			else
			{
				Vector3 force = windFrc * windDir;
				for ( int c = 0; c < spans[s].connections.Count; c++ )
					spans[s].connections[c].windFrc = force * windEffect;
			}
		}

		while ( time > 0.0f )
		{
			for ( int s = 0; s < spans.Count; s++ )
			{
				if ( spans[s].buildmesh )
					spans[s].UpdateSpan(this, timeStep);
			}

			time -= timeStep;
		}
	}

	public void RunPhysics(float t)
	{
		RebuildWire();
		float time = t;

		if ( spans.Count >= 1 )
		{
			Matrix4x4 stm = spans[0].start.transform.localToWorldMatrix;
			for ( int i = 0; i < spans.Count; i++ )
			{
				Matrix4x4 etm = spans[i].end.transform.localToWorldMatrix;

				//spans[i].buildmesh = true;
				//spans[i].AllowUpdates = true;
				spans[i].startTm = stm;
				spans[i].endTm = etm;
				spans[i].time = 0.0f;
				stm = etm;
			}

			while ( time > 0.0f )
			{
				for ( int s = 0; s < spans.Count; s++ )
					spans[s].UpdateSpan(this, timeStep);

				time -= timeStep;
			}

			for ( int s = 0; s < spans.Count; s++ )
				spans[s].MoveMasses(this);

			for ( int s = 0; s < spans.Count; s++ )
				spans[s].BuildMesh(this);
		}
	}

	public Vector3 SetWeight(float alpha, int strand, float weight, bool snap)
	{
		alpha = Mathf.Clamp(alpha, 0.0f, 0.9999f);
		float fa = alpha * (float)spans.Count;
		int sindex = (int)fa;	//(alpha * (float)spans.Count);
		float aa = fa - (float)sindex;

		MegaWireSpan span = spans[sindex];
		MegaWireConnection con = span.connections[strand];

		// may need to knock 2 of as have sentry masses
		float ma1 = aa * (float)(con.masses.Count - 1);
		int i = (int)ma1 + 1;	//(aa * (float)(con.masses.Count - 2)) + 1;
		float u = ma1 - (float)(i - 1);
		if ( snap )
		{
			if ( u > 0.5f )
				u = 0.999f;
			else
				u = 0.0f;
		}

		Vector3 a = con.masspos[i - 1];
		Vector3 b = con.masspos[i];
		Vector3 c = con.masspos[i + 1];
		Vector3 d = con.masspos[i + 2];

		Vector3 frc = Vector3.zero;
		frc.y = -weight * (1.0f - u);

		con.masses[i - 1].forcec = frc;

		frc.y = -weight * u;
		con.masses[i].forcec = frc;

		return 0.5f * ((-a + 3.0f * b - 3.0f * c + d) * (u * u * u) + (2.0f * a - 5.0f * b + 4.0f * c - d) * (u * u) + (-a + c) * u + 2.0f * b);
	}

	public Vector3 GetPos(float alpha, int strand, bool snap)
	{
		alpha = Mathf.Clamp(alpha, 0.0f, 0.9999f);
		float fa = alpha * (float)spans.Count;
		int sindex = (int)fa;	//(alpha * (float)spans.Count);
		float aa = fa - (float)sindex;

		MegaWireSpan span = spans[sindex];
		MegaWireConnection con = span.connections[strand];

		// may need to knock 2 of as have sentry masses
		float ma1 = aa * (float)(con.masses.Count - 1);
		int i = (int)ma1 + 1;	//(aa * (float)(con.masses.Count - 2)) + 1;
		float u = ma1 - (float)(i - 1);
		if ( snap )
		{
			if ( u > 0.5f )
				u = 0.999f;
			else
				u = 0.0f;
		}

		Vector3 a = con.masspos[i - 1];
		Vector3 b = con.masspos[i];
		Vector3 c = con.masspos[i + 1];
		Vector3 d = con.masspos[i + 2];

		return 0.5f * ((-a + 3.0f * b - 3.0f * c + d) * (u * u * u) + (2.0f * a - 5.0f * b + 4.0f * c - d) * (u * u) + (-a + c) * u + 2.0f * b);
	}

	void Start()
	{
		if ( spans.Count > 0 && spans[0].mesh == null )
		{
			Create();
			if ( warmPhysicsTime > 0.0f )
			{
				RunPhysics(warmPhysicsTime);
			}
		}
	}

	public void SetWireVisible(bool show)
	{
		for ( int i = 0; i < spans.Count; i++ )
		{
#if UNITY_3_5
			spans[i].gameObject.SetActiveRecursively(show);
#else
			spans[i].gameObject.SetActive(show);
#endif
		}
	}

	[ContextMenu("Rebuild Wire")]
	public void RebuildWire()
	{
		Init();
		for ( int s = 0; s < spans.Count; s++ )
			spans[s].MoveMasses(this);

		for ( int s = 0; s < spans.Count; s++ )
			spans[s].BuildMesh(this);
	}

	void LateUpdate()
	{
		if ( !Enabled || DisableAll )
			return;

		Matrix4x4 stm = Matrix4x4.identity;
		
		if ( spans.Count > 0 )
			stm = spans[0].start.transform.localToWorldMatrix;

		// We should only be doing this check if the span is asleep
		for ( int i = 0; i < spans.Count; i++ )
		{
			Matrix4x4 etm = spans[i].end.transform.localToWorldMatrix;

			if ( stm != spans[i].startTm )
			{
				spans[i].startTm = stm;
				spans[i].time = awakeTime;
			}

			if ( etm != spans[i].endTm )
			{
				spans[i].endTm = etm;
				spans[i].time = awakeTime;
			}

			stm = etm;
		}

		if ( Rebuild )
		{
			Rebuild = false;
			RebuildWire();
		}

		WireUpdate(timeStep);

		for ( int s = 0; s < spans.Count; s++ )
		{
			if ( spans[s].buildmesh )
			{
				spans[s].MoveMasses(this);
				spans[s].BuildMesh(this);
				spans[s].buildmesh = false;
			}
		}

		builduvs = false;
	}
}
