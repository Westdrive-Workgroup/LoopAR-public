using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManualController : MonoBehaviour
{
    [SerializeField] private int _inputControlIndex;

    public int InputControlIndex
    {
        get { return _inputControlIndex;}
        set { _inputControlIndex = value; }
    }
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

        Debug.Log("the control index was at start : " + _inputControlIndex);
        _RealInputController = _inputControlIndex;
    }

    // Update is called once per frame
    void Update()
    {

                accelerationInput = Input.GetAxis("Vertical");    // W or Arrow up, 
                steeringInput= Input.GetAxis("Horizontal");         //A or D steering or Arrow Keys left right
                brakeInput = Input.GetAxis("Jump");
                accelerationInput = Input.GetAxis("XOne_Trigger Right");    // W or Arrow up acceleration forward or backwards.
                steeringInput= Input.GetAxis("Horizontal");
                brakeInput = Input.GetAxis("XOne_Trigger Left");
                reverse = Input.GetAxis("Fire3");

                
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
