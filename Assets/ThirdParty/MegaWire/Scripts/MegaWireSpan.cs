 
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class MegaWireSpan : MonoBehaviour
{
	public bool			visible			= true;
	public bool			on				= true;	// can turn off spans?
	public Transform	start;
	public Transform	end;
	public List<MegaWireConnection>	connections = new List<MegaWireConnection>();
	public Mesh			mesh			= null;
	public float		WireLength;
	public bool			AllowUpdates	= true;
	public bool			buildmesh		= false;
	public float		time			= 0.0f;	// If > 0.0f then run physics regardless (pole moved)
	public Matrix4x4	startTm;
	public Matrix4x4	endTm;
	public int			vcount;
	public Vector3[]	verts;
	public Vector2[]	uvs;
	public int[]		tris;
	public Vector3[]	norms;

	void OnBecameVisible()
	{
		visible = true;
	}

	void OnBecameInvisible()
	{
		visible = false;
	}

	public void Init(MegaWire wire)
	{
		if ( start && end )
		{
			WireLength = Vector3.Distance(start.position, end.position);
			for ( int i = 0; i < connections.Count; i++ )
				connections[i].Init(wire);
		}
	}

	public void UpdateSpan(MegaWire wire, float ts)
	{
		for ( int i = 0; i < connections.Count; i++ )
			connections[i].Update(wire, ts);
	}

	public void MoveMasses(MegaWire wire)
	{
		for ( int i = 0; i < connections.Count; i++ )
			connections[i].MoveMasses(wire);
	}

	public void BuildMesh(MegaWire wire)
	{
		wire.strandedMesher.BuildMesh(wire, this);
	}
}
