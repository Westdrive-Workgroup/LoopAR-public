using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentTrafficEventManager : MonoBehaviour
{
    //currently this script is completely useless. It might get a new Meaning if we start to hide variables and get back to the Manager -> Controller System.
    //Since for the MVP I doubt that it is necessary to follow that, I will keep this functionality whenever we need to come back  to it.  
    public static PersistentTrafficEventManager Instance { get; private set; }

    private List<EventBehavior> _eventBehaviorListeners;

    // Start is called before the first frame update
    
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
        // 
    }

    public void RegisterTrafficListeners(EventBehavior listener)
    {
        _eventBehaviorListeners.Add(listener);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateEvent()
    {
        
        foreach (var eventListener in _eventBehaviorListeners)
        {
            Debug.Log("..initiated for ... " + eventListener);
            eventListener.AvoidInterference(10f);
        }
    }
}
