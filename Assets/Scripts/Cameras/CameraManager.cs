using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject blackScreen;

    private VRCam _vRCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            this.gameObject.GetComponent<ChaseCam>().enabled = false;
            _vRCamera = this.gameObject.GetComponent<VRCam>();
            VRModeFunctionality();
        }
        else
        {
            this.gameObject.GetComponent<VRCam>().enabled = false;
            NonVRModeFunctionality();
        }
        
    }

    private void VRModeFunctionality()
    {
        // _vRCamera.SetPosition(firstPersonCamera.transform.position);
    }

    private void NonVRModeFunctionality()
    {
        blackScreen.SetActive(true);
    }
    
    // todo choose a camera
    // todo fade functions
}
