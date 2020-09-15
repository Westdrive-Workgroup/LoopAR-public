using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
public class ManualController : MonoBehaviour
{
    public enum InputType: int {Keyboard, XboxOneController, SteeringWheel}
    [HideInInspector] public InputType InputControlIndex;
    private CarController _carController;
    private bool _manualDriving = false;
    private bool toggleReverse;
    private SteeringWheelForceFeedback steeringWheelForceFeedback;
   
    private int _RealInputController;
    public delegate void OnReceivedInput(float steeringInput, float accelerationInput, float brakeInput);
    public event OnReceivedInput NotifyInputObservers;

    private float accelerationInput;
    private float brakeInput;
    private float steeringInput;
    private float reverse; //I know a bool would be better, but input systems are strange
    /*[Range(1, 5)] [SerializeField]*/ private float brakeFactor = 1.1f;
    
    
    private void Start()
    {
        if (GetComponent<SteeringWheelForceFeedback>()!=null)
        {
            steeringWheelForceFeedback = GetComponent<SteeringWheelForceFeedback>();
        }
        
        _carController = GetComponent<CarController>();
        
        if (GetComponent<ControlSwitch>() != null)
        {
            _manualDriving = GetComponent<ControlSwitch>().GetManualDrivingState();
        }
        else
        {
            _manualDriving = true;
        }
        
        SetInputSource(CalibrationManager.Instance.GetSteeringInputDevice());
        // Debug.Log("the control index was at start : " + InputControlIndex);
    }

    // Update is called once per frame
    void Update()
    {
        switch(InputControlIndex)
        {
            case InputType.Keyboard:
                    accelerationInput = Input.GetAxis("Vertical");    // W or Arrow up, 
                    steeringInput= Input.GetAxis("Horizontal");  
                    brakeInput = Input.GetAxis("Jump");
                    break;
            case InputType.XboxOneController:
                    accelerationInput = Input.GetAxis("XOne_Trigger Right"); 
                    steeringInput= Input.GetAxis("Horizontal");
                    brakeInput = 
                    reverse = Input.GetAxis("Fire3");
                    
                    if (reverse > 0f)
                    {
                        toggleReverse =! toggleReverse;
                    }
                    break;
            case InputType.SteeringWheel:
                    steeringInput=  Mathf.Clamp(Input.GetAxis("Horizontal (Steering)"),-1f,1f);
                    //Debug.Log(Input.GetAxis("Horizontal (Steering)"));
                    accelerationInput = Mathf.Clamp01(Input.GetAxis("Pedal0"));
                    brakeInput = Mathf.Clamp01(Input.GetAxis("Pedal1"));
                   // reverse = Input.GetAxis("Fire3");
                    /*if (Input.GetButtonDown("ShifterLeft"))
                    {
                        Debug.Log("reverse!");
                        toggleReverse =! toggleReverse;
                    }*/
                    break;
        }
        
        if (toggleReverse)
        {
            accelerationInput = -accelerationInput;
        }
        NotifyInputObservers?.Invoke(steeringInput, accelerationInput, brakeInput * brakeFactor);
        
        if (_manualDriving)
        {
            _carController.MoveVehicle(accelerationInput,brakeInput * brakeFactor, steeringInput);
            if (steeringWheelForceFeedback != null)
            {
                steeringWheelForceFeedback.SetManualForceFeedbackEffect(8000*steeringInput);    //-1 , 0  1
            }
        }
    }
    
    public void SetManualDriving(bool state)
    {
        _manualDriving = state;
    }

    void FixedUpdate()
    {
        
    }

    public float GetSteeringInput()
    {
        return steeringInput;
    }

    private void SetInputSource(string inputDevice)
    {
        var input = (InputType)Enum.Parse(typeof(InputType), inputDevice);
        
        switch(input)
        {
            case InputType.Keyboard:
                InputControlIndex = InputType.Keyboard;
                break;
            case InputType.XboxOneController:
                InputControlIndex = InputType.XboxOneController;
                break;
            case InputType.SteeringWheel:
                InputControlIndex = InputType.SteeringWheel;
                break;
        }
    }
}
