//Terrain Slicing & Dynamic Loading Kits copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace DynamicLoadingKitEditors
{
    using UnityEditor;
    using UnityEngine;
    using DynamicLoadingKit;
    using System.IO;

    [CustomEditor(typeof(WorldGrid))]
    public class WorldGridEditor : Editor
    {
        WorldGridBaseEditor baseEditor;

        public override void OnInspectorGUI()
        {
            if (baseEditor == null)
                baseEditor = new WorldGridBaseEditor(serializedObject);
            
            baseEditor.OnInspectorGUI();
        }


        [MenuItem("Assets/Dynamic Loading Kit/Create World Grid")]
        public static void CreateWorldGrid()
        {
            WorldGrid grid = ScriptableObjectAssetCreator.GenerateAndRetrieveScriptableObjectAssetAtSelectedFolder<WorldGrid>("WorldGrid");
            WorldGridBaseEditor editor = new WorldGridBaseEditor(new SerializedObject(grid));
            editor.InitializeWorldGrid();
        }
    }

    //This WorldGrid.cs script has been moved to the .dll, and as such this editor script is no longer necessary.

    //I am leaving this commented version in the project so your existing WorldGrid.cs file is overwritten (for people who are updating), 
    //in order to avoid name resolution conflicts, HOWEVER you are free to delete this file.

    //Note, however, the commented version will remain in the package in the future (for the afore mentioned reason). When importing the package from now on,
    //you can uncheck the WorldGrid.cs script to avoid unecessarily importing it.

    //Additional Note: The move to the .dll has the unfortunate side effect of making your existing WorldGrid Assets unusable.
    //If you've deleted your terrain prefabs, remember that you can create a txt asset with the necessary data and use that to set your data.

    //I apologize for this inconvenience!
}