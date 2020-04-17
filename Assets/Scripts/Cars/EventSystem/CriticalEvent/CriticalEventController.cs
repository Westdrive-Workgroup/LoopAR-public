using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CriticalEventController: MonoBehaviour
{
    [Space] [Header("Triggers")]
    [SerializeField] private TrafficEventTrigger startTrigger;
    [SerializeField] private TrafficEventTrigger endTrigger;
    
    [Space] [Header("Accident Case")]
    [Tooltip("The gameobject which is the parents of the accident elements")] [SerializeField] private GameObject testAccident;
    [Tooltip("Should the testAccident be active or not when experiment begins")] [SerializeField] private bool active;
    
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

       if (active)
           testAccident.SetActive(true);
       else
           testAccident.SetActive(false);
    }
    

    public void Triggered()
    {
        PersistentTrafficEventManager.Instance.HandleEvent();
        if (!_activatedEvent)
        {
            ActivateRestrictedZones();
            testAccident.SetActive(true);
            _activatedEvent = true;
        }
        else
        {
            DeactivateRestrictedZones();
            
            if (!active)
                testAccident.SetActive(false);
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
