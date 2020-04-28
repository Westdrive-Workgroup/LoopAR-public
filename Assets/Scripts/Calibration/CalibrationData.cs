using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class CalibrationData
{
    public String ParticipantUuid;
    public Vector3 SeatCalibrationOffset;
    public Vector3 EyeValidationError;
}
