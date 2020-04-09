using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    public GameObject participantsCar;
    
    private List<ActivationTrigger> _activationTriggers;


    private void Awake()
    {
        _activationTriggers = new List<ActivationTrigger>();
        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persitent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        participantsCar.SetActive(false);
        
        // inform all triggers to disable their gameobjects at the beginning of the experiment
        foreach (var trigger in _activationTriggers)
        {
            trigger.DeactivateTheGameObjects();
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            participantsCar.SetActive(true);   
        }
    }
    
    
    // main menu


    // Reception desk for classes to register themselves
    public void RegisterToExperimentManager(ActivationTrigger listner)
    {
        _activationTriggers.Add(listner);
    }

    // start experiment
    private void StartExperiment()
    {
        participantsCar.SetActive(true);
    }

    // calibration
    
    // End experiment
    
}
