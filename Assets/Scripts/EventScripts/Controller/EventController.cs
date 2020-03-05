using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Object = System.Object;

public class EventController : MonoBehaviour
{
    [SerializeField] private List<EventObject> eventObjects;

    private float startTime;
    private float endTime;
    private float reactionTime;
    private string eventName;

    public void StartEvent(GameObject eventTriggered)
    {
//        Transform eventTriggeredTransform = eventTriggered.transform;
//        int children = eventTriggeredTransform.childCount;
//        for (int i = 0; i < children; ++i)
//            eventTriggeredTransform.GetChild(i).gameObject.SetActive(true);

        startTime = Time.time;
        eventName = transform.GetChild(0).name;
        Debug.Log("Das "+ eventName +" beginnt um: "+ startTime);
        foreach (var eventObject in eventObjects)
        {
            EventObject test = eventObject;

            test.StartEventAction();
        }
    }

    public void EndEvent(GameObject eventTriggered)
    {
//        Transform eventTriggeredTransform = eventTriggered.transform;
//        int children = eventTriggeredTransform.childCount;
//        for (int i = 0; i < children; ++i)
//            eventTriggeredTransform.GetChild(i).gameObject.SetActive(false);

        endTime = Time.time;
        reactionTime = endTime - startTime;
        Debug.Log("Das "+ eventName +" endet um: "+ endTime);
        Debug.Log("Die Reaktionszeit beträgt :"+ reactionTime);

        foreach (var eventObject in eventObjects)
        {
            EventObject test = eventObject;

            test.EndEventAction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }




}