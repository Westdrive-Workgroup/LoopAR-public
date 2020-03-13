using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIControler),typeof(ManualController))]
public class ControlSwitch : MonoBehaviour
{
    private AIControler _aiControl;

    private ManualController _manualControl;

    private bool manualDrivingIsActive;
    // Start is called before the first frame update
    void Start()
    {
        _aiControl = GetComponent<AIControler>();
        _manualControl = GetComponent<ManualController>();
    }

    // Update is called once per frame
    void Update()
    {
        manualDrivingIsActive = _manualControl.ManualDriving;
        if (manualDrivingIsActive)
        {
            _aiControl.manualOverride = true;
        }
        else
        {
            _aiControl.manualOverride = false;
        }
    }
}
