using UnityEngine;
using System.Collections;

namespace Jacovone
{
	public class MathUtils
	{
		// Quaternion interpolation from http://caig.cs.nctu.edu.tw/course/CA/Lecture/slerp.pdf
		/// <summary>
		/// Quaternion interpolation by bezier
		/// </summary>
		/// <returns>The bezier interpolation of quaternions</returns>
		/// <param name="p">The reference "P" point in interpolation</param>
		/// <param name="prevP">The Previous point of "P" in the interpolation</param>
		/// <param name="nextP">The Next point of "P" in interpolation.</param>
		/// <param name="nextNextP">the Next of the next point of "P" in interpolation.</param>
		/// <param name="stepPos">Step position or interpolation percentage.</param>
		public static Quaternion QuaternionBezier (Quaternion p, Quaternion prevP, Quaternion nextP, Quaternion nextNextP, float stepPos)
		{
			Quaternion an = MathUtils.QuaternionNormalize (Quaternion.Slerp (Quaternion.Slerp (prevP, p, 2f), nextP, 0.5f));
			Quaternion an1 = MathUtils.QuaternionNormalize (Quaternion.Slerp (Quaternion.Slerp (p, nextP, 2f), nextNextP, 0.5f));
			Quaternion bn1 = MathUtils.QuaternionNormalize (Quaternion.Slerp (an1, nextP, 2f));
		
			Quaternion p1 = Quaternion.Slerp (p, an, stepPos);
			Quaternion p2 = Quaternion.Slerp (an, bn1, stepPos);
			Quaternion p3 = Quaternion.Slerp (bn1, nextP, stepPos);
			Quaternion p12 = Quaternion.Slerp (p1, p1, stepPos);
			Quaternion p23 = Quaternion.Slerp (p2, p3, stepPos);
		
			return Quaternion.Slerp (p12, p23, stepPos);
		}

		/// <summary>
		/// Normalizes a quaternion
		/// </summary>
		/// <returns>The normalized Quaternion.</returns>
		/// <param name="q">Quaternion to normalize</param>
		public static Quaternion QuaternionNormalize (Quaternion q)
		{
			float norm = Mathf.Sqrt (q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
			if (norm > 0.0f) {
				q.x /= norm;
				q.y /= norm;
				q.z /= norm;
				q.w /= norm;
			} else {
				q.x = 0.0f;
				q.y = 0.0f;
				q.z = 0.0f;
				q.w = 1.0f;
			}
			return q;
		}

		/// <summary>
		/// Computes the 3D interpolation of two vectors by Bezier
		/// </summary>
		/// <returns>The bezier interpolation of two points.</returns>
		/// <param name="p0">P0 the first point.</param>
		/// <param name="p1">P1 the first tangent point.</param>
		/// <param name="p2">P2 the second tangent point.</param>
		/// <param name="p3">P3 the second point.</param>
		/// <param name="t">The interpolation percentage.</param>
		public static Vector3 Vector3Bezier (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{	
			float tt = t * t;
			float ttt = t * tt;
			float u = 1.0f - t;
			float uu = u * u;
			float uuu = u * uu;
		
			Vector3 B = new Vector3 ();
			B = uuu * p0;
			B += 3.0f * uu * t * p1;
			B += 3.0f * u * tt * p2;
			B += ttt * p3;
		
			return B;
		}

		/// <summary>
		/// Computes the interpolation of two scalars by bezier
		/// </summary>
		/// <returns>The bezier interpolation.</returns>
		/// <param name="p0">P0 first number</param>
		/// <param name="p1">P1 first control number</param>
		/// <param name="p2">P2 second control number</param>
		/// <param name="p3">P3 second number</param>
		/// <param name="t">The interpolation percentage.</param>
		public static float FloatBezier (float p0, float p1, float p2, float p3, float t)
		{	
			float tt = t * t;
			float ttt = t * tt;
			float u = 1.0f - t;
			float uu = u * u;
			float uuu = u * uu;
		
			float B;
			B = uuu * p0;
			B += 3.0f * uu * t * p1;
			B += 3.0f * u * tt * p2;
			B += ttt * p3;
		
			return B;
		}
	}
}