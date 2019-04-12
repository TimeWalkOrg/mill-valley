
#if false
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MegaWirePoleBuilder))]
public class MegaWirePoleBuilderEditor : Editor
{
	List<GameObject>	parts = new List<GameObject>();

	public string partspath = "Assets/MegaWire/Parts/PoleParts.fbx";

	GameObject	rootpart;

	void BuildList(GameObject obj)
	{
		if ( obj )
		{
			// Should only add if we have a mesh?
			parts.Add(obj);
			for ( int i = 0; i < obj.transform.childCount; i++ )
			{
				BuildList(obj.transform.GetChild(i).gameObject);
			}
		}
	}

	void FindParts()
	{
		//if ( rootpart == null )
		{
			GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/MegaWire/Parts/PoleParts.fbx", typeof(GameObject));

			if ( obj )
			{
				parts.Clear();
				BuildList(obj);

				Debug.Log("Parts " + parts.Count);
				for ( int i = 0; i < parts.Count; i++ )
				{
					Debug.Log("Found Part " + parts[i].name);
				}

				// Now need to build up sub part lists

			}
			else
			{
				Debug.Log("Could not load parts: " + partspath);
			}

			rootpart = obj;
		}
	}

	public override void OnInspectorGUI()
	{
		MegaWirePoleBuilder mod = (MegaWirePoleBuilder)target;

		if ( parts.Count == 0 )
		{
			FindParts();
		}

#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017
#else
		EditorGUIUtility.LookLikeControls();
#endif

		partspath = EditorGUILayout.TextField("Path", partspath);

		if ( GUILayout.Button("Reload Parts") )
		{
			FindParts();
		}

		if ( GUILayout.Button("Clear Pole") )
		{
			// Remove all child objects
		}

		if ( GUILayout.Button("Build Mesh") )
		{
			// Combine all parts to single mesh
		}



		if ( GUI.changed )
			EditorUtility.SetDirty(target);
	}
}
#endif