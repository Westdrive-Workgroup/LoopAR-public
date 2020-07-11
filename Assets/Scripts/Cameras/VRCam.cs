using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCam : MonoBehaviour
{
    public GameObject seatPosition;
    public bool seatActivated;
    
    private Vector3 _formerPosition;

    [SerializeField] private GameObject calibrationOffset;
    [SerializeField] private GameObject _camera;
    
    private void Start()
    {
        if (CalibrationManager.Instance != null)
        {
            calibrationOffset.transform.localPosition = CalibrationManager.Instance.GetSeatCalibrationOffset();
        }
        else
        {
            calibrationOffset.transform.localPosition = Vector3.zero;
            Debug.LogWarning("no Calibration Manager found, please at to the scene");
        }
        
        _formerPosition = new Vector3();
    }

    private void LateUpdate()
    {
        if (seatActivated)
        {
            transform.SetPositionAndRotation(seatPosition.transform.position,seatPosition.transform.rotation);
        }
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _formerPosition = position;
        seatActivated = false;
    }

    public void Seat()
    {
        seatActivated = true;
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
        return calibrationOffset;
    }
    
    public void SetOffset(Vector3 localOffset)
    {
        calibrationOffset.transform.localPosition = localOffset;
    }

}
