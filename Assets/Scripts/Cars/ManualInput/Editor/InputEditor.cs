using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using EditorGUI = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUI;

[CustomEditor(typeof(ManualController))]
public class InputEditor : Editor
{
   
    private string[] _inputChoices = new[] {"Keyboard Input", "Xbox Controller Input"};
    private int _choiceIndex;
    [SerializeField] private ManualController myManualController;
    protected virtual void OnEnable()
    {
        
    }
    
   

    public override void OnInspectorGUI()
    {
        myManualController = (ManualController) target;
        SerializedProperty InputProp = serializedObject.FindProperty("_inputControlIndex");
        // if (Application.IsPlaying(myManualController.gameObject))
        // {
        //     InputProp = serializedObject.FindProperty("_inputControlIndex");
        //     EditorGUILayout.LabelField(_inputChoices[InputProp.intValue]);        //in later versions i would go with greyout version
        //     return;
        // }
        
        
        _choiceIndex = EditorGUILayout.Popup("InputOptions", _choiceIndex, _inputChoices);
        InputProp.intValue = _choiceIndex;

        if (string.IsNullOrEmpty(myManualController.gameObject.scene.path)) return;
        
        //serializedObject.Update();

       
        
        serializedObject.ApplyModifiedProperties();
        if (serializedObject.hasModifiedProperties)
        {
            Debug.Log(" there is a change " + serializedObject.hasModifiedProperties);
        }

        
  

    }

}

