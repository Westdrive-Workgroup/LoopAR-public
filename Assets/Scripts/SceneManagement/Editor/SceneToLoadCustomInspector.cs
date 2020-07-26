using UnityEngine;
using System.Collections;
using NUnit.Framework;
using UnityEditor;

[CustomEditor(typeof(SceneToLoad))]
public class SceneToLoadCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SceneToLoad scene = (SceneToLoad)target;
 
        scene.sceneToLoad = (SceneToLoad.Scenes)EditorGUILayout.EnumPopup("Scene To Load", scene.sceneToLoad);
        
        switch (scene.sceneToLoad)
        {
            case SceneToLoad.Scenes.Westbrueck:
                scene.sceneValue = (int) SceneToLoad.Scenes.Westbrueck;
                break;
            case SceneToLoad.Scenes.Countryroad:
                scene.sceneValue = (int) SceneToLoad.Scenes.Countryroad;
                break;
            case SceneToLoad.Scenes.Autobahn:
                scene.sceneValue = (int) SceneToLoad.Scenes.Autobahn;
                break;
            case SceneToLoad.Scenes.MainMenu:
                scene.sceneValue = (int) SceneToLoad.Scenes.MainMenu;
                break;
        }
    }
}

