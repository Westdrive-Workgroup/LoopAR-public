using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficEventTrigger : MonoBehaviour
{

    private GameObject currentObject;
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
        if (other.gameObject == currentObject)
            return;

        currentObject = other.gameObject;
            

        if (other.gameObject.GetComponent<ManualController>() != null)
        {
            Debug.Log("Triggered");
            PersistentTrafficEventManager.Instance.HandleEvent();
            

        }
        
    }
}
