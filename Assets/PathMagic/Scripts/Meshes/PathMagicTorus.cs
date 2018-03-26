using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Jacovone.Meshes
{
	public class PathMagicTorus : PathMagicMesh
	{
		/// <summary>
		/// The torus will be closed in the front face?
		/// </summary>
		public bool closeFront = false;

		/// <summary>
		/// The torus will be closed in the back face?
		/// </summary>
		public bool closeBack = false;

		/// <summary>
		/// The type of the radius. Can be a fixed float or
		/// a AnimationCurve.
		/// </summary>
		public RadiusType radiusType;

		/// <summary>
		/// Radius type.
		/// </summary>
		public enum RadiusType
		{
			Constant,
			Curve
		}

		/// <summary>
		/// The radius of the torus in case of radiusType is set to Constant.
		/// </summary>
		public float radius = 1f;

		/// <summary>
		/// The radius curve in case of radiusType is set to Curve.
		/// </summary>
		public AnimationCurve radiusCurve;

		/// <summary>
		/// The angle offset. Single sections will be rotated by a value proportional to this value;
		/// </summary>
		public float angleOffset = 0f;

		/// <summary>
		/// The twist. At each piece, the torus will be rotated by an angle proportional
		/// to this parameter.
		/// </summary>
		public float twist = 0f;

		/// <summary>
		/// The collection of vertices suitable to generate triangles to close the front face of the torus
		/// </summary>
		protected List<int> frontVertices;

		/// <summary>
		/// The collection of vertices suitable to generate triangles to close the back face of the torus
		/// </summary>
		protected List<int> backVertices;

		/// <summary>
		/// The current twist amount. Used during the generation process.
		/// </summary>
		private float fTwist;

		/// <summary>
		/// The front normal. Stored to assign this normal direction
		/// to the front face (if there is one)
		/// </summary>
		private Vector3 frontNormal;

		/// <summary>
		/// The back normal. Stored to assign this normal direction
		/// to the back face (if there is one)
		/// </summary>
		private Vector3 backNormal;

		/// <summary>
		/// Position of the center of the front plane in world coordinates
		/// </summary>
		private Vector3 frontPosition;

		/// <summary>
		/// Position of the center of the back plane in world coordinates
		/// </summary>
		private Vector3 backPosition;

		/// <summary>
		/// Abstract method. It will be called to give the possibility to initialize variables.
		/// </summary>
		override protected void InitializeMesh ()
		{
			fTwist = 0f;
			frontVertices = new List<int> (sections);
			backVertices = new List<int> (sections);
		}

		/// <summary>
		/// Abstract method. It will be called to finalize the mesh, such as close the mesh at the end.
		/// </summary>
		override protected void FinalizeMesh ()
		{
			if (closeFront) {
				for (int j = 0; j < frontVertices.Count - 1; j++) {

					int a = frontVertices [0], 
					b = frontVertices [j + 1], 
					c = frontVertices [j];

					// Dup vertices
					Vector3 pa = vertices [a];
					Vector3 pb = vertices [b];
					Vector3 pc = vertices [c];

					vertices.Add (pa);
					UVs.Add (pa - frontPosition);
					normals.Add (frontNormal);
					a = vertices.Count - 1;
					vertices.Add (pb);
					UVs.Add (pb - frontPosition);
					normals.Add (frontNormal);
					b = vertices.Count - 1;
					vertices.Add (pc);
					UVs.Add (pc - frontPosition);
					normals.Add (frontNormal);
					c = vertices.Count - 1;

					if (flipped) {
						triangles2.Add (c);
						triangles2.Add (b);
						triangles2.Add (a);
					} else {
						triangles2.Add (a);
						triangles2.Add (b);
						triangles2.Add (c);
					}
				}
			}

			if (closeBack) {
				for (int j = 0; j < backVertices.Count - 1; j++) {

					int a = backVertices [0], 
					b = backVertices [j + 1], 
					c = backVertices [j];

					// Dup vertices
					Vector3 pa = vertices [a];
					Vector3 pb = vertices [b];
					Vector3 pc = vertices [c];

					vertices.Add (pa);
					UVs.Add (pa - backPosition);
					normals.Add (backNormal);
					a = vertices.Count - 1;
					vertices.Add (pb);
					UVs.Add (pb - backPosition);
					normals.Add (backNormal);
					b = vertices.Count - 1;
					vertices.Add (pc);
					UVs.Add (pc - backPosition);
					normals.Add (backNormal);
					c = vertices.Count - 1;

					if (flipped) {
						triangles2.Add (a);
						triangles2.Add (b);
						triangles2.Add (c);
					} else {
						triangles2.Add (c);
						triangles2.Add (b);
						triangles2.Add (a);
					}
				}
			}
		}

		/// <summary>
		/// Generates a single mesh part.
		/// </summary>
		/// <param name="piece">The piece. For each piece, this method is called.</param>
		/// <param name="position">The current position on the path.</param>
		/// <param name="direction">The current direction at the position on the path.</param>
		/// <param name="xd">This vector indicates an ortogonal direction to the path direction.</param>
		/// <param name="yd">This vector indicates an ortogonal direction to the direction and the xd vector.
		/// Use xd and yd as a plane ortogonal to the direction.</param>
		override protected void GenerateMeshPart (int piece, Vector3 position, Vector3 direction, Vector3 xd, Vector3 yd)
		{
			float r;
			if (radiusType == RadiusType.Constant)
				r = radius;
			else {

				float minTime = float.MaxValue;
				float maxTime = float.MinValue;

				for (int i = 0; i < radiusCurve.length; i++) {
					float time = radiusCurve.keys [i].time;
					if (time > maxTime)
						maxTime = time;
					if (time < minTime)
						minTime = time;
				}

				float timeSpan = maxTime - minTime;
				float evalTime = minTime + ((float)piece / (float)pieces) * timeSpan;

				r = radiusCurve.Evaluate (evalTime);
			}

			for (int j = 0; j <= sections; j++) {
				float angle = Mathf.Deg2Rad * angleOffset + fTwist + j * 2 * Mathf.PI / sections;

				// Generate vertex
				Vector3 point = position + r * Mathf.Sin (angle) * xd + r * Mathf.Cos (angle) * yd;
				vertices.Add (transform.InverseTransformPoint (point));

				// Generate UVs
				UVs.Add (new Vector2 (j * 1f / (float)(sections), piece * 1f / (float)(pieces)));

				// Generate normals
				Vector3 normal = flipped ? (position - point).normalized : (point - position).normalized;
				normals.Add (normal);

				if (piece == 0) {
					frontVertices.Add (vertices.Count - 1);
					frontNormal = flipped ? direction : -direction;
					frontPosition = transform.InverseTransformPoint (position);
				} else if (piece == pieces) {
					backVertices.Add (vertices.Count - 1);
					backNormal = flipped ? -direction : direction;
					backPosition = transform.InverseTransformPoint (position);
				}
			}

			fTwist += twist / 100;

			// Add triangles
			if (piece > 0) {

				for (int j = 0; j < sections; j++) {

					int baseIndex = piece * (sections + 1);

					int a = baseIndex + j;
					int b = baseIndex + j - (sections + 1);
					int c = baseIndex + j + 1;
					int d = baseIndex + j + 1 - (sections + 1);

					AddQuad1 (a, b, c, d);
				}
			}
		}


		/// <summary>
		/// Gets the number of materials constructed by the mesh generator.
		/// Default implementation returns 1.
		/// </summary>
		/// <returns>The number of materials.</returns>
		protected override int GetNumberOfMaterials ()
		{
			return 2;
		}
	}
}
