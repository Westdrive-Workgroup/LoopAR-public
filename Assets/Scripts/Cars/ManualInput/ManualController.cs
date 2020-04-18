using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualController : MonoBehaviour
{
    private CarController _carController;
    private bool _manualDriving = false;

    public delegate void OnReceivedInput(float steeringInput, float accelerationInput, float brakeInput);
    public event OnReceivedInput NotifyInputObservers;

    private void Awake()
    {
     
           
    }

    private void Start()
    {
        _carController = GetComponent<CarController>();
        _manualDriving = true;
        
        if (GetComponent<ControlSwitch>() != null)
        {
            if (GetComponent<ControlSwitch>().isActiveAndEnabled)
            {
                // Debug.Log("got here");
                _manualDriving = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float brakeInput = Input.GetAxis("Jump");
        float accelerationInput = Input.GetAxis("Vertical");// W or Arrow up acceleration forward or backwards.
        float steeringInput = Input.GetAxis("Horizontal"); //A or D steering

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
