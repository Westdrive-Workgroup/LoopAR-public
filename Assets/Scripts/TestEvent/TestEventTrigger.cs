using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class TestEventTrigger : MonoBehaviour
{
    [Tooltip("Event to trigger.")]
    public GameEvent Event;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            Event.Raise();
        }
    }
}
