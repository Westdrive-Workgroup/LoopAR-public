//Terrain Slicing & Dynamic Loading Kits copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace DynamicLoadingKitEditors
{
    using UnityEngine;
    using UnityEditor;

    public static class AssetBundleIntegration
    {
        static string integrationDefineSymbol = "DLK_AssetBundleIntegrationEnabled";

        [MenuItem("Assets/Dynamic Loading Kit/Enable AssetBundle Integration For Current Platform")]
        public static void EnableAssetBundleIntegration()
        {
            if(!AssetDatabase.IsValidFolder("Assets/AssetBundleManager"))
            {
                if(EditorUtility.DisplayDialog("Missing Folder", "You must import the Asset Bundle Manager package from the Unity Asset Store before enabling Asset Bundle Integration", "Go To Store Now", "Close"))
                {
                    Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/45836");
                }
                return;
            }

            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string define = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            string[] defineSplit = define.Split(';');
            for (int i = 0; i < defineSplit.Length; i++)
            {
                if (defineSplit[i] == integrationDefineSymbol)
                    return;//integration already enabled for this platform
            }

            define = define + ";" + integrationDefineSymbol;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, define);
        }

        [MenuItem("Assets/Dynamic Loading Kit/Disable AssetBundle Integration For Current Platform")]
        public static void DisableAssetBundleIntegration()
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string define = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            string[] defineSplit = define.Split(';');
            int indexOfIntegrationDefine = -1;//does not exists
            for (int i = 0; i < defineSplit.Length; i++)
            {
                if (defineSplit[i] == integrationDefineSymbol)
                    indexOfIntegrationDefine = i;
            }

            //if it does equal -1 then the define does not exists, therefore 
            //integration is already disabled for this platform
            if (indexOfIntegrationDefine != -1)
            {
                string[] newDefineSplit = defineSplit.RemoveIndices(indexOfIntegrationDefine);
                string newDefine = string.Join(";", newDefineSplit);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefine);
            }
        }

        static string[] RemoveIndices(this string[] arr, int removeAt)
        {
            string[] newArr = new string[arr.Length - 1];

            int i = 0;
            int j = 0;
            while (i < arr.Length)
            {
                if (i != removeAt)
                {
                    newArr[j] = arr[i];
                    j++;
                }

                i++;
            }

            return newArr;
        }
    }
}