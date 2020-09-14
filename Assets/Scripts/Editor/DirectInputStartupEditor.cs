using System.Collections;
using System.Collections.Generic;
using ForceFeedback;
using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]
public class DirectInputStartupEditor 
{
    static DirectInputStartupEditor()
    {
        UnityEditor.EditorApplication.quitting += OnQuitting;
        Debug.Log(FFB.GetActiveWindow());
        FFB.ForceFeedBackInit();    // is the window already running?
        Debug.Log("ForceFeedBack UP and Running");
    }
    

    private static void OnQuitting()
    {
        Debug.Log("Releasing Direct Input!");
        FFB.Release();
    }
}
