using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class EventBehaviourDataFrame
{
    public string EventName;
    
    public double StartofEventTimeStamp;
    public double EndOfEventTimeStamp;
    public double EventDuration;
    
    public bool SuccessfulCompletionState;
    public string HitObjectName;
}
