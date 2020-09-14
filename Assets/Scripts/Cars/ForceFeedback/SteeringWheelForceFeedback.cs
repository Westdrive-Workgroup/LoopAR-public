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
    [SerializeField] private CarController _carController;   
    [SerializeField] private ManualController _manualController;

    [SerializeField] private ControlSwitch _controlSwitch;
    // Start is called before the first frame update
    public bool shouldInit = true;
    private float target;
    private float current;
    void Start()
    {
        _carController = GetComponent<CarController>();
        _manualController = GetComponent<ManualController>();
        _controlSwitch = GetComponent<ControlSwitch>();
        
        if(shouldInit)
            FFB.ForceFeedBackInit();
        
        FFB.AcquireDevice();
    }

    #if UNITY_EDITOR
    private void StopDirectInput(PlayModeStateChange state)
    {
        
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
             FFB.StopForceFeedback();     //Currently buggy, crashs edtior
        }
    }

    private void ReleaseDirectInput(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
             FFB.FreeDirectInput();     //Currently buggy, crashs edtior
        }

           
    }
    #endif
    private void Update()
    {
        target=_carController.GetSterring();
        current= _manualController.GetSteeringInput();
        if (!_controlSwitch.GetManualDrivingState())
        {
            int sign = 0;
            if ((current - target) > 0)
                sign = -1;
            else
            {
                sign = 1;
            }
            SetAutoPilotForceFeedbackEffect(8000  * sign * ( Mathf.Abs(current) - Mathf.Abs(target)));
           
            
        }
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

    public void SteerToPosition()
    {
        
    }
    
    //Carcontroller.GetSteering (-1, 0 ,1 ) * 540 //Target 
    //Input *540 //Input Angle 

    // IF -1 Right Direction
    // IF 1 Left Direction 

    /*IEnumerator GoToPosition()
    {
        while (1)
        {
            _carController.GetSterring()
        }
    }*/
    
    
    #if !UNITY_EDITOR
    private void OnDestroy()
    {
        FFB.FreeDirectInput();
    }
    #endif
}
