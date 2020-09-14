using System;
using System.Collections;
using System.Collections.Generic;
using ForceFeedback;
using UnityEditor;
using UnityEngine;


public class SteeringWheelModelController : MonoBehaviour
{
    [SerializeField] private GameObject SteeringWheelModel;
    [SerializeField] private CarController carController;
    [SerializeField] private ControlSwitch controlSwitch;
    
    // less performance intense comparision
    private bool controlSwitch_present; 
    //these are important for AI because they steer like crazy
    private float currentWheelSteering;
    private float LerpedSteering;
    
    //this is for manual Controller, here we can have a 1 to 1 transition
    private float directSteering;
    
    private float xRot; //this value is important for 
    // Start is called before the first frame update
    private void Start()
    {
        xRot = gameObject.transform.rotation.eulerAngles.x;        //dont mind this, it just needs to take that shifted x axis of the Wheel
        currentWheelSteering = 0f;
        if (controlSwitch != null)
        {
            controlSwitch_present = true;
        }
        else
        {
            controlSwitch_present=false;
        }
    }
    // Update is called once per frame
    
    //1.5 turns are 180 + 360 degrees , therefore 540

    public void SmoothedSteering()
    {
        this.transform.localRotation = Quaternion.Euler(xRot,0,LerpedSteering*540);
    }

    public void DirectSteering()
    {
        this.transform.localRotation = Quaternion.Euler(xRot,0,directSteering* 540);
    }

    private void LateUpdate()
    {
        if (controlSwitch_present && !controlSwitch.GetManualDrivingState())
        {
            SmoothedSteering();
        }
        else
        {
            DirectSteering();
        }
      
        //float LerpedSteering = Mathf.SmoothDamp(currentWheelSteering, carController.GetSterring(), ref smoothvalue,
         //   Time.deltaTime);
       // Debug.Log(LerpedSteering);
        //this.transform.localRotation = Quaternion.Euler(xRot,0,LerpedSteering*540);
        currentWheelSteering = LerpedSteering;
    }

    private void FixedUpdate()
    {
        directSteering = carController.GetSterring();
        LerpedSteering = Mathf.Lerp(currentWheelSteering, directSteering,Time.deltaTime);
    }
        // float LerpedSteering = Mathf.Lerp(currentWheelSteering, carController.GetSterring(),Time.deltaTime);
        //currentWheelSteering = LerpedSteering;
    
}
