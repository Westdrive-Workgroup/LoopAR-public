using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCam : MonoBehaviour
{
    
    public GameObject seatPosition;
    public bool UseCalibrationOffset;
    private bool SeatActivated;
    private GameObject _camera;
    
    private Vector3 _formerPosition;

    private Vector3 CalibrationOffset;

    private void Awake()
    {
        CalibrationOffset.x = PlayerPrefs.GetFloat("_hmd_offset_x");
        CalibrationOffset.y = PlayerPrefs.GetFloat("_hmd_offset_y");
        CalibrationOffset.z = PlayerPrefs.GetFloat("_hmd_offset_z");
        
        _camera= this.transform.GetChild(0).gameObject;
        if (UseCalibrationOffset)
        {
            _camera.transform.position = CalibrationOffset;
        }
    }

    private void Start()
    {
        CalibrationOffset = new Vector3();
        SeatActivated = false;
        _formerPosition = new Vector3();
        _camera= this.transform.GetChild(0).gameObject;
        
       
        _camera.transform.localPosition = CalibrationOffset;

    }

    private void LateUpdate()
    {
        
        if (SeatActivated)
        {
            transform.SetPositionAndRotation(seatPosition.transform.position,seatPosition.transform.rotation);
        }
       
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _formerPosition = position;
        SeatActivated = false;
    }

    public void Seat()
    {
        SeatActivated = true;
    }

    public void UnSeat()
    {
        transform.position = _formerPosition;
    }

    public GameObject GetCamera()
    {
        return this.transform.GetChild(0).gameObject;
    }


}
