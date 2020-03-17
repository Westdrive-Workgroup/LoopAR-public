using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController))]
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

    public void AvoidInterference(float eventSpeed)
    {
        GetComponent<AimedSpeed>().ActivateEventSpeed();
    }

    public void ReestablishNormalBehavior()
    {
        GetComponent<AimedSpeed>().DeActivateEventSpeed();
        Debug.Log("normal behavior scrits");
    }
    
    
    
    
    
}
