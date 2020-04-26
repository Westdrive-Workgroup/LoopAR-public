using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class CameraCalibration : MonoBehaviour
{
    private GameObject _camera;
    public GameObject calibrationOffset;
    // Start is called before the first frame update

    private void Start()
    {
        calibrationOffset.transform.localPosition = CalibrationManager.Instance.GetSeatCalibrationOffset();
        _camera = GetComponent<VRCam>().GetCamera();
        
        //_camera.transform.SetParent(calibrationOffset.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<VRCam>().Seat();
            
        }
        
    }

  
    

}
