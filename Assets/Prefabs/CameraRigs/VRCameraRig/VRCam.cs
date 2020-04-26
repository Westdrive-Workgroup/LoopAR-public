using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCam : MonoBehaviour
{
    
    public GameObject seatPosition;
    private bool SeatActivated;
    
    
    private Vector3 _formerPosition;

    [SerializeField] private GameObject _calibrationOffset;
    [SerializeField] private GameObject _camera;

    private void Awake()
    {
      
    }

    private void Start()
    { 
        _calibrationOffset.transform.localPosition = CalibrationManager.Instance.GetSeatCalibrationOffset();
        
        _formerPosition = new Vector3();

        

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
        return _camera;
    }

    public GameObject GetCameraOffset()
    {
        return _calibrationOffset;
    }
    
    public void SetOffset(Vector3 localOffset)
    {
        _calibrationOffset.transform.localPosition = localOffset;
    }

}
