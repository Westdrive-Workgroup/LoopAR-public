using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficEventTrigger : MonoBehaviour
{
    private CriticalEventController _eventController;
    private GameObject _targetVehicle;
    private GameObject _currentTarget;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        if (other.gameObject == _targetVehicle)
        {
            Debug.Log("Triggered " + other.gameObject);
            //PersistentTrafficEventManager.Instance.HandleEvent();
            _eventController.Triggered();
        }
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
