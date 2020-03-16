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
        GetComponent<AIController>().SetAimedSpeed(eventSpeed);
    }

    public void ReestablishNormalBehavior()
    {
        Debug.Log("normal behavior scrits");
    }
    
    
    
    
    
}
