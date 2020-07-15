using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SceneToLoad))]
public class SceneToLoadCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SceneToLoad scene = (SceneToLoad)target;
 
        scene.sceneToLoad = (SceneToLoad.Scenes)EditorGUILayout.EnumPopup("Scene To Load", scene.sceneToLoad);
    }
}
