using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController),typeof(ManualController))]
public class ControlSwitch : MonoBehaviour
{
    private AIController _aiController;

    private ManualController _manualControl;

    [SerializeField] private bool manualDriving;
    
    // Start is called before the first frame update
    void Start()
    {
        _aiController = GetComponent<AIController>();
        _manualControl = GetComponent<ManualController>();

        _aiController.SetManualOverride(manualDriving);
    }
    

    private void SetManualDrivingState(bool state)
    {
        _aiController.SetManualOverride(state);
        _manualControl.SetManualDriving(state);
    }

    public bool GetManualDrivingState()
    {
        return manualDriving;
    }

    public void SwitchControl()
    {
        Debug.Log("Manual Driving is  switching");
        manualDriving = !manualDriving;
        
        SetManualDrivingState(manualDriving);
    }

    public void SwitchControl(bool state)
    {
        manualDriving = state;       
        SetManualDrivingState(manualDriving);
    }
}
