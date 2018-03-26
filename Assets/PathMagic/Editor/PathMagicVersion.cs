using UnityEngine;
using System.Collections;

namespace Jacovone
{
	/// <summary>
	/// Path magic version.
	/// </summary>
	public class PathMagicVersion : MonoBehaviour
	{
		/// <summary>
		/// The major version.
		/// </summary>
		public static int majorVersion = 1;
	
		/// <summary>
		/// The minor version.
		/// </summary>
		public static int minorVersion = 2;
	
		/// <summary>
		/// The revision version number.
		/// </summary>
		public static int revisionVersion = 5;

		/// <summary>
		/// Gets the complete version string.
		/// </summary>
		/// <value>The complete version string.</value>
		public static string Version { 
			get { return "v" + PathMagicVersion.majorVersion + "." + PathMagicVersion.minorVersion + "." + PathMagicVersion.revisionVersion; }
		}
	}
}