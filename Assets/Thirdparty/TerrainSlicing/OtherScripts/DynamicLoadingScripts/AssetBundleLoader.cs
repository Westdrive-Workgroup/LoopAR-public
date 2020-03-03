//Terrain Slicing & Dynamic Loading Kits copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace DynamicLoadingKit
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Collections.Generic;

#if DLK_AssetBundleIntegrationEnabled
    using AssetBundles;
#endif

    [AddComponentMenu("Dynamic Loading Kit/Cell Object Loaders/Asset Bundle Loader")]
    public class AssetBundleLoader : BaseSceneLoader
    {
        public enum StreamingType
        {
            SimulationModeOrLocalAssetServer,
            StreamingAssetsFolder,
            RemoteServer
        }

        public enum AssetBundleManagerLogMode
        {
            All,
            JustErrors
        }

        [SerializeField]
        AssetBundleManagerLogMode logMode = AssetBundleManagerLogMode.JustErrors;

        [SerializeField]
        StreamingType streamingType;

        [SerializeField]
        string serverURL = "http://www.MyWebsite/MyAssetBundles";

        [SerializeField]
        string[] variantPossibilities;

        [SerializeField]
        string activeVariant;

        bool initialized;

        void Awake()
        {
            //In order to get rid of warnings in the console we need to make assignments to some variables which 
            //are only used when the compilation directive DLK_AssetBundleIntegrationEnabled is defined 

            //You might think why not put these variables in a #if statement? unfortunately the serialization 
            //system does not like that
            AssetBundleManagerLogMode discardLogMode = logMode;
            logMode = discardLogMode;
            string discardURL = serverURL;
            serverURL = discardURL;
        }

        public override bool IsSingleFrameAttachmentPreloadRequired
        {
            get
            {
                return false;
            }
        }

        protected sealed override YieldInstruction LoadCellObjectIntoLevel(string objectName)
        {
#if DLK_AssetBundleIntegrationEnabled
            string sceneName = objectName;
            string assetBundleName = Char.ToLowerInvariant(sceneName[0]) + sceneName.Substring(1);
            if (!initialized)
            {
                return StartCoroutine(InitializeAndThenLoadSceneIntoLevel(assetBundleName, sceneName));
            }
            else
            {
                return StartCoroutine(LoadSceneIntoLevel(assetBundleName, sceneName));
            }
#else
            throw new NotImplementedException("You must enable Asset Bundle Integration to utilize the Asset Bundle Loader.");
#endif
        }

        // Single frame attachment is not possible with Asset Bundles, as they need to be downloaded, which
        // takes time.Therefore, we will leave this method as not implemented, so that it throws

        //an exception if called (which is possible under certain circumstances).
        public override void AttachCellObjectsToCellsInSingleFrame<T>(List<T> cells, int loaderID)
        {
            throw new NotImplementedException("Loading Asset Bundles in a single frame is not possible. Ensure that 'Initialize on Awake' is unchecked on your Component Manager component and that you are manually calling the InitializeGradually method on the Component Manager. This will ensure that the starting world is loaded over a series of frames.");

        }

       



        
        IEnumerator<YieldInstruction> LoadSceneIntoLevel(string assetBundleName, string sceneName)
        {
#if DLK_AssetBundleIntegrationEnabled
            AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetBundleName, sceneName, true);

            if (request != null)
            {
                while (request.MoveNext())
                    yield return null;//request.Current just returns null, so we can do that instead

                AssetBundleManager.UnloadAssetBundle(assetBundleName);
            }
            else
            {
                Debug.Log("Could not load level " + assetBundleName + " Asset Bundle.");
                yield break;
            }
#else
            throw new NotImplementedException("You must enable Asset Bundle Integration to utilize the Asset Bundle Loader.");
#endif
        }

        IEnumerator<YieldInstruction> InitializeAndThenLoadSceneIntoLevel(string assetBundleName, string sceneName)
        {
#if DLK_AssetBundleIntegrationEnabled
            initialized = true;

            if (variantPossibilities != null && variantPossibilities.Length > 0)
                SetVariant(activeVariant);
            else
            {
                variantPossibilities = null;
                activeVariant = null;
            }

            AssetBundleManager.logMode = logMode == AssetBundleManagerLogMode.All ? AssetBundleManager.LogMode.All : AssetBundleManager.LogMode.JustErrors;

            if (streamingType == StreamingType.SimulationModeOrLocalAssetServer)
                AssetBundleManager.SetDevelopmentAssetBundleServer();
            else if (streamingType == StreamingType.StreamingAssetsFolder)
                AssetBundleManager.SetSourceAssetBundleURL("file:///" + Application.streamingAssetsPath + "/");
            else
                AssetBundleManager.SetSourceAssetBundleURL(serverURL);

            // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
            var initializeRequest = AssetBundleManager.Initialize();

            if (initializeRequest != null)
            {
                while (initializeRequest.MoveNext())
                    yield return null;
            }

            AssetBundleLoadOperation levelLoadRequest = AssetBundleManager.LoadLevelAsync(assetBundleName, sceneName, true);

            if (levelLoadRequest != null)
            {
                while (levelLoadRequest.MoveNext())
                    yield return null;//request.Current just returns null, so we can do that instead

                AssetBundleManager.UnloadAssetBundle(assetBundleName);
            }
            else
            {
                Debug.Log("Could not load level " + assetBundleName + " Asset Bundle.");
                yield break;
            }
#else
            throw new NotImplementedException("You must enable Asset Bundle Integration to utilize the Asset Bundle Loader.");
#endif
        }

        public void SetVariant(string variantName)
        {
#if DLK_AssetBundleIntegrationEnabled
            if (variantPossibilities == null)
                return;

            for (int i = 0; i < variantPossibilities.Length; i++)
            {
                if (variantName == variantPossibilities[i])
                {
                    SetVariant(i);
                    return;
                }
            }

            throw new System.ArgumentException("The variant name provided (" + variantName + ") does not match any of the variant possibilities provided.");
#else
            throw new NotImplementedException("You must enable Asset Bundle Integration to utilize the Asset Bundle Loader.");
#endif 
        }

        public void SetVariant(int index)
        {
#if DLK_AssetBundleIntegrationEnabled
            if (variantPossibilities == null)
                return;

            string[] activeVariants = AssetBundleManager.ActiveVariants;
            int indexToPlaceVariant = -1;

            //try to find a variant in the active variants array that matches
            //one of the possible variants
            for (int i = 0; i < activeVariants.Length; i++)
            {
                for (int j = 0; j < variantPossibilities.Length; j++)
                {
                    if (variantPossibilities[j] == activeVariants[i])
                    {
                        indexToPlaceVariant = i;
                        goto Exit;
                    }
                }
            }

            Exit:
            //means we could not find a matching variant in the ActiveVariants array, which is okay.
            //We just need to resize the array and place the variant at the end.
            if (indexToPlaceVariant == -1)
            {
                Array.Resize<string>(ref activeVariants, activeVariants.Length + 1);
                indexToPlaceVariant = activeVariants.Length - 1;
            }

            activeVariants[indexToPlaceVariant] = variantPossibilities[index];
            AssetBundleManager.ActiveVariants = activeVariants;
            activeVariant = variantPossibilities[indexToPlaceVariant];
#else
            throw new NotImplementedException("You must enable Asset Bundle Integration to utilize the Asset Bundle Loader.");
#endif
        }
    }
}