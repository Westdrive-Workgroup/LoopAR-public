using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// name is at the moment a bit miss leading. It is a overall manager class, which is aware of all events in that scene, the Participant and the AI Cars.
/// It is aware of starting events and ending events globally.
/// It is intended to handle the reaction of AI cars in case of events of AI cars to avoid them of interfering into the event
/// </summary>

[DisallowMultipleComponent]
public class PersistentTrafficEventManager : MonoBehaviour
{
    public static PersistentTrafficEventManager Instance { get; private set; }
    [SerializeField] private GameObject participantsCar; //needs a functionality to find the participants Car
    
    
    private List<EventBehavior> _eventBehaviorListeners;
    private ControlSwitch _participantsControlSwitch;
    private bool _activatedEvent;

    [SerializeField] private float eventSpeed = 5f;
    
    private void Awake()
    {
        _eventBehaviorListeners = new List<EventBehavior>();
        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persistent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        _participantsControlSwitch = participantsCar.GetComponent<ControlSwitch>();
        _activatedEvent = false; 
    }

    public void RegisterTrafficListeners(EventBehavior listener)
    {
        _eventBehaviorListeners.Add(listener);
    }
    

    public void HandleEvent()
    {
        if (_activatedEvent)
        {
            FinalizeEvent();
        }
        else
        {
            _activatedEvent = true;
            InitiateEvent();
        }
    }

    private void InitiateEvent()
    {
        foreach (var eventListener in _eventBehaviorListeners)
        {
            eventListener.AvoidInterference(10f);
        }
        _participantsControlSwitch.SwitchControl();
    }

    private void FinalizeEvent()
    {
        foreach (var eventListener in _eventBehaviorListeners)
        {
            Debug.Log("setting back to normal");
            eventListener.ReestablishNormalBehavior();
        }
        
        _participantsControlSwitch.SwitchControl();
    }

    public GameObject GetParticipantsCar()
    {
        return participantsCar;
    }

    public float GetEventSpeed()
    {
        return eventSpeed;
    }
}
