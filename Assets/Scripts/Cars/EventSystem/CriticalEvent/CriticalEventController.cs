using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CriticalEventController: MonoBehaviour
{
    [Space] [Header("Consistent Event Objects")]
    [SerializeField] private TrafficEventTrigger startTrigger;
    [SerializeField] private TrafficEventTrigger endTrigger;
    [SerializeField] private GameObject consistentEventObjects;

    [Space]
    [Header("Event Objects")]
    
    [Tooltip("The gameobject which is the parent of the event object")]
    [SerializeField] private GameObject eventObjectParent;
    [SerializeField] private List<GameObject> eventObjects;
    private List<GameObject> _setEventObjects;
    [Tooltip("Should the event subject be active or not when experiment begins")] 
    [SerializeField] private bool eventObjectActive;
    [SerializeField] private GameObject respawnPoint;
    
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
       EventObjectsActivationSwitch(eventObjectParent);
       
       TurnOffMeshRenderers(consistentEventObjects);
    }
    

    public void Triggered(bool state)
    {
        _activatedEvent = state;
        
        // PersistentTrafficEventManager.Instance.HandleEvent();
        if (!_activatedEvent)
        {
            _activatedEvent = true;
            ActivateRestrictedZones();
            eventObjectParent.SetActive(true);
            PersistentTrafficEventManager.Instance.InitiateEvent(eventObjects);
            // PersistentTrafficEventManager.Instance.SetEventObject(_setEventObjects);
            // PersistentTrafficEventManager.Instance.SetEventObject(eventObject);
            ExperimentManager.Instance.SetRespawnPositionAndRotation(respawnPoint.transform.position, respawnPoint.transform.rotation);
        }
        else
        {
            _activatedEvent = false;
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
    }

    public void TurnOffMeshRenderers(GameObject trigger)
    {
        _meshRenderers = trigger.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    private void EventObjectsActivationSwitch(GameObject parent)
    {
        if (eventObjectActive)
            parent.SetActive(true);
        else
            parent.SetActive(false);
    }
}
