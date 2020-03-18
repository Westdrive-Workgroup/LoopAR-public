using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalEventController: MonoBehaviour
{
    public TrafficEventTrigger startTrigger;

    public TrafficEventTrigger endTrigger;
    
    private RestrictedZoneTrigger[] _restrictedZoneTriggers;

    private GameObject targetedCar;
    
    void Start()
    {
        targetedCar = PersistentTrafficEventManager.Instance.GetParticipantsCar();
        
        startTrigger.TargetVehicle(targetedCar);
        endTrigger.TargetVehicle(targetedCar);
        
        _restrictedZoneTriggers = GetComponentsInChildren<RestrictedZoneTrigger>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
