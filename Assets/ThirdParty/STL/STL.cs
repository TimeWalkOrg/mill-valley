/*
	STL v 1.2
	Carl Emil Carlsen
	http://sixthsensor.dk
	May 2012
	
	
	DOCUMENTATION
	=============
	
	use ExportBinary() to export a binary
	STL file and ExportText() to export
	a text based STL file. binary files
	take up far less disk space.
	
	
	METHODS
	
		string ExportBinary( MeshFilter filter )
			exports one mesh to a binary STL file 
			on your desktop and returns the path 
			to the file.
		
		string ExportBinary( MeshFilter[] filters )
			combines a number of meshes, exports them
			to a single binary STL file on your desktop
			and returns the resulting file path.
			
		void ExportBinary( MeshFilter filter, string filePath )
			exports one mesh to a binary STL file 
			at a given file path.
			
		void ExportBinary( MeshFilter[] filter, string filePath )
			combines a number of meshes and exports 
			them to a single text binary STL file at a 
			given file path.
			
		string ExportText( MeshFilter filter )
			exports one mesh to a text besed STL file 
			on your desktop and returns the path to 
			the file.
		
		string ExportText( MeshFilter[] filters )
			combines a number of meshes, exports them
			to a single text besed STL file on your 
			desktopand returns the resulting file path.
			
		void ExportText( MeshFilter filter, string filePath )
			exports one mesh to a text besed STL file 
			at a given file path.
			
		void ExportText( MeshFilter[] filter, string filePath )
			combines a number of meshes and exports 
			them to a single text besed STL file at a 
			given file path.
			
		
	EXAMPLE OF USE
		
		// export meshes from all children of the game object that
		// this script lives on to a STL file on my desktop.
		MeshFilter[] filters = gameObject.GetComponentsInChildren<MeshFilter>();
		STL.ExportBinary( filters );
	
	
	HISTORY
		
		1.0 initial version
		
		1.1 fixes
			– fixed a float formating bug in exported
			  text based STL files.
			– fixed a missing end statement in exported
			  text based STL files.
		
		1.2 fixes
			– fixed an out of memory issue with ExportText()
			  when exporting extremely large meshes.
			– removed a warning message that was displayed 
			  when cancelling an export in the editor.
		
*/	


using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;


public class STL
{
	
	public static string ExportBinary( MeshFilter filter )
	{
		return ExportBinary( new MeshFilter[]{ filter } );
	}
	
	
	public static string ExportBinary( MeshFilter[] filters )
	{
		string filePath = ExportPath() + "/" + Application.loadedLevelName + " " + DateTimeCode() + ".stl";
		ExportBinary( filters, filePath );
		return filePath;
	}
	
	
	public static void ExportBinary( MeshFilter filter, string filePath )
	{
		ExportBinary( new MeshFilter[]{ filter } );
	}
	
	
	public static void ExportBinary( MeshFilter[] filters, string filePath )
	{
		try
		{
			using( BinaryWriter writer = new BinaryWriter( File.Open( filePath, FileMode.Create ) ) )
			{
				// write header
				writer.Write( new char[ 80 ] );
				
				// count all triangles and write
				int triangleIndexCount = 0;
				foreach( MeshFilter filter in filters ) {
					for( int s = 0; s < filter.sharedMesh.subMeshCount; s++ ){
						triangleIndexCount += filter.sharedMesh.GetTriangles( s ).Length;
					}
				}
				uint triangleCount = (uint) ( triangleIndexCount / 3 );
				writer.Write( triangleCount );
				
				// for each mesh filter ...
				int i;
				short attribute = 0;
				Vector3 u, v, normal;
				int[] triangles;
				Vector3[] vertices;
				Mesh mesh;
				foreach( MeshFilter filter in filters )
				{
					// get vertices and tranform them
					mesh = filter.sharedMesh;
					vertices = mesh.vertices;
					for( int vx = 0; vx < vertices.Length; vx++ ){
						vertices[ vx ] = filter.transform.localToWorldMatrix.MultiplyPoint( vertices[ vx ] );
					}
					
					// for each sub mesh ...
					for( int s = 0; s < mesh.subMeshCount; s++ )
					{
						// get trianlges
						triangles = mesh.GetTriangles( s );
						
						// for each triangle ...
						for( int t = 0; t < triangles.Length; t += 3 )
						{
							// calculate and write normal
							u = vertices[ triangles[ t+1 ] ] - vertices[ triangles[ t ] ];
							v = vertices[ triangles[ t+2 ] ] - vertices[ triangles[ t ] ];
							normal = new Vector3( u.y * v.z - u.z * v.y, u.z * v.x - u.x * v.z, u.x * v.y - u.y * v.x );
							for( i = 0; i < 3; i++ ) writer.Write( normal[ i ] );
							
							// write vertices
							for( i = 0; i < 3; i++ ) writer.Write( vertices[ triangles[ t ] ][i] );
							for( i = 0; i < 3; i++ ) writer.Write( vertices[ triangles[ t+1 ] ][i] );
							for( i = 0; i < 3; i++ ) writer.Write( vertices[ triangles[ t+2 ] ][i] );
							
							// write attribute byte count
							writer.Write( attribute );
						}
					}
				}
				
				// the end
				writer.Close();
			}
		}
		catch( System.Exception e ){
			Debug.LogWarning( "FAILED exporting STL object at : " + filePath + "\n" + e );
		}
	}
	
	
	public static string ExportText( MeshFilter filter )
	{
		return ExportText( new MeshFilter[]{ filter } );
	}
	
	
	public static string ExportText( MeshFilter[] filters )
	{
		string filePath = ExportPath() + "/" + Application.loadedLevelName + " " + DateTimeCode() + ".stl";
		ExportText( filters, filePath );
		return filePath;
	}
	
	
	public static void ExportText( MeshFilter filter, string filePath )
	{
		ExportText( new MeshFilter[]{ filter } );
	}
	
	
	public static void ExportText( MeshFilter[] filters, string filePath )
	{
		try
		{
			bool append = false;
			using( StreamWriter sw = new StreamWriter( filePath, append ) ) 
			{
				// write header to disk
				sw.WriteLine( "solid Unity Mesh" );
				
				// for each mesh filter ...
				Vector3 u, v, normal;
				int[] triangles;
				Vector3[] vertices;
				Mesh mesh;
				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture( "en-US" );
				foreach( MeshFilter filter in filters )
				{
					// a new string builder for each mesh to avoid out of memory errors
					StringBuilder sb = new StringBuilder();
					
					// get vertices and tranform them
					mesh = filter.sharedMesh;
					vertices = mesh.vertices;
					for( int vx = 0; vx < vertices.Length; vx++ ){
						vertices[ vx ] = filter.transform.localToWorldMatrix.MultiplyPoint( vertices[ vx ] );
					}
					
					// for each sub mesh ...
					for( int s = 0; s < mesh.subMeshCount; s++ )
					{
						// get trianlges
						triangles = mesh.GetTriangles( s );
						
						// for each triangle ...
						for( int t = 0; t < triangles.Length; t += 3 )
						{
							// calculate and write normal
							u = vertices[ triangles[ t+1 ] ] - vertices[ triangles[ t ] ];
							v = vertices[ triangles[ t+2 ] ] - vertices[ triangles[ t ] ];
							normal = new Vector3( u.y * v.z - u.z * v.y, u.z * v.x - u.x * v.z, u.x * v.y - u.y * v.x );
							sb.AppendLine( "facet normal " + normal.x.ToString("e",ci) + " " + normal.y.ToString("e",ci) + " " + normal.z.ToString("e",ci) );
							
							// begin triangle
							sb.AppendLine( "outer loop" );
							
							// write vertices
							sb.AppendLine( "vertex " + vertices[ triangles[ t ] ].x.ToString("e",ci) + " " + vertices[ triangles[ t ] ].y.ToString("e",ci) + " " + vertices[ triangles[ t ] ].z.ToString("e",ci) );
							sb.AppendLine( "vertex " + vertices[ triangles[ t+1 ] ].x.ToString("e",ci) + " " + vertices[ triangles[ t+1 ] ].y.ToString("e",ci) + " " + vertices[ triangles[ t+1 ] ].z.ToString("e",ci) );
							sb.AppendLine( "vertex " + vertices[ triangles[ t+2 ] ].x.ToString("e",ci) + " " + vertices[ triangles[ t+2 ] ].y.ToString("e",ci) + " " + vertices[ triangles[ t+2 ] ].z.ToString("e",ci) );
							
							// end triangle
							sb.AppendLine( "endloop" );
							sb.AppendLine( "endfacet" );
						}
					}
					
					// write string builder memory to the disk
					sw.Write( sb.ToString() );
				}
				
				// write ending to disk and close writer
				sw.WriteLine( "endsolid Unity Mesh" );
				sw.Close();
			}
		}
		catch( System.Exception e ){
			Debug.LogWarning( "FAILED exporting wavefront obj at : " + filePath + "\n" + e );
		}
	}
	
	
	
	private static string ExportPath()
	{
		string exportPath = "";
		if( Application.platform == RuntimePlatform.OSXEditor ){
			exportPath = System.Environment.GetEnvironmentVariable( "HOME" ) + "/Desktop";
		} else {
			exportPath = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop );
		}
		return exportPath;
	}
	
	
	private static string DateTimeCode(){
		return System.DateTime.Now.ToString("yy") + System.DateTime.Now.ToString("MM") + System.DateTime.Now.ToString("dd") + "_" + System.DateTime.Now.ToString("hh") + System.DateTime.Now.ToString("mm") + System.DateTime.Now.ToString("ss");
	}
}