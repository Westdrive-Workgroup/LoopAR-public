using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController),typeof(ManualController))]
public class ControlSwitch : MonoBehaviour
{
    private AIController _aiControl;

    private ManualController _manualControl;

    private bool manualDrivingIsActive;
    // Start is called before the first frame update
    void Start()
    {
        _aiControl = GetComponent<AIController>();
        _manualControl = GetComponent<ManualController>();

        manualDrivingIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void activateManualDriving()
    {
        _aiControl.manualOverride = true;
        _manualControl.SetManualDriving(true);
    }

    public void switchControl()
    {
        Debug.Log("is activated switch");
        manualDrivingIsActive = !manualDrivingIsActive;

        if (manualDrivingIsActive)
        {
            activateManualDriving();
        }
    }

    public void switchControl(bool state)
    {
        manualDrivingIsActive = state;
    }
}
