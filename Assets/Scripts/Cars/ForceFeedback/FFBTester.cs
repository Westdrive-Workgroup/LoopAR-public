using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ForceFeedback;

public class FFBTester : MonoBehaviour
{
    public int force = 1000;
    void Start()
    {
        FFB.ForceFeedBackInit();
        FFB.AcquireDevice();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FFB.SetDeviceForceFeedback(force, 0);
           
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            FFB.SetDeviceForceFeedback(-force, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FFB.StopForceFeedback();
        }
    }

    private void OnDestroy()
    {
        FFB.FreeDirectInput();
    }
}
