using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PersistentTrafficEventManager.Instance.RegisterTrafficListeners(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
}
