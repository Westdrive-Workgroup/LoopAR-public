using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class EyeTrackingDataFrame
{
    public double TimeStamp;
    public double TobiiTimeStamp;
    
    public Vector3 HMDposition;
    public Vector3 NoseVector; //HMD foward;

    public Vector3 EyePosWorldCombined;
    public Vector3 EyeDirWorldCombined;

    public Vector3 EyePosLocalCombined;
    public Vector3 EyeDirLocalCombined;


    public string HitObjectName;
    public Vector3 HitObjectPosition;
    public Vector3 HitPointOnObject;

    public bool RightEyeIsBlinkingWorld;        //why though?
    public bool RightEyeIsBlinkingLocal;

    public bool LeftEyeIsBlinkingWorld;
    public bool LeftEyeIsBlinkingLocal;

    //why though?
    // trigger pressed 
}
