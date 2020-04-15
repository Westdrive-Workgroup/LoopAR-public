using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EyeValidationData
{
    public int participantNr;
    public int ValidationTrial;
    public int block;

    public int ValidationPointIdx;
    public double UnixTimestamp;
    public float Timestamp;
    public Transform HeadTransform;
    public Vector3 PointToFocus;
    public Vector3 LeftEyeAngleOffset;
    public Vector3 RightEyeAngleOffset;
    public Vector3 CombinedEyeAngleOffset;
    public Vector3 ValidationResults;
}