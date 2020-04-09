using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    public GameObject participantsCar;


    private void Awake()
    {
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

    // main menu
        
    

    
    void Start()
    {
        participantsCar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // start experiment
    private void StartExperiment()
    {
        participantsCar.SetActive(true);
    }
    
    // calibration
    
    // End experiment
    
}
