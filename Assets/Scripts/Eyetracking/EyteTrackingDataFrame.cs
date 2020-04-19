using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class EyeTrackingDataFrame
{
    public double UnixTimeStamp;
    public double TimeStamp;
    public double TobiiTimeStamp;
    
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


    public int NumberOfDataStrings()
    {
        return 1 + //TimeStamp
               1 + //TimeStamp
               1 + //TobiiTimeStamp
               3 + //HmdPosition
               3 + //NoseVector
               3 + //EyePosWorld
               3 + //EyeDirWorld      
               3 + //EyePosLocal
               3 + //EyeDirLocal
               1 +
               1 +
               1 +
               1 +
               1 +
               hitObjects.Count;
    }
}
