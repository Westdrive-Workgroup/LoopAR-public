using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualController : MonoBehaviour
{
    private CarControl _carController;
    public bool ManualDriving;
    private void Start()
    {
        _carController = GetComponent<CarControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float brakeInput = Input.GetAxis("Jump");
        float accelerationInput = Input.GetAxis("Vertical");// W or Arrow up acceleration forward or backwards.
        float steeringInput = Input.GetAxis("Horizontal"); //A or D steering

        if (steeringInput != 0 || accelerationInput != 0 || brakeInput!=0)
        {
            ManualDriving = true;
            _carController.MoveVehicle(accelerationInput,brakeInput,steeringInput);
        }
        else
        {
            ManualDriving = false;
        }
        
        
        
    }
}
