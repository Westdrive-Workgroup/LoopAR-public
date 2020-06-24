using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ManualController : MonoBehaviour
{
    private CarController _carController;
    private bool _manualDriving = false;
    private bool toggleReverse;

    public delegate void OnReceivedInput(float steeringInput, float accelerationInput, float brakeInput);
    public event OnReceivedInput NotifyInputObservers;


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
    }

    // Update is called once per frame
    void Update()
    {
        
        float brakeInput = Input.GetAxis("XOne_Trigger Left");
        float accelerationInput = Input.GetAxis("XOne_Trigger Right");    // W or Arrow up acceleration forward or backwards.
        float steeringInput = Input.GetAxis("Horizontal");    //A or D steering
        float reverse = Input.GetAxis("Fire3");
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
}
