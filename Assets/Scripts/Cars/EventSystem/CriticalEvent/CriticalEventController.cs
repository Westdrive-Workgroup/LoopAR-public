using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CriticalEventController: MonoBehaviour
{
    [Space] [Header("Triggers")]
    [SerializeField] private TrafficEventTrigger startTrigger;
    [SerializeField] private TrafficEventTrigger endTrigger;
    [Tooltip("The gameobject which is the parents of the event subjects")]
    [SerializeField] private GameObject triggers;

    [Space]
    [Header("Event Objects")]
    
    [Tooltip("The gameobject which is the parents of the event object")]
    [SerializeField] private GameObject eventObjectParent;
    [SerializeField] private List<GameObject> eventObjects;
    private List<GameObject> _setEventObjects;
    [Tooltip("Should the event subject be active or not when experiment begins")] 
    [SerializeField] private bool eventObjectActive;
    
    // [SerializeField] private GameObject eventObject;
    
    private RestrictedZoneTrigger[] _restrictedZoneTriggers;
    private GameObject _targetedCar;
    private bool _activatedEvent;
    private MeshRenderer[] _meshRenderers;
    
    void Start()
    {
        if (PersistentTrafficEventManager.Instance != null)
        {
            _targetedCar = PersistentTrafficEventManager.Instance.GetParticipantsCar();
        }

        startTrigger.TargetVehicle(_targetedCar);
        endTrigger.TargetVehicle(_targetedCar);
        
        startTrigger.SetController(this);
        endTrigger.SetController(this);

        _setEventObjects = eventObjects;
        
        _restrictedZoneTriggers = GetComponentsInChildren<RestrictedZoneTrigger>();

       DeactivateRestrictedZones();
       EventSubjectsActivationSwitch(eventObjectParent);
       
       TurnOffMeshRenderers(triggers);
    }
    

    public void Triggered()
    {
        // PersistentTrafficEventManager.Instance.HandleEvent();
        if (!_activatedEvent)
        {
            ActivateRestrictedZones();
            eventObjectParent.SetActive(true);
            PersistentTrafficEventManager.Instance.InitiateEvent(eventObjects);
            // PersistentTrafficEventManager.Instance.SetEventObject(_setEventObjects);
            // PersistentTrafficEventManager.Instance.SetEventObject(eventObject);
            _activatedEvent = true;
        }
        else
        {
            DeactivateRestrictedZones();
            PersistentTrafficEventManager.Instance.FinalizeEvent();
            if (!eventObjectActive)
                eventObjectParent.SetActive(false);
        }
        
        
    }

    private void ActivateRestrictedZones()
    {
        foreach (var restrictedZoneTrigger in _restrictedZoneTriggers)
        {
            restrictedZoneTrigger.gameObject.SetActive(true);
            restrictedZoneTrigger.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void DeactivateRestrictedZones()
    {
        foreach (var restrictedZoneTrigger in _restrictedZoneTriggers)
        {
            restrictedZoneTrigger.gameObject.SetActive(false);
        }
        _activatedEvent = false;
    }

    public void TurnOffMeshRenderers(GameObject trigger)
    {
        _meshRenderers = trigger.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    private void EventSubjectsActivationSwitch(GameObject parent)
    {
        if (eventObjectActive)
            parent.SetActive(true);
        else
            parent.SetActive(false);
    }
}
