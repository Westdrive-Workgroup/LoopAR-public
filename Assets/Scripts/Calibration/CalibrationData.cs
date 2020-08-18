using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class CalibrationData
{
    // public int ParticipationNumber;
    public String ParticipantUuid;
    public bool VRmode;
    public Vector3 EyeValidationError;
    public Vector3 SeatCalibrationOffset;
}
