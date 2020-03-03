using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(ERTutorial))]
public class ERTutorialEditor : Editor  {

	public ERTutorial scr;
	

	public void OnEnable() 
	{
		scr = (ERTutorial)target;
	}

	public override void OnInspectorGUI() 
	{

		GUILayout.Space(25);

		GUILayout.Label("EasyRoads3D Tutorials: ");

		for(int i = 0; i < scr.links.Length; i++){
			if(GUILayout.Button (scr.linkText[i])){
			//	Debug.Log (scr.links[i]);
				Application.OpenURL (scr.links[i]);
			}
		}

		if(scr.terrain != null && scr.mat != null){

			GUILayout.Space(20);

			EditorGUILayout.HelpBox("To cut the sewer hole in the terrain please apply the custom terrain material to the terrain here below. It uses a simple shader example with a mask, no normal maps etc.\n\nThe shader is located in:\n /Assets/EasyRoads3D/Resources/Materials/misc/ER Terrain Material", MessageType.Info, true);

			if(GUILayout.Button ("Add Custom Terrain Material")){
				scr.terrain.materialType = Terrain.MaterialType.Custom;
				scr.terrain.materialTemplate = scr.mat;
			}
			if(GUILayout.Button ("Standard Terrain Material")){
				scr.terrain.materialType = Terrain.MaterialType.BuiltInStandard;
				scr.terrain.materialTemplate = null;
			}
		}

		GUILayout.Space(50);

	}

	public  void OnSceneGUI() 
	{

	}

}
