/*
	STLMenuItem
	Carl Emil Carlsen
	http://sixthsensor.dk
	May 2012
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class STLMenuItem : ScriptableObject
{   
	
	[ MenuItem( "File/Export/Selected Mesh(es)/STL (binary)" ) ]
	public static void ExportBinarySTL()
	{
		// get selected meshes //
		GameObject[] objects = Selection.gameObjects;
		List<MeshFilter> filterList = new List<MeshFilter>();
		for( int g = 0; g < objects.Length; g++ ){
			MeshFilter[] filters = objects[ g ].GetComponentsInChildren<MeshFilter>();
			for( int f = 0; f < filters.Length; f++ ){
				if( filters[ f ] != null ){
					filterList.Add( filters[ f ] );
				}
			}
		}
		
		// display dialog if nothing no meshes are selected //
		if( filterList.Count == 0 ){
			EditorUtility.DisplayDialog( "Nothing to export", "Select one or more GameObjects with MeshFilter components attached.", "Close" );
			return;
		}
		
		// get default directory //
		string defaultDirectory = "";
		if( Application.platform == RuntimePlatform.OSXEditor ){
			defaultDirectory = System.Environment.GetEnvironmentVariable( "HOME" ) + "/Desktop";
		} else {
			defaultDirectory = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop );
		}
		
		// get default file name //
		string defaultName = DateTimeCode();
		if( Application.loadedLevelName != "" ) defaultName = Application.loadedLevelName + " " + defaultName;
		
		// display dialog to get save path //
		string filePath = EditorUtility.SaveFilePanel( "Save binary STL file", defaultDirectory, defaultName, "stl" );
		
		// export //
		STL.ExportBinary( filterList.ToArray(), filePath );
		
		// display feedback //
		string meshesPlural = filterList.Count == 1 ? "mesh" : "meshes";
		EditorUtility.DisplayDialog( "STL export complete", "Exported " + filterList.Count + " Unity " + meshesPlural + " combined in a binary STL file.", "Close" );
	}
	
	
	[ MenuItem( "File/Export/Selected Mesh(es)/STL (text)" ) ]
	public static void ExportTextSTL()
	{
		// get selected meshes //
		GameObject[] objects = Selection.gameObjects;
		List<MeshFilter> filterList = new List<MeshFilter>();
		for( int g = 0; g < objects.Length; g++ ){
			MeshFilter[] filters = objects[ g ].GetComponentsInChildren<MeshFilter>();
			for( int f = 0; f < filters.Length; f++ ){
				if( filters[ f ] != null ){
					filterList.Add( filters[ f ] );
				}
			}
		}
		
		// display dialog if nothing no meshes are selected //
		if( filterList.Count == 0 ){
			EditorUtility.DisplayDialog( "Nothing to export", "Select one or more GameObjects with MeshFilter components attached.", "Close" );
			return;
		}
		
		// get default directory //
		string defaultDirectory = "";
		if( Application.platform == RuntimePlatform.OSXEditor ){
			defaultDirectory = System.Environment.GetEnvironmentVariable( "HOME" ) + "/Desktop";
		} else {
			defaultDirectory = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop );
		}
		
		// get default file name //
		string defaultName = DateTimeCode();
		if( Application.loadedLevelName != "" ) defaultName = Application.loadedLevelName + " " + defaultName;
		
		// display dialog to get save path //
		string filePath = EditorUtility.SaveFilePanel( "Save binary STL file", defaultDirectory, defaultName, "stl" );
		
		// export //
		STL.ExportText( filterList.ToArray(), filePath );
		
		// display feedback //
		string meshesPlural = filterList.Count == 1 ? "mesh" : "meshes";
		EditorUtility.DisplayDialog( "STL export complete", "Exported " + filterList.Count + " Unity " + meshesPlural + " combined in a text based STL file.", "Close" );
	}
	
			
	private static string DateTimeCode(){
		return System.DateTime.Now.ToString("yy") + System.DateTime.Now.ToString("MM") + System.DateTime.Now.ToString("dd") + "_" + System.DateTime.Now.ToString("hh") + System.DateTime.Now.ToString("mm") + System.DateTime.Now.ToString("ss");
	}
}