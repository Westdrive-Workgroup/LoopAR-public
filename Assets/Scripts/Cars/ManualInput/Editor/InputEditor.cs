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
    private ManualController Manu;
    private SerializedProperty InputProp;
    private void OnEnable()
    {
        InputProp = serializedObject.FindProperty("_inputControlIndex");
    }
    
   

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
            _choiceIndex = EditorGUILayout.Popup("InputOptions", _choiceIndex, _inputChoices);
            Debug.Log(_choiceIndex);

            InputProp.intValue = _choiceIndex;
            Debug.Log(InputProp.intValue);
            //InputProp.= _choiceIndex;
        
            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
    }

    private void Start()
    {
    }
}

