using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneDataRecorder : MonoBehaviour
{
    public static SceneDataRecorder Instance { get; private set; }
    
    private List<EventBehaviourDataFrame> _eventBehaviourDataFrames;
    private SceneData _sceneData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _eventBehaviourDataFrames = new List<EventBehaviourDataFrame>();
    }

    public void AssignEventData(string eventName, double startTime, double endTime, bool successState, string hitObject=null)
    {
        EventBehaviourDataFrame eventBehaviour = new EventBehaviourDataFrame();

        eventBehaviour.EventName = eventName;
        eventBehaviour.StartofEventTimeStamp = startTime;
        eventBehaviour.EndOfEventTimeStamp = endTime;
        eventBehaviour.EventDuration = endTime - startTime;
        eventBehaviour.SuccessfulCompletionState = successState;
        eventBehaviour.HitObjectName = hitObject;

        _eventBehaviourDataFrames.Add(eventBehaviour);
    }

    public SceneData GetDataFrame()
    {
        _sceneData.AverageSceneFPS = EyetrackingManager.Instance.GetAverageSceneFPS();
        _sceneData.EventBehavior = _eventBehaviourDataFrames;
        return _sceneData;
    }
}
