using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Jacovone.Meshes
{
	public class PathMagicRoad : PathMagicMesh
	{
		/// <summary>
		/// The road will be closed in the front face?
		/// </summary>
		public bool closeFront = false;

		/// <summary>
		/// The road will be closed in the back face?
		/// </summary>
		public bool closeBack = false;

		/// <summary>
		/// The type of the width. Can be a fixed float or
		/// a AnimationCurve.
		/// </summary>
		public WidthType widthType;

		/// <summary>
		/// Radius type.
		/// </summary>
		public enum WidthType
		{
			Constant,
			Curve
		}

		/// <summary>
		/// The width of the road in case of widthType is set to Constant.
		/// </summary>
		public float width = 1f;

		/// <summary>
		/// The height of the road.
		/// </summary>
		public float height = 0.3f;

		/// <summary>
		/// The width curve in case of widthType is set to Curve.
		/// </summary>
		public AnimationCurve widthCurve;

		/// <summary>
		/// The collection of vertices suitable to generate triangles to close the front face of the torus
		/// </summary>
		protected List<int> frontVertices;

		/// <summary>
		/// The collection of vertices suitable to generate triangles to close the back face of the torus
		/// </summary>
		protected List<int> backVertices;

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
		/// Abstract method. It will be called to give the possibility to initialize variables.
		/// </summary>
		override protected void InitializeMesh ()
		{
			frontVertices = new List<int> (sections);
			backVertices = new List<int> (sections);
		}

		/// <summary>
		/// Abstract method. It will be called to finalize the mesh, such as close the mesh at the end.
		/// </summary>
		override protected void FinalizeMesh ()
		{
			if (closeFront) {
				int a = frontVertices [0], 
				b = frontVertices [1], 
				c = frontVertices [2],
				d = frontVertices [3];

				// Dup vertices
				Vector3 pa = vertices [a];
				Vector3 pb = vertices [b];
				Vector3 pc = vertices [c];
				Vector3 pd = vertices [d];

				vertices.Add (pa);
				UVs.Add (new Vector2 (0, 0));
				normals.Add (frontNormal);
				a = vertices.Count - 1;
				vertices.Add (pb);
				UVs.Add (new Vector2 (1, 0));
				normals.Add (frontNormal);
				b = vertices.Count - 1;
				vertices.Add (pc);
				UVs.Add (new Vector2 (1, 1));
				normals.Add (frontNormal);
				c = vertices.Count - 1;
				vertices.Add (pd);
				UVs.Add (new Vector2 (0, 1));
				normals.Add (frontNormal);
				d = vertices.Count - 1;

				AddQuad3 (c, b, d, a);
			}

			if (closeBack) {
				int a = backVertices [0], 
				b = backVertices [1], 
				c = backVertices [2],
				d = backVertices [3];

				// Dup vertices
				Vector3 pa = vertices [a];
				Vector3 pb = vertices [b];
				Vector3 pc = vertices [c];
				Vector3 pd = vertices [d];

				vertices.Add (pa);
				UVs.Add (new Vector2 (0, 0));
				normals.Add (backNormal);
				a = vertices.Count - 1;
				vertices.Add (pb);
				UVs.Add (new Vector2 (1, 0));
				normals.Add (backNormal);
				b = vertices.Count - 1;
				vertices.Add (pc);
				UVs.Add (new Vector2 (1, 1));
				normals.Add (backNormal);
				c = vertices.Count - 1;
				vertices.Add (pd);
				UVs.Add (new Vector2 (0, 1));
				normals.Add (backNormal);
				d = vertices.Count - 1;

				AddQuad3 (a, b, d, c);
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
			if (widthType == WidthType.Constant)
				r = width;
			else {

				float minTime = float.MaxValue;
				float maxTime = float.MinValue;

				for (int i = 0; i < widthCurve.length; i++) {
					float time = widthCurve.keys [i].time;
					if (time > maxTime)
						maxTime = time;
					if (time < minTime)
						minTime = time;
				}

				float timeSpan = maxTime - minTime;
				float evalTime = minTime + ((float)piece / (float)pieces) * timeSpan;

				r = widthCurve.Evaluate (evalTime);
			}

			// Generate first Vertex
			Vector3 point = position + r / 2 * xd - height * Vector3.up;
			vertices.Add (transform.InverseTransformPoint (point));

			// Generate UVs
			UVs.Add (new Vector2 (0, piece * 1f / (float)(pieces)));

			// Generate normals
			Vector3 normal = flipped ? xd : -xd;
			normals.Add (normal);

			if (piece == 0) {
				frontVertices.Add (vertices.Count - 1);
				frontNormal = flipped ? direction : -direction;
			} else if (piece == pieces) {
				backVertices.Add (vertices.Count - 1);
				backNormal = flipped ? -direction : direction;
			}

			// Generate second vertex
			point = position + r / 2 * xd;
			vertices.Add (transform.InverseTransformPoint (point));

			// Generate UVs
			UVs.Add (new Vector2 (1, piece * 1f / (float)(pieces)));

			// Generate normals
			normal = flipped ? xd : -xd;
			normals.Add (normal);

			if (piece == 0) {
				frontVertices.Add (vertices.Count - 1);
				frontNormal = flipped ? direction : -direction;
			} else if (piece == pieces) {
				backVertices.Add (vertices.Count - 1);
				backNormal = flipped ? -direction : direction;
			}

			for (int j = 0; j <= sections; j++) {

				// Generate vertex
				point = position + (r / 2f - (float)j / (float)sections * r) * xd;
				vertices.Add (transform.InverseTransformPoint (point));

				// Generate UVs
				UVs.Add (new Vector2 ((float)j / (float)sections, piece * 1f / (float)(pieces)));

				// Generate normals
				normal = flipped ? yd : -yd;
				normals.Add (normal);
			}

			// Generate first vertex
			point = position - r / 2 * xd;
			vertices.Add (transform.InverseTransformPoint (point));

			// Generate UVs
			UVs.Add (new Vector2 (1, piece * 1f / (float)(pieces)));

			// Generate normals
			normal = flipped ? -xd : xd;
			normals.Add (normal);

			if (piece == 0) {
				frontVertices.Add (vertices.Count - 1);
				frontNormal = flipped ? direction : -direction;
			} else if (piece == pieces) {
				backVertices.Add (vertices.Count - 1);
				backNormal = flipped ? -direction : direction;
			}

			// Generate second Vertex
			point = position - r / 2 * xd - height * Vector3.up;
			vertices.Add (transform.InverseTransformPoint (point));

			// Generate UVs
			UVs.Add (new Vector2 (0, piece * 1f / (float)(pieces)));

			// Generate normals
			normal = flipped ? -xd : xd;
			normals.Add (normal);

			if (piece == 0) {
				frontVertices.Add (vertices.Count - 1);
				frontNormal = flipped ? direction : -direction;
			} else if (piece == pieces) {
				backVertices.Add (vertices.Count - 1);
				backNormal = flipped ? -direction : direction;
			}

			// Add triangles
			if (piece > 0) {
				int baseIndex = piece * (sections + 5);

				int a = baseIndex;
				int b = baseIndex - (sections + 5);
				int c = baseIndex + 1;
				int d = baseIndex + 1 - (sections + 5);

				AddQuad2 (a, b, c, d);

				for (int j = 2; j < sections + 2; j++) {

					a = baseIndex + j;
					b = baseIndex + j - (sections + 5);
					c = baseIndex + j + 1;
					d = baseIndex + j + 1 - (sections + 5);

					AddQuad1 (a, b, c, d);
				}

				a = baseIndex + sections + 3;
				b = baseIndex + sections + 3 - (sections + 5);
				c = baseIndex + sections + 3 + 1;
				d = baseIndex + sections + 3 + 1 - (sections + 5);

				AddQuad2 (a, b, c, d);
			}
		}

		/// <summary>
		/// Gets the number of materials constructed by the mesh generator.
		/// Default implementation returns 1.
		/// </summary>
		/// <returns>The number of materials.</returns>
		protected override int GetNumberOfMaterials ()
		{
			return 3;
		}
	}
}
