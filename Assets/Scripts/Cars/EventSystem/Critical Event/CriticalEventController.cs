using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalEventController: MonoBehaviour
{
    public TrafficEventTrigger startTrigger;

    public TrafficEventTrigger endTrigger;
    
    private RestrictedZoneTrigger[] _restrictedZoneTriggers;

    private GameObject targetedCar;

    private bool activatedEvent;
    
    void Start()
    {
        targetedCar = PersistentTrafficEventManager.Instance.GetParticipantsCar();
        
        startTrigger.TargetVehicle(targetedCar);
        endTrigger.TargetVehicle(targetedCar);
        
        startTrigger.SetController(this);
        endTrigger.SetController(this);
        
        _restrictedZoneTriggers = GetComponentsInChildren<RestrictedZoneTrigger>();

       DeactivateRestrictedZones();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Triggered()
    {
        PersistentTrafficEventManager.Instance.HandleEvent();
        if (!activatedEvent)
        {
            ActivateRestrictedZones();
            activatedEvent = true;
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
            activatedEvent = false;
        }
    }
    
    
    
    
}
