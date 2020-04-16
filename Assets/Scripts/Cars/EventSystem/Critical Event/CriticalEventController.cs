using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalEventController: MonoBehaviour
{
    public TrafficEventTrigger startTrigger;

    public TrafficEventTrigger endTrigger;
    
    private RestrictedZoneTrigger[] _restrictedZoneTriggers;

    private GameObject _targetedCar;

    private bool _activatedEvent;
    
    void Start()
    {
        if(PersistentTrafficEventManager.Instance!=null)
            _targetedCar = PersistentTrafficEventManager.Instance.GetParticipantsCar();
        
        startTrigger.TargetVehicle(_targetedCar);
        endTrigger.TargetVehicle(_targetedCar);
        
        startTrigger.SetController(this);
        endTrigger.SetController(this);
        
        _restrictedZoneTriggers = GetComponentsInChildren<RestrictedZoneTrigger>();

       DeactivateRestrictedZones();
    }
    

    public void Triggered()
    {
        PersistentTrafficEventManager.Instance.HandleEvent();
        if (!_activatedEvent)
        {
            ActivateRestrictedZones();
            _activatedEvent = true;
        }
        else
        {
            DeactivateRestrictedZones();
        }
        
    }

    private void ActivateRestrictedZones()
    {
        foreach (var restrictedZoneTrigger in _restrictedZoneTriggers)
        {
            restrictedZoneTrigger.gameObject.SetActive(true);
        }
    }

    private void DeactivateRestrictedZones()
    {
        foreach (var restrictedZoneTrigger in _restrictedZoneTriggers)
        {
            restrictedZoneTrigger.gameObject.SetActive(false);
            _activatedEvent = false;
        }
    }
    
    
    
    
}
