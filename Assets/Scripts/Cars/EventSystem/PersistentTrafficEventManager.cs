using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentTrafficEventManager : MonoBehaviour
{
    public static PersistentTrafficEventManager Instance { get; private set; }
    public GameObject participantsCar; //needs a functionality to find the participants Car
    
    
    private List<EventBehavior> _eventBehaviorListeners;
    private ControlSwitch participantsControlSwitch;
    private bool activatedEvent;

    public float EventSpeed= 5f;
    
    private void Awake()
    {
        _eventBehaviorListeners = new List<EventBehavior>();
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persitent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        participantsControlSwitch = participantsCar.GetComponent<ControlSwitch>();
        Debug.Log("return " + participantsControlSwitch);
        activatedEvent = false; // 
    }

    public void RegisterTrafficListeners(EventBehavior listener)
    {
        _eventBehaviorListeners.Add(listener);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }


    public void HandleEvent()
    {
        if (activatedEvent)
        {
            FinalizeEvent();
        }
        else
        {
            activatedEvent = true;
            InitiateEvent();
        }
    }

    private void InitiateEvent()
    {
        
        foreach (var eventListener in _eventBehaviorListeners)
        {
            eventListener.AvoidInterference(10f);
        }
        participantsControlSwitch.SwitchControl();
    }

    private void FinalizeEvent()
    {
        foreach (var eventListener in _eventBehaviorListeners)
        {
            Debug.Log("setting back to normal");
            eventListener.ReestablishNormalBehavior();
        }
        
        participantsControlSwitch.SwitchControl();
    }
}
