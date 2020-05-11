using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController),typeof(ManualController))]
public class ControlSwitch : MonoBehaviour
{
    private AIController _aiController;

    private ManualController _manualControl;

    [SerializeField] private bool manualDrivingState;
    
    // Start is called before the first frame update
    void Start()
    {
        _aiController = GetComponent<AIController>();
        _manualControl = GetComponent<ManualController>();

        _aiController.SetManualOverride(manualDrivingState);
    }
    

    private void SetManualDrivingState(bool state)
    {
        _aiController.manualOverride = state;
        _manualControl.SetManualDriving(state);
    }

    public bool GetManualDrivingState()
    {
        return manualDrivingState;
    }

    public void SwitchControl()
    {
        Debug.Log("Manual Driving is  switching");
        manualDrivingState = !manualDrivingState;
        
        SetManualDrivingState(manualDrivingState);
    }

    public void SwitchControl(bool state)
    {
        manualDrivingState = state;       
        SetManualDrivingState(manualDrivingState);
    }
}
