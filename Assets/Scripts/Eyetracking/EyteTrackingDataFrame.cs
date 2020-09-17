using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class EyeTrackingDataFrame
{
    public double UnixTimeStamp;
    public double TobiiTimeStamp;

    public float FPS;
    
    public Vector3 HmdPosition;
    public Vector3 NoseVector; //HMD foward;

    public Vector3 EyePosWorldCombined;
    public Vector3 EyeDirWorldCombined;

    public Vector3 EyePosLocalCombined;
    public Vector3 EyeDirLocalCombined;

    

    public bool RightEyeIsBlinkingWorld;        //why though?
    public bool RightEyeIsBlinkingLocal;

    public bool LeftEyeIsBlinkingWorld;
    public bool LeftEyeIsBlinkingLocal;
    
    public List<HitObjectInfo> hitObjects;

    //why though?
    // trigger pressed 
    
}
