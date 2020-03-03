using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EasyRoads3Dv3;
using EasyRoads3Dv3Editor;

public class ERAPITutorial : ScriptableObject {
	[MenuItem( "EasyRoads3D/Move Network" )]
	public static void   MoveNetwork() 
	{
		ERSurfaceScript scr = Selection.activeGameObject.GetComponentInChildren(typeof(ERSurfaceScript)) as ERSurfaceScript;
		if(scr != null)DestroyImmediate(scr.gameObject);
		Debug.Log("done");
	}



}
