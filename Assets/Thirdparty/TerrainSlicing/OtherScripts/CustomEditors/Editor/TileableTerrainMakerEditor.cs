//Terrain Slicing & Dynamic Loading Kit copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace TerrainSlicingKit
{
    using UnityEditor;
    using UnityEngine;
    using PlayModeUtilities;

    [CustomEditor(typeof(TileableTerrainMaker))]
    class TileableTerrainMakerEditor : Editor
    {
        private Terrain baseTerrain;
        private int maxWidth;
        
        public SerializedProperty effectedRegionWidth;
        public SerializedProperty columns;
        public SerializedProperty rows;

        public SerializedProperty terrains;

        public SerializedProperty showFoldout;
        public SerializedProperty onlyTileInner;
        public SerializedProperty onlyTileOuter;
        public SerializedProperty emptyLocations;
        public SerializedProperty smoothEdges;

        public void OnEnable()
        {
            TileableTerrainMaker targetScript = (TileableTerrainMaker)target;
            GameObject gO = targetScript.gameObject;
            baseTerrain = gO.GetComponent<Terrain>();
            if (baseTerrain == null)
            {
                EditorUtility.DisplayDialog("Error", "This script only works with terrains!", "OK");
                DestroyImmediate(targetScript);
            }
            else
            {
                effectedRegionWidth = serializedObject.FindProperty("effectedRegionWidth");
                columns = serializedObject.FindProperty("columns");
                rows = serializedObject.FindProperty("rows");
                terrains = serializedObject.FindProperty("terrains");

                showFoldout = serializedObject.FindProperty("showFoldout");
                onlyTileInner = serializedObject.FindProperty("onlyTileInner");
                onlyTileOuter = serializedObject.FindProperty("onlyTileOuter");
                emptyLocations = serializedObject.FindProperty("emptyLocations");
                smoothEdges = serializedObject.FindProperty("smoothEdges");

                targetScript.terrains[0] = baseTerrain;
                maxWidth = baseTerrain.terrainData.heightmapResolution / 2;
            }
        }



        //Our GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            EditorGUIUtility.LookLikeInspector();
#elif UNITY_4
            EditorGUIUtility.LookLikeControls();
#else
            EditorGUIUtility.labelWidth = 0;
            EditorGUIUtility.fieldWidth = 0;
#endif

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Hover over the field labels (left of each field) or buttons to view");
            EditorGUILayout.LabelField("more detailed information about each option.\n");
            EditorGUILayout.Space();

            effectedRegionWidth.intValue = EditorGUILayout.IntSlider(LabelDatabase.effectedRegionLabel, effectedRegionWidth.intValue, 1, maxWidth);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(onlyTileInner, LabelDatabase.onlyTileInnerLabel);
            if (onlyTileInner.boolValue)
                onlyTileOuter.boolValue = false;

            EditorGUILayout.PropertyField(onlyTileOuter, LabelDatabase.onlyTileOuterLabel);
            if (onlyTileOuter.boolValue)
                onlyTileInner.boolValue = false;
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(smoothEdges, LabelDatabase.autoSmoothLabel);
            EditorGUILayout.Space();

            if (GUILayout.Button(LabelDatabase.tileHeightButtonLabel))
            {
                if (baseTerrain != null)
                    MakeHeightMapTileable();
                else
                    EditorUtility.DisplayDialog("Error", "Base Terrain Not Found! Where did it go?", "OK");
            }

#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            EditorGUILayout.LabelField("Use Edit->Undo or Ctrl-Z to undo the 'Make HeightMap Tileable'");
            EditorGUILayout.LabelField("and 'Smooth' operations'");
#endif

            EditorGUILayout.Space();

            if (GUILayout.Button(LabelDatabase.tileAlphaButtonLabel))
            {
                if (baseTerrain != null)
                    MakeAlphaMapTileable();
                else
                {
                    EditorUtility.DisplayDialog("Error", "Base Terrain Not Found! Where did it go?", "OK");
                }
            }

            if (((TileableTerrainMaker)serializedObject.targetObject).backupAlphaMap != null)
            {
                if (GUILayout.Button("Undo AlphaMap Changes"))
                    RestoreAlphaMaps();
                EditorGUILayout.LabelField("Use the buttom above to undo the 'Make AlphaMap(s) Tileable' operation.");
                EditorGUILayout.LabelField("Make sure to undo immediately! If you perform other actions, it might be impossible to undo!");
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Terrain Group Information");


            showFoldout.boolValue = EditorGUILayout.Foldout(showFoldout.boolValue, LabelDatabase.autoFillLabel);
            if (showFoldout.boolValue)
            {
                //Terrain object selection section
                EditorGUILayout.PropertyField(rows, LabelDatabase.rowsLabel);
                EditorGUILayout.PropertyField(columns, LabelDatabase.columnsLabel);
                EditorGUILayout.PropertyField(emptyLocations, LabelDatabase.emptyLocationsLabel);
                
                if (emptyLocations.boolValue && !onlyTileOuter.boolValue)
                    EditorGUILayout.HelpBox("Empty Locations can exist when tiling the outer region ONLY.", MessageType.Error);

                if (terrains.arraySize != (rows.intValue * columns.intValue))
                    terrains.arraySize = rows.intValue * columns.intValue;
                
                if (GUILayout.Button("Auto Fill Terrains"))
                    FillSelections(emptyLocations.boolValue);

                EditorGUILayout.PropertyField(terrains, true);

                bool emptyLocationFound = false;
                for (int i = 0; i < terrains.arraySize; i++)
                {
                    if (terrains.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    {
                        emptyLocationFound = true;
                        break;
                    }
                }

                if (!emptyLocations.boolValue && emptyLocationFound)
                    EditorGUILayout.HelpBox("An empty location exist, but you have indicated there are no empty locations! Fill in the terrains, or check the 'Empty Locations Exist' checkbox if appropriate.", MessageType.Error);


            }
            serializedObject.ApplyModifiedProperties();
        }//End the OnGUI function



        void FillSelections(bool emptyLocationsExist)
        {
            SelectionFiller selectionFiller = new SelectionFiller();
            Terrain[,] terrainsFound;

            if (emptyLocationsExist)
                terrainsFound = selectionFiller.FillSelections_EmptyVersion(((MonoBehaviour)serializedObject.targetObject).transform, rows.intValue, columns.intValue);
            else
                terrainsFound = selectionFiller.FillSelections_NormalVersion(((MonoBehaviour)serializedObject.targetObject).transform, rows.intValue, columns.intValue);

            if (terrainsFound != null)
            {
                for(int row = 0; row < terrainsFound.GetLength(0); row++)
                {
                    for (int column = 0; column < terrainsFound.GetLength(1); column++)
                        terrains.GetArrayElementAtIndex(row * rows.intValue + column).objectReferenceValue = terrainsFound[row, column];
                }
            }
            else
                EditorUtility.DisplayDialog("Error", "No terrains were found.", "OK");
        }

        Terrain[,] GetTerrainsFromTerrainsProperty()
        {
            Terrain[,] terrainsAsTerrains = new Terrain[rows.intValue, columns.intValue];
            for (int row = 0; row < terrainsAsTerrains.GetLength(0); row++)
            {
                for (int column = 0; column < terrainsAsTerrains.GetLength(1); column++)
                     terrainsAsTerrains[row, column] = (Terrain)terrains.GetArrayElementAtIndex(row * rows.intValue + column).objectReferenceValue;
            }

            return terrainsAsTerrains;
        }

        void MakeHeightMapTileable()
        {
            PortionToTile portionToTile = DeterminePortionToTile(onlyTileInner.boolValue, onlyTileOuter.boolValue);

            Terrain[,] terrains2D = GetTerrainsFromTerrainsProperty();
            TerrainErrorChecker errorChecker = new TerrainErrorChecker(terrains2D, portionToTile, emptyLocations.boolValue);

            if (!errorChecker.ProceedWithHeightmapTiling())
                return;

            //Allows the user to undo the heightmap changes after
#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            Undo.RegisterUndo(EditorTerrainTools.Get1DTerrainDataArrayFrom2DTerrainArray(targetScript.terrains), "Make HeightMap(s) Tileable");
#else
        int startingGroup = Undo.GetCurrentGroup() + 1;
        TerrainData[] terrains = EditorTerrainTools.Get1DTerrainDataArrayFrom2DTerrainArray(terrains2D);

        foreach (TerrainData terrain in terrains)
        {
            Undo.IncrementCurrentGroup();
            Undo.RegisterCompleteObjectUndo(terrain, "Make HeightMap(s) Tileable");
        }
        Undo.CollapseUndoOperations(startingGroup);
#endif


            TerrainData[,] data = EditorTerrainTools.Get2DTerrainDataArrayFrom2DTerrainArray(terrains2D);

            HeightmapTiler heightTiler = new HeightmapTiler(data, effectedRegionWidth.intValue, smoothEdges.boolValue);
            heightTiler.TileHeightmaps(portionToTile);
        }



        void MakeAlphaMapTileable()
        {
            PortionToTile portionToTile = DeterminePortionToTile(onlyTileInner.boolValue, onlyTileOuter.boolValue);

            Terrain[,] terrains2D = GetTerrainsFromTerrainsProperty();

            TerrainErrorChecker errorChecker = new TerrainErrorChecker(terrains2D, portionToTile, emptyLocations.boolValue);

            if (!errorChecker.ProceedWithAlphamapTiling())
                return;

            TerrainData[,] data = EditorTerrainTools.Get2DTerrainDataArrayFrom2DTerrainArray(terrains2D);

            //Can only occur when the alpha maps are of equal size/resolution, so don't worry about error checking
            TileableTerrainMaker targetScript = (TileableTerrainMaker)serializedObject.targetObject;
            targetScript.backupAlphaMap = new AlphamapBackupController(data);

            AlphamapTiler alphaTiler = new AlphamapTiler(data, effectedRegionWidth.intValue);

            alphaTiler.TileAlphamaps(portionToTile);
        }


        private void RestoreAlphaMaps()
        {
            TileableTerrainMaker targetScript = (TileableTerrainMaker)serializedObject.targetObject;
            if (targetScript.backupAlphaMap == null)
            {
                EditorUtility.DisplayDialog("Error", "No backup alphamaps were found.", "OK");
            }
            else
                targetScript.backupAlphaMap.RestoreAlphaMaps();

            targetScript.backupAlphaMap = null;
        }

        private PortionToTile DeterminePortionToTile(bool tileInnerOnly, bool tileOuterOnly)
        {
            if (tileInnerOnly)
                return PortionToTile.TileInner;
            else if (tileOuterOnly)
                return PortionToTile.TileOuter;
            else
                return PortionToTile.TileAll;
        }
    }
}