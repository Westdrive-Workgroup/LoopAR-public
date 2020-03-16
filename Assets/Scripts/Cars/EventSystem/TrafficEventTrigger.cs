using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficEventTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;

        if (gameObject.GetComponent<ManualController>() != null)
        {
            Debug.Log("Triggered");
            PersistentTrafficEventManager.Instance.InitiateEvent();
            
            
        }
        
    }
}
