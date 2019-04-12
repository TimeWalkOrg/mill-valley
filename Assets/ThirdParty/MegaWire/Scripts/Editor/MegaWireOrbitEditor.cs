
using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;

[CustomEditor(typeof(MegaWireOrbit))]
public class MegaWireOrbitEditor : Editor
{
	//static Vector3 pos = Vector3.zero;

	static void SetCam(MegaWireOrbit morbit)
	{
		//MOrbit morbit = (MOrbit)target;
		SceneView sv = UnityEditor.SceneView.currentDrawingSceneView;

		if ( sv )
		{
			//sv.pivot = pos;
			Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
			if ( cam )
			{
				//Vector3 pos = cam.transform.position;
				//pos.x += 0.01f;
				//cam.transform.position = pos;
				//morbit.transform.position = cam.transform.position;
				//morbit.transform.rotation = cam.transform.rotation;
			}
		}
	}

	static MegaWireOrbitEditor()
	{
		EditorApplication.update += Update;
	}

	//static bool initdev = false;
	static void Update()
	{
		MegaWireOrbit[] morbit = (MegaWireOrbit[])FindObjectsOfType(typeof(MegaWireOrbit));

		for ( int i = 0; i < morbit.Length; i++ )
		{
			SetCam(morbit[i]);
		}
	}
}
