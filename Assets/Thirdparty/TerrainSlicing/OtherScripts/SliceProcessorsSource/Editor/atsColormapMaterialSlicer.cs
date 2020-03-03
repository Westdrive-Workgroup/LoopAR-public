//Terrain Slicing & Dynamic Loading Kit copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.

//Note that the #if UNITY_EDITOR precompiliation directive is necessary. While we know slice processors
//will not be used at runtime, Unity does not and will complain about the various references to the Unity Editor
//if the code is not surrounded with the precompilation directive.
namespace TerrainSlicingKit
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    
    public class atsColormapMaterialSlicer : SliceProcessor
    {
        #region Fields
        [SerializeField]
        [Tooltip("The master folder to save sliced materials and textures. Note that files will not be saved directly in this folder. Instead, each slicing job will create a sub folder within this folder, which will in turn contain sub folders for each individual terrain slice created by that slicing job.")]
        string whereToSaveSlicedMaterialsAndTextures = "/TerrainSlicing/SlicedMaterialsAndTextures";

        [SerializeField]
        [Tooltip("Each slice job will generate a folder (within the folder specified above) with the same name as the 'Base Name of Created Slices' option in your Slice Configuration File. If this option is checked, this folder will be cleaned before slicing begins, removing the existing sliced materials and textures. If unchecked, a new unique folder will be created, preserving your existing materials and textures.")]
        bool overwriteExistingFolder;
        

        SliceConfiguration sliceConfiguration;
        string subFolder;

        #endregion

        #region Pre Slicing Prep
        public sealed override void PreSlicingPrep(SliceConfiguration sliceConfiguration)
        {
            this.sliceConfiguration = sliceConfiguration;
            subFolder = whereToSaveSlicedMaterialsAndTextures;
            if (!subFolder.EndsWith("/"))
                subFolder += "/";
            subFolder += sliceConfiguration.SliceOutputGroupIdentifier;

            WhatToDoIfFolderAlreadyExists whatToDoIfFolderAlreadyExists = overwriteExistingFolder ? WhatToDoIfFolderAlreadyExists.OverwriteExistingFolder : WhatToDoIfFolderAlreadyExists.CreateDuplicateFolder;

            string masterSaveDirectory, masterUnitySavePath;
            CreateFolder(subFolder, whatToDoIfFolderAlreadyExists, out masterSaveDirectory, out masterUnitySavePath);
        }

        void CreateFolderForStoringAllSlicedMaterialsAndTextures(string folderName, out string saveDirectory, out string unitySavePath)
        {
            string placeToSave = whereToSaveSlicedMaterialsAndTextures;
            if (placeToSave.StartsWith("Assets"))
                placeToSave.Remove(0, 6);

            if (placeToSave[0] != '/')
                placeToSave = "/" + placeToSave;

            if (placeToSave[placeToSave.Length - 1] == '/')
                placeToSave.Remove(placeToSave.Length - 1);

            placeToSave += ("/" + folderName);

            string systemPathToSaveIn = Application.dataPath + placeToSave;

            if (Directory.Exists(systemPathToSaveIn))
            {
                if (overwriteExistingFolder)
                {
                    Directory.Delete(systemPathToSaveIn, true);
                    RefreshAssetDatabase();
                }
                else
                {
                    string uniqueSystemPath;
                    int i = 2;
                    do
                    {
                        uniqueSystemPath = string.Format("{0} [Slice {1}]", systemPathToSaveIn, i);
                        i++;
                    }
                    while (Directory.Exists(uniqueSystemPath));
                    systemPathToSaveIn = uniqueSystemPath;
                }
            }
            Directory.CreateDirectory(systemPathToSaveIn);

            systemPathToSaveIn += "/";

            saveDirectory = systemPathToSaveIn;
            unitySavePath = "Assets" + systemPathToSaveIn.Substring(Application.dataPath.Length);
        }

        #endregion

        public sealed override void ProcessSlice(Terrain sourceTerrain, TerrainSlice terrainSlice)
        {
#if UNITY_EDITOR
            string applicationSavePath, unitySavePath;
            string finalFolderName = sliceConfiguration.OutputNameGenerator.GenerateName(sliceConfiguration.SliceOutputGroupIdentifier, terrainSlice.Row, terrainSlice.Column);

            CreateFolder(subFolder + "/" + finalFolderName, WhatToDoIfFolderAlreadyExists.OverwriteExistingFolder, out applicationSavePath, out unitySavePath);
            SliceMaterial(terrainSlice, sourceTerrain.materialTemplate, applicationSavePath, unitySavePath);
            AssetDatabase.SaveAssets();
#endif
        }
        
        
        /// <summary>
        /// Slice the material
        /// </summary>
        /// <param name="slicedTerrain">The sliced terrain which we're generating sliced 
        /// materials/textures for</param>
        /// <param name="materialToSlice">The material to slice</param>
        /// <param name="saveDirectory">The system specific save path for the sliced files</param>
        /// <param name="unitySavePath">The Unity relative save path for the sliced files</param>
        void SliceMaterial(TerrainSlice slicedTerrain, Material materialToSlice, string applicationSavePath, string unitySavePath)
        {
            string fullAssetPathOfSourceMaterial = AssetDatabase.GetAssetPath(materialToSlice);
            string fullAssetPathOfNewMaterial = unitySavePath + sliceConfiguration.OutputNameGenerator.GenerateName(materialToSlice.name, slicedTerrain.Row, slicedTerrain.Column) + ".mat";

            AssetDatabase.CopyAsset(fullAssetPathOfSourceMaterial, fullAssetPathOfNewMaterial);
            RefreshAssetDatabase();
            Material slicedMaterial = AssetDatabase.LoadAssetAtPath<Material>(fullAssetPathOfNewMaterial);
            SliceProperties(slicedTerrain, slicedMaterial, applicationSavePath, unitySavePath);
            slicedTerrain.SliceTerrain.materialTemplate = slicedMaterial;
        }
        
        void SliceProperties(TerrainSlice terrainSlice, Material materialSlice, string applicationSavePath, string unityPathToSaveTextures)
        {
            List<string> texturePropertiesToSliceNormally = new List<string>(), texturePropertiesToSliceAbnormally = new List<string>();

            for (int i = 0; i < ShaderUtil.GetPropertyCount(materialSlice.shader); i++)
            {
                ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(materialSlice.shader, i);
                if (propertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    string texturePropertyName = ShaderUtil.GetPropertyName(materialSlice.shader, i);
                    if (texturePropertyName.StartsWith("_Splat"))
                        texturePropertiesToSliceAbnormally.Add(texturePropertyName);

                    else if (!texturePropertyName.StartsWith("_CombinedNormal"))
                        texturePropertiesToSliceNormally.Add(texturePropertyName);
                }
            }
            SliceTextureScaleAndOffset_NormalMethod(terrainSlice, materialSlice, texturePropertiesToSliceNormally);
            SliceTextureScaleAndOffset_AbnormalMethod(terrainSlice, materialSlice, texturePropertiesToSliceAbnormally);
        }
                
        //This allows you to right click on a folder to create the processor in that folder (nifty!)
        [MenuItem("Assets/Terrain Slicing Kit/Create Slice Processor/ats Colormap Material Slicer")]
        public static void CreateATSPostSliceProcessor1()
        {
            EditorExtensions.GenerateScriptableObjectAssetAtSelectedFolder<atsColormapMaterialSlicer>("atsColormapMaterialSlicer.asset");
        }

        //This allows for the creation of the processor via the Terrain menu item.
        //You can ignore this method in your own processor, CreateATSPostSliceProcessor1 should be sufficient.
        [MenuItem("Terrain/Terrain Slicing Kit/Create Slice Processor/ats Colormap Material Slicer")]
        public static void CreateATSPostSliceProcessor2()
        {
            EditorExtensions.GenerateScriptableObjectAssetAtSelectedFolder<atsColormapMaterialSlicer>("atsColormapMaterialSlicer.asset");
        }
    }
}