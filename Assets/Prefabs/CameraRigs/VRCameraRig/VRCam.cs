using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCam : MonoBehaviour
{
    public GameObject seatPosition;

    private void LateUpdate()
    {
        this.transform.SetPositionAndRotation(seatPosition.transform.position,seatPosition.transform.rotation);
    }
}
