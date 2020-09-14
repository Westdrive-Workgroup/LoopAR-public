using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ForceFeedback;using System.Collections;
using System.Collections.Generic;
using Autodesk.Fbx;
using UnityEditor;
using UnityEngine;

public class SteeringWheelForceFeedback : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
        FFB.ForceFeedBackInit();
        FFB.AcquireDevice();

        EditorApplication.playModeStateChanged += ReleaseDirectInput;
    }

    private void ReleaseDirectInput(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // FFB.FreeDirectInput();     //Currently buggy, crashs edtior
        }

           
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void bumpyRoad(int jitter)
    {
        //might be a coroutine
        //car speed needs to be taken into account
    }

    public void SetAutoPilotForceFeedbackEffect(float force)
    {
        int rounded = (int) -force;
        FFB.SetDeviceForceFeedback(rounded, 0);
    }

    public void SetManualForceFeedbackEffect(float force)
    {
        int rounded = (int) force;
        FFB.SetDeviceForceFeedback(rounded,0);
    }
    
    
    private void OnDestroy()
    {
        FFB.FreeDirectInput();
    }
}
