using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficEventTrigger : MonoBehaviour
{
    [Space] [Header("Start Event Delay")]
    [Tooltip("0 to 15 seconds")] [Range(0,15)] [SerializeField] private float delay;
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
            // todo inform HUD
            Debug.Log("Informed HUD " + Time.time);
            
            StartCoroutine(StartDelayedEvent(delay));
            Debug.Log("Triggered " + other.gameObject + " " + Time.time);
            //PersistentTrafficEventManager.Instance.HandleEvent();
        }
    }

    IEnumerator StartDelayedEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        _eventController.Triggered();
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
