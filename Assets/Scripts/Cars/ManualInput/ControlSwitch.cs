using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController),typeof(ManualController))]
public class ControlSwitch : MonoBehaviour
{
    private AIController _aiController;

    private ManualController _manualControl;

    private bool _manualDrivingState;
    
    // Start is called before the first frame update
    void Start()
    {
        _aiController = GetComponent<AIController>();
        _manualControl = GetComponent<ManualController>();

        _manualDrivingState = false;
    }
    

    private void SetManualDrivingState(bool state)
    {
        _aiController.manualOverride = state;
        _manualControl.SetManualDriving(state);
    }

    public void SwitchControl()
    {
        Debug.Log("Manual Driving is  switching");
        _manualDrivingState = !_manualDrivingState;
        
        SetManualDrivingState(_manualDrivingState);
    }

    public void SwitchControl(bool state)
    {
        _manualDrivingState = state;
    }
}
