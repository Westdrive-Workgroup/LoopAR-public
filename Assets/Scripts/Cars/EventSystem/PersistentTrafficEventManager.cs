using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentTrafficEventManager : MonoBehaviour
{
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
}
