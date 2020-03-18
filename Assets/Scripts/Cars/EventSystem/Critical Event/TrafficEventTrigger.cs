using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficEventTrigger : MonoBehaviour
{
    private CriticalEventController _eventController;
    private GameObject targetVehicle;
    private GameObject currentTarget;
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
        if (other.gameObject == currentTarget)
            return;

        currentTarget = other.gameObject;
        
        if (other.gameObject == targetVehicle)
        {
            Debug.Log("Triggered " + other.gameObject);
            //PersistentTrafficEventManager.Instance.HandleEvent();
            _eventController.Triggered();
            

        }
        
    }


    public void TargetVehicle(GameObject vehicle)
    {
        targetVehicle = vehicle;
    }

    public void SetController(CriticalEventController eventController)
    {
        _eventController = eventController;
    }
}
