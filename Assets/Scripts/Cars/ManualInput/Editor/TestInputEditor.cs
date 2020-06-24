using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputSystem))]
public class TestInputEditor: Editor
{
    private string[] _inputChoices = new[] {"KeyboardInput", "XboxControllerInput"};
    private int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        InputSystem myInput = (InputSystem) target;
        DrawDefaultInspector();
        
        _choiceIndex = EditorGUILayout.Popup("InputOptions", _choiceIndex, _inputChoices);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
