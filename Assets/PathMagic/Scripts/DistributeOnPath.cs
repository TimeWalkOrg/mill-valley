using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Jacovone
{
	[ExecuteInEditMode]
	public class DistributeOnPath : MonoBehaviour
	{
		public GameObject target;

		public int count = 0;

		public float startingFrom;
		public float endTo;

		public PathMagic path;

		// Use this for initialization
		void Awake ()
		{
		}

		public void Generate ()
		{
			if (count < 0)
				count = 0;

			List<GameObject> elements = new List<GameObject> ();
			for (int i = 0; i < count; i++) {
				if (i < transform.childCount) {
					elements.Insert (i, transform.GetChild (i).gameObject);
				} else {
					elements.Insert (i, Instantiate (target));
					elements [i].transform.parent = transform;
					elements [i].transform.localScale = new Vector3 (1, 1, 1);
				}
			}

			if (elements.Count < transform.childCount) {
				for (int i = transform.childCount - 1; i >= elements.Count; i--) {
					DestroyImmediate (transform.GetChild (i).gameObject);
				}
			}

			for (int i = 0; i < elements.Count; i++) {

				float pos = startingFrom + (float)(1f / count * i) * (endTo - startingFrom);

				Vector3 position;
				Quaternion rotation;

				if (path.PresampledPath) {

					float velocity;
					int waypoint;

					path.sampledPositionAndRotationAndVelocityAndWaypointAtPos (
						pos,
						out position,
						out rotation,
						out velocity,
						out waypoint);


				} else {

					position = path.computePositionAtPos (pos);

					rotation = path.computeRotationAtPos (pos);
				}

				elements [i].transform.position = path.transform.TransformPoint (
					position
				);

				elements [i].transform.rotation = transform.rotation * rotation;
			}
		}

		#if UNITY_EDITOR
		public void Update ()
		{
			if (!Application.isPlaying)
				Generate ();
		}
		#endif
	}
}
