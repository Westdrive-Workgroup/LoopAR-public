using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            camera.GetComponent<ChaseCam>().enabled = false;
        }
        else
        {
            camera.GetComponent<VRCam>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // todo choose a camera
    // todo fade functions
}
