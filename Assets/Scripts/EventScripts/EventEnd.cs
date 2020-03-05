using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnd : MonoBehaviour
{
    [SerializeField] private EventController eventController;
    [SerializeField] private GameObject Event;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            eventController.EndEvent(Event);
        }
    }
}