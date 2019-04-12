using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Jacovone.Meshes
{
	/// <summary>
	/// A base class for all PathMagic mesh classes. Mesh classes use
	/// one or more PathMagic to generate meshes procedurally
	/// </summary>
	[RequireComponent (typeof(MeshRenderer))]
	[RequireComponent (typeof(MeshFilter))]
	[ExecuteInEditMode]
	public abstract class PathMagicMesh : MonoBehaviour
	{
		/// <summary>
		/// The MeshFilter component (required)
		/// </summary>
		private MeshFilter _mf;

		/// <summary>
		/// The mesh.
		/// </summary>
		public Mesh mesh;

		/// <summary>
		/// The first connected PathMagic.
		/// </summary>
		public PathMagic path;

		/// <summary>
		/// A standard property. Defines the start point of the path to generate the mesh.
		/// </summary>
		public float startingFrom = 0f;

		/// <summary>
		/// A standard property. Defines the end point of the path to generate the mesh.
		/// </summary>
		public float endTo = 1f;

		/// <summary>
		/// A standard property. Defines the number of pieces in which the path will be subdivided.
		/// In other words it defines the "definition" in terms of precision.
		/// </summary>
		public int pieces = 20;

		/// <summary>
		/// At each step (piece), will be generated a single section of the mesh.
		/// It makes sense for a specified mesh.
		/// </summary>
		public int sections = 5;

		/// <summary>
		/// Generaet a flipped version of the mesh?
		/// </summary>
		public bool flipped = false;

		/// <summary>
		/// Protected structure to store generated vertices.
		/// </summary>
		protected List<Vector3> vertices;

		/// <summary>
		/// Protected structure to store generated triangles.
		/// </summary>
		protected List<int> triangles;

		/// <summary>
		/// Protected structure to store generated triangles for the second submesh.
		/// </summary>
		protected List<int> triangles2;

		/// <summary>
		/// Protected structure to store generated triangles for the third submesh.
		/// </summary>
		protected List<int> triangles3;

		/// <summary>
		/// Protected structure to store generated normals.
		/// </summary>
		protected List<Vector3> normals;

		/// <summary>
		/// Protected structure to store UV coordinates.
		/// </summary>
		protected List<Vector2> UVs;

		/// <summary>
		/// Auto update mesh when path is modified. Only in edito mode.
		/// </summary>
		public bool autoUpdateMesh = true;

		/// <summary>
		/// The main generate method. It make all standard passes, and then calls 
		/// a virtual method overridden in subclasses.
		/// </summary>
		public void Generate ()
		{
			if (path == null) {
				Debug.LogWarning ("Please assign path attribute!");
				return;
			}

			if (pieces < 3)
				pieces = 3;
			if (sections < 2)
				sections = 2;

			_mf = GetComponent<MeshFilter> ();
			mesh = _mf.sharedMesh;
			if (mesh == null) {
				mesh = _mf.sharedMesh = new Mesh ();
			}

			if (mesh == null)
				mesh = new Mesh ();

			mesh.Clear ();

			vertices = new List<Vector3> ();
			normals = new List<Vector3> ();
			triangles = new List<int> ();
			triangles2 = new List<int> ();
			triangles3 = new List<int> ();
			UVs = new List<Vector2> ();

			InitializeMesh ();

			for (int piece = 0; piece <= pieces; piece++) {

				float pos = startingFrom + (float)(1f / pieces * piece) * (endTo - startingFrom);

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

					position = path.transform.TransformPoint (position);
					rotation = path.transform.rotation * rotation;
				} else {

					position = path.transform.TransformPoint (
						path.computePositionAtPos (pos)
					);

					rotation = path.transform.rotation * path.computeRotationAtPos (pos);
				}

				Vector3 direction = rotation * Vector3.forward;
				Vector3 xd = (rotation * Vector3.right).normalized;
				Vector3 yd = (rotation * -Vector3.up).normalized;

				GenerateMeshPart (piece, position, direction, xd, yd);
			}

			FinalizeMesh ();

			mesh.vertices = vertices.ToArray ();
			mesh.normals = normals.ToArray ();
			mesh.subMeshCount = GetNumberOfMaterials ();
			mesh.uv = UVs.ToArray ();

			if (GetNumberOfMaterials () >= 1)
				mesh.SetTriangles (triangles, 0);
			if (GetNumberOfMaterials () >= 2)
				mesh.SetTriangles (triangles2, 1);
			if (GetNumberOfMaterials () >= 3)
				mesh.SetTriangles (triangles3, 2);

			mesh.RecalculateBounds ();
		}

		#if UNITY_EDITOR
		public void Update ()
		{
			if (!Application.isPlaying && autoUpdateMesh)
				Generate ();
		}
		#endif

		/// <summary>
		/// Adds a quad to the triangles1.
		/// </summary>
		/// <param name="a">First vertex index</param>
		/// <param name="b">Second vertex index</param>
		/// <param name="c">Third vertex index</param>
		/// <param name="d">Fourth vertex index</param>
		protected void AddQuad1 (int a, int b, int c, int d)
		{
			if (flipped) {
				triangles.Add (c);
				triangles.Add (b);
				triangles.Add (a);

				triangles.Add (d);
				triangles.Add (b);
				triangles.Add (c);
			} else {
				triangles.Add (a);
				triangles.Add (b);
				triangles.Add (c);

				triangles.Add (c);
				triangles.Add (b);
				triangles.Add (d);
			}
		}

		/// <summary>
		/// Adds a quad to the triangles1.
		/// </summary>
		/// <param name="a">First vertex index</param>
		/// <param name="b">Second vertex index</param>
		/// <param name="c">Third vertex index</param>
		/// <param name="d">Fourth vertex index</param>
		protected void AddQuad2 (int a, int b, int c, int d)
		{
			if (flipped) {
				triangles2.Add (c);
				triangles2.Add (b);
				triangles2.Add (a);

				triangles2.Add (d);
				triangles2.Add (b);
				triangles2.Add (c);
			} else {
				triangles2.Add (a);
				triangles2.Add (b);
				triangles2.Add (c);

				triangles2.Add (c);
				triangles2.Add (b);
				triangles2.Add (d);
			}
		}

		/// <summary>
		/// Adds a quad to the triangles1.
		/// </summary>
		/// <param name="a">First vertex index</param>
		/// <param name="b">Second vertex index</param>
		/// <param name="c">Third vertex index</param>
		/// <param name="d">Fourth vertex index</param>
		protected void AddQuad3 (int a, int b, int c, int d)
		{
			if (flipped) {
				triangles3.Add (c);
				triangles3.Add (b);
				triangles3.Add (a);

				triangles3.Add (d);
				triangles3.Add (b);
				triangles3.Add (c);
			} else {
				triangles3.Add (a);
				triangles3.Add (b);
				triangles3.Add (c);

				triangles3.Add (c);
				triangles3.Add (b);
				triangles3.Add (d);
			}
		}

		/// <summary>
		/// Gets the number of materials constructed by the mesh generator.
		/// Default implementation returns 1.
		/// </summary>
		/// <returns>The number of materials.</returns>
		protected virtual int GetNumberOfMaterials ()
		{
			return 1;
		}

		/// <summary>
		/// Abstract method. It will be called to give the possibility to initialize variables.
		/// </summary>
		protected abstract void InitializeMesh ();

		/// <summary>
		/// Abstract method. It will be called to finalize the mesh, such as close the mesh at the end.
		/// </summary>
		protected abstract void FinalizeMesh ();

		/// <summary>
		/// Generates a single mesh part.
		/// </summary>
		/// <param name="piece">The piece. For each piece, this method is called.</param>
		/// <param name="position">The current position on the path.</param>
		/// <param name="direction">The current direction at the position on the path.</param>
		/// <param name="xd">This vector indicates an ortogonal direction to the path direction.</param>
		/// <param name="yd">This vector indicates an ortogonal direction to the direction and the xd vector.
		/// Use xd and yd as a plane ortogonal to the direction.</param>
		protected abstract void GenerateMeshPart (int piece, Vector3 position, Vector3 direction, Vector3 xd, Vector3 yd);
	}
}
