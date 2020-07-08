using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TrafficEventTrigger : MonoBehaviour
{
    /*[Space][Header("Start Event Delay")]
    [Tooltip("0 to 15 seconds")] [Range(0,15)] [SerializeField] */
    private float _startEventDelay = 0;
    private float _endEventDelay = 0;

    [Space][Header("Event state")]
    [SerializeField] private bool activateEvent;
    
    private CriticalEventController _eventController;
    private GameObject _targetVehicle;
    private GameObject _currentTarget;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Start trigger? " + activateEvent);
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        
        if (other.gameObject == _targetVehicle)
        {
            if (activateEvent)
            {
                _startEventDelay = _eventController.GetEventStartDelay();
                _targetVehicle.gameObject.GetComponentInChildren<HUD_Advance>().DriverAlert();
                StartCoroutine(DelayedInEvent(_startEventDelay));
            }
            else
            {
                // todo call the related HUD function
                _endEventDelay = _eventController.GetEventEndDelay();
                StartCoroutine(DelayedInEvent(_endEventDelay));
            }
        }
    }

    IEnumerator DelayedInEvent(float delaySeconds)
    {
        float time1 = Time.time;
        Debug.Log("Delay seconds: " + delaySeconds);
        Debug.Log("Before " + time1);
        yield return new WaitForSeconds(delaySeconds);

        float time2 = Time.time;
        Debug.Log("After " + time2);
        _eventController.Triggered(activateEvent);
        Debug.Log("Time diff: " + (time1 - time2));
    }

    public void TargetVehicle(GameObject vehicle)
    {
        _targetVehicle = vehicle;
    }

    public void SetController(CriticalEventController eventController)
    {
        _eventController = eventController;
    }
}
