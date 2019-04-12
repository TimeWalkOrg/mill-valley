using UnityEngine;
using System.Collections;
using Jacovone;

/// <summary>
/// An utility class to smooth move to a specific waypoint. This class was made to
/// show how to work with waypoints by scripting.
/// </summary>
[RequireComponent (typeof(PathMagic))]
public class GotoWaypoint : MonoBehaviour
{
	// The requested waypoint
	public int requestedWaypoint = 0;

	// Cache copy of the PathMagic instance
	PathMagic pathMagic;

	// Cache of last requested waypoint to detect changes
	private int lastRequestedWaypoint = -1;

	// Computed requested pos for specific requested waypoint
	private float lastRequestedPos = 0;

	// Use this for initialization
	void Start ()
	{
		pathMagic = GetComponent<PathMagic> ();
		requestedWaypoint = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lastRequestedWaypoint != requestedWaypoint) {
			lastRequestedWaypoint = requestedWaypoint;
			lastRequestedPos = ComputePosForWaypoint (pathMagic, lastRequestedWaypoint);
		}

		pathMagic.CurrentPos = Mathf.Lerp (pathMagic.CurrentPos, lastRequestedPos, 0.1f);
	}

	/// <summary>
	/// Calculates the float position ([0..1])of a waypoint related to the whole path. This implementation
	/// does not take account of effective distances from waypoints but only waypoints number.
	/// </summary>
	/// <returns>The position for waypoint index.</returns>
	/// <param name="index">The reference index.</param>
	private float CalcPosForWaypointIndex (PathMagic pm, int index)
	{
		return (float)index / (pm.waypoints.Length - (pm.loop ? 0f : 1f));
	}

	/// <summary>
	/// Computes the position for a specific waypoint. This implementation takes account of the effective
	/// distances from waypoints. The implementation takes also account of the fact that the path is
	/// pre-sampled or not.
	/// </summary>
	/// <returns>The position for waypoint.</returns>
	/// <param name="waypoint">Waypoint.</param>
	public float ComputePosForWaypoint (PathMagic pm, int waypoint)
	{
		float pos = 0f;
		float step = 0.0001f;

		if (!pm.presampledPath) {
			// Compute the pos to the minWaypoint in non-pre-sampled path
			pos = CalcPosForWaypointIndex (pm, waypoint);
		} else {
			// Compute the pos to the minWaypoint in pre-sampled path
			int i = 0;
			while (pm.WaypointSamples [i] != waypoint) {
				pos += pm.SamplesDistances [i++];
			}

			pos /= pm.TotalDistance;

			float p = pos;
			Vector3 position;
			Quaternion rotation;
			float vel;
			int wp;
			float lastDistanceFromWaypoint;

			pm.sampledPositionAndRotationAndVelocityAndWaypointAtPos (p, out position, out rotation, out vel, out wp);

			do {
				lastDistanceFromWaypoint = Vector3.Distance (position, pm.Waypoints [waypoint].Position);

				p += step;
				if (p > 1f)
					p = 1f;

				pm.sampledPositionAndRotationAndVelocityAndWaypointAtPos (p, out position, out rotation, out vel, out wp);
			} while(Vector3.Distance (position, pm.Waypoints [waypoint].Position) <= lastDistanceFromWaypoint && p < 1);

			pos = p;
		}

		return pos;
	}
}
