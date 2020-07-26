using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController))]
public class EventBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PersistentTrafficEventManager.Instance!=null)
            PersistentTrafficEventManager.Instance.RegisterTrafficListeners(this);
    }
    

    public void AvoidInterference(float eventSpeed)
    {
        GetComponent<AimedSpeed>().ActivateEventSpeed();
    }

    public void ReestablishNormalBehavior()
    {
        GetComponent<AimedSpeed>().DeActivateEventSpeed();
        // Debug.Log("normal behavior scrits");
    }
   
}
