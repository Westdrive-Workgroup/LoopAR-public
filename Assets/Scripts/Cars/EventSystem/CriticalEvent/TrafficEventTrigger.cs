using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TrafficEventTrigger : MonoBehaviour
{
    [Space][Header("Event state")]
    [SerializeField] private bool activateEvent;
    
    private CriticalEventController _eventController;
    private GameObject _currentTarget;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;

        if (other.GetComponent<ManualController>() != null)
        {
            _eventController.Triggered(activateEvent);

            if (activateEvent)
            {
                _eventController.SetEventStartData(TimeManager.Instance.GetCurrentUnixTimeStamp(), _eventController.gameObject.name);
            }
            else
            {
                _eventController.SetEventEndData(TimeManager.Instance.GetCurrentUnixTimeStamp(), true);
            }
        }
    }

    public void SetController(CriticalEventController eventController)
    {
        _eventController = eventController;
    }
}
