using UnityEngine;

public class MegaWireOrigin : MonoBehaviour
{
	Vector3			position;
	MegaWireSpan[]	spans;
	MegaWire		wire;

	void Start()
	{
		position	= transform.position;
		spans		= GetComponentsInChildren<MegaWireSpan>();
		wire		= GetComponentInChildren<MegaWire>();
	}

	void Update()
	{
		if ( transform.position != position )
		{
			Vector3 delta = transform.position - position;

			for ( int i = 0; i < spans.Length; i++ )
			{
				MegaWireSpan span = spans[i];

				for ( int c = 0; c < span.connections.Count; c++ )
				{
					MegaWireConnection con = span.connections[c];

					for ( int m = 0; m < con.masses.Count; m++ )
					{
						con.masses[m].pos += delta;
						con.masses[m].last = con.masses[m].pos;
					}
				}
			}

			wire.transform.position = wire.transform.position - delta;
			position = transform.position;
		}
	}
}