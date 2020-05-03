using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CriticalEventController: MonoBehaviour
{
    [Space]
    [Header("Consistent Event Objects")]
    [SerializeField] private TrafficEventTrigger startTrigger;
    [SerializeField] private TrafficEventTrigger endTrigger;
    [SerializeField] private GameObject consistentEventObjects;

    [Space]
    [Header("Event Objects")]
    [Tooltip("The gameObject which is the parent of the event object")]
    [SerializeField] private GameObject eventObjectParent;
    [SerializeField] private List<GameObject> eventObjects;
    [Tooltip("Should the event subject be active or not when experiment begins")] 
    [SerializeField] private GameObject respawnPoint;

    [Space] [Header("Event Setting")]
    [Tooltip("End the event automatically after given (0 - 15) seconds in case the participant stays idle.")] 
    [Range(0,20)] [SerializeField] private float eventIdleDuration;
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
        
        _restrictedZoneTriggers = GetComponentsInChildren<RestrictedZoneTrigger>();

       DeactivateRestrictedZones();
       EventObjectsActivationSwitch(eventObjectParent);
       
       TurnOffMeshRenderers(consistentEventObjects);
    }
    

    public void Triggered(bool state)
    {
        _activatedEvent = state;
        
        // PersistentTrafficEventManager.Instance.HandleEvent();
        if (_activatedEvent)
        {
            ActivateTheEvent();
        }
        else
        {
            DeactivateTheEvent();
        }
        
        StartCoroutine(EndIdleEvent(eventIdleDuration));
    }

    private IEnumerator EndIdleEvent(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_activatedEvent)
            ExperimentManager.Instance.ParticipantFailed();
    }

    private void ActivateTheEvent()
    {
        ActivateRestrictedZones();
        eventObjectParent.SetActive(true);
        PersistentTrafficEventManager.Instance.InitiateEvent(eventObjects);
        ExperimentManager.Instance.SetRespawnPositionAndRotation(respawnPoint.transform.position, respawnPoint.transform.rotation);
    }
    
    private void DeactivateTheEvent()
    {
        DeactivateRestrictedZones();
        PersistentTrafficEventManager.Instance.FinalizeEvent();
        if (!eventObjectActive)
            eventObjectParent.SetActive(false);
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
