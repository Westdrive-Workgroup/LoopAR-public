using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCam : MonoBehaviour
{
    public GameObject seatPosition;
    private bool SeatActivated;
    private Vector3 _formerPosition;
    private void Start()
    {
        SeatActivated = false;
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
    
    
}
