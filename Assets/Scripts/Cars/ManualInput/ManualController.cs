using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManualController : MonoBehaviour
{
    [HideInInspector] public int InputControlIndex;
    
    private CarController _carController;
    private bool _manualDriving = false;
    private bool toggleReverse;
    
   
    private int _RealInputController;
    public delegate void OnReceivedInput(float steeringInput, float accelerationInput, float brakeInput);
    public event OnReceivedInput NotifyInputObservers;

    private float accelerationInput;
    private float brakeInput;
    private float steeringInput;
    private float reverse; //I know a bool would be better, but input systems are strange
    
    private void Start()
    {
        _carController = GetComponent<CarController>();
        
        if (GetComponent<ControlSwitch>() != null)
        {
            _manualDriving = GetComponent<ControlSwitch>().GetManualDrivingState();
        }
        else
        {
            _manualDriving = true;
        }

        Debug.Log("the control index was at start : " + InputControlIndex);
    }

    // Update is called once per frame
    void Update()
    {
        switch(InputControlIndex)
        {
            case 0:
                    accelerationInput = Input.GetAxis("Vertical");    // W or Arrow up, 
                    steeringInput= Input.GetAxis("Horizontal");  
                    brakeInput = Input.GetAxis("Jump");
                    break;
            case 1:
                    accelerationInput = Input.GetAxis("XOne_Trigger Right"); 
                    steeringInput= Input.GetAxis("Horizontal");
                    brakeInput = Input.GetAxis("XOne_Trigger Left");
                    reverse = Input.GetAxis("Fire3");
                    break;
            case 2: 
                    steeringInput=  Mathf.Clamp(Input.GetAxis("Horizontal")*1.3f,-1f,1f);
                    accelerationInput = Mathf.Clamp01(Input.GetAxis("Pedal0"));
                    brakeInput = Mathf.Clamp01(Input.GetAxis("Pedal1"));
                    break;
        }
        

                
                if (reverse > 0f)
        {
            toggleReverse =! toggleReverse;
        }

        if (toggleReverse)
        {
            accelerationInput = -accelerationInput;
        }
        NotifyInputObservers?.Invoke(steeringInput, accelerationInput, brakeInput);
        
        if (_manualDriving)
        {
            _carController.MoveVehicle(accelerationInput,brakeInput,steeringInput);
        }
    }
    
    public void SetManualDriving(bool state)
    {
        _manualDriving = state;
    }

    // public void SetInputSource(int InputIndex)
    // {
    //     _inputControlIndex = InputIndex;
    // }
}
