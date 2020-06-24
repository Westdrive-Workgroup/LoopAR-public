using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent,Serializable]
public class ManualController : MonoBehaviour
{
    private CarController _carController;
    private bool _manualDriving = false;
    private bool toggleReverse;
    
    public int _inputControlIndex;
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

        Debug.Log("der wert war am start bei : " + _inputControlIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (_inputControlIndex)
        {
            case 0:
                accelerationInput = Input.GetAxis("Vertical");    // W or Arrow up, 
                steeringInput= Input.GetAxis("Horizontal");         //A or D steering or Arrow Keys left right
                brakeInput = Input.GetAxis("Jump");
                Debug.Log(_inputControlIndex + "Update");
                return;
            case 1:
                accelerationInput = Input.GetAxis("XOne_Trigger Right");    // W or Arrow up acceleration forward or backwards.
                steeringInput= Input.GetAxis("Horizontal");
                brakeInput = Input.GetAxis("XOne_Trigger Left");
                reverse = Input.GetAxis("Fire3");
                Debug.Log(_inputControlIndex + "Update");
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

    public void SetInputSource(int InputIndex)
    {
        _inputControlIndex = InputIndex;
    }
}
