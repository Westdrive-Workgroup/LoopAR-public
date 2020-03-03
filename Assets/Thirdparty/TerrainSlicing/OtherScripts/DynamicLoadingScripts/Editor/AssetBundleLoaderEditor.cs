//Terrain Slicing & Dynamic Loading Kits copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace DynamicLoadingKitEditors
{
    using UnityEditor;
    using UnityEngine;
    using DynamicLoadingKit;
    
    [CustomEditor(typeof(AssetBundleLoader))]
    class AssetBundleLoaderEditor : Editor
    {
        AssetBundleLoaderBaseEditor baseEditor;

        public override void OnInspectorGUI()
        {
            if (baseEditor == null)
                baseEditor = new AssetBundleLoaderBaseEditor(serializedObject);

            baseEditor.OnInspectorGUI();
        }
    }

    internal class AssetBundleLoaderBaseEditor : BaseSceneLoaderBaseEditor
    {
        public AssetBundleLoaderBaseEditor(SerializedObject serializedObject)
            : base(serializedObject)
        { }

        protected sealed override void DrawInspector()
        {
#if !DLK_AssetBundleIntegrationEnabled
            EditorGUILayout.HelpBox("To use the Asset Bundle Loader, download the AssetBundle Manager & Example Scenes package from the Unity Asset Store and then click on the 'Enable AssetBundle Integration For Current Platform' button below (or from the Unity Menu @ Assets -> Dynamic Loading Kit -> Enable AssetBundle Integration from the Main Menu).", MessageType.Info);

            if (GUILayout.Button("AssetBundle Manager & Example Scenes Package"))
                Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/45836");

            if (GUILayout.Button("Enable AssetBundle Integration For Current Platform"))
                AssetBundleIntegration.EnableAssetBundleIntegration();
#else
            base.DrawInspector();

            helper.DrawSerializedPropertyField("logMode");
            helper.DrawSerializedPropertyField("streamingType", streamingTypeLabel);

            if (helper.GetPropertyByName("streamingType").intValue == (int)AssetBundleLoader.StreamingType.RemoteServer)
                helper.DrawSerializedPropertyField("serverURL", "Server URL");

            helper.DrawSerializedPropertyArrayField("variantPossibilities");
            if (helper.GetPropertyByName("variantPossibilities").arraySize > 0)
                helper.DrawSerializedPropertyField("activeVariant");

#endif
        }

#if DLK_AssetBundleIntegrationEnabled

        GUIContent streamingTypeLabel = new GUIContent("Streaming Type*", "This controls where the asset bundles are streamed from.\n\nIf SimulationModeOrLocalAssetServer is selected, the asset bundles will be simulated if Simulation Mode is enabled, or loaded from the local asset server if Simulation Mode is disabled. Note that the local asset server may not work correctly if 'Development Build' is not enabled in your Build Settings.\n\nIf StreamingAssetsFolder is selected, the asset bundles will be loaded from the StreamingAssets folder (can be used in editor or standalone build).\n\nIf RemoteServer is selected, the asset bundles will be loaded from the remote server specified in the 'Server URL' field (works in editor or standalone).");
#endif
    }
}