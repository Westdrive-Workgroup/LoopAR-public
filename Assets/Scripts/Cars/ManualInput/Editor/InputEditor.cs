using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using EditorGUI = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUI;

[CustomEditor(typeof(ManualController))][CanEditMultipleObjects][System.Serializable]
public class InputEditor : Editor
{
   
    private string[] _inputChoices = new[] {"Keyboard Input", "Xbox Controller Input"};
    private int _choiceIndex;
    private SerializedProperty InputProp;
 
    protected virtual void OnEnable()
    {
        InputProp = serializedObject.FindProperty("InputControlIndex");    //actually not the correct one? its private
    }
    
   

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        serializedObject.Update();
        
       
        _choiceIndex = EditorGUILayout.Popup("InputOptions", _choiceIndex, _inputChoices);
        
        if (GUI.changed && (InputProp.intValue != _choiceIndex))
        {
            InputProp.intValue = _choiceIndex;
        }
        else
        {
            _choiceIndex = InputProp.intValue;
        }




        if (serializedObject.hasModifiedProperties)
        {
            Debug.Log(" there is a change " + serializedObject.hasModifiedProperties);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty((ManualController) target);
        }
  
        serializedObject.ApplyModifiedProperties();
    }

}

